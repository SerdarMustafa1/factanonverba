﻿using AutoMapper;
using Collabed.JobPortal.Job;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.PaymentRequests;
using Collabed.JobPortal.Types;
using System;
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
                    op => op.MapFrom(o => o.ExperienceLevel.HasValue ? o.ExperienceLevel.Value.GetName() : null));
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
    }
    public static ContractType? MapJobType(string jobType)
    {
        if (string.IsNullOrEmpty(jobType))
            return null;

        Enum.TryParse(jobType, out ContractType jobTypeEnum);
        return jobTypeEnum;
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
}
