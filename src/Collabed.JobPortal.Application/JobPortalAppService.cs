using System;
using System.Collections.Generic;
using System.Text;
using Collabed.JobPortal.Localization;
using Volo.Abp.Application.Services;

namespace Collabed.JobPortal;

/* Inherit your application services from this class.
 */
public abstract class JobPortalAppService : ApplicationService
{
    protected JobPortalAppService()
    {
        LocalizationResource = typeof(JobPortalResource);
    }
}
