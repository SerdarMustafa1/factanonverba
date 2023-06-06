using Microsoft.AspNetCore.Mvc;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Volo.Abp.Account;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.Validation;

namespace Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.Password;

public class AccountProfilePasswordManagementGroupViewComponent : AbpViewComponent
{
    protected IProfileAppService ProfileAppService { get; }

    public AccountProfilePasswordManagementGroupViewComponent(
        IProfileAppService profileAppService)
    {
        ProfileAppService = profileAppService;
    }

    public virtual async Task<IViewComponentResult> InvokeAsync()
    {
        var user = await ProfileAppService.GetAsync();

        var model = new ChangePasswordInfoModel
        {
            HideOldPasswordInput = !user.HasPassword,
            IsExternalLogin = user.IsExternal
        };

        return View("~/Pages/Account/Components/ProfileManagementGroup/Password/Default.cshtml", model);
    }

    public class ChangePasswordInfoModel
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [Display(Name = "Current Password")]
        [DataType(DataType.Password)]
        [DisableAuditing]
        public string CurrentPassword { get; set; }

        [Required(ErrorMessage = " ")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        [Display(Name = "DisplayName:NewPassword")]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*[^a-zA-Z])(?=.*\d).{8,}$", ErrorMessage = " ")]
        public string Password { get; set; }

        [Required(ErrorMessage = "Your passwords don't match")]
        [DisplayName("Confirm Password")]
        [DataType(DataType.Password)]
        [Compare(nameof(Password), ErrorMessage = "Passwords don't match each other")]
        [DisableAuditing]
        public string ConfirmPassword { get; set; }

        public bool IsExternalLogin { get; set; }

        public bool HideOldPasswordInput { get; set; }
    }
}
