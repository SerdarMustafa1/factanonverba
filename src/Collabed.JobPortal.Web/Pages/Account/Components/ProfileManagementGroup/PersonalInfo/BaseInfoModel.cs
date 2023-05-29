using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Collabed.JobPortal.Web.Pages.Account.Components.ProfileManagementGroup.PersonalInfo;

public partial class AccountProfilePersonalInfoManagementGroupViewComponent
{
    public class BaseInfoModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }



        [HiddenInput] public string ConcurrencyStamp { get; set; }
    }
}