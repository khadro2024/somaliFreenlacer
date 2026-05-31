using SomaliFreelanceMarketplace.Models.Entities;
using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Data;

/// <summary>
/// Seeds sample data for every module so the system is easy to explore.
/// Runs once when no jobs exist (besides migrations).
/// </summary>
public static class DemoDataSeeder
{
    public const string DemoPassword = "Demo@123";

    public static void Seed(ApplicationDbContext db)
    {
        if (db.Users.Any(u => u.Email == "client1@sfm.so"))
            return;

        var pw = BCrypt.Net.BCrypt.HashPassword(DemoPassword);

        // —— Users ——
        var client1 = new User
        {
            FullName = "Ahmed Hassan",
            Email = "client1@sfm.so",
            PasswordHash = pw,
            Role = UserRole.Client,
            IsVerified = true,
            ProfileImageUrl = DemoImageUrls.ProfileClient1,
            CreatedAt = DateTime.UtcNow.AddDays(-30)
        };
        var client2 = new User
        {
            FullName = "Fatima Trading Co.",
            Email = "client2@sfm.so",
            PasswordHash = pw,
            Role = UserRole.Client,
            IsVerified = true,
            ProfileImageUrl = DemoImageUrls.ProfileClient2,
            CreatedAt = DateTime.UtcNow.AddDays(-20)
        };

        var freelancer1 = new User
        {
            FullName = "Hassan Ali",
            Email = "dev@sfm.so",
            PasswordHash = pw,
            Role = UserRole.Freelancer,
            IsVerified = true,
            ProfileImageUrl = DemoImageUrls.ProfileDev,
            CreatedAt = DateTime.UtcNow.AddDays(-60)
        };
        var freelancer2 = new User
        {
            FullName = "Amina Mohamed",
            Email = "design@sfm.so",
            PasswordHash = pw,
            Role = UserRole.Freelancer,
            IsVerified = true,
            ProfileImageUrl = DemoImageUrls.ProfileDesign,
            CreatedAt = DateTime.UtcNow.AddDays(-45)
        };
        var freelancer3 = new User
        {
            FullName = "Omar Abdi",
            Email = "writer@sfm.so",
            PasswordHash = pw,
            Role = UserRole.Freelancer,
            IsVerified = false,
            ProfileImageUrl = DemoImageUrls.ProfileWriter,
            CreatedAt = DateTime.UtcNow.AddDays(-10)
        };

        db.Users.AddRange(client1, client2, freelancer1, freelancer2, freelancer3);
        db.SaveChanges();

        db.FreelancerProfiles.AddRange(
            new FreelancerProfile
            {
                UserId = freelancer1.UserId,
                Skills = "React, .NET, SQL Server, Node.js",
                Bio = "Full-stack developer with 5+ years building web apps for Somali businesses.",
                HourlyRate = 25,
                Portfolio = "https://github.com/hassan-demo",
                Rating = 4.8,
                RatingCount = 12
            },
            new FreelancerProfile
            {
                UserId = freelancer2.UserId,
                Skills = "UI/UX, Figma, Branding, Logo Design",
                Bio = "Creative designer focused on modern Somali brands and mobile-first UI.",
                HourlyRate = 20,
                Portfolio = "https://behance.net/amina-demo",
                Rating = 4.6,
                RatingCount = 8
            },
            new FreelancerProfile
            {
                UserId = freelancer3.UserId,
                Skills = "Somali Content, Copywriting, SEO",
                Bio = "Bilingual writer (Somali/English) for blogs and marketing.",
                HourlyRate = 15,
                Portfolio = "",
                Rating = 0,
                RatingCount = 0
            }
        );
        db.SaveChanges();

        // —— Jobs ——
        var jobOpen1 = new Job
        {
            ClientId = client1.UserId,
            CategoryId = 1,
            Title = "E-commerce Website for Hargeisa Shop",
            Description = "Need a modern online store with product catalog, cart, and mobile-friendly design. Somali language support preferred.",
            Budget = 800,
            Status = JobStatus.Open,
            CoverImageUrl = DemoImageUrls.JobEcommerce,
            CreatedAt = DateTime.UtcNow.AddDays(-3)
        };
        var jobOpen2 = new Job
        {
            ClientId = client2.UserId,
            CategoryId = 2,
            Title = "Logo & Brand Kit for New Restaurant",
            Description = "Logo, color palette, and social media templates for a Mogadishu restaurant launch.",
            Budget = 350,
            Status = JobStatus.Open,
            CoverImageUrl = DemoImageUrls.JobLogo,
            CreatedAt = DateTime.UtcNow.AddDays(-1)
        };
        var jobInProgress = new Job
        {
            ClientId = client1.UserId,
            CategoryId = 1,
            Title = "Company Dashboard — React + API",
            Description = "Admin dashboard with charts, user management, and reports. API already exists.",
            Budget = 1200,
            Status = JobStatus.InProgress,
            CoverImageUrl = DemoImageUrls.JobDashboard,
            CreatedAt = DateTime.UtcNow.AddDays(-14)
        };
        var jobCompleted = new Job
        {
            ClientId = client2.UserId,
            CategoryId = 3,
            Title = "Blog Articles (10 posts, Somali)",
            Description = "SEO-friendly blog posts about logistics and trade in Somalia.",
            Budget = 200,
            Status = JobStatus.Closed,
            CoverImageUrl = DemoImageUrls.JobBlog,
            CreatedAt = DateTime.UtcNow.AddDays(-40)
        };

        db.Jobs.AddRange(jobOpen1, jobOpen2, jobInProgress, jobCompleted);
        db.SaveChanges();

        // —— Applications ——
        db.Applications.AddRange(
            new Application
            {
                JobId = jobOpen1.JobId,
                FreelancerId = freelancer1.UserId,
                Proposal = "I built 3 e-commerce sites for Somali clients. I can deliver in 3 weeks with React and secure payments.",
                BidAmount = 750,
                Status = ApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-2)
            },
            new Application
            {
                JobId = jobOpen1.JobId,
                FreelancerId = freelancer2.UserId,
                Proposal = "I can handle UI/UX and work with your developer for integration.",
                BidAmount = 400,
                Status = ApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Application
            {
                JobId = jobOpen2.JobId,
                FreelancerId = freelancer2.UserId,
                Proposal = "I will provide 3 logo concepts and full brand guidelines within 10 days.",
                BidAmount = 320,
                Status = ApplicationStatus.Pending,
                CreatedAt = DateTime.UtcNow.AddHours(-12)
            },
            new Application
            {
                JobId = jobOpen2.JobId,
                FreelancerId = freelancer3.UserId,
                Proposal = "I can write taglines and menu copy in Somali as part of the package.",
                BidAmount = 150,
                Status = ApplicationStatus.Rejected,
                CreatedAt = DateTime.UtcNow.AddDays(-1)
            },
            new Application
            {
                JobId = jobInProgress.JobId,
                FreelancerId = freelancer1.UserId,
                Proposal = "Experienced with React dashboards. Weekly updates and source code on completion.",
                BidAmount = 1100,
                Status = ApplicationStatus.Accepted,
                CreatedAt = DateTime.UtcNow.AddDays(-12)
            },
            new Application
            {
                JobId = jobInProgress.JobId,
                FreelancerId = freelancer2.UserId,
                Proposal = "UI-only package available.",
                BidAmount = 500,
                Status = ApplicationStatus.Rejected,
                CreatedAt = DateTime.UtcNow.AddDays(-11)
            }
        );
        db.SaveChanges();

        // —— Projects + Payments ——
        var projectWorking = new Project
        {
            JobId = jobInProgress.JobId,
            FreelancerId = freelancer1.UserId,
            ClientId = client1.UserId,
            Status = ProjectStatus.Working,
            StartedAt = DateTime.UtcNow.AddDays(-10)
        };
        var projectDone = new Project
        {
            JobId = jobCompleted.JobId,
            FreelancerId = freelancer3.UserId,
            ClientId = client2.UserId,
            Status = ProjectStatus.Completed,
            StartedAt = DateTime.UtcNow.AddDays(-35),
            CompletedAt = DateTime.UtcNow.AddDays(-5)
        };

        db.Projects.AddRange(projectWorking, projectDone);
        db.SaveChanges();

        db.Payments.AddRange(
            new Payment
            {
                ProjectId = projectWorking.ProjectId,
                Amount = 1100,
                Status = PaymentStatus.Held,
                CreatedAt = DateTime.UtcNow.AddDays(-9)
            },
            new Payment
            {
                ProjectId = projectDone.ProjectId,
                Amount = 200,
                Status = PaymentStatus.Released,
                CreatedAt = DateTime.UtcNow.AddDays(-34),
                ReleasedAt = DateTime.UtcNow.AddDays(-4)
            }
        );
        db.SaveChanges();

        // —— Messages ——
        db.Messages.AddRange(
            new Message
            {
                SenderId = client1.UserId,
                ReceiverId = freelancer1.UserId,
                Text = "Salaan Hassan, waan ku faraxsanahay inaad qabatay mashruuca dashboard-ka.",
                SentAt = DateTime.UtcNow.AddDays(-9),
                IsRead = true
            },
            new Message
            {
                SenderId = freelancer1.UserId,
                ReceiverId = client1.UserId,
                Text = "Mahadsanid! Waxaan bilaabay wireframes. Ma rabtaa demo usbuucan dambe?",
                SentAt = DateTime.UtcNow.AddDays(-8),
                IsRead = true
            },
            new Message
            {
                SenderId = client1.UserId,
                ReceiverId = freelancer1.UserId,
                Text = "Haa, fadlan soo dir link-ga demo-ga.",
                SentAt = DateTime.UtcNow.AddDays(-7),
                IsRead = false
            },
            new Message
            {
                SenderId = client2.UserId,
                ReceiverId = freelancer2.UserId,
                Text = "Amina, ma arki karnaa tusaalooyin logo ah oo aad sameysay?",
                SentAt = DateTime.UtcNow.AddHours(-6),
                IsRead = false
            }
        );
        db.SaveChanges();

        // —— Ratings ——
        db.Ratings.Add(new Rating
        {
            ProjectId = projectDone.ProjectId,
            RaterId = client2.UserId,
            RatedUserId = freelancer3.UserId,
            Stars = 4,
            ReviewText = "Content quality was good. Delivered on time after verification.",
            CreatedAt = DateTime.UtcNow.AddDays(-4)
        });
        db.SaveChanges();

        SeedWorkImages(db, freelancer1.UserId, freelancer2.UserId, freelancer3.UserId, projectWorking.ProjectId, projectDone.ProjectId);
    }

