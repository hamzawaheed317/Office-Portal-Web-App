using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using OfficePortal.Models.Admin_Models;
using OfficePortal.Models.Admin_Models.Announcement_Models;
using OfficePortal.Models.Employee_Models;


namespace OfficePortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<TrainingRequestViewModel> TrainingRequestViewModel { get; set; } = default!;
        public DbSet<MissionandTrainingForm> MissionandTrainingForm { get; set; } = default!;
        public DbSet<Announcement> Announcement { get; set; } = default!;
        public DbSet<OfficePortal.Models.Comment> Comment { get; set; } = default!;
        public DbSet<EventModel> EventModel { get; set; } = default!;

    }
}
