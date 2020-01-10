using Microsoft.EntityFrameworkCore.Migrations;

namespace PTAnalitic.Infrastructure.Migrations
{
    public partial class ProductHistoryModified : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "ProductId",
                table: "ProductHistory",
                nullable: true,
                oldClrType: typeof(long));

            migrationBuilder.AlterColumn<string>(
                name: "PointOfSale",
                table: "ProductHistory",
                nullable: true,
                oldClrType: typeof(long));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "ProductId",
                table: "ProductHistory",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "PointOfSale",
                table: "ProductHistory",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
