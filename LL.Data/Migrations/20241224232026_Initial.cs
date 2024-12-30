using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LL.Data.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Languages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Languages", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageContents",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageContents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "MessageStatuses",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SeminarWordRank",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeminarWordRank", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "WordTypes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "LoginHistory",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoginHistory", x => x.Id);
                    table.ForeignKey(
                        name: "FK_LoginHistory_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    RecipientId = table.Column<int>(type: "int", nullable: true),
                    RecipientAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    MessageContentId = table.Column<int>(type: "int", nullable: false),
                    DateSend = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_MessageContents_MessageContentId",
                        column: x => x.MessageContentId,
                        principalTable: "MessageContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_MessageStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MessageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Messages_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "NewUserRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewUserRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewUserRequests_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ResetPasswordRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ResetPasswordRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ResetPasswordRequests_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Seminar",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageFromId = table.Column<int>(type: "int", nullable: false),
                    LanguageToId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seminar", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seminar_Languages_LanguageFromId",
                        column: x => x.LanguageFromId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seminar_Languages_LanguageToId",
                        column: x => x.LanguageToId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Seminar_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserLogs",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Salt = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_UserLogs_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserTokens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Token = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Expiration = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Words",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    AudioPath = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LanguageId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Words", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Words_Languages_LanguageId",
                        column: x => x.LanguageId,
                        principalTable: "Languages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Words_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "MessageLogs",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    MessageId = table.Column<int>(type: "int", nullable: false),
                    RecipientId = table.Column<int>(type: "int", nullable: true),
                    RecipientAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Subject = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    StatusId = table.Column<int>(type: "int", nullable: false),
                    MessageContentId = table.Column<int>(type: "int", nullable: false),
                    DateSend = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageLogs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_MessageLogs_MessageContents_MessageContentId",
                        column: x => x.MessageContentId,
                        principalTable: "MessageContents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageLogs_MessageStatuses_StatusId",
                        column: x => x.StatusId,
                        principalTable: "MessageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageLogs_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageLogs_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_MessageLogs_Users_RecipientId",
                        column: x => x.RecipientId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SeminarWords",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    SeminarId = table.Column<int>(type: "int", nullable: false),
                    SeminarWordRankId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SeminarWords", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SeminarWords_SeminarWordRank_SeminarWordRankId",
                        column: x => x.SeminarWordRankId,
                        principalTable: "SeminarWordRank",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeminarWords_Seminar_SeminarId",
                        column: x => x.SeminarId,
                        principalTable: "Seminar",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeminarWords_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_SeminarWords_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordLinks",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SourceId = table.Column<int>(type: "int", nullable: false),
                    TargetId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordLinks_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordLinks_Words_SourceId",
                        column: x => x.SourceId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordLinks_Words_TargetId",
                        column: x => x.TargetId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordMeanings",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    WordId = table.Column<int>(type: "int", nullable: false),
                    TypeId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordMeanings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordMeanings_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordMeanings_WordTypes_TypeId",
                        column: x => x.TypeId,
                        principalTable: "WordTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordMeanings_Words_WordId",
                        column: x => x.WordId,
                        principalTable: "Words",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Statements",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    SeminarWordId = table.Column<int>(type: "int", nullable: false),
                    Original = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Translated = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statements", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Statements_SeminarWords_SeminarWordId",
                        column: x => x.SeminarWordId,
                        principalTable: "SeminarWords",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statements_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "WordDefinitions",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    WordMeaningId = table.Column<int>(type: "int", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    CreatedById = table.Column<int>(type: "int", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_WordDefinitions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_WordDefinitions_Users_CreatedById",
                        column: x => x.CreatedById,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_WordDefinitions_WordMeanings_WordMeaningId",
                        column: x => x.WordMeaningId,
                        principalTable: "WordMeanings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Languages",
                columns: new[] { "Id", "CreatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 768, DateTimeKind.Unspecified).AddTicks(8322), new TimeSpan(0, 0, 0, 0, 0)), "English" },
                    { 2, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 768, DateTimeKind.Unspecified).AddTicks(8331), new TimeSpan(0, 0, 0, 0, 0)), "Spanish" }
                });

            migrationBuilder.InsertData(
                table: "MessageStatuses",
                columns: new[] { "Id", "CreatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 768, DateTimeKind.Unspecified).AddTicks(7708), new TimeSpan(0, 0, 0, 0, 0)), "Queued" },
                    { 2, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 768, DateTimeKind.Unspecified).AddTicks(7754), new TimeSpan(0, 0, 0, 0, 0)), "Sent" },
                    { 3, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 768, DateTimeKind.Unspecified).AddTicks(7756), new TimeSpan(0, 0, 0, 0, 0)), "Failed" }
                });

            migrationBuilder.InsertData(
                table: "SeminarWordRank",
                columns: new[] { "Id", "CreatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(3673), new TimeSpan(0, 0, 0, 0, 0)), "High" },
                    { 2, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(3679), new TimeSpan(0, 0, 0, 0, 0)), "Medium" },
                    { 3, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(3681), new TimeSpan(0, 0, 0, 0, 0)), "Low" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "IsActive", "PasswordHash", "Salt" },
                values: new object[] { -1, "", false, "", "" });

            migrationBuilder.InsertData(
                table: "WordTypes",
                columns: new[] { "Id", "CreatedDate", "Value" },
                values: new object[,]
                {
                    { 1, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(1050), new TimeSpan(0, 0, 0, 0, 0)), "Noun" },
                    { 2, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(1057), new TimeSpan(0, 0, 0, 0, 0)), "Verb" },
                    { 3, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(1058), new TimeSpan(0, 0, 0, 0, 0)), "Adjective" },
                    { 4, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(1059), new TimeSpan(0, 0, 0, 0, 0)), "Adverb" },
                    { 5, new DateTimeOffset(new DateTime(2024, 12, 24, 23, 20, 26, 769, DateTimeKind.Unspecified).AddTicks(1061), new TimeSpan(0, 0, 0, 0, 0)), "Interjection" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoginHistory_CreatedById",
                table: "LoginHistory",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_CreatedById",
                table: "MessageLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_MessageContentId",
                table: "MessageLogs",
                column: "MessageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_MessageId",
                table: "MessageLogs",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_RecipientId",
                table: "MessageLogs",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_StatusId",
                table: "MessageLogs",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_MessageContentId",
                table: "Messages",
                column: "MessageContentId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                column: "RecipientId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_StatusId",
                table: "Messages",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_NewUserRequests_CreatedById",
                table: "NewUserRequests",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordRequests_UserId",
                table: "ResetPasswordRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Seminar_CreatedById",
                table: "Seminar",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Seminar_LanguageFromId",
                table: "Seminar",
                column: "LanguageFromId");

            migrationBuilder.CreateIndex(
                name: "IX_Seminar_LanguageToId",
                table: "Seminar",
                column: "LanguageToId");

            migrationBuilder.CreateIndex(
                name: "IX_SeminarWords_CreatedById",
                table: "SeminarWords",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_SeminarWords_SeminarId",
                table: "SeminarWords",
                column: "SeminarId");

            migrationBuilder.CreateIndex(
                name: "IX_SeminarWords_SeminarWordRankId",
                table: "SeminarWords",
                column: "SeminarWordRankId");

            migrationBuilder.CreateIndex(
                name: "IX_SeminarWords_WordId",
                table: "SeminarWords",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_CreatedById",
                table: "Statements",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Statements_SeminarWordId",
                table: "Statements",
                column: "SeminarWordId");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_CreatedById",
                table: "UserLogs",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_UserLogs_UserId",
                table: "UserLogs",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_UserTokens_UserId",
                table: "UserTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_WordDefinitions_CreatedById",
                table: "WordDefinitions",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WordDefinitions_WordMeaningId",
                table: "WordDefinitions",
                column: "WordMeaningId");

            migrationBuilder.CreateIndex(
                name: "IX_WordLinks_CreatedById",
                table: "WordLinks",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WordLinks_SourceId",
                table: "WordLinks",
                column: "SourceId");

            migrationBuilder.CreateIndex(
                name: "IX_WordLinks_TargetId",
                table: "WordLinks",
                column: "TargetId");

            migrationBuilder.CreateIndex(
                name: "IX_WordMeanings_CreatedById",
                table: "WordMeanings",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_WordMeanings_TypeId",
                table: "WordMeanings",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_WordMeanings_WordId",
                table: "WordMeanings",
                column: "WordId");

            migrationBuilder.CreateIndex(
                name: "IX_Words_CreatedById",
                table: "Words",
                column: "CreatedById");

            migrationBuilder.CreateIndex(
                name: "IX_Words_LanguageId",
                table: "Words",
                column: "LanguageId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoginHistory");

            migrationBuilder.DropTable(
                name: "MessageLogs");

            migrationBuilder.DropTable(
                name: "NewUserRequests");

            migrationBuilder.DropTable(
                name: "ResetPasswordRequests");

            migrationBuilder.DropTable(
                name: "Statements");

            migrationBuilder.DropTable(
                name: "UserLogs");

            migrationBuilder.DropTable(
                name: "UserTokens");

            migrationBuilder.DropTable(
                name: "WordDefinitions");

            migrationBuilder.DropTable(
                name: "WordLinks");

            migrationBuilder.DropTable(
                name: "Messages");

            migrationBuilder.DropTable(
                name: "SeminarWords");

            migrationBuilder.DropTable(
                name: "WordMeanings");

            migrationBuilder.DropTable(
                name: "MessageContents");

            migrationBuilder.DropTable(
                name: "MessageStatuses");

            migrationBuilder.DropTable(
                name: "SeminarWordRank");

            migrationBuilder.DropTable(
                name: "Seminar");

            migrationBuilder.DropTable(
                name: "WordTypes");

            migrationBuilder.DropTable(
                name: "Words");

            migrationBuilder.DropTable(
                name: "Languages");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
