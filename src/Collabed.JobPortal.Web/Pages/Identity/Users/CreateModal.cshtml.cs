using Collabed.JobPortal.User;
using Collabed.JobPortal.Users;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Auditing;
using Volo.Abp.Identity;
using Volo.Abp.ObjectExtending;
using Volo.Abp.Validation;

namespace Collabed.JobPortal.Web.Pages.Identity.Users;

public class CreateModalModel : IdentityPageModel
{
    [BindProperty]
    public UserInfoViewModel UserInfo { get; set; }

    [BindProperty]
    public AssignedRoleViewModel[] Roles { get; set; }

    protected IIdentityUserAppService IdentityUserAppService { get; }
    protected IBmtAccountAppService UserAppService { get; }

    public CreateModalModel(IIdentityUserAppService identityUserAppService, IBmtAccountAppService userAppService)
    {
        IdentityUserAppService = identityUserAppService;
        UserAppService = userAppService;
    }

    public virtual async Task<IActionResult> OnGetAsync()
    {
        UserInfo = new UserInfoViewModel();

        var roleDtoList = (await IdentityUserAppService.GetAssignableRolesAsync()).Items;

        Roles = ObjectMapper.Map<IReadOnlyList<IdentityRoleDto>, AssignedRoleViewModel[]>(roleDtoList);

        foreach (var role in Roles)
        {
            role.IsAssigned = role.IsDefault;
        }

        return Page();
    }

    public virtual async Task<NoContentResult> OnPostAsync()
    {
        ValidateModel();

        var input = ObjectMapper.Map<UserInfoViewModel, IdentityUserCreateDto>(UserInfo);
        UserInfo.MapExtraPropertiesTo(input, MappingPropertyDefinitionChecks.Destination);
        input.RoleNames = Roles.Where(r => r.IsAssigned).Select(r => r.Name).ToArray();

        // TODO: Set extra properties (first name, last name, user type)
        var createdUser = await IdentityUserAppService.CreateAsync(input);

        return NoContent();
    }

    public class UserInfoViewModel : ExtensibleObject
    {
        [Required]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxUserNameLength))]
        public string UserName { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxNameLength))]
        public string Name { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxSurnameLength))]
        public string Surname { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [DisableAuditing]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPasswordLength))]
        public string Password { get; set; }

        [Required]
        [EmailAddress]
        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxEmailLength))]
        public string Email { get; set; }

        [DynamicStringLength(typeof(IdentityUserConsts), nameof(IdentityUserConsts.MaxPhoneNumberLength))]
        public string PhoneNumber { get; set; }

        public bool IsActive { get; set; } = true;

        public bool LockoutEnabled { get; set; } = true;

        [Required]
        public UserType UserType { get; set; }
    }

    public class AssignedRoleViewModel
    {
        [Required]
        [HiddenInput]
        public string Name { get; set; }

        public bool IsAssigned { get; set; }

        public bool IsDefault { get; set; }
    }
}
