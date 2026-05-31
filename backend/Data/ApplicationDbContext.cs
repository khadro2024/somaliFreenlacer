using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

    public DbSet<User> Users => Set<User>();
    public DbSet<FreelancerProfile> FreelancerProfiles => Set<FreelancerProfile>();
    public DbSet<Category> Categories => Set<Category>();
    public DbSet<Job> Jobs => Set<Job>();
    public DbSet<Application> Applications => Set<Application>();
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Payment> Payments => Set<Payment>();
    public DbSet<Message> Messages => Set<Message>();
    public DbSet<Rating> Ratings => Set<Rating>();
    public DbSet<WorkImage> WorkImages => Set<WorkImage>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(e =>
        {
            e.HasKey(x => x.UserId);
            e.HasIndex(x => x.Email).IsUnique();
        });

        modelBuilder.Entity<FreelancerProfile>(e =>
        {
            e.HasKey(x => x.ProfileId);
            e.HasOne(x => x.User).WithOne(x => x.FreelancerProfile)
                .HasForeignKey<FreelancerProfile>(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Job>(e =>
        {
            e.HasKey(x => x.JobId);
            e.HasOne(x => x.Client).WithMany(x => x.PostedJobs)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Category).WithMany(c => c.Jobs)
                .HasForeignKey(x => x.CategoryId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        modelBuilder.Entity<Application>(e =>
        {
            e.HasKey(x => x.ApplicationId);
            e.HasIndex(x => new { x.JobId, x.FreelancerId }).IsUnique();
            e.HasOne(x => x.Job).WithMany(j => j.Applications)
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Freelancer).WithMany(x => x.Applications)
                .HasForeignKey(x => x.FreelancerId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Project>(e =>
        {
            e.HasKey(x => x.ProjectId);
            e.HasIndex(x => x.JobId).IsUnique();
            e.HasOne(x => x.Client).WithMany(x => x.ClientProjects)
                .HasForeignKey(x => x.ClientId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Freelancer).WithMany(x => x.FreelancerProjects)
                .HasForeignKey(x => x.FreelancerId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Job).WithOne(x => x.Project)
                .HasForeignKey<Project>(x => x.JobId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Payment>(e =>
        {
            e.HasKey(x => x.PaymentId);
            e.HasOne(x => x.Project).WithOne(x => x.Payment)
                .HasForeignKey<Payment>(x => x.ProjectId);
        });

        modelBuilder.Entity<Message>(e =>
        {
            e.HasKey(x => x.MessageId);
            e.HasOne(x => x.Sender).WithMany(x => x.SentMessages)
                .HasForeignKey(x => x.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Receiver).WithMany(x => x.ReceivedMessages)
                .HasForeignKey(x => x.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<Rating>(e =>
        {
            e.HasKey(x => x.RatingId);
            e.HasIndex(x => new { x.ProjectId, x.RaterId }).IsUnique();
            e.HasOne(x => x.Rater).WithMany(x => x.RatingsGiven)
                .HasForeignKey(x => x.RaterId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.RatedUser).WithMany(x => x.RatingsReceived)
                .HasForeignKey(x => x.RatedUserId)
                .OnDelete(DeleteBehavior.Restrict);
            e.HasOne(x => x.Project).WithMany(p => p.Ratings)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<WorkImage>(e =>
        {
            e.HasKey(x => x.WorkImageId);
            e.HasOne(x => x.User).WithMany(u => u.WorkImages)
                .HasForeignKey(x => x.UserId)
                .OnDelete(DeleteBehavior.Cascade);
            e.HasOne(x => x.Job).WithMany(j => j.WorkImages)
                .HasForeignKey(x => x.JobId)
                .OnDelete(DeleteBehavior.NoAction);
            e.HasOne(x => x.Project).WithMany(p => p.WorkImages)
                .HasForeignKey(x => x.ProjectId)
                .OnDelete(DeleteBehavior.NoAction);
        });

        SeedData(modelBuilder);
    }

    private static void SeedData(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Category>().HasData(
            new Category { CategoryId = 1, Name = "Web Development", Description = "Websites and web apps" },
            new Category { CategoryId = 2, Name = "Design", Description = "UI/UX and graphics" },
            new Category { CategoryId = 3, Name = "Writing", Description = "Content and copywriting" },
            new Category { CategoryId = 4, Name = "Marketing", Description = "Digital marketing" },
            new Category { CategoryId = 5, Name = "Translation", Description = "Somali and other languages" }
        );

    }
}
