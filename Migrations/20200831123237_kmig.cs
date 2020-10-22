using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDeckAPI.Migrations
{
    public partial class kmig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                maxLength: 45,
                nullable: false,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(45)",
                oldMaxLength: 45,
                oldNullable: true);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Art",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8891), "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Chemistry",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8890), "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Foreign Languages",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8864), "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "IT",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8893), "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Math",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(7284), "" });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Others",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 686, DateTimeKind.Utc).AddTicks(8894), "" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Administrator",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(222), "" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Content Maker",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(301), "" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Owner",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 692, DateTimeKind.Utc).AddTicks(7685), "" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Support",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(296), "" });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "User",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 31, 12, 32, 36, 693, DateTimeKind.Utc).AddTicks(302), "" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<byte[]>(
                name: "Password",
                table: "Users",
                type: "varbinary(45)",
                maxLength: 45,
                nullable: true,
                oldClrType: typeof(byte[]),
                oldMaxLength: 45);

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Art",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6664), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Chemistry",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6661), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Foreign Languages",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6615), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "IT",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6666), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Math",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(4837), null });

            migrationBuilder.UpdateData(
                table: "Categories",
                keyColumn: "Category_Name",
                keyValue: "Others",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6667), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Administrator",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2043), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Content Maker",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2077), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Owner",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(737), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "Support",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2075), null });

            migrationBuilder.UpdateData(
                table: "Roles",
                keyColumn: "Role_Name",
                keyValue: "User",
                columns: new[] { "LastUpdate", "Tag" },
                values: new object[] { new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2079), null });
        }
    }
}
