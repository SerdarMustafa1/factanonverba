using Collabed.JobPortal.Account.Emailing.Templates;
using Collabed.JobPortal.Applications;
using Collabed.JobPortal.BlobStorage;
using Collabed.JobPortal.Email;
using Collabed.JobPortal.Jobs;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Volo.Abp.Account.Emailing;
using Volo.Abp.Account.Emailing.Templates;
using Volo.Abp.Account.Localization;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Emailing;
using Volo.Abp.Identity;
using Volo.Abp.MultiTenancy;
using Volo.Abp.TextTemplating;
using Volo.Abp.UI.Navigation.Urls;

namespace Collabed.JobPortal.Account.Emailing
{
    [Dependency(ReplaceServices = true)]
    [ExposeServices(typeof(IAccountEmailer), typeof(IBmtAccountEmailer))]
    public class BmtAccountEmailer : AccountEmailer, IBmtAccountEmailer
    {
        private readonly IFileAppService _fileAppService;
        public BmtAccountEmailer(
            IEmailSender emailSender,
            ITemplateRenderer templateRenderer,
            IStringLocalizer<AccountResource> stringLocalizer,
            IAppUrlProvider appUrlProvider,
            ICurrentTenant currentTenant,
            IFileAppService fileAppService) : base(emailSender, templateRenderer, stringLocalizer, appUrlProvider, currentTenant)
        {
            _fileAppService = fileAppService;
        }

        public async Task SendEmailVerificationRequestAsync(IdentityUser user, string link)
        {
            var emailContent = await TemplateRenderer.RenderAsync(
                BmtAccountEmailTemplates.EmailVerification, new { link = link });

            var mailMessage = new MailMessage
            {
                From= new MailAddress(EmailTemplates.WelcomeSender, EmailTemplates.WelcomeTitle),
                Body = emailContent,
                IsBodyHtml = true,
                Subject = EmailTemplates.ConfirmEmailSubject,
            };
            mailMessage.To.Add(user.Email);

            await EmailSender.SendAsync(mailMessage);
        }

        public async Task SendApplicationEmailToThirdPartyAsync(ThirdPartyJobApplicationDto jobApplicationDto, string aplitrakEmailAddress)
        {
            var screeningQuestions = ConvertScreeningQuestions(jobApplicationDto.ScreeningQuestions);

            var emailContent = await TemplateRenderer.RenderAsync(
                BmtAccountEmailTemplates.ThirdPartyApplication, new
                {
                    firstname = jobApplicationDto.FirstName,
                    lastname = jobApplicationDto.LastName,
                    phonenumber = jobApplicationDto.PhoneNumber,
                    postcode = jobApplicationDto.PostCode,
                    companyname = jobApplicationDto.CompanyName,
                    jobposition = jobApplicationDto.JobPosition,
                    cvfilename = jobApplicationDto.CvFileName,
                    coverletter = jobApplicationDto.CoverLetter,
                    portfoliolink = jobApplicationDto.PortfolioLink,
                    email = jobApplicationDto.Email,
                    questionsandanswers = screeningQuestions
                });

            var mailMessage = new MailMessage
            {
                From= new MailAddress(EmailTemplates.InfoSender),
                Body = emailContent,
                IsBodyHtml = true,
                Subject = EmailTemplates.JobApplicationSubject,
            };
            mailMessage.To.Add(aplitrakEmailAddress);
            mailMessage.ReplyToList.Add(new MailAddress(jobApplicationDto.Email, $"{jobApplicationDto.FirstName} {jobApplicationDto.LastName}"));
            var blob = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name  = jobApplicationDto.CvBlobName });

            using var stream = new MemoryStream();
            stream.Write(blob.Content, 0, blob.Content.Length);
            stream.Seek(0, SeekOrigin.Begin);
            mailMessage.Attachments.Add(new Attachment(stream, blob.Name, jobApplicationDto.ContentType));

            await EmailSender.SendAsync(mailMessage);
        }

        private static string ConvertScreeningQuestions(IEnumerable<ScreeningQuestionDto> screeningQuestions)
        {
            var screeningQuestionsString = new StringBuilder();
            foreach (var question in screeningQuestions)
            {
                screeningQuestionsString.Append(question.Text);
                screeningQuestionsString.Append(": ");
                screeningQuestionsString.Append(question.Answer);
                if (question.AutoRejectAnswer.HasValue)
                {
                    if (question.AutoRejectAnswer.Value == question.Answer)
                    {
                        screeningQuestionsString.Append(" [No Match]");
                    }
                    else
                    {
                        screeningQuestionsString.Append(" [Match]");
                    }
                }
                screeningQuestionsString.Append(" <br> ");
            }
            return screeningQuestionsString.ToString();
        }

        public override async Task SendPasswordResetLinkAsync(
        IdentityUser user,
        string resetToken,
        string appName,
        string returnUrl = null,
        string returnUrlHash = null)
        {
            Debug.Assert(CurrentTenant.Id == user.TenantId, "This method can only work for current tenant!");

            var url = await AppUrlProvider.GetResetPasswordUrlAsync(appName);

            //TODO: Use AbpAspNetCoreMultiTenancyOptions to get the key
            var link = $"{url}?userId={user.Id}&{TenantResolverConsts.DefaultTenantKey}={user.TenantId}&resetToken={UrlEncoder.Default.Encode(resetToken)}";

            if (!returnUrl.IsNullOrEmpty())
            {
                link += "&returnUrl=" + NormalizeReturnUrl(returnUrl);
            }

            if (!returnUrlHash.IsNullOrEmpty())
            {
                link += "&returnUrlHash=" + returnUrlHash;
            }

            var emailContent = await TemplateRenderer.RenderAsync(
                AccountEmailTemplates.PasswordResetLink,
                new { link = link, email = user.Email }
            );

            await EmailSender.SendAsync(
                user.Email,
                StringLocalizer["PasswordReset"],
                emailContent
            );
        }
    }
}
