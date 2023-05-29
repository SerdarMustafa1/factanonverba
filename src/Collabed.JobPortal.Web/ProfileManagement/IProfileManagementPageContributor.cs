using System.Threading.Tasks;

namespace Collabed.JobPortal.Web.ProfileManagement;

public interface IProfileManagementPageContributor
{
    Task ConfigureAsync(ProfileManagementPageCreationContext context);
}
