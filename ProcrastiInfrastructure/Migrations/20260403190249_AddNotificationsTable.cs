using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace ProcrastiInfrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddNotificationsTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.CreateTable(
            //    name: "achievements",
            //    columns: table => new
            //    {
            //        achievementid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        description = table.Column<string>(type: "text", nullable: true),
            //        icon = table.Column<string>(type: "text", nullable: true),
            //        ishidden = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("achievements_pkey", x => x.achievementid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "categories",
            //    columns: table => new
            //    {
            //        categoryid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("categories_pkey", x => x.categoryid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "globalstats",
            //    columns: table => new
            //    {
            //        id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        totallossamount = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
            //        lastupdated = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("globalstats_pkey", x => x.id);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "titles",
            //    columns: table => new
            //    {
            //        titleid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
            //        isunique = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("titles_pkey", x => x.titleid);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "activities",
            //    columns: table => new
            //    {
            //        activityid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        categoryid = table.Column<int>(type: "integer", nullable: true),
            //        mentionscount = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
            //        isverified = table.Column<bool>(type: "boolean", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("activities_pkey", x => x.activityid);
            //        table.ForeignKey(
            //            name: "fk_category",
            //            column: x => x.categoryid,
            //            principalTable: "categories",
            //            principalColumn: "categoryid",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "users",
            //    columns: table => new
            //    {
            //        userid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        titleid = table.Column<int>(type: "integer", nullable: true),
            //        username = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
            //        email = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
            //        passwordhash = table.Column<string>(type: "text", nullable: false),
            //        profilepicture = table.Column<string>(type: "text", nullable: true),
            //        totalloss = table.Column<int>(type: "integer", nullable: true, defaultValue: 0),
            //        joineddate = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
            //        isbanned = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false),
            //        isadmin = table.Column<bool>(type: "boolean", nullable: true, defaultValue: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("users_pkey", x => x.userid);
            //        table.ForeignKey(
            //            name: "fk_title",
            //            column: x => x.titleid,
            //            principalTable: "titles",
            //            principalColumn: "titleid",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "logs",
            //    columns: table => new
            //    {
            //        logid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        userid = table.Column<int>(type: "integer", nullable: true),
            //        activityid = table.Column<int>(type: "integer", nullable: true),
            //        logtype = table.Column<int>(type: "log_type", nullable: false),
            //        amount = table.Column<int>(type: "integer", nullable: false),
            //        rating = table.Column<int>(type: "integer", nullable: false),
            //        comment = table.Column<string>(type: "text", nullable: true),
            //        createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
            //        isvisible = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
            //        likescount = table.Column<int>(type: "integer", nullable: true, defaultValue: 0)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("logs_pkey", x => x.logid);
            //        table.ForeignKey(
            //            name: "fk_activity",
            //            column: x => x.activityid,
            //            principalTable: "activities",
            //            principalColumn: "activityid",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "fk_user",
            //            column: x => x.userid,
            //            principalTable: "users",
            //            principalColumn: "userid",
            //            onDelete: ReferentialAction.SetNull);
            //    });

            migrationBuilder.CreateTable(
                name: "notifications",
                columns: table => new
                {
                    notificationid = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    userid = table.Column<int>(type: "integer", nullable: false),
                    message = table.Column<string>(type: "text", nullable: false),
                    isviewed = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: false, defaultValueSql: "now()"),
                    title = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    type = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    link = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("notifications_pkey", x => x.notificationid);
                    table.ForeignKey(
                        name: "fk_notification_user",
                        column: x => x.userid,
                        principalTable: "users",
                        principalColumn: "userid",
                        onDelete: ReferentialAction.Cascade);
                });

            //migrationBuilder.CreateTable(
            //    name: "userachievements",
            //    columns: table => new
            //    {
            //        userid = table.Column<int>(type: "integer", nullable: false),
            //        achievementid = table.Column<int>(type: "integer", nullable: false),
            //        unlockedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("userachievements_pkey", x => new { x.userid, x.achievementid });
            //        table.ForeignKey(
            //            name: "userachievements_achievementid_fkey",
            //            column: x => x.achievementid,
            //            principalTable: "achievements",
            //            principalColumn: "achievementid",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "userachievements_userid_fkey",
            //            column: x => x.userid,
            //            principalTable: "users",
            //            principalColumn: "userid",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "usertitles",
            //    columns: table => new
            //    {
            //        userid = table.Column<int>(type: "integer", nullable: false),
            //        titleid = table.Column<int>(type: "integer", nullable: false),
            //        unlockedat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()")
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("usertitles_pkey", x => new { x.userid, x.titleid });
            //        table.ForeignKey(
            //            name: "usertitles_titleid_fkey",
            //            column: x => x.titleid,
            //            principalTable: "titles",
            //            principalColumn: "titleid",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "usertitles_userid_fkey",
            //            column: x => x.userid,
            //            principalTable: "users",
            //            principalColumn: "userid",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "comments",
            //    columns: table => new
            //    {
            //        commentid = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        logid = table.Column<int>(type: "integer", nullable: true),
            //        authorid = table.Column<int>(type: "integer", nullable: true),
            //        content = table.Column<string>(type: "text", nullable: false),
            //        createdat = table.Column<DateTime>(type: "timestamp without time zone", nullable: true, defaultValueSql: "now()"),
            //        parentcommentid = table.Column<int>(type: "integer", nullable: true)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("comments_pkey", x => x.commentid);
            //        table.ForeignKey(
            //            name: "fk_author",
            //            column: x => x.authorid,
            //            principalTable: "users",
            //            principalColumn: "userid",
            //            onDelete: ReferentialAction.SetNull);
            //        table.ForeignKey(
            //            name: "fk_log",
            //            column: x => x.logid,
            //            principalTable: "logs",
            //            principalColumn: "logid",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "fk_parentcomment",
            //            column: x => x.parentcommentid,
            //            principalTable: "comments",
            //            principalColumn: "commentid",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateTable(
            //    name: "likes",
            //    columns: table => new
            //    {
            //        userid = table.Column<int>(type: "integer", nullable: false),
            //        logid = table.Column<int>(type: "integer", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_likes", x => new { x.userid, x.logid });
            //        table.ForeignKey(
            //            name: "FK_likes_logs_logid",
            //            column: x => x.logid,
            //            principalTable: "logs",
            //            principalColumn: "logid",
            //            onDelete: ReferentialAction.Cascade);
            //        table.ForeignKey(
            //            name: "FK_likes_users_userid",
            //            column: x => x.userid,
            //            principalTable: "users",
            //            principalColumn: "userid",
            //            onDelete: ReferentialAction.Cascade);
            //    });

            //migrationBuilder.CreateIndex(
            //    name: "achievements_code_key",
            //    table: "achievements",
            //    column: "code",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_activities_categoryid",
            //    table: "activities",
            //    column: "categoryid");

            //migrationBuilder.CreateIndex(
            //    name: "categories_name_key",
            //    table: "categories",
            //    column: "name",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_comments_authorid",
            //    table: "comments",
            //    column: "authorid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_comments_logid",
            //    table: "comments",
            //    column: "logid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_comments_parentcommentid",
            //    table: "comments",
            //    column: "parentcommentid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_likes_logid",
            //    table: "likes",
            //    column: "logid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_logs_activityid",
            //    table: "logs",
            //    column: "activityid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_logs_userid",
            //    table: "logs",
            //    column: "userid");

            migrationBuilder.CreateIndex(
                name: "IX_notifications_userid",
                table: "notifications",
                column: "userid");

            //migrationBuilder.CreateIndex(
            //    name: "titles_code_key",
            //    table: "titles",
            //    column: "code",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_userachievements_achievementid",
            //    table: "userachievements",
            //    column: "achievementid");

            //migrationBuilder.CreateIndex(
            //    name: "IX_users_titleid",
            //    table: "users",
            //    column: "titleid");

            //migrationBuilder.CreateIndex(
            //    name: "users_email_key",
            //    table: "users",
            //    column: "email",
            //    unique: true);

            //migrationBuilder.CreateIndex(
            //    name: "IX_usertitles_titleid",
            //    table: "usertitles",
            //    column: "titleid");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropTable(
            //    name: "comments");

            //migrationBuilder.DropTable(
            //    name: "globalstats");

            //migrationBuilder.DropTable(
            //    name: "likes");

            migrationBuilder.DropTable(
                name: "notifications");

            //migrationBuilder.DropTable(
            //    name: "userachievements");

            //migrationBuilder.DropTable(
            //    name: "usertitles");

            //migrationBuilder.DropTable(
            //    name: "logs");

            //migrationBuilder.DropTable(
            //    name: "achievements");

            //migrationBuilder.DropTable(
            //    name: "activities");

            //migrationBuilder.DropTable(
            //    name: "users");

            //migrationBuilder.DropTable(
            //    name: "categories");

            //migrationBuilder.DropTable(
            //    name: "titles");
        }
    }
}
