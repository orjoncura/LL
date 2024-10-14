using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LL.Data.Migrations
{
    /// <inheritdoc />
    public partial class Message : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoginHistory_Users_UserId",
                table: "LoginHistory");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageLogs_Users_ReceiverId",
                table: "MessageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_MessageLogs_Users_SenderId",
                table: "MessageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages");

            migrationBuilder.DropIndex(
                name: "IX_MessageLogs_ReceiverId",
                table: "MessageLogs");

            migrationBuilder.DropIndex(
                name: "IX_LoginHistory_UserId",
                table: "LoginHistory");

            migrationBuilder.DropColumn(
                name: "PersonId",
                table: "UserLogs");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "Messages");

            migrationBuilder.DropColumn(
                name: "ReceiverId",
                table: "MessageLogs");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "LoginHistory");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "Messages",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_SenderId",
                table: "Messages",
                newName: "IX_Messages_RecipientId");

            migrationBuilder.RenameColumn(
                name: "SenderId",
                table: "MessageLogs",
                newName: "RecipientId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageLogs_SenderId",
                table: "MessageLogs",
                newName: "IX_MessageLogs_RecipientId");

            migrationBuilder.AddColumn<string>(
                name: "IP",
                table: "LoginHistory",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "NewUserRequests",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IP = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NewUserRequests", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NewUserRequests_Users_UserId",
                        column: x => x.UserId,
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
                    Token = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
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

            migrationBuilder.CreateIndex(
                name: "IX_NewUserRequests_UserId",
                table: "NewUserRequests",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_ResetPasswordRequests_UserId",
                table: "ResetPasswordRequests",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_MessageLogs_Users_RecipientId",
                table: "MessageLogs",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_RecipientId",
                table: "Messages",
                column: "RecipientId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MessageLogs_Users_RecipientId",
                table: "MessageLogs");

            migrationBuilder.DropForeignKey(
                name: "FK_Messages_Users_RecipientId",
                table: "Messages");

            migrationBuilder.DropTable(
                name: "NewUserRequests");

            migrationBuilder.DropTable(
                name: "ResetPasswordRequests");

            migrationBuilder.DropColumn(
                name: "IP",
                table: "LoginHistory");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "Messages",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_Messages_RecipientId",
                table: "Messages",
                newName: "IX_Messages_SenderId");

            migrationBuilder.RenameColumn(
                name: "RecipientId",
                table: "MessageLogs",
                newName: "SenderId");

            migrationBuilder.RenameIndex(
                name: "IX_MessageLogs_RecipientId",
                table: "MessageLogs",
                newName: "IX_MessageLogs_SenderId");

            migrationBuilder.AddColumn<int>(
                name: "PersonId",
                table: "UserLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "Messages",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "ReceiverId",
                table: "MessageLogs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "LoginHistory",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ReceiverId",
                table: "Messages",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_MessageLogs_ReceiverId",
                table: "MessageLogs",
                column: "ReceiverId");

            migrationBuilder.CreateIndex(
                name: "IX_LoginHistory_UserId",
                table: "LoginHistory",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoginHistory_Users_UserId",
                table: "LoginHistory",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageLogs_Users_ReceiverId",
                table: "MessageLogs",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MessageLogs_Users_SenderId",
                table: "MessageLogs",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_ReceiverId",
                table: "Messages",
                column: "ReceiverId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Messages_Users_SenderId",
                table: "Messages",
                column: "SenderId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
