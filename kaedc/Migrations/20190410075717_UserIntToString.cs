using Microsoft.EntityFrameworkCore.Migrations;

namespace kaedc.Migrations
{
    public partial class UserIntToString : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "KaedcUser",
                table: "Transaction",
                nullable: true,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "KaedcUser",
                table: "Transaction",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}
