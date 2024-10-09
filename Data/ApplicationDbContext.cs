using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;


namespace OfficePortal.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext (DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<OfficePortal.Models.TrainingRequestViewModel> TrainingRequestViewModel { get; set; } = default!;
        public DbSet<OfficePortal.Models.MissionandTrainingForm> MissionandTrainingForm { get; set; } = default!;
        public DbSet<OfficePortal.Models.Announcement> Announcement { get; set; } = default!;
        public DbSet<OfficePortal.Models.Comment> Comment { get; set; } = default!;
        public DbSet<OfficePortal.Models.EventModel> EventModel { get; set; } = default!;

    }
}
