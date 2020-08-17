using Microsoft.EntityFrameworkCore.Migrations;

namespace TTDataAccessLibrary.Migrations
{
    public partial class AddSeedFlags : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Flags",
                columns: new[] { "BitFlag", "Name" },
                values: new object[,]
                {
                    { 1, "IsFixIncome" },
                    { 2, "IsConvertible" },
                    { 4, "IsSwap" },
                    { 8, "IsCash" },
                    { 16, "IsFuture" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Flags",
                keyColumns: new[] { "BitFlag", "Name" },
                keyValues: new object[] { 1, "IsFixIncome" });

            migrationBuilder.DeleteData(
                table: "Flags",
                keyColumns: new[] { "BitFlag", "Name" },
                keyValues: new object[] { 2, "IsConvertible" });

            migrationBuilder.DeleteData(
                table: "Flags",
                keyColumns: new[] { "BitFlag", "Name" },
                keyValues: new object[] { 4, "IsSwap" });

            migrationBuilder.DeleteData(
                table: "Flags",
                keyColumns: new[] { "BitFlag", "Name" },
                keyValues: new object[] { 8, "IsCash" });

            migrationBuilder.DeleteData(
                table: "Flags",
                keyColumns: new[] { "BitFlag", "Name" },
                keyValues: new object[] { 16, "IsFuture" });
        }
    }
}
