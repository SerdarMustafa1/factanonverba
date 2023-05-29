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
    public class IndividualAccountModel : ExtensibleObject, IHasConcurrencyStamp
    {
        [HiddenInput]
        public Guid Id { get; set; }

        [RequiredWhenPersonal(ErrorMessage = "Email cannot be empty")]
        [ExtendedEmailAddress("Email address is not valid")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [RequiredWhenPersonal(ErrorMessage = "First Name cannot be empty")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        [Display(Name = "First Name")]
        public string Name { get; set; }

        [RequiredWhenPersonal(ErrorMessage = "Last Name cannot be empty")]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        [Display(Name = "Last Name")]
        public string Surname { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        [RegularExpression("^(\\d{10})$", ErrorMessage = "Please enter a valid UK Phone Number")]
        [Display(Name = "Phone Number")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Post Code")]
        public string PostCode { get; set; }

        [HiddenInput] public string CvFileName { get; set; }

        public IFormFile NewCv { get; set; }

        [HiddenInput] public string ConcurrencyStamp { get; set; }

        [HiddenInput] public UserType? UserType { get; set; }
    }
}
