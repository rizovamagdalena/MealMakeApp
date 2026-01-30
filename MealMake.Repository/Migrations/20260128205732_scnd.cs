using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MealMake.Repository.Migrations
{
    /// <inheritdoc />
    public partial class scnd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MealCollectionCategories_CollectionCategories_CollectionCategoryId",
                table: "MealCollectionCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MealCollections_AspNetUsers_UserId",
                table: "MealCollections");

            migrationBuilder.DropIndex(
                name: "IX_CollectionCategories_Name",
                table: "CollectionCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CollectionCategories",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "CollectionCategories",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCategories_UserId",
                table: "CollectionCategories",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_CollectionCategories_AspNetUsers_UserId",
                table: "CollectionCategories",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealCollectionCategories_CollectionCategories_CollectionCategoryId",
                table: "MealCollectionCategories",
                column: "CollectionCategoryId",
                principalTable: "CollectionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_MealCollections_AspNetUsers_UserId",
                table: "MealCollections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_CollectionCategories_AspNetUsers_UserId",
                table: "CollectionCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MealCollectionCategories_CollectionCategories_CollectionCategoryId",
                table: "MealCollectionCategories");

            migrationBuilder.DropForeignKey(
                name: "FK_MealCollections_AspNetUsers_UserId",
                table: "MealCollections");

            migrationBuilder.DropIndex(
                name: "IX_CollectionCategories_UserId",
                table: "CollectionCategories");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "CollectionCategories");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "CollectionCategories",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_CollectionCategories_Name",
                table: "CollectionCategories",
                column: "Name",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_MealCollectionCategories_CollectionCategories_CollectionCategoryId",
                table: "MealCollectionCategories",
                column: "CollectionCategoryId",
                principalTable: "CollectionCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_MealCollections_AspNetUsers_UserId",
                table: "MealCollections",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
