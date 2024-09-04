using OracaoApp.Utils.Attributes;
using OracaoApp.Utils.Exceptions;
using System.Security.Claims;

namespace OracaoApp.API.Services;

[SingletonService]
public class AuthService(IHttpContextAccessor _contextAccessor)
{
    private string AuthHeader => _contextAccessor.HttpContext?.Request.Headers.Authorization.FirstOrDefault() ?? throw new NotAuthorizedException("Token de acesso não encontrado na requisição.");
    public string AuthToken => AuthHeader?.Length > 7 ? AuthHeader![7..] : "";
    public Guid UserId => Guid.Parse(_contextAccessor.HttpContext?.User.Claims.Where(x => x.Type == ClaimTypes.NameIdentifier).Select(x => x.Value).FirstOrDefault() ?? throw new NotAuthorizedException("UserId não encontrado no token de acesso."));
    public string Username => _contextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "preferred_username").Select(x => x.Value).FirstOrDefault() ?? "";
    public string Email => _contextAccessor.HttpContext?.User.Claims.Where(x => x.Type == "email").Select(x => x.Value).FirstOrDefault() ?? "";
}
