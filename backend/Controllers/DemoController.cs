using Microsoft.AspNetCore.Mvc;
using SomaliFreelanceMarketplace.Data;
using SomaliFreelanceMarketplace.Models.DTOs;

namespace SomaliFreelanceMarketplace.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DemoController : ControllerBase
{
    [HttpGet("accounts")]
    public ActionResult<IEnumerable<DemoAccountDto>> GetDemoAccounts() =>
        Ok(new[]
        {
            new DemoAccountDto { Role = "Admin", Email = "admin@sfm.so", Password = "Grils wep12", FullName = "SFM", Purpose = "Admin panel — users, jobs, payments" },
            new DemoAccountDto { Role = "Client", Email = "client1@sfm.so", Password = DemoDataSeeder.DemoPassword, FullName = "Ahmed Hassan", Purpose = "Jobs open + project in progress (escrow held)" },
            new DemoAccountDto { Role = "Client", Email = "client2@sfm.so", Password = DemoDataSeeder.DemoPassword, FullName = "Fatima Trading Co.", Purpose = "Open design job + completed project" },
            new DemoAccountDto { Role = "Freelancer", Email = "dev@sfm.so", Password = DemoDataSeeder.DemoPassword, FullName = "Hassan Ali", Purpose = "Developer — active project, pending applications" },
            new DemoAccountDto { Role = "Freelancer", Email = "design@sfm.so", Password = DemoDataSeeder.DemoPassword, FullName = "Amina Mohamed", Purpose = "Designer — applications on open jobs" },
            new DemoAccountDto { Role = "Freelancer", Email = "writer@sfm.so", Password = DemoDataSeeder.DemoPassword, FullName = "Omar Abdi", Purpose = "Writer — not verified (test payment release rule)" },
        });
}
