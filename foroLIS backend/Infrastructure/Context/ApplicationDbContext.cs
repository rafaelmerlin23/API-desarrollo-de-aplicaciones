using foroLIS_backend.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace foroLIS_backend.Infrastructure.Context
{
    public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) 
        : IdentityDbContext<Users>(options)
       
    {
        public DbSet<Survey> Surveys { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<FieldSurvey> Fields { get; set; }
        public DbSet<UserFieldSurvey> UsersFields { get; set; }
        public DbSet<UserHistorial> UserHistorials { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Reaction> Reactions { get; set; }
        public DbSet<Mention> Mentions { get; set; }
        public DbSet<MediaFile> MediaFiles { get; set; }
        public DbSet<FilePost> FilePosts { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<CommunityMessage> CommunityMessages { get; set; }
        public DbSet<CommunitySurvey> CommunitySurveys { get; set; }
        public DbSet<CommunityFieldSurvey> CommunityFields { get; set; }
        public DbSet<CommunityFieldsUser> CommunityUserFields {  get; set; }
        public DbSet<CommunityLikes> CommunityLikes { get; set; }
        public DbSet<CommunityMessageFile> CommunityMessageFiles { get; set; }
        public DbSet<CommunityMessageComment> CommunityMessageComments { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            
            builder.Entity<UserFieldSurvey>()
            .HasKey(ufs => new { ufs.UserId, ufs.FieldSurveyId });

            builder.Entity<Survey>()
                .HasIndex(s => s.PostId)
                .IsUnique();

            builder.Entity<UserFieldSurvey>()
                .HasOne(ufs => ufs.Users)
                .WithMany()  
                .HasForeignKey(ufs => ufs.UserId);

            builder.Entity<UserFieldSurvey>()
                .HasOne(ufs => ufs.FieldSurvey)
                .WithMany()  
                .HasForeignKey(ufs => ufs.FieldSurveyId);

            builder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull); 

            builder.Entity<Comment>()
                .HasOne(c => c.Users)
                .WithMany(u => u.Comments)
                .HasForeignKey(c => c.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Survey>()
                .HasOne(s => s.Post)
                .WithMany(p => p.Surveries)
                .HasForeignKey(s => s.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<UserHistorial>()
                .HasOne(uh => uh.Users)
                .WithMany(u => u.UserHistorials)
                .HasForeignKey(uh => uh.UserId)
                .OnDelete(DeleteBehavior.NoAction); 
                
            builder.Entity<UserHistorial>()
                .HasOne(uh => uh.Post)
                .WithMany(p => p.UserHistorials)
                .HasForeignKey(uh => uh.PostId)
                .OnDelete(DeleteBehavior.NoAction);

            builder.Entity<Reaction>()
                .HasKey(r => new { r.ReactionType, r.PostId, r.CommentId,r.UserId });

            builder.Entity<Reaction>()
                .HasOne(r => r.Post)
                .WithMany(p => p.Reactions)
                .HasForeignKey(r => r.PostId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Reaction>()
                .HasOne(r => r.Comment)
                .WithMany(c => c.Reactions)
                .HasForeignKey(r => r.CommentId)
                .OnDelete(DeleteBehavior.ClientSetNull);
            builder.Entity<Reaction>()
                .HasOne(r => r.Users)
                .WithMany(u => u.Reactions)
                .HasForeignKey(r => r.UserId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.Entity<Donation>()
                .HasOne(d => d.Donor)
                .WithMany()
                .HasForeignKey(d => d.DonorId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<Donation>()
                .HasOne(d => d.Receiver)
                .WithMany()
                .HasForeignKey(d => d.ReceiverId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<CommunityMessage>()
                .HasOne(m => m.Post)
                .WithMany(p => p.CommunityMessages)
                .HasForeignKey(m => m.PostId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CommunityMessage>()
                .HasOne(cm => cm.Survey)
                .WithOne(cs => cs.CommunityMessage)
                .HasForeignKey<CommunitySurvey>(cs => cs.Id);
            builder.Entity<CommunitySurvey>()
                .HasMany(s => s.Fields)
                .WithOne(f => f.Survey)
                .HasForeignKey(f => f.SurveyId);

            builder.Entity<CommunityLikes>()
                .HasKey(cl => new { cl.CommunityMessageId, cl.UserId });
            builder.Entity<CommunityLikes>()
                .HasOne(cl => cl.User)
                .WithMany()
                .HasForeignKey(cl => cl.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CommunityLikes>()
                .HasOne(cl => cl.CommunityMessage)
                .WithMany()
                .HasForeignKey(cl => cl.CommunityMessageId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CommunityFieldsUser>()
                .HasKey(cf => new { cf.CommunityFieldId, cf.UserId });
            builder.Entity<CommunityFieldsUser>()
                .HasOne(cf => cf.CommunityFieldSurvey)
                .WithMany()
                .HasForeignKey(cf => cf.CommunityFieldId)
                    .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CommunityFieldsUser>()
                .HasOne(cf => cf.User)
                .WithMany(u => u.CommunityFieldSurveys)
                .HasForeignKey(cf => cf.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<CommunityMessageComment>()
                 .HasOne(c => c.CommunityMessage)
                 .WithMany()
                 .HasForeignKey(c => c.CommunityMessageId)
                 .OnDelete(DeleteBehavior.Restrict);

            base.OnModelCreating(builder);

        }
    }
}
