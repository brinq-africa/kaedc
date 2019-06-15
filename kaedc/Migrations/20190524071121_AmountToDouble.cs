using Microsoft.EntityFrameworkCore.Migrations;

namespace kaedc.Migrations
{
    public partial class AmountToDouble : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<double>(
                name: "Amount",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Amount",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(double));
        }
    }
}
