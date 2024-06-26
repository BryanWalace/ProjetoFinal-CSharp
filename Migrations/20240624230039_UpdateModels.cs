using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Loja.Migrations
{
    /// <inheritdoc />
    public partial class UpdateModels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "DataContatacao",
                table: "Contratos",
                newName: "DataContratacao");

            migrationBuilder.AddColumn<int>(
                name: "Id",
                table: "DepositoProdutos",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Id",
                table: "DepositoProdutos");

            migrationBuilder.RenameColumn(
                name: "DataContratacao",
                table: "Contratos",
                newName: "DataContatacao");
        }
    }
}
