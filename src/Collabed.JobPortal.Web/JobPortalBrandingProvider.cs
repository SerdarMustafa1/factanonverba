using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace Collabed.JobPortal.Web;

[Dependency(ReplaceServices = true)]
public class JobPortalBrandingProvider : DefaultBrandingProvider
{
    public override string AppName => "BuildMyTalent";
    public override string LogoUrl => "/images/logo/BMT_Primary Full_symbol.png";
}
