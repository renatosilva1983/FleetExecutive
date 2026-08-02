using FleetExecutive.Infrastructure.Email;
using Microsoft.Extensions.Logging;

namespace FleetExecutive.Infrastructure.Tests.Email;

public class NoOpEmailSenderTests
{
    [Fact]
    public async Task SendAsync_NaoLancaEConcluiComSucesso()
    {
        var sender = new NoOpEmailSender(new Mock<ILogger<NoOpEmailSender>>().Object);

        var task = sender.SendAsync("destino@example.com", "Assunto", "Corpo");

        await task; // não deve lançar
        Assert.True(task.IsCompletedSuccessfully);
    }
}
