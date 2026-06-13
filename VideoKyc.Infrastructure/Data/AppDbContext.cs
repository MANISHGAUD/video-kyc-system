using Microsoft.EntityFrameworkCore;
using VideoKyc.Domain.Entities;

namespace VideoKyc.Infrastructure.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

        public DbSet<UserSession> UserSessions { get; set; }
        public DbSet<ChatMessage> ChatMessages { get; set; }
        public DbSet<SessionQuestion> SessionQuestions { get; set; }
        public DbSet<SessionKycDetail> SessionKycDetails { get; set; }
        public DbSet<SessionCapture> SessionCaptures { get; set; }
        public DbSet<SessionRecording> SessionRecordings { get; set; }

        public DbSet<SessionLocation> SessionLocations { get; set; }
        public DbSet<SessionStateHistory> SessionStateHistories { get; set; }
        public DbSet<SessionDeviceInfo> SessionDeviceInfos{get;  set;}
        protected override void OnModelCreating(
        ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserSession>()
                .Property(x => x.Status)
                .HasConversion<string>();

            modelBuilder.Entity<UserSession>()
                .HasOne(x => x.Location)
                .WithOne(x => x.Session)
                .HasForeignKey<SessionLocation>(
                    x => x.SessionId);
            modelBuilder.Entity<UserSession>()
                .HasOne(x => x.DeviceInfo)
                .WithOne()
                .HasForeignKey<SessionDeviceInfo>(
                    x => x.SessionId);

            base.OnModelCreating(modelBuilder);
        }
    }
}