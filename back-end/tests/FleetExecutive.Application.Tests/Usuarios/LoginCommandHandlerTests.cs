using FleetExecutive.Application.Common.Exceptions;
using FleetExecutive.Application.Common.Interfaces;
using FleetExecutive.Application.Usuarios.Commands;
using FleetExecutive.Domain.Usuarios;
using Microsoft.EntityFrameworkCore;
using MockQueryable.Moq;

namespace FleetExecutive.Application.Tests.Usuarios;

public class LoginCommandHandlerTests
{
    private const string Email = "user@example.com";
    private const string Senha = "senha-correta";
    private static readonly Guid TenantId = Guid.NewGuid();

    private readonly Mock<IApplicationDbContext> _db = new();
    private readonly Mock<IPasswordHasher> _hasher = new();
    private readonly Mock<IJwtTokenGenerator> _jwt = new();
    private readonly Mock<ICurrentTenant> _tenant = new();
    private readonly Mock<DbSet<UserSession>> _sessions = new List<UserSession>().BuildMockDbSet();

    public LoginCommandHandlerTests()
    {
        _db.Setup(x => x.UserSessions).Returns(_sessions.Object);
        _db.Setup(x => x.SaveChangesAsync(It.IsAny<CancellationToken>())).ReturnsAsync(1);
        _jwt.Setup(x => x.GerarAccessToken(It.IsAny<User>(), It.IsAny<Guid>())).Returns("access-token");
        _jwt.Setup(x => x.GerarRefreshToken()).Returns("refresh-token");
        _tenant.SetupGet(x => x.IsResolved).Returns(true);
        _tenant.SetupGet(x => x.TenantId).Returns(TenantId);
    }

    private void ComUsuarios(params User[] users) =>
        _db.Setup(x => x.Users).Returns(users.ToList().BuildMockDbSet().Object);

    private LoginCommandHandler CriarHandler() =>
        new(_db.Object, _hasher.Object, _jwt.Object, _tenant.Object);

    private static User UsuarioAtivo() =>
        User.Criar("Fulano", Email, "hash-armazenado", Perfil.Administrador);

    [Fact]
    public async Task EmailInexistente_LancaAuthenticationException()
    {
        ComUsuarios(); // nenhum usuário

        await Assert.ThrowsAsync<AuthenticationException>(() =>
            CriarHandler().Handle(new LoginCommand(Email, Senha, false), CancellationToken.None));
    }

    [Fact]
    public async Task UsuarioInativo_LancaAuthenticationException()
    {
        var user = UsuarioAtivo();
        user.Desativar();
        ComUsuarios(user);

        await Assert.ThrowsAsync<AuthenticationException>(() =>
            CriarHandler().Handle(new LoginCommand(Email, Senha, false), CancellationToken.None));
    }

    [Fact]
    public async Task SenhaInvalida_LancaAuthenticationException()
    {
        ComUsuarios(UsuarioAtivo());
        _hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(false);

        await Assert.ThrowsAsync<AuthenticationException>(() =>
            CriarHandler().Handle(new LoginCommand(Email, Senha, false), CancellationToken.None));
    }

    [Fact]
    public async Task TenantNaoResolvido_LancaInvalidOperationException()
    {
        ComUsuarios(UsuarioAtivo());
        _hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);
        _tenant.SetupGet(x => x.IsResolved).Returns(false);
        _tenant.SetupGet(x => x.TenantId).Returns((Guid?)null);

        await Assert.ThrowsAsync<InvalidOperationException>(() =>
            CriarHandler().Handle(new LoginCommand(Email, Senha, false), CancellationToken.None));
    }

    [Fact]
    public async Task CredenciaisValidasSemManterConectado_RetornaTokenSemRefreshEPersiste()
    {
        var user = UsuarioAtivo();
        ComUsuarios(user);
        _hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = await CriarHandler().Handle(new LoginCommand(Email, Senha, false), CancellationToken.None);

        Assert.Equal("access-token", result.AccessToken);
        Assert.Equal(string.Empty, result.RefreshToken);
        Assert.Equal(nameof(Perfil.Administrador), result.Perfil);
        Assert.NotNull(user.UltimoLoginEm); // RegistrarLogin foi chamado
        _sessions.Verify(x => x.Add(It.IsAny<UserSession>()), Times.Never);
        _db.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task CredenciaisValidasComManterConectado_CriaSessaoComRefreshToken()
    {
        ComUsuarios(UsuarioAtivo());
        _hasher.Setup(x => x.Verify(It.IsAny<string>(), It.IsAny<string>())).Returns(true);

        var result = await CriarHandler().Handle(new LoginCommand(Email, Senha, true), CancellationToken.None);

        Assert.Equal("refresh-token", result.RefreshToken);
        _sessions.Verify(x => x.Add(It.IsAny<UserSession>()), Times.Once);
        _db.Verify(x => x.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
