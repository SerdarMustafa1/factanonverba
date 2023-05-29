using System.Threading.Tasks;

namespace Collabed.JobPortal.Account
{
    public interface IUserProfileAppService
    {
        Task<ProfileDto> GetAsync();
        Task<ProfileDto> UpdateAsync(UpdateBaseProfileDto input);
        Task ChangePasswordAsync(ChangePasswordInput input);
    }
}
