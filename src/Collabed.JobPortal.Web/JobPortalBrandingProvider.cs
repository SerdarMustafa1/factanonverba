using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Collabed.JobPortal.Web;

[Dependency(ReplaceServices = true)]
public class JobPortalBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "JobPortal";
}
