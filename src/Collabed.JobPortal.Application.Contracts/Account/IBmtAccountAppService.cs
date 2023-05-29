using Collabed.JobPortal.Account;
using Collabed.JobPortal.User;
using System;
using System.IO;
using System.Threading.Tasks;
using Volo.Abp.Account;

namespace Collabed.JobPortal.Users
{
    public interface IBmtAccountAppService : IAccountAppService
    {
        Task SendEmailVerificationRequestAsync(SendEmailVerificationDto input);
        Task<bool> CheckIfEmailExistsAsync(string emailAddress);
        Task<bool> CheckIfUsernameExistsAsync(string userName);
        Task<bool> CheckPasswordCredentials(string loginInput, string password);
        Task<UserProfileDto> GetLoggedUserProfileAsync();
        Task UploadCvToUserProfile(Guid UserId, Stream fileStream, string fileName, string contentType);
        Task<UserProfileDto> GetUserProfileByIdAsync(Guid userId);
        Task UpdateUserProfileAsync(UpdateUserProfileDto updateProfileDto);
        Task DeleteUserAsync(Guid userId);
        Task<UserType> GetUserTypeByIdAsync(Guid userId);
    }
}
