using System;
using System.Collections.Generic;
using ProcrastiDomain.Model;
using Microsoft.EntityFrameworkCore;

namespace ProcrastiInfrastructure;

public partial class ProcrastiContext : DbContext
{
    public ProcrastiContext()
    {
    }

    public ProcrastiContext(DbContextOptions<ProcrastiContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Achievement> Achievements { get; set; }

    public virtual DbSet<Activity> Activities { get; set; }

    public virtual DbSet<Category> Categories { get; set; }

    public virtual DbSet<Comment> Comments { get; set; }

    public virtual DbSet<Globalstat> Globalstats { get; set; }

    public virtual DbSet<Log> Logs { get; set; }

    public virtual DbSet<Title> Titles { get; set; }

    public virtual DbSet<User> Users { get; set; }

    public virtual DbSet<Userachievement> Userachievements { get; set; }

    public virtual DbSet<Usertitle> Usertitles { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.HasPostgresEnum("log_type", new[] { "win", "loss" });

        modelBuilder.Entity<Achievement>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("achievements_pkey");

            entity.ToTable("achievements");

            entity.HasIndex(e => e.Code, "achievements_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("achievementid");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Description).HasColumnName("description");
            entity.Property(e => e.Icon).HasColumnName("icon");
            entity.Property(e => e.Ishidden)
                .HasDefaultValue(false)
                .HasColumnName("ishidden");
            entity.Property(e => e.Title)
                .HasMaxLength(100)
                .HasColumnName("title");
        });

        modelBuilder.Entity<Activity>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("activities_pkey");

            entity.ToTable("activities");

            entity.Property(e => e.Id).HasColumnName("activityid");
            entity.Property(e => e.Categoryid).HasColumnName("categoryid");
            entity.Property(e => e.Mentionscount)
                .HasDefaultValue(0)
                .HasColumnName("mentionscount");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");

            entity.HasOne(d => d.Category).WithMany(p => p.Activities)
                .HasForeignKey(d => d.Categoryid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_category");
        });

        modelBuilder.Entity<Category>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("categories_pkey");

            entity.ToTable("categories");

