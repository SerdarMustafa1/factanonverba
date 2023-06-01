using Collabed.JobPortal.Applications;
using System.Threading.Tasks;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Identity;

namespace Collabed.JobPortal.Account.Emailing.Templates
{
    public interface IBmtAccountEmailer : IAccountEmailer
    {
        Task SendEmailVerificationRequestAsync(IdentityUser user, string callbackUrl);
        Task SendApplicationEmailToCompanyAsync(ApplicationEmailDto jobApplicationDto, bool isNative);
        Task SendApplicationConfirmationAsync(ApplicationConfirmationDto application);
    }
}
