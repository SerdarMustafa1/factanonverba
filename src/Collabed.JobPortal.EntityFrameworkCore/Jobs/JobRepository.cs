using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.EntityFrameworkCore;
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
                //.Join(dbContext.Set<Organisations.Organisation>(), job => job.OrganisationId, org => org.Id, (job, organisation) => new { job, organisation )
                //.Join(dbContext.Set<Language>(), j => j.job.LocalLanguageId, lang => lang.Id, (job, lang) => new { job.job, job.organisation, lang })
                //.Join(dbContext.Set<Location>(), j => j.job.OfficeLocationId, location => location.Id, (job, location) => new { job.job, job.organisation, job.lang, location })
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
                    //OrganisationName = x.organisation != null ? x.organisation.Name : null,
                    //LocalLanguage = x.lang != null ? x.lang.Name : null,
                    //OfficeLocation = x.location != null ? x.location.Name : null
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
