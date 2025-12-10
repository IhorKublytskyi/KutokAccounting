using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KutokAccounting.DataProvider.Migrations
{
    /// <inheritdoc />
    public partial class ChangeTransactionValuePropertyToMoneyType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<long>(
                name: "value",
                table: "transaction",
                type: "NUMERIC",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "value",
                table: "transaction",
                type: "INTEGER",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "NUMERIC");
        }
    }
}
