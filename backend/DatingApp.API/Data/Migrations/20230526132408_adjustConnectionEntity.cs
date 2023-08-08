using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DatingApp.API.Data.Migrations
{
	/// <inheritdoc />
	public partial class adjustConnectionEntity : Migration
	{
		/// <inheritdoc />
		protected override void Up(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "ConnetionId",
				table: "Connections",
				newName: "ConnectionId");
		}

		/// <inheritdoc />
		protected override void Down(MigrationBuilder migrationBuilder)
		{
			migrationBuilder.RenameColumn(
				name: "ConnectionId",
				table: "Connections",
				newName: "ConnetionId");
		}
	}
}