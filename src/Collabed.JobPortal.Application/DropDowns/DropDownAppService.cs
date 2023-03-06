using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Services;
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


        public DropDownAppService(ILogger<DropDownAppService> logger, IRepository<Category> categoriesRepository, IRepository<Language> languagesRepository, IRepository<Location> locationsRepository, IRepository<Schedule> schedulesRepository, IRepository<SupplementalPay> supplementalPaysRepository, IRepository<SupportingDocument> supportingDocumentsRepository)
        {
            _logger = logger;
            _categoriesRepository = categoriesRepository;
            _languagesRepository = languagesRepository;
            _locationsRepository = locationsRepository;
            _schedulesRepository = schedulesRepository;
            _supplementalPaysRepository = supplementalPaysRepository;
            _supportingDocumentsRepository = supportingDocumentsRepository;
        }

        public async Task<IEnumerable<DropDownDto>> GetCategoriesAsync()
        {
            return (await _categoriesRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name));
        }

        public async Task<IEnumerable<DropDownDto>> GetJobSchedulesAsync()
        {
            return (await _schedulesRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name));
        }

        public async Task<IEnumerable<DropDownDto>> GetSupplementalPaysAsync()
        {
            return (await _supplementalPaysRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name));
        }

        public async Task<IEnumerable<DropDownDto>> GetSupporitngDocumentsAsync()
        {
            return (await _supportingDocumentsRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name));
        }

        public async Task<IEnumerable<DropDownDto>> GetLanguagesAsync()
        {
            return (await _languagesRepository.GetListAsync()).Select(x => new DropDownDto(x.Id, x.Name));
        }

        public async Task<IEnumerable<DropDownDto>> GetLocationsAsync(string searchTerm)
        {
            return (await _locationsRepository.GetListAsync(x => x.Name.StartsWith(searchTerm))).Select(x => new DropDownDto(x.Id, x.Name));
        }
    }
}
