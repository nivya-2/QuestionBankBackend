using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace QuestionBank.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class InitSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSkills_Interviews_InterviewId",
                table: "InterviewSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSkills_Skills_SkillId",
                table: "InterviewSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions");

            migrationBuilder.AlterColumn<float>(
                name: "Experience",
                table: "Interviews",
                type: "float",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "real",
                oldNullable: true);

            migrationBuilder.InsertData(
                table: "Interviews",
                columns: new[] { "Id", "CreatedBy", "CreatedOn", "Experience", "Role", "Status", "UpdatedBy", "UpdatedOn" },
                values: new object[] { 1, "seeder", new DateTime(2025, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc), 2f, "Backend Developer", 0, "seeder", new DateTime(2025, 6, 27, 0, 0, 0, 0, DateTimeKind.Utc) });

            migrationBuilder.InsertData(
                table: "Skills",
                columns: new[] { "Id", "CreatedOn", "Name", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, "C#", null },
                    { 2, null, "EF Core", null },
                    { 3, null, "Angular", null }
                });

            migrationBuilder.InsertData(
                table: "InterviewSkills",
                columns: new[] { "Id", "CreatedOn", "InterviewId", "SkillId", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, 1, 1, null },
                    { 2, null, 1, 2, null }
                });

            migrationBuilder.InsertData(
                table: "Questions",
                columns: new[] { "Id", "CreatedOn", "InterviewId", "QuestionText", "UpdatedOn" },
                values: new object[,]
                {
                    { 1, null, 1, "Explain DI in C#", null },
                    { 2, null, 1, "What is EF Core?", null }
                });

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSkills_Interviews_InterviewId",
                table: "InterviewSkills",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSkills_Skills_SkillId",
                table: "InterviewSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSkills_Interviews_InterviewId",
                table: "InterviewSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_InterviewSkills_Skills_SkillId",
                table: "InterviewSkills");

            migrationBuilder.DropForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions");

            migrationBuilder.DeleteData(
                table: "InterviewSkills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "InterviewSkills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Questions",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Interviews",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Skills",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.AlterColumn<float>(
                name: "Experience",
                table: "Interviews",
                type: "real",
                nullable: true,
                oldClrType: typeof(float),
                oldType: "float",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSkills_Interviews_InterviewId",
                table: "InterviewSkills",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_InterviewSkills_Skills_SkillId",
                table: "InterviewSkills",
                column: "SkillId",
                principalTable: "Skills",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Questions_Interviews_InterviewId",
                table: "Questions",
                column: "InterviewId",
                principalTable: "Interviews",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
