using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace TaskManagementSystem.Api.Migrations
{
    /// <inheritdoc />
    public partial class SeedTask : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Tasks",
                columns: new[] { "Id", "Description", "DueDate", "IsCompleted", "Title" },
                values: new object[,]
                {
                    { new Guid("11111111-1111-1111-1111-111111111111"), "Complete the final draft of the project report and send it to the manager.", new DateTime(2025, 8, 19, 10, 0, 0, 0, DateTimeKind.Unspecified), false, "Finish project report" },
                    { new Guid("22222222-2222-2222-2222-222222222222"), "Buy vegetables, fruits, and milk for the week.", new DateTime(2025, 8, 17, 18, 0, 0, 0, DateTimeKind.Unspecified), false, "Grocery shopping" },
                    { new Guid("33333333-3333-3333-3333-333333333333"), "Routine dental check-up at the downtown clinic.", new DateTime(2025, 8, 23, 8, 30, 0, 0, DateTimeKind.Unspecified), false, "Dentist appointment" },
                    { new Guid("44444444-4444-4444-4444-444444444444"), "Pay the monthly electricity bill before the due date.", new DateTime(2025, 8, 14, 23, 59, 0, 0, DateTimeKind.Unspecified), true, "Pay electricity bill" },
                    { new Guid("55555555-5555-5555-5555-555555555555"), "Weekly sync-up meeting with the development team.", new DateTime(2025, 8, 16, 20, 0, 0, 0, DateTimeKind.Unspecified), false, "Team meeting" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("11111111-1111-1111-1111-111111111111"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("22222222-2222-2222-2222-222222222222"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("33333333-3333-3333-3333-333333333333"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("44444444-4444-4444-4444-444444444444"));

            migrationBuilder.DeleteData(
                table: "Tasks",
                keyColumn: "Id",
                keyValue: new Guid("55555555-5555-5555-5555-555555555555"));
        }
    }
}
