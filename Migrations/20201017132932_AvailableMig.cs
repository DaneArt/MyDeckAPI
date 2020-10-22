using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDeckAPI.Migrations
{
    public partial class AvailableMig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AvailableQuickTrain",
                table: "Decks",
                nullable: false,
                defaultValue: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Art",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 975, DateTimeKind.Utc).AddTicks(7470));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Chemistry",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 975, DateTimeKind.Utc).AddTicks(7468));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Foreign Languages",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 975, DateTimeKind.Utc).AddTicks(7435));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "IT",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 975, DateTimeKind.Utc).AddTicks(7472));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Math",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 975, DateTimeKind.Utc).AddTicks(6171));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Others",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 975, DateTimeKind.Utc).AddTicks(7474));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Administrator",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 980, DateTimeKind.Utc).AddTicks(433));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Content Maker",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 980, DateTimeKind.Utc).AddTicks(462));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Owner",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 979, DateTimeKind.Utc).AddTicks(9605));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Support",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 980, DateTimeKind.Utc).AddTicks(461));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "User",
                column: "LastUpdate",
                value: new DateTime(2020, 10, 17, 13, 29, 31, 980, DateTimeKind.Utc).AddTicks(464));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AvailableQuickTrain",
                table: "Decks");

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Art",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8891));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Chemistry",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8890));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Foreign Languages",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8864));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "IT",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8893));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Math",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(7284));

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Others",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8894));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Administrator",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(222));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Content Maker",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(301));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Owner",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 692, DateTimeKind.Utc).AddTicks(7685));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Support",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(296));

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "User",
                column: "LastUpdate",
                value: new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(302));
        }
    }
}
