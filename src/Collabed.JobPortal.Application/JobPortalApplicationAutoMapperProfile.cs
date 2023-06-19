using AutoMapper;
using Collabed.JobPortal.Account;
using Collabed.JobPortal.Applications;
using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Job;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.PaymentRequests;
using Collabed.JobPortal.Types;
using Collabed.JobPortal.Users;
using System;
using System.Collections.Generic;
using System.Linq;
using Volo.Abp.AutoMapper;

namespace Collabed.JobPortal;

public class JobPortalApplicationAutoMapperProfile : Profile
{
    public JobPortalApplicationAutoMapperProfile()
    {
        /* You can configure your AutoMapper mapping configuration here.
         * Alternatively, you can split your mapping configurations
         * into multiple profile classes for a better organization. */
        CreateMap<Jobs.Job, JobDto>()
            .ForMember(d => d.SalaryMinimum,
                    op => op.MapFrom(o => o.SalaryFrom))
            .ForMember(d => d.SalaryMaximum,
                    op => op.MapFrom(o => o.SalaryTo));
        CreateMap<JobWithDetails, JobDto>()
            .ForMember(d => d.PublishedDate,
                    op => op.MapFrom(o => o.CreationTime))
            .ForMember(d => d.SalaryMinimum,
                    op => op.MapFrom(o => o.SalaryFrom))
            .ForMember(d => d.SalaryMaximum,
                    op => op.MapFrom(o => o.SalaryTo))
            .ForMember(d => d.ContractType,
                    op => op.MapFrom(o => o.Type.HasValue ? o.Type.Value.GetName() : null))
            .ForMember(d => d.EmploymentType,
                    op => op.MapFrom(o => o.EmploymentType.HasValue ? o.EmploymentType.Value.GetName() : null))
            .ForMember(d => d.SalaryPeriod,
                    op => op.MapFrom(o => o.SalaryPeriod.HasValue ? o.SalaryPeriod.Value.GetName() : null))
            .ForMember(d => d.JobLocation,
                    op => op.MapFrom(o => o.JobLocation.HasValue ? o.JobLocation.Value.GetName() : null))
            .ForMember(d => d.ExperienceLevel,
                    op => op.MapFrom(o => o.ExperienceLevel.HasValue ? o.ExperienceLevel.Value.GetName() : null))
            .ForMember(d => d.OrganisationName,
                    op => op.MapFrom(o => !string.IsNullOrEmpty(o.OrganisationName) ? o.OrganisationName : o.CompanyName));
        CreateMap<JobApplicant, JobApplicationDto>()
            .ForMember(d => d.FirstName,
                    op => op.Ignore())
            .ForMember(d => d.LastName,
                    op => op.Ignore())
            .ForMember(d => d.Email,
                    op => op.Ignore())
            .ForMember(d => d.PhoneNumber,
                    op => op.Ignore());
        CreateMap<JobDto, CreateUpdateJobDto>();
        CreateMap<Organisation, OrganisationDto>();
        CreateMap<PaymentRequest, PaymentRequestDto>();
        CreateMap<CreateJobDto, Jobs.Job>()
            .ForMember(d => d.SalaryFrom,
                    op => op.MapFrom(o => o.SalaryMinimum))
            .ForMember(d => d.SalaryTo,
                    op => op.MapFrom(o => o.SalaryMaximum))
            .ForMember(d => d.SalaryBenefits,
                    op => op.MapFrom(o => o.SalaryOtherBenefits))
            .ForMember(d => d.Type,
                    op => op.MapFrom(o => o.ContractType))
            .ForMember(d => d.SalaryPeriod,
                    op => op.MapFrom(o => o.PaymentOption))
            .ForMember(d => d.Schedules,
                    op => op.Ignore())
            .ForMember(d => d.SupportingDocuments,
                    op => op.Ignore())
            .ForMember(d => d.ScreeningQuestions,
                    op => op.Ignore())
            .ForMember(d => d.ApplicationDeadline,
                    op => op.MapFrom(o => o.ApplicationDeadline.HasValue ? o.ApplicationDeadline : DateTime.UtcNow.AddDays(JobConsts.DefaultJobDuration)));
        CreateMap<ExternalJobRequest, Jobs.Job>()
            .ForMember(d => d.Type,
                    op => op.MapFrom(o => MapJobType(o.Type)))
            .ForMember(d => d.CategoryId,
                    op => op.MapFrom(o => o.CategoryId))
            .ForMember(d => d.SalaryPeriod,
                    op => op.MapFrom(o => MapSalaryPeriodType(o.SalaryPeriod)))
            .ForMember(d => d.SalaryCurrency,
                    op => op.MapFrom(o => MapCurrency(o.SalaryCurrency)))
            .ForMember(d => d.ApplicationDeadline,
                    op => op.MapFrom(o => DateTime.UtcNow.AddDays(o.DaysToAdvertise)))
            .ForMember(d => d.Id, op => op.Ignore())
            .ForMember(d => d.StartDateText,
                    op => op.MapFrom(o => o.StartDate))
            .ForMember(d => d.StartDate, op => op.Ignore())
            .ForMember(d => d.ScreeningQuestions, op => op.Ignore())
            .IgnoreAuditedObjectProperties();
        CreateMap<AdzunaJobResult, Jobs.Job>()
            .ForMember(d => d.Type,
                    op => op.MapFrom(o => MapAdzunaJobType(o.ContractType)))
            .ForMember(d => d.EmploymentType,
                    op => op.MapFrom(o => MapEmploymentType(o.ContractTime)))
            .ForMember(d => d.SalaryPeriod,
                    op => op.MapFrom(o => (SalaryPeriod?)SalaryPeriod.Annually))
            .ForMember(d => d.SalaryCurrency,
                    op => op.MapFrom(o => CurrencyType.GBP))
            .ForMember(d => d.ApplicationDeadline,
                    op => op.MapFrom(o => DateTime.UtcNow.AddDays(30)))
            .ForMember(d => d.CompanyName,
                    op => op.MapFrom(o => o.Company != null ? o.Company.Name : default))
            .ForMember(d => d.IsSalaryEstimated,
                    op => op.MapFrom(o => o.IsSalaryEstimated == "1"))
            .ForMember(d => d.Id, op => op.Ignore())
            .ForMember(d => d.StartDate, op => op.Ignore())
            .ForMember(d => d.ScreeningQuestions, op => op.Ignore())
            .ForMember(d => d.JobLocation, op => op.Ignore())
            .IgnoreAuditedObjectProperties();
        CreateMap<ApplicationDto, ApplicationEmailDto>()
            .ForMember(d => d.CompanyName, op => op.Ignore())
            .ForMember(d => d.JobPosition, op => op.Ignore());
        CreateMap<SupportingDocument, SupportingDocumentDto>();
        CreateMap<ScreeningQuestion, ScreeningQuestionDto>()
            .ForMember(d => d.Text,
                    op => op.MapFrom(o => o.Name));
        CreateMap<UserProfile, UserProfileDto>();
        CreateMap<Jobs.Job, JobSummaryDto>()
            .ForMember(d => d.ApplicationsCount,
                    op => op.MapFrom(o => MapApplications(o.Applicants, TabName.Applications)))
            .ForMember(d => d.InProcessCount,
                    op => op.MapFrom(o => MapApplications(o.Applicants, TabName.InProcess)))
            .ForMember(d => d.HiredCount,
                    op => op.MapFrom(o => MapApplications(o.Applicants, TabName.Hired)))
            .ForMember(d => d.DaysLeft,
                    op => op.MapFrom(o => MapDaysLeft(o.ApplicationDeadline, o.Status)))
        .ForMember(d => d.Status,
                    op => op.MapFrom(o => Enum.GetName(typeof(JobStatus), o.Status)));
    }
    public static ContractType? MapJobType(string jobType)
    {
        if (string.IsNullOrEmpty(jobType))
            return null;

        Enum.TryParse(jobType, out ContractType jobTypeEnum);
        return jobTypeEnum;
    }
    public static ContractType? MapAdzunaJobType(string jobType)
    {
        if (string.IsNullOrEmpty(jobType))
            return null;

        if (jobType == "permanent")
            return ContractType.Permanent;

        if (jobType == "contract")
            return ContractType.Contract;

        return null;
    }
    public static EmploymentType? MapEmploymentType(string employmentType)
    {
        if (string.IsNullOrEmpty(employmentType))
            return null;

        if (employmentType == "full_time")
            return EmploymentType.Fulltime;
        if (employmentType == "part_time")
            return EmploymentType.Parttime;

        return null;
    }

