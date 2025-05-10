using TaskManagementAPI.Models;

namespace TaskManagementAPI.IRepository
{
    public interface IAuthService
    {
        Task<LoginResponseModel?> Authenticate(LoginModel user);
    }
}