    internal static void SeedWorkImages(ApplicationDbContext db, int devId, int designId, int writerId, int projectWorkingId, int projectDoneId)
    {
        db.WorkImages.AddRange(
            new WorkImage { UserId = devId, ImageUrl = DemoImageUrls.PortfolioWeb1, PublicId = "demo", Caption = "E-commerce project", ImageType = ImageType.Portfolio },
            new WorkImage { UserId = devId, ImageUrl = DemoImageUrls.PortfolioWeb2, PublicId = "demo", Caption = "Mobile app UI", ImageType = ImageType.Portfolio },
            new WorkImage { UserId = designId, ImageUrl = DemoImageUrls.PortfolioDesign1, PublicId = "demo", Caption = "Restaurant branding", ImageType = ImageType.Portfolio },
            new WorkImage { UserId = designId, ImageUrl = DemoImageUrls.PortfolioDesign2, PublicId = "demo", Caption = "Social media kit", ImageType = ImageType.Portfolio },
            new WorkImage { UserId = devId, ImageUrl = DemoImageUrls.ProjectDashboard, PublicId = "demo", Caption = "Dashboard wireframe v2", ImageType = ImageType.ProjectWork, ProjectId = projectWorkingId },
            new WorkImage { UserId = writerId, ImageUrl = DemoImageUrls.ProjectBlog, PublicId = "demo", Caption = "Blog post samples", ImageType = ImageType.ProjectWork, ProjectId = projectDoneId }
        );
        db.SaveChanges();
    }
}
