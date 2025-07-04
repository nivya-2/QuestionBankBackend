using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace QuestionBank.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class changedexperiencedatatype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Interviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "Experience",
                value: 2m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Interviews",
                keyColumn: "Id",
                keyValue: 1,
                column: "Experience",
                value: 2f);
        }
    }
}
