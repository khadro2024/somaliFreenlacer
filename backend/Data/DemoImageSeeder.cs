using Microsoft.EntityFrameworkCore;
using SomaliFreelanceMarketplace.Models.Enums;

namespace SomaliFreelanceMarketplace.Data;

/// <summary>Adds demo image URLs when demo users exist but images were not seeded yet.</summary>
public static class DemoImageSeeder
{
    public static void Seed(ApplicationDbContext db)
    {
        if (db.WorkImages.Any())
            return;

        var client1 = db.Users.FirstOrDefault(u => u.Email == "client1@sfm.so");
        if (client1 == null)
            return;

        var client2 = db.Users.First(u => u.Email == "client2@sfm.so");
        var dev = db.Users.First(u => u.Email == "dev@sfm.so");
        var design = db.Users.First(u => u.Email == "design@sfm.so");
        var writer = db.Users.First(u => u.Email == "writer@sfm.so");

        client1.ProfileImageUrl ??= DemoImageUrls.ProfileClient1;
        client2.ProfileImageUrl ??= DemoImageUrls.ProfileClient2;
        dev.ProfileImageUrl ??= DemoImageUrls.ProfileDev;
        design.ProfileImageUrl ??= DemoImageUrls.ProfileDesign;
        writer.ProfileImageUrl ??= DemoImageUrls.ProfileWriter;

        var admin = db.Users.FirstOrDefault(u => u.Role == UserRole.Admin);
        if (admin != null && admin.ProfileImageUrl == null)
            admin.ProfileImageUrl = DemoImageUrls.ProfileDev;

        foreach (var job in db.Jobs.ToList())
        {
            if (job.CoverImageUrl != null) continue;
            job.CoverImageUrl = job.Title.Contains("E-commerce") ? DemoImageUrls.JobEcommerce
                : job.Title.Contains("Logo") ? DemoImageUrls.JobLogo
                : job.Title.Contains("Dashboard") ? DemoImageUrls.JobDashboard
                : DemoImageUrls.JobBlog;
        }

        db.SaveChanges();

        var projectWorking = db.Projects.Include(p => p.Job)
            .FirstOrDefault(p => p.Job.Title.Contains("Dashboard"));
        var projectDone = db.Projects.Include(p => p.Job)
            .FirstOrDefault(p => p.Job.Title.Contains("Blog"));

        if (projectWorking != null && projectDone != null)
            DemoDataSeeder.SeedWorkImages(db, dev.UserId, design.UserId, writer.UserId, projectWorking.ProjectId, projectDone.ProjectId);
    }
}
