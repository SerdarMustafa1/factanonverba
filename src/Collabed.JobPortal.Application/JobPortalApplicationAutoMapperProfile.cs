using AutoMapper;
using Collabed.JobPortal.Job;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.PaymentRequests;
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
		CreateMap<Jobs.Job, JobDto>();
		CreateMap<JobDto, CreateUpdateJobDto>();
		CreateMap<Organisation, OrganisationDto>();
		CreateMap<PaymentRequest, PaymentRequestDto>();
		CreateMap<ExternalJobRequest, Jobs.Job>()
			  .ForMember(d => d.Type,
						op => op.MapFrom(o => MapJobType(o.Type)))
			  .ForMember(d => d.SalaryPeriod,
						op => op.MapFrom(o => MapSalaryPeriodType(o.SalaryPeriod)))
			  .ForMember(d => d.SalaryCurrency,
						op => op.MapFrom(o => MapCurrency(o.SalaryCurrency)))
			  .ForMember(d => d.JobOrigin, op => op.Ignore())
			  .ForMember(d => d.OrganisationId, op => op.Ignore())
			  .ForMember(d => d.Applicants, op => op.Ignore())
			  .ForMember(d => d.Id, op => op.Ignore())
			  .IgnoreAuditedObjectProperties();
	}
	public static JobType MapJobType(string jobType)
	{
		Enum.TryParse(jobType, out JobType jobTypeEnum);
		return jobTypeEnum;
	}
	public static SalaryPeriodType MapSalaryPeriodType(string salaryPeriodType)
	{
		Enum.TryParse(salaryPeriodType, out SalaryPeriodType salaryPeriodTypeEnum);
		return salaryPeriodTypeEnum;
	}
	public static CurrencyType MapCurrency(string currency)
	{
		Enum.TryParse(currency, out CurrencyType currencyEnum);
		return currencyEnum;
	}
}
