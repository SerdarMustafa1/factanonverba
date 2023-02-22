using Collabed.JobPortal.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace Collabed.JobPortal.Controllers
{
    public abstract class PaymentController : AbpControllerBase
    {
        protected PaymentController()
        {
            LocalizationResource = typeof(JobPortalResource);
        }
    }
}
