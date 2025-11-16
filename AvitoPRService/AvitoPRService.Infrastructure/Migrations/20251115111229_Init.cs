using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AvitoPRService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "team",
                columns: table => new
                {
                    team_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_team", x => x.team_name);
                });

            migrationBuilder.CreateTable(
                name: "user",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    username = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    team_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_user", x => x.user_id);
                    table.ForeignKey(
                        name: "FK_user_team_team_name",
                        column: x => x.team_name,
                        principalTable: "team",
                        principalColumn: "team_name",
                        onDelete: ReferentialAction.SetNull);
                });

            migrationBuilder.CreateTable(
                name: "pull_request",
                columns: table => new
                {
                    pull_request_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pull_request_name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    author_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    status = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    merged_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_pull_request", x => x.pull_request_id);
                    table.ForeignKey(
                        name: "FK_pull_request_user_author_id",
                        column: x => x.author_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "reviewer",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    pull_request_id = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_reviewer", x => new { x.user_id, x.pull_request_id });
                    table.ForeignKey(
                        name: "FK_reviewer_pull_request_pull_request_id",
                        column: x => x.pull_request_id,
                        principalTable: "pull_request",
                        principalColumn: "pull_request_id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_reviewer_user_user_id",
                        column: x => x.user_id,
                        principalTable: "user",
                        principalColumn: "user_id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_pull_request_author_id",
                table: "pull_request",
                column: "author_id");

            migrationBuilder.CreateIndex(
                name: "idx_reviewer_pr",
                table: "reviewer",
                column: "pull_request_id");

            migrationBuilder.CreateIndex(
                name: "IX_team_team_name",
                table: "team",
                column: "team_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_user_team_name",
                table: "user",
                column: "team_name");

            migrationBuilder.CreateIndex(
                name: "IX_user_username",
                table: "user",
                column: "username");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "reviewer");

            migrationBuilder.DropTable(
                name: "pull_request");

            migrationBuilder.DropTable(
                name: "user");

            migrationBuilder.DropTable(
                name: "team");
        }
    }
}
