using Microsoft.EntityFrameworkCore.Migrations;

namespace DockerMvc.Migrations
{
    public partial class proplayer : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_RoleCatId",
                table: "SubCategorias");

            migrationBuilder.DropIndex(
                name: "IX_SubCategorias_RoleCatId",
                table: "SubCategorias");

            migrationBuilder.DropColumn(
                name: "RoleCatId",
                table: "SubCategorias");

            migrationBuilder.AddColumn<string>(
                name: "Image",
                table: "SubCategorias",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_CatId",
                table: "SubCategorias",
                column: "CatId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_CatId",
                table: "SubCategorias",
                column: "CatId",
                principalTable: "Categorias",
                principalColumn: "CatId",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_SubCategorias_Categorias_CatId",
                table: "SubCategorias");

            migrationBuilder.DropIndex(
                name: "IX_SubCategorias_CatId",
                table: "SubCategorias");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "SubCategorias");

            migrationBuilder.AddColumn<int>(
                name: "RoleCatId",
                table: "SubCategorias",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_SubCategorias_RoleCatId",
                table: "SubCategorias",
                column: "RoleCatId");

            migrationBuilder.AddForeignKey(
                name: "FK_SubCategorias_Categorias_RoleCatId",
                table: "SubCategorias",
                column: "RoleCatId",
                principalTable: "Categorias",
                principalColumn: "CatId",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
