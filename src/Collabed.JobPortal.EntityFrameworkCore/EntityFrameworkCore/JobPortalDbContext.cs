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
            b.HasOne<Organisations.Organisation>().WithMany().HasForeignKey(x => x.OrganisationId).IsRequired();
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

            b.HasIndex(x => x.CustomerId);
            b.HasIndex(x => x.State);
        });
    }
}