    public static SalaryPeriod? MapSalaryPeriodType(string salaryPeriodType)
    {
        if (string.IsNullOrEmpty(salaryPeriodType))
            return null;

        return salaryPeriodType switch
        {
            "hour" => (SalaryPeriod?)SalaryPeriod.Hourly,
            "day" => (SalaryPeriod?)SalaryPeriod.Daily,
            "week" => (SalaryPeriod?)SalaryPeriod.Weekly,
            "month" => (SalaryPeriod?)SalaryPeriod.Monthly,
            "annum" => (SalaryPeriod?)SalaryPeriod.Annually,
            _ => null,
        };
    }

    public static CurrencyType MapCurrency(string currency)
    {
        Enum.TryParse(currency, out CurrencyType currencyEnum);
        return currencyEnum;
    }

    public static int MapApplications(List<JobApplicant> applicants, TabName tabName)
    {
        if (!applicants.Any())
            return 0;

        return tabName switch
        {
            TabName.Applications => applicants.Count,
            TabName.InProcess => applicants.Where(x => x.ApplicationStatus == ApplicationStatus.Interview || x.ApplicationStatus == ApplicationStatus.Review).Count(),
            TabName.Hired => applicants.Where(x => x.ApplicationStatus == ApplicationStatus.Hired).Count(),
            _ => applicants.Count,
        };
    }

    public static string MapDaysLeft(DateTime deadline, JobStatus status)
    {
        if (status == JobStatus.Deleted || status == JobStatus.Closed)
            return "-";

        var daysLeft = (deadline - DateTime.Today).Days + 1;
        if (daysLeft > 1)
            return $"{daysLeft} days";
        if (daysLeft == 1)
            return $"{daysLeft} day";

        return "-";
    }

    public enum TabName
    {
        Applications,
        InProcess,
        Hired
    }
}
