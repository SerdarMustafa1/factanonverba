using System.Collections.Generic;

namespace Collabed.JobPortal.Web.ProfileManagement;

public class ProfileManagementPageOptions
{
    public List<IProfileManagementPageContributor> Contributors { get; }

    public ProfileManagementPageOptions()
    {
        Contributors = new List<IProfileManagementPageContributor>();
    }
}
