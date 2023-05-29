using Collabed.JobPortal.User;
using Collabed.JobPortal.Web.Attributes;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.ComponentModel.DataAnnotations;
using Volo.Abp.Domain.Entities;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Collabed.JobPortal.Web.Models
{
    public class CompanyAccountModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [RequiredWhenCompany(ErrorMessage = "Email cannot be empty")]
        [ExtendedEmailAddress("Email address is not valid")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        //[DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        //[Display(Name = "First Name")]
        //public string Name { get; set; }

        //[DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        //[Display(Name = "Last Name")]
        //public string Surname { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        [RegularExpression("^(\\d{10})$", ErrorMessage = "Please enter a valid UK Phone Number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [RequiredWhenCompany]
        [Display(Name = "Company Name")]
        public string CompanyName { get; set; }

        [HiddenInput]
        public string LogoFileName { get; set; }

        [Display(Name = "New Logo")]
        public IFormFile NewLogo { get; set; }

        [HiddenInput] public string ConcurrencyStamp { get; set; }

        [HiddenInput] public UserType? UserType { get; set; }
    }
}
