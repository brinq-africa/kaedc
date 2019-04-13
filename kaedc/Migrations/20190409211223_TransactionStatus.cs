using Microsoft.EntityFrameworkCore.Migrations;

namespace kaedc.Migrations
{
    public partial class TransactionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "transactionsStatus",
                table: "Transaction",
                nullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "ServiceProviderPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "CoordinatorPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "BrinqFullPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<decimal>(
                name: "AgentPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(int));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "transactionsStatus",
                table: "Transaction");

            migrationBuilder.AlterColumn<int>(
                name: "ServiceProviderPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "CoordinatorPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "BrinqFullPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<int>(
                name: "AgentPercentage",
                table: "Service",
                nullable: false,
                oldClrType: typeof(decimal));
        }
    }
}
