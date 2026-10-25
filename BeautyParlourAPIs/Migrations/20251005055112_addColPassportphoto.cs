using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BeautyParlourAPIs.Migrations
{
    /// <inheritdoc />
    public partial class addColPassportphoto : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Passportphotopath",
                table: "admissions",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Passportphotopath",
                table: "admissions");
        }
    }
}
