using Collabed.JobPortal.Account;
using System.Threading.Tasks;
using Volo.Abp.Account;

namespace Collabed.JobPortal.Users
{
    public interface IBmtAccountAppService : IAccountAppService
    {
        Task SendEmailVerificationRequestAsync(SendEmailVerificationDto input);
        Task<bool> CheckIfEmailExistsAsync(string emailAddress);
        Task<bool> CheckIfUsernameExistsAsync(string userName);
        Task<bool> CheckPasswordCredentials(string loginInput, string password);
    }
}
