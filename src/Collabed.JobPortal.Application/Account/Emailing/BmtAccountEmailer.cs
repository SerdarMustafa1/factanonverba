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
                BmtAccountEmailTemplates.EmailVerification, new { link = link, includePassword = false });

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

        public async Task SendEmailVerificationInJobApplicationRequestAsync(IdentityUser user, string link, string password)
        {
            var showPsw = false;
            string pswDetailsText = "";

            if (!string.IsNullOrEmpty(password))
            {
                showPsw = true;
                pswDetailsText = $@"
            <p class=""text-16"" style=""line-height: 24px; font-size: 16px; width: 100%; font-weight: 300; color: #475467; margin: 0 0 1rem;"">
                This is your temporary password - {password}
                Sign in and create a permanent password to save your details, manage your personal information and apply for further roles easily without having to re-enter your details.
            </p>
            ";

            }

            var emailContent = await TemplateRenderer.RenderAsync(
                BmtAccountEmailTemplates.EmailVerification, new { link = link, password = pswDetailsText });

            var mailMessage = new MailMessage
            {
                From = new MailAddress(EmailTemplates.WelcomeSender, EmailTemplates.WelcomeTitle),
                Body = emailContent,
                IsBodyHtml = true,
                Subject = EmailTemplates.ConfirmEmailSubject,
            };
            mailMessage.To.Add(user.Email);

            await EmailSender.SendAsync(mailMessage);
        }

        public async Task SendApplicationConfirmationAsync(ApplicationConfirmationDto application)
        {
            var url = await AppUrlProvider.GetUrlAsync("MVC");
            var emailContent = await TemplateRenderer.RenderAsync(
                    BmtAccountEmailTemplates.ApplicationConfirmation, new
                    {
                        link = url + "/JobDashboard",
                        firstname = application.FirstName,
                        lastname = application.LastName,
                        reference = application.JobReference,
                        title = application.JobTitle,
                        company = application.CompanyName
                    });

            var mailMessage = new MailMessage
            {
                From= new MailAddress(EmailTemplates.InfoSender, EmailTemplates.BuildMyTalentTitle),
                Body = emailContent,
                IsBodyHtml = true,
                Subject = EmailTemplates.ApplicationConfirmationSubject + application.JobTitle,
            };
            mailMessage.To.Add(application.Email);

            await EmailSender.SendAsync(mailMessage);
        }

        public async Task SendApplicationEmailToCompanyAsync(ApplicationEmailDto jobApplicationDto, bool isNative)
        {
            var screeningQuestions = ConvertScreeningQuestions(jobApplicationDto.ScreeningQuestions);
            var emailTemplate = "";

            if(isNative)
            {
                emailTemplate = string.IsNullOrEmpty(jobApplicationDto.CompanyName) ? BmtAccountEmailTemplates.NativeApplicationNoCompanyName : BmtAccountEmailTemplates.NativeApplication;
            }
            else
            {
                emailTemplate = string.IsNullOrEmpty(jobApplicationDto.CompanyName) ? BmtAccountEmailTemplates.ThirdPartyApplicationNoCompanyName : BmtAccountEmailTemplates.ThirdPartyApplication;
            }

            var emailContent = await TemplateRenderer.RenderAsync(emailTemplate, new
            {
                firstname = jobApplicationDto.FirstName,
                lastname = jobApplicationDto.LastName,
                phonenumber = jobApplicationDto.PhoneNumber,
                postcode = jobApplicationDto.PostCode,
                companyname = jobApplicationDto.CompanyName,
                jobposition = jobApplicationDto.JobPosition,
                jobreference = jobApplicationDto.JobReference,
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
            mailMessage.To.Add(jobApplicationDto.CompanyEmail);
            mailMessage.ReplyToList.Add(new MailAddress(jobApplicationDto.Email, $"{jobApplicationDto.FirstName} {jobApplicationDto.LastName}"));
            if (!string.IsNullOrWhiteSpace(jobApplicationDto.CvBlobName))
            {
                var blob = await _fileAppService.GetBlobAsync(new GetBlobRequestDto { Name  = jobApplicationDto.CvBlobName });

                using (var stream = new MemoryStream())
                {
                    stream.Write(blob.Content, 0, blob.Content.Length);
                    stream.Seek(0, SeekOrigin.Begin);
                    mailMessage.Attachments.Add(new Attachment(stream, jobApplicationDto.CvFileName, jobApplicationDto.CvContentType));
                    await EmailSender.SendAsync(mailMessage);
                }
                return;
            }

            await EmailSender.SendAsync(mailMessage);
        }

        public async Task SendApplicationRejectionAsync(ApplicantDto rejection)
        {
            var url = await AppUrlProvider.GetUrlAsync("MVC");

            var emailContent = await TemplateRenderer.RenderAsync(
                BmtAccountEmailTemplates.ApplicationRejection, new
                {
                    link = url + "/JobDashboard",
                    firstname = rejection.ApplicantFirstName,
                    jobtitle = rejection.JobTitle,
                    company = rejection.CompanyName
                });

            var mailMessage = new MailMessage
            {
                From= new MailAddress(EmailTemplates.InfoSender, EmailTemplates.BuildMyTalentTitle),
                Body = emailContent,
                IsBodyHtml = true,
                Subject = EmailTemplates.ApplicationUpdateSubject + rejection.JobTitle,
            };

            mailMessage.To.Add(rejection.ApplicantEmail);

            await EmailSender.SendAsync(mailMessage);
        }

        public async Task SendApplicationInterviewAsync(ApplicantDto rejection)
        {
            var emailContent = await TemplateRenderer.RenderAsync(
                BmtAccountEmailTemplates.ApplicationInterview, new
                {
                    firstname = rejection.ApplicantFirstName,
                    jobtitle = rejection.JobTitle,
                    company = rejection.CompanyName
                });

            var mailMessage = new MailMessage
            {
                From= new MailAddress(EmailTemplates.InfoSender, EmailTemplates.BuildMyTalentTitle),
                Body = emailContent,
                IsBodyHtml = true,
                Subject = EmailTemplates.ApplicationUpdateSubject + rejection.JobTitle,
            };

            mailMessage.To.Add(rejection.ApplicantEmail);

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
