using UserService.Models;

namespace UserService.Interfaces
{
    public interface IAuthService
    {
        string LoginCoorporate(Principal principal);
        string LoginPersonal(Principal principal);
    }
}
