using Collabed.JobPortal.Candidates;
using Collabed.JobPortal.Jobs;
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
    /* Add DbSet properties for your Aggregate Roots / Entities here. */

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

    //Job board
    public DbSet<Jobs.Job> Jobs { get; set; }
    public DbSet<Clients.Client> Clients { get; set; }
    public DbSet<Candidate> Candidates { get; set; }


    #endregion

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

        builder.Entity<Candidate>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Candidates", JobPortalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            //many-to-many relationship with Jobs table => CandidateJobs
            b.HasMany(a => a.AppliedJobs).WithOne().HasForeignKey(x => x.CandidateId);
        });

        builder.Entity<CandidateJobs>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "CandidateJobs", JobPortalConsts.DbSchema);
            b.ConfigureByConvention();
            //define composite key
            b.HasKey(x => new { x.CandidateId, x.JobId });
            //many-to-many configuration
            b.HasOne<Candidate>().WithMany(x => x.AppliedJobs).HasForeignKey(x => x.CandidateId).IsRequired();
            b.HasOne<Jobs.Job>().WithMany().HasForeignKey(x => x.JobId).IsRequired();
            b.HasIndex(x => new { x.CandidateId, x.JobId });
        });

        builder.Entity<Clients.Client>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Clients", JobPortalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasMany(a => a.PostedJobs).WithOne(b => b.Client);
        });

        builder.Entity<Jobs.Job>(b =>
        {
            b.ToTable(JobPortalConsts.DbTablePrefix + "Jobs", JobPortalConsts.DbSchema);
            b.ConfigureByConvention(); //auto configure for the base class props
            b.HasOne(a => a.Client).WithMany(b => b.PostedJobs);
            b.Property(e => e.Type).HasConversion<int>();
        });
    }
}
