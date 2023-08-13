using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhosThatPokemonAPI.Migrations
{
    /// <inheritdoc />
    public partial class first : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Pokemon",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    picture = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pokemon", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Type",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Type", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    pseudo = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    level = table.Column<int>(type: "int", nullable: false),
                    xp = table.Column<int>(type: "int", nullable: false),
                    password = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    isAdmin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "PokemonType",
                columns: table => new
                {
                    PokemonsId = table.Column<int>(type: "int", nullable: false),
                    TypesId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PokemonType", x => new { x.PokemonsId, x.TypesId });
                    table.ForeignKey(
                        name: "FK_PokemonType_Pokemon_PokemonsId",
                        column: x => x.PokemonsId,
                        principalTable: "Pokemon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PokemonType_Type_TypesId",
                        column: x => x.TypesId,
                        principalTable: "Type",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserPokemon",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PokemonId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    IsWin = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserPokemon", x => x.Id);
                    table.ForeignKey(
                        name: "FK_UserPokemon_Pokemon_PokemonId",
                        column: x => x.PokemonId,
                        principalTable: "Pokemon",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserPokemon_User_UserId",
                        column: x => x.UserId,
                        principalTable: "User",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PokemonType_TypesId",
                table: "PokemonType",
                column: "TypesId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPokemon_PokemonId",
                table: "UserPokemon",
                column: "PokemonId");

            migrationBuilder.CreateIndex(
                name: "IX_UserPokemon_UserId",
                table: "UserPokemon",
                column: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PokemonType");

            migrationBuilder.DropTable(
                name: "UserPokemon");

            migrationBuilder.DropTable(
                name: "Type");

            migrationBuilder.DropTable(
                name: "Pokemon");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
