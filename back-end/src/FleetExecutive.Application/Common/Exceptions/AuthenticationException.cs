namespace FleetExecutive.Application.Common.Exceptions;

/// <summary>
/// Erro genérico de autenticação — nunca deixar a mensagem revelar se o e-mail existe ou não
/// (ver Estrutura/07-autenticacao-seguranca-rbac.md, anti user-enumeration). A mensagem exposta
/// ao cliente HTTP deve ser sempre "e-mail ou senha inválidos", independente da causa real.
/// </summary>
public class AuthenticationException : Exception
{
    public AuthenticationException() : base("E-mail ou senha inválidos.") { }
}
