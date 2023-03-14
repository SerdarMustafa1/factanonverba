using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
using Volo.Abp.Caching;
using Volo.Abp.Domain.Repositories;

namespace Collabed.JobPortal.DropDowns
{
    public class DropDownAppService : ApplicationService
    {
        private readonly ILogger<DropDownAppService> _logger;
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<Language> _languagesRepository;
        private readonly IRepository<Location> _locationsRepository;
        private readonly IRepository<Schedule> _schedulesRepository;
        private readonly IRepository<SupplementalPay> _supplementalPaysRepository;
        private readonly IRepository<SupportingDocument> _supportingDocumentsRepository;
        private readonly IDistributedCache<IEnumerable<DropDownDto>> _dropDownCache;


        public DropDownAppService(ILogger<DropDownAppService> logger, IRepository<Category> categoriesRepository, IRepository<Language> languagesRepository, IRepository<Location> locationsRepository, IRepository<Schedule> schedulesRepository, IRepository<SupplementalPay> supplementalPaysRepository, IRepository<SupportingDocument> supportingDocumentsRepository, IDistributedCache<IEnumerable<DropDownDto>> dropDownCache)
        {
            _logger = logger;
            _categoriesRepository = categoriesRepository;
            _languagesRepository = languagesRepository;
            _locationsRepository = locationsRepository;
            _schedulesRepository = schedulesRepository;
            _supplementalPaysRepository = supplementalPaysRepository;
            _supportingDocumentsRepository = supportingDocumentsRepository;
            _dropDownCache = dropDownCache;
        }

        public async Task<IEnumerable<DropDownDto>> GetCategoriesAsync()
        {
            return await _dropDownCache.GetOrAddAsync("categories",
                async () => (await _categoriesRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                }
            );
        }

        public async Task<IEnumerable<DropDownDto>> GetJobSchedulesAsync()
        {
            return await _dropDownCache.GetOrAddAsync("jobSchedules",
                async () => (await _schedulesRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                }
            );
        }

        public async Task<IEnumerable<DropDownDto>> GetSupplementalPaysAsync()
        {
            return await _dropDownCache.GetOrAddAsync("supplementalPays",
                async () => (await _supplementalPaysRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                }
            );
        }

        public async Task<IEnumerable<DropDownDto>> GetSupporitngDocumentsAsync()
        {
            return await _dropDownCache.GetOrAddAsync("supportingDocuments",
                async () => (await _supportingDocumentsRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                }
            );
        }

        public async Task<IEnumerable<DropDownDto>> GetLanguagesAsync()
        {
            return await _dropDownCache.GetOrAddAsync("languages",
                async () => (await _languagesRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                }
            );
        }

        public async Task<IEnumerable<DropDownDto>> GetLocationsBySearchTermAsync(string searchTerm)
        {
            return await _dropDownCache.GetOrAddAsync("locations_"+searchTerm,
                async () => (await _locationsRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddDays(30),
                }
            );
        }

        public async Task<IEnumerable<DropDownDto>> GetLocationsAsync()
        {
            return await _dropDownCache.GetOrAddAsync("locations",
                async () => (await _locationsRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name)),
                () => new DistributedCacheEntryOptions
                {
                    AbsoluteExpiration = DateTimeOffset.Now.AddYears(1), // This list will never change
                }
            );
        }
    }
}
