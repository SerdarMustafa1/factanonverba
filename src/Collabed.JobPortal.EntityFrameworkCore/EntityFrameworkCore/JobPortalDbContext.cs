using Collabed.JobPortal.DropDowns;
using Collabed.JobPortal.Jobs;
using Collabed.JobPortal.Organisations;
using Collabed.JobPortal.PaymentRequests;
using Microsoft.EntityFrameworkCore;
using Volo.Abp.AuditLogging.EntityFrameworkCore;
using Volo.Abp.BackgroundJobs.EntityFrameworkCore;
using Volo.Abp.Data;
using Volo.Abp.DependencyInjection;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.EntityFrameworkCore.Modeling;
using Volo.Abp.FeatureManagement.EntityFrameworkCore;
using Volo.Abp.Identity;
using Volo.Abp.Identity.EntityFrameworkCore;
using Volo.Abp.OpenIddict.EntityFrameworkCore;
using Volo.Abp.PermissionManagement.EntityFrameworkCore;
using Volo.Abp.SettingManagement.EntityFrameworkCore;

namespace Collabed.JobPortal.EntityFrameworkCore;

[ReplaceDbContext(typeof(IIdentityDbContext))]
[ConnectionStringName("Default")]
public class JobPortalDbContext :
    AbpDbContext<JobPortalDbContext>,
    IIdentityDbContext
{

    #region Entities from the modules

    /* Notice: We only implemented IIdentityDbContext and ITenantManagementDbContext
     * and replaced them for this DbContext. This allows you to perform JOIN
     * queries for the entities of these modules over the repositories easily. You
     * typically don't need that for other modules. But, if you need, you can
     * implement the DbContext interface of the needed module and use ReplaceDbContext
     * attribute just like IIdentityDbContext and ITenantManagementDbContext.
     *
     * More info: Replacing a DbContext of a module ensures that the related module
     * uses this DbContext on runtime. Otherwise, it will use its own DbContext class.
     */

    //Identity
    public DbSet<IdentityUser> Users { get; set; }
    public DbSet<IdentityRole> Roles { get; set; }
    public DbSet<IdentityClaimType> ClaimTypes { get; set; }
    public DbSet<OrganizationUnit> OrganizationUnits { get; set; }
    public DbSet<IdentitySecurityLog> SecurityLogs { get; set; }
    public DbSet<IdentityLinkUser> LinkUsers { get; set; }
    #endregion

    //Job board
    public DbSet<Jobs.Job> Jobs { get; set; }
    public DbSet<JobApplicant> JobApplicants { get; set; }
    public DbSet<Organisations.Organisation> Organisations { get; set; }
    public DbSet<OrganisationMember> OrganisationMembers { get; set; }
    public DbSet<PaymentRequest> PaymentRequests { get; set; }
    public DbSet<Category> Categories { get; set; }
    public DbSet<JobCategory> JobCategories { get; set; }
    public DbSet<Language> Languages { get; set; }
    public DbSet<JobLanguage> JobLanguages { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<JobSchedule> JobSchedules { get; set; }
    public DbSet<SupplementalPay> SupplementalPays { get; set; }
    public DbSet<JobSupplementalPay> JobSupplementalPays { get; set; }
    public DbSet<SupportingDocument> SupportingDocuments { get; set; }
    public DbSet<JobSupportingDocument> JobSupportingDocuments { get; set; }

    public JobPortalDbContext(DbContextOptions<JobPortalDbContext> options)
        : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        /* Include modules to your migration db context */

        builder.ConfigurePermissionManagement();
        builder.ConfigureSettingManagement();
        builder.ConfigureBackgroundJobs();
        builder.ConfigureAuditLogging();
        builder.ConfigureIdentity();
        builder.ConfigureOpenIddict();
        builder.ConfigureFeatureManagement();

        /* Configure your own tables/entities inside here */

        builder.Entity<Jobs.Job>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Jobs", JobPortalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasMany(x => x.Applicants).WithOne().HasForeignKey(x => x.JobId).IsRequired();
            b.HasMany(x => x.Categories).WithOne().HasForeignKey(x => x.JobId);
            b.HasMany(x => x.Languages).WithOne().HasForeignKey(x => x.JobId);
            b.HasMany(x => x.Schedules).WithOne().HasForeignKey(x => x.JobId);
            b.HasMany(x => x.SupplementalPays).WithOne().HasForeignKey(x => x.JobId);
            b.HasMany(x => x.SupportingDocuments).WithOne().HasForeignKey(x => x.JobId);
            b.HasOne<Organisations.Organisation>().WithMany().HasForeignKey(x => x.OrganisationId);
            b.HasOne<Location>().WithMany().HasForeignKey(x => x.LocationId);
            b.Property(e => e.Type).HasConversion<int>();
        });

        builder.Entity<JobApplicant>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "JobApplicants", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.JobId, x.UserId });
            //many-to-many configuration
            b.HasOne<Jobs.Job>().WithMany(x => x.Applicants).HasForeignKey(x => x.UserId).IsRequired();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
            b.HasIndex(x => new { x.JobId, x.UserId });
        });

        #region Dropdown many-to-many relationships
        builder.Entity<Category>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Categories", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Language>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Languages", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Schedule>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Schedules", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<SupplementalPay>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "SupplementalPays", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<SupportingDocument>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "SupportingDocuments", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
        });

        builder.Entity<Location>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Locations", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(e => e.Latitude).HasPrecision(8, 6);
            b.Property(e => e.Longitude).HasPrecision(9, 6);
            b.HasIndex(e => e.Name);
        });

        builder.Entity<JobCategory>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "JobCategories", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.JobId, x.CategoryId });
            //many-to-many configuration
            b.HasOne<Jobs.Job>().WithMany(x => x.Categories).HasForeignKey(x => x.JobId);
            b.HasIndex(x => new { x.JobId, x.CategoryId });
        });

        builder.Entity<JobLanguage>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "JobLanguages", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.JobId, x.LanguageId });
            //many-to-many configuration
            b.HasOne<Jobs.Job>().WithMany(x => x.Languages).HasForeignKey(x => x.JobId);
            b.HasOne<Language>().WithMany().HasForeignKey(x => x.LanguageId);
            b.HasIndex(x => new { x.JobId, x.LanguageId });
        });

        builder.Entity<JobSchedule>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "JobSchedules", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.JobId, x.ScheduleId });
            //many-to-many configuration
            b.HasOne<Jobs.Job>().WithMany(x => x.Schedules).HasForeignKey(x => x.JobId);
            b.HasOne<Schedule>().WithMany().HasForeignKey(x => x.ScheduleId);
            b.HasIndex(x => new { x.JobId, x.ScheduleId });
        });

        builder.Entity<JobSupplementalPay>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "JobSupplementalPays", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.JobId, x.SupplementalPayId });
            //many-to-many configuration
            b.HasOne<Jobs.Job>().WithMany(x => x.SupplementalPays).HasForeignKey(x => x.JobId);
            b.HasOne<SupplementalPay>().WithMany().HasForeignKey(x => x.SupplementalPayId);
            b.HasIndex(x => new { x.JobId, x.SupplementalPayId });
        });

        builder.Entity<JobSupportingDocument>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "JobSupportingDocuments", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.JobId, x.SupportingDocumentId });
            //many-to-many configuration
            b.HasOne<Jobs.Job>().WithMany(x => x.SupportingDocuments).HasForeignKey(x => x.JobId);
            b.HasOne<SupportingDocument>().WithMany().HasForeignKey(x => x.SupportingDocumentId);
            b.HasIndex(x => new { x.JobId, x.SupportingDocumentId });
        });
        #endregion

        builder.Entity<Organisations.Organisation>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Organisations", JobPortalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasMany(a => a.Members).WithOne().HasForeignKey(x => x.UserId);
            b.HasMany(a => a.PostedJobs).WithOne().HasForeignKey(b => b.OrganisationId);
        });

        builder.Entity<OrganisationMember>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "OrganisationMembers", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.OrganisationId, x.UserId });
            //many-to-many configuration
            b.HasOne<Organisations.Organisation>().WithMany(x => x.Members).HasForeignKey(x => x.OrganisationId).IsRequired();
            b.HasOne<IdentityUser>().WithMany().HasForeignKey(x => x.UserId).IsRequired();
            b.HasIndex(x => new { x.OrganisationId, x.UserId });
        });

        builder.Entity<PaymentRequest>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "PaymentRequests", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            b.Property(x => x.CustomerId).HasMaxLength(PaymentRequestConsts.MaxCustomerIdLength);
            b.Property(x => x.ProductId).HasMaxLength(PaymentRequestConsts.MaxProductIdLength);
            b.Property(x => x.ProductName).IsRequired().HasMaxLength(PaymentRequestConsts.MaxProductNameLength);
            b.Property(x => x.Price).HasPrecision(10, 2);
            b.HasIndex(x => x.CustomerId);
            b.HasIndex(x => x.State);
        });
    }
}