            entity.HasIndex(e => e.Name, "categories_name_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("categoryid");
            entity.Property(e => e.Name)
                .HasMaxLength(50)
                .HasColumnName("name");
        });

        modelBuilder.Entity<Comment>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("comments_pkey");

            entity.ToTable("comments");

            entity.Property(e => e.Id).HasColumnName("commentid");
            entity.Property(e => e.Authorid).HasColumnName("authorid");
            entity.Property(e => e.Content).HasColumnName("content");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Logid).HasColumnName("logid");
            entity.Property(e => e.Parentcommentid).HasColumnName("parentcommentid");

            entity.HasOne(d => d.Author).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Authorid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_author");

            entity.HasOne(d => d.Log).WithMany(p => p.Comments)
                .HasForeignKey(d => d.Logid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_log");

            entity.HasOne(d => d.Parentcomment).WithMany(p => p.InverseParentcomment)
                .HasForeignKey(d => d.Parentcommentid)
                .OnDelete(DeleteBehavior.Cascade)
                .HasConstraintName("fk_parentcomment");
        });

        modelBuilder.Entity<Globalstat>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("globalstats_pkey");

            entity.ToTable("globalstats");

            entity.Property(e => e.Id).HasColumnName("id");
            entity.Property(e => e.Lastupdated)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("lastupdated");
            entity.Property(e => e.Totallossamount)
                .HasDefaultValue(0)
                .HasColumnName("totallossamount");
        });

        modelBuilder.Entity<Log>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("logs_pkey");

            entity.ToTable("logs");

            entity.Property(e => e.Id).HasColumnName("logid");
            entity.Property(e => e.Activityid).HasColumnName("activityid");
            entity.Property(e => e.Amount).HasColumnName("amount");
            entity.Property(e => e.Comment).HasColumnName("comment");
            entity.Property(e => e.Createdat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("createdat");
            entity.Property(e => e.Isvisible)
                .HasDefaultValue(true)
                .HasColumnName("isvisible");
            entity.Property(e => e.Likescount)
                .HasDefaultValue(0)
                .HasColumnName("likescount");
            entity.Property(e => e.Rating).HasColumnName("rating");
            entity.Property(e => e.Userid).HasColumnName("userid");

            entity.HasOne(d => d.Activity).WithMany(p => p.Logs)
                .HasForeignKey(d => d.Activityid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_activity");

            entity.HasOne(d => d.User).WithMany(p => p.LogsNavigation)
                .HasForeignKey(d => d.Userid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_user");
        });

        modelBuilder.Entity<Title>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("titles_pkey");

            entity.ToTable("titles");

            entity.HasIndex(e => e.Code, "titles_code_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("titleid");
            entity.Property(e => e.Code)
                .HasMaxLength(50)
                .HasColumnName("code");
            entity.Property(e => e.Isunique)
                .HasDefaultValue(false)
                .HasColumnName("isunique");
            entity.Property(e => e.Name)
                .HasMaxLength(100)
                .HasColumnName("name");
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("users_pkey");

            entity.ToTable("users");

            entity.HasIndex(e => e.Email, "users_email_key").IsUnique();

            entity.Property(e => e.Id).HasColumnName("userid");
            entity.Property(e => e.Email)
                .HasMaxLength(50)
                .HasColumnName("email");
            entity.Property(e => e.Isadmin)
                .HasDefaultValue(false)
                .HasColumnName("isadmin");
            entity.Property(e => e.Isbanned)
                .HasDefaultValue(false)
                .HasColumnName("isbanned");
            entity.Property(e => e.Joineddate)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("joineddate");
            entity.Property(e => e.Passwordhash).HasColumnName("passwordhash");
            entity.Property(e => e.Profilepicture).HasColumnName("profilepicture");
            entity.Property(e => e.Titleid).HasColumnName("titleid");
            entity.Property(e => e.Totalloss)
                .HasDefaultValue(0)
                .HasColumnName("totalloss");
            entity.Property(e => e.Username)
                .HasMaxLength(50)
                .HasColumnName("username");

            entity.HasOne(d => d.Title).WithMany(p => p.Users)
                .HasForeignKey(d => d.Titleid)
                .OnDelete(DeleteBehavior.SetNull)
                .HasConstraintName("fk_title");

            entity.HasMany(d => d.Logs).WithMany(p => p.Users)
                .UsingEntity<Dictionary<string, object>>(
                    "Like",
                    r => r.HasOne<Log>().WithMany()
                        .HasForeignKey("Logid")
                        .HasConstraintName("likes_logid_fkey"),
                    l => l.HasOne<User>().WithMany()
                        .HasForeignKey("Userid")
                        .HasConstraintName("likes_userid_fkey"),
                    j =>
                    {
                        j.HasKey("Userid", "Logid").HasName("likes_pkey");
                        j.ToTable("likes");
                        j.IndexerProperty<int>("Userid").HasColumnName("userid");
                        j.IndexerProperty<int>("Logid").HasColumnName("logid");
                    });
        });

        modelBuilder.Entity<Userachievement>(entity =>
        {
            entity.HasKey(e => new { e.Userid, e.Achievementid }).HasName("userachievements_pkey");

            entity.ToTable("userachievements");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Achievementid).HasColumnName("achievementid");
            entity.Property(e => e.Unlockedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("unlockedat");

            entity.HasOne(d => d.Achievement).WithMany(p => p.Userachievements)
                .HasForeignKey(d => d.Achievementid)
                .HasConstraintName("userachievements_achievementid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Userachievements)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("userachievements_userid_fkey");
        });

        modelBuilder.Entity<Usertitle>(entity =>
        {
            entity.HasKey(e => new { e.Userid, e.Titleid }).HasName("usertitles_pkey");

            entity.ToTable("usertitles");

            entity.Property(e => e.Userid).HasColumnName("userid");
            entity.Property(e => e.Titleid).HasColumnName("titleid");
            entity.Property(e => e.Unlockedat)
                .HasDefaultValueSql("now()")
                .HasColumnType("timestamp without time zone")
                .HasColumnName("unlockedat");

            entity.HasOne(d => d.Title).WithMany(p => p.Usertitles)
                .HasForeignKey(d => d.Titleid)
                .HasConstraintName("usertitles_titleid_fkey");

            entity.HasOne(d => d.User).WithMany(p => p.Usertitles)
                .HasForeignKey(d => d.Userid)
                .HasConstraintName("usertitles_userid_fkey");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
