using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CustomerService.Migrations
{
    /// <inheritdoc />
    public partial class initializeToServer : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Doctors",
                type: "int",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "BookedStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4874));

            migrationBuilder.UpdateData(
                table: "BookedStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4876));

            migrationBuilder.UpdateData(
                table: "BookedStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4381));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4395));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4397));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4398));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4399));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4401));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4921));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4923));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4925));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4926));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4929));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4930));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4747));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4748));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4750));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4817));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4818));

            migrationBuilder.UpdateData(
                table: "VisitTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4697));

            migrationBuilder.UpdateData(
                table: "VisitTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 22, 35, 21, 718, DateTimeKind.Local).AddTicks(4699));

            migrationBuilder.CreateIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors",
                column: "DepartmentId",
                principalTable: "Departments",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Doctors_Departments_DepartmentId",
                table: "Doctors");

            migrationBuilder.DropIndex(
                name: "IX_Doctors_DepartmentId",
                table: "Doctors");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Doctors");

            migrationBuilder.UpdateData(
                table: "BookedStatuses",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5854));

            migrationBuilder.UpdateData(
                table: "BookedStatuses",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5856));

            migrationBuilder.UpdateData(
                table: "BookedStatuses",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5858));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5256));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5271));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5273));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5274));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5276));

            migrationBuilder.UpdateData(
                table: "CallPurposes",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5277));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5901));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5904));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5905));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5906));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5908));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 6,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5909));

            migrationBuilder.UpdateData(
                table: "Nationalities",
                keyColumn: "Id",
                keyValue: 7,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5911));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5801));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5803));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 3,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5804));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 4,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5806));

            migrationBuilder.UpdateData(
                table: "OutcomesOfCall",
                keyColumn: "Id",
                keyValue: 5,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5807));

            migrationBuilder.UpdateData(
                table: "VisitTypes",
                keyColumn: "Id",
                keyValue: 1,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5741));

            migrationBuilder.UpdateData(
                table: "VisitTypes",
                keyColumn: "Id",
                keyValue: 2,
                column: "CreatedAt",
                value: new DateTime(2026, 2, 22, 11, 53, 39, 392, DateTimeKind.Local).AddTicks(5745));
        }
    }
}
