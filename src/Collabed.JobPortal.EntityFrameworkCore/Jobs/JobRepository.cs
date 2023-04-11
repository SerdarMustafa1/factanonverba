using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.EntityFrameworkCore;
using Collabed.JobPortal.Types;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Dynamic.Core;
using System.Threading;
using System.Threading.Tasks;
using Volo.Abp.Domain.Repositories.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore;

namespace Collabed.JobPortal.Jobs
{
    public class JobRepository : EfCoreRepository<JobPortalDbContext, Job, Guid>, IJobRepository
    {
        public JobRepository(IDbContextProvider<JobPortalDbContext> dbContextProvider) : base(dbContextProvider)
        {
        }

        public async Task<List<Job>> GetListAsync(string sorting, int skipCount, int maxResultCount, CancellationToken cancellationToken = default)
        {
            var query = await ApplyFilterAsync();

            return await query
                .OrderBy(!string.IsNullOrWhiteSpace(sorting) ? sorting : nameof(Job.Title))
                .PageBy(skipCount, maxResultCount)
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<List<JobWithDetails>> GetListBySearchCriteriaAsync(string sorting, int skipCount, int maxResultCount, int categoryId,
            string keyword, bool locationsFound, (decimal? lat, decimal? lon) location, int? searchRadius, bool? netZero, ContractType? contractType, EmploymentType? employmentType,
            JobLocation? workplace, int? salaryMinimum, int? salaryMaximum, CancellationToken cancellationToken = default)
        {
            var context = await GetDbContextAsync();
            var query = from job in context.Set<Job>()
                        join org in context.Set<Organisations.Organisation>()
                            on job.OrganisationId equals org.Id into grouping
                        from org in grouping.DefaultIfEmpty()
                        join loc in context.Set<Location>()
                           on job.OfficeLocationId equals loc.Id into grouping2
                        from loc in grouping2.DefaultIfEmpty()
                        join lang in context.Set<Language>()
                           on job.LocalLanguageId equals lang.Id into grouping3
                        from lang in grouping3.DefaultIfEmpty()
                        select new { job, org, loc, lang };


            query = query.Where(x => x.job.CategoryId == categoryId);

            if (netZero.HasValue)
            {
                query = query.Where(x => x.job.IsNetZeroCompliant == netZero.Value);
            }
            if (contractType.HasValue)
            {
                query = query.Where(x => x.job.Type == contractType.Value);
            }
            if (employmentType.HasValue)
            {
                query = query.Where(x => x.job.EmploymentType == employmentType.Value);
            }
            if (workplace.HasValue)
            {
                query = query.Where(x => x.job.JobLocation == workplace.Value);
            }
            if (salaryMinimum.HasValue)
            {
                query = query.Where(x => x.job.MaxSalaryConverted.HasValue && x.job.MaxSalaryConverted >= salaryMinimum.Value);

                if (!salaryMaximum.HasValue)
                {
                    query = query.Where(x => x.job.MinSalaryConverted.HasValue && x.job.MinSalaryConverted >= salaryMinimum.Value);
                }
            }
            if (salaryMaximum.HasValue)
            {
                query = query.Where(x => x.job.MinSalaryConverted.HasValue && x.job.MinSalaryConverted < salaryMaximum.Value);
            }
            if (locationsFound && searchRadius.HasValue && searchRadius.Value > 0)
            {
                query = query.Where(x => context.CalcDistanceMiles(location.lat.Value, location.lon.Value, x.loc.Latitude, x.loc.Longitude) <= searchRadius.Value);
            }
            if (!string.IsNullOrEmpty(keyword))
            {
                var fuzzySearchRatio = Convert.ToInt32(keyword.Length*0.8);
                query = query.Where(x => context.FuzzyMatchString(x.job.Title, keyword) >= fuzzySearchRatio);
            }

            return await query
                //.OrderBy(!string.IsNullOrWhiteSpace(sorting) ? sorting : nameof(Job.Title))
                .PageBy(skipCount, maxResultCount)
                .Select(x => new JobWithDetails
                {
                    Reference = x.job.Reference,
                    Title = x.job.Title,
                    Description = x.job.Description,
                    SubDescription = x.job.SubDescription,
                    Skills = x.job.Skills,
                    StartDate = x.job.StartDate,
                    StartDateText = x.job.StartDateText,
                    OfferVisaSponsorship = x.job.OfferVisaSponsorship,
                    SalaryFrom = x.job.SalaryFrom,
                    SalaryTo = x.job.SalaryTo,
                    SalaryBenefits = x.job.SalaryBenefits,
                    ApplicationDeadline = x.job.ApplicationDeadline,
                    SupplementalPay = x.job.SupplementalPay,
                    JobOrigin = x.job.JobOrigin,
                    Status = x.job.Status,
                    CreationTime = x.job.CreationTime,
                    Type = x.job.Type,
                    EmploymentType = x.job.EmploymentType,
                    SalaryPeriod = x.job.SalaryPeriod,
                    JobLocation = x.job.JobLocation,
                    ExperienceLevel = x.job.ExperienceLevel,
                    CompanyName = x.job.CompanyName,
                    IsNetZeroCompliant = x.job.IsNetZeroCompliant,
                    OrganisationId = x.job.OrganisationId,
                    LocalLanguageId = x.job.LocalLanguageId,
                    OfficeLocationId = x.job.OfficeLocationId,
                    OrganisationName = x.org != null ? x.org.Name : null,
                    LocalLanguage = x.lang != null ? x.lang.Name : null,
                    OfficeLocation = x.loc != null ? x.loc.Name : null
                })
                .ToListAsync(GetCancellationToken(cancellationToken));
        }

        public async Task<Job> GetByReferenceAsync(string reference)
        {
            var query = await ApplyFilterAsync();
            return await query.FirstOrDefaultAsync(x => x.Reference == reference);
        }

        public async Task<JobWithDetails> GetWithDetailsByReferenceAsync(string reference)
        {
            var dbContext = await GetDbContextAsync();
            var query = await ApplyFilterAsync();

            var result = await query
                .Select(x => new JobWithDetails
                {
                    Reference = x.Reference,
                    Title = x.Title,
                    Description = x.Description,
                    SubDescription = x.SubDescription,
                    Skills = x.Skills,
                    StartDate = x.StartDate,
                    StartDateText = x.StartDateText,
                    OfferVisaSponsorship = x.OfferVisaSponsorship,
                    SalaryFrom = x.SalaryFrom,
                    SalaryTo = x.SalaryTo,
                    SalaryBenefits = x.SalaryBenefits,
                    ApplicationDeadline = x.ApplicationDeadline,
                    SupplementalPay = x.SupplementalPay,
                    JobOrigin = x.JobOrigin,
                    Status = x.Status,
                    CreationTime = x.CreationTime,
                    Type = x.Type,
                    EmploymentType = x.EmploymentType,
                    SalaryPeriod = x.SalaryPeriod,
                    JobLocation = x.JobLocation,
                    ExperienceLevel = x.ExperienceLevel,
                    CompanyName = x.CompanyName,
                    IsNetZeroCompliant = x.IsNetZeroCompliant,
                    OrganisationId = x.OrganisationId,
                    LocalLanguageId = x.LocalLanguageId,
                    OfficeLocationId = x.OfficeLocationId
                })
                .FirstOrDefaultAsync(x => x.Reference == reference);

            if (result.OrganisationId.HasValue)
            {
                var organisationDbSet = (await GetDbContextAsync()).Set<Organisations.Organisation>();
                result.OrganisationName = (await organisationDbSet.FirstOrDefaultAsync(x => x.Id == result.OrganisationId.Value)).Name;
            }
            if (result.LocalLanguageId.HasValue)
            {
                var localLanguageDbSet = (await GetDbContextAsync()).Set<Language>();
                result.LocalLanguage = (await localLanguageDbSet.FirstOrDefaultAsync(x => x.Id == result.LocalLanguageId.Value)).Name;
            }
            if (result.OfficeLocationId.HasValue)
            {
                var locationDbSet = (await GetDbContextAsync()).Set<Location>();
                result.OfficeLocation = (await locationDbSet.FirstOrDefaultAsync(x => x.Id == result.OfficeLocationId.Value)).Name;
            }

            return result;
        }

        public async Task DeleteByReferenceAsync(string reference)
        {
            var query = await ApplyFilterAsync();
            var entity = await query.FirstOrDefaultAsync(x => x.Reference == reference);
            await DeleteAsync(entity);
        }

        public async Task<bool> CheckIfJobExistsByReference(string reference)
        {
            var query = await ApplyFilterAsync();
            return await query.AnyAsync(x => x.Reference == reference);
        }

        private async Task<IQueryable<Job>> ApplyFilterAsync()
        {
            var dbContext = await GetDbContextAsync();

            return await GetDbSetAsync();
        }
    }
}
