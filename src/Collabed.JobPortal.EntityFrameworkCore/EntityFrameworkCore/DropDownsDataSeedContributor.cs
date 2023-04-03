using Collabed.JobPortal.DropDowns;
using System.Collections.Generic;
using System.Threading.Tasks;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.Domain.Repositories;

namespace Collabed.JobPortal.EntityFrameworkCore
{
    public class DropDownsDataSeedContributor : IDataSeedContributor, ITransientDependency
    {
        private readonly IRepository<Category> _categoriesRepository;
        private readonly IRepository<Language> _languagesRepository;
        private readonly IRepository<Schedule> _scheduleRepository;
        private readonly IRepository<SupplementalPay> _suppliersPayRepository;
        private readonly IRepository<SupportingDocument> _supportingDocumentRepository;

        public DropDownsDataSeedContributor(IRepository<Category> categoriesRepository, IRepository<Language> languagesRepository, IRepository<Schedule> scheduleRepository, IRepository<SupplementalPay> suppliersPayRepository, IRepository<SupportingDocument> supportingDocumentRepository)
        {
            _categoriesRepository = categoriesRepository;
            _languagesRepository = languagesRepository;
            _scheduleRepository = scheduleRepository;
            _suppliersPayRepository = suppliersPayRepository;
            _supportingDocumentRepository = supportingDocumentRepository;
        }

        public async Task SeedAsync(DataSeedContext context)
        {
            await SeedCategoriesAsync();
            await SeedLanguagesAsync();
            await SeedSchedulesAsync();
            await SeedAsyncSupplementalPaysAsync();
            await SeedSupportingDocumentsAsync();
        }

        private async Task SeedSupportingDocumentsAsync()
        {
            if (await _supportingDocumentRepository.AnyAsync())
            {
                return;
            }

            var supportingDocs = new List<SupportingDocument>
            {
                new SupportingDocument(1, "CV"),
                new SupportingDocument(3, "Cover Letter"),
                new SupportingDocument(5, "Online Portfolio"),
            };

            await _supportingDocumentRepository.InsertManyAsync(supportingDocs);
        }

        private async Task SeedAsyncSupplementalPaysAsync()
        {
            if (await _suppliersPayRepository.AnyAsync())
            {
                return;
            }

            var supplementalPays = new List<SupplementalPay>
            {
                new SupplementalPay(1, "Bonus Scheme"),
                new SupplementalPay(2, "Yearly Scheme"),
                new SupplementalPay(3, "Performance Bonus"),
                new SupplementalPay(4, "Commission Pay"),
                new SupplementalPay(5, "Loyalty Pay")
            };

            await _suppliersPayRepository.InsertManyAsync(supplementalPays);
        }

        private async Task SeedSchedulesAsync()
        {
            if (await _scheduleRepository.AnyAsync())
            {
                return;
            }

            var schedules = new List<Schedule>
            {
                new Schedule(1, "Monday to Friday"),
                new Schedule(2, "Day Shift"),
                new Schedule(3, "Night Shift"),
                new Schedule(4, "Overtime"),
                new Schedule(5, "Weekend Availability"),
                new Schedule(6, "No Weekends"),
                new Schedule(7, "Weekends Only"),
                new Schedule(8, "Flexible"),
                new Schedule(9, "As required")
            };

            await _scheduleRepository.InsertManyAsync(schedules);
        }

        private async Task SeedLanguagesAsync()
        {
            if (await _languagesRepository.AnyAsync())
            {
                return;
            }

            var languages = new List<Language>
            {
                new Language(1, "English"), // English is pre-selected value. Do not change PK
                new Language(2, "French"),
                new Language(3, "German"),
                new Language(4, "Polish"),
                new Language(5, "Spanish")
            };

            await _languagesRepository.InsertManyAsync(languages);
        }

        private async Task SeedCategoriesAsync()
        {
            if (await _categoriesRepository.AnyAsync())
            {
                return;
            }

            var categoriesSeed = new List<Category> {
                new Category(1,"Assessors"),
                new Category(2,"Architecture and Design"),
                new Category(3,"Construction"),
                new Category(4,"Engineering"),
                new Category(5,"Planning"),
                new Category(6,"Surveying")
            };

            await _categoriesRepository.InsertManyAsync(categoriesSeed);
        }
    }
}
