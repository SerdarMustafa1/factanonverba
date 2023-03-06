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
                new SupportingDocument(2, "Reference Letter"),
                new SupportingDocument(3, "Cover Letter"),
                new SupportingDocument(4, "Certifications"),
                new SupportingDocument(5, "Portfolio"),
                new SupportingDocument(6, "Other")
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
                new Language(1, "English"),
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
                new Category(1,"Acoustics Consultant"),
                new Category(2,"Architect Part 1"),
                new Category(3,"Architect Part 2"),
                new Category(4,"Architect Part 3"),
                new Category(5,"Architectural Technician"),
                new Category(6,"Architectural Technologist"),
                new Category(7,"Building Control Officer"),
                new Category(8,"Building Services Engineer"),
                new Category(9,"Building Site Inspector"),
                new Category(10,"Building Surveyor"),
                new Category(11,"CAD Technician"),
                new Category(12,"Civil Engineering Technician"),
                new Category(13,"Commercial Energy Assessor"),
                new Category(14,"Conservation Officer"),
                new Category(15,"Construction Contracts Manager"),
                new Category(16,"Construction Manager"),
                new Category(17,"Construction Plant Hire Adviser"),
                new Category(18,"Construction Site Supervisor"),
                new Category(19,"Domestic Energy Assessor"),
                new Category(20,"Engineering Construction Technician"),
                new Category(21,"Estimator"),
                new Category(22,"Exhibition Designer"),
                new Category(23,"Facilities Manager"),
                new Category(24,"Fire Risk Assessor"),
                new Category(25,"Heating and Ventilation Engineer"),
                new Category(26,"Heritage Officer"),
                new Category(27,"Interior Designer"),
                new Category(28,"Land Surveyor"),
                new Category(29,"Landscape Architect"),
                new Category(30,"Mechanical Engineering Technician"),
                new Category(31,"Planner"),
                new Category(32,"Planning and Development Surveyor"),
                new Category(33,"Project Manager"),
                new Category(34,"Rural Surveyor"),
                new Category(35,"Site Manager"),
                new Category(36,"Structural Engineer"),
                new Category(37,"Surveyor"),
                new Category(38,"Technical Surveyor"),
                new Category(39,"Thermal Insulation Engineer"),
                new Category(40,"Town Planner"),
                new Category(41,"Urban Planner")
            };

            await _categoriesRepository.InsertManyAsync(categoriesSeed);
        }
    }
}
