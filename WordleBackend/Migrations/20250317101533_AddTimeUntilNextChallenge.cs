using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WordleBackend.Migrations
{
    /// <inheritdoc />
    public partial class AddTimeUntilNextChallenge : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "TimeUntilNextChallenge",
                table: "DailyChallengeParticipations",
                type: "REAL",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TimeUntilNextChallenge",
                table: "DailyChallengeParticipations");
        }
    }
}
