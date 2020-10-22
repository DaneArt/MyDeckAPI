using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace MyDeckAPI.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Categories",
                columns: table => new
                {
                    Category_Name = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Categories", x => x.Category_Name);
                });

            migrationBuilder.CreateTable(
                name: "Files",
                columns: table => new
                {
                    File_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Type = table.Column<string>(nullable: false),
                    Md5 = table.Column<string>(nullable: false),
                    Path = table.Column<string>(nullable: false),
                    Size = table.Column<long>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Files", x => x.File_Id);
                });

            migrationBuilder.CreateTable(
                name: "Roles",
                columns: table => new
                {
                    Role_Name = table.Column<string>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Roles", x => x.Role_Name);
                });

            migrationBuilder.CreateTable(
                name: "Decks",
                columns: table => new
                {
                    Deck_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Title = table.Column<string>(nullable: false, defaultValue: ""),
                    Description = table.Column<string>(nullable: false, defaultValue: ""),
                    IsPrivate = table.Column<bool>(nullable: false),
                    Icon = table.Column<Guid>(nullable: false),
                    Category_Name = table.Column<string>(nullable: false, defaultValue: "Others"),
                    Author = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Decks", x => x.Deck_Id);
                    table.ForeignKey(
                        name: "FK_Decks_Categories_Category_Name",
                        column: x => x.Category_Name,
                        principalTable: "Categories",
                        principalColumn: "Category_Name",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Decks_Files_Icon",
                        column: x => x.Icon,
                        principalTable: "Files",
                        principalColumn: "File_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    User_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    UserName = table.Column<string>(nullable: false),
                    Email = table.Column<string>(nullable: false),
                    GoogleId = table.Column<string>(nullable: true),
                    Password = table.Column<byte[]>(maxLength: 45, nullable: true),
                    Avatar = table.Column<Guid>(nullable: false),
                    Locale = table.Column<string>(nullable: true),
                    Role_Name = table.Column<string>(nullable: true, defaultValue: "User")
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.User_Id);
                    table.ForeignKey(
                        name: "FK_Users_Files_Avatar",
                        column: x => x.Avatar,
                        principalTable: "Files",
                        principalColumn: "File_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Users_Roles_Role_Name",
                        column: x => x.Role_Name,
                        principalTable: "Roles",
                        principalColumn: "Role_Name",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Cards",
                columns: table => new
                {
                    Card_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Answer = table.Column<Guid>(nullable: false),
                    Question = table.Column<Guid>(nullable: false),
                    Parent_Deck_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cards", x => x.Card_Id);
                    table.ForeignKey(
                        name: "FK_Cards_Files_Answer",
                        column: x => x.Answer,
                        principalTable: "Files",
                        principalColumn: "File_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Cards_Decks_Parent_Deck_Id",
                        column: x => x.Parent_Deck_Id,
                        principalTable: "Decks",
                        principalColumn: "Deck_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Cards_Files_Question",
                        column: x => x.Question,
                        principalTable: "Files",
                        principalColumn: "File_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Sessions",
                columns: table => new
                {
                    Session_Id = table.Column<Guid>(nullable: false),
                    User_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    RefreshToken = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Sessions", x => new { x.Session_Id, x.User_Id });
                    table.ForeignKey(
                        name: "FK_Sessions_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Subscribes",
                columns: table => new
                {
                    Follower_Id = table.Column<Guid>(nullable: false),
                    Publisher_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Subscribe_Id = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subscribes", x => new { x.Follower_Id, x.Publisher_Id });
                    table.ForeignKey(
                        name: "FK_Subscribes_Users_Follower_Id",
                        column: x => x.Follower_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subscribes_Users_Publisher_Id",
                        column: x => x.Publisher_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "UserDecks",
                columns: table => new
                {
                    User_Id = table.Column<Guid>(nullable: false),
                    Deck_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserDecks", x => new { x.User_Id, x.Deck_Id });
                    table.ForeignKey(
                        name: "FK_UserDecks_Decks_Deck_Id",
                        column: x => x.Deck_Id,
                        principalTable: "Decks",
                        principalColumn: "Deck_Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_UserDecks_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Statistics",
                columns: table => new
                {
                    User_Id = table.Column<Guid>(nullable: false),
                    Card_Id = table.Column<Guid>(nullable: false),
                    LastUpdate = table.Column<DateTime>(nullable: false),
                    Tag = table.Column<string>(nullable: true),
                    Wins = table.Column<int>(nullable: false),
                    Trains = table.Column<int>(nullable: false),
                    Lvl = table.Column<int>(nullable: false),
                    Last_Train = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Statistics", x => new { x.User_Id, x.Card_Id });
                    table.ForeignKey(
                        name: "FK_Statistics_Cards_Card_Id",
                        column: x => x.Card_Id,
                        principalTable: "Cards",
                        principalColumn: "Card_Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Statistics_Users_User_Id",
                        column: x => x.User_Id,
                        principalTable: "Users",
                        principalColumn: "User_Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.InsertData(
                table: "Categories",
                columns: new[] { "Category_Name", "LastUpdate", "Tag" },
                values: new object[,]
                {
                    { "Math", new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(4837), null },
                    { "Foreign Languages", new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6615), null },
                    { "Chemistry", new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6661), null },
                    { "Art", new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6664), null },
                    { "IT", new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6666), null },
                    { "Others", new DateTime(2020, 8, 11, 2, 4, 28, 888, DateTimeKind.Utc).AddTicks(6667), null }
                });

            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Role_Name", "LastUpdate", "Tag" },
                values: new object[,]
                {
                    { "Owner", new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(737), null },
                    { "Administrator", new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2043), null },
                    { "Support", new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2075), null },
                    { "Content Maker", new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2077), null },
                    { "User", new DateTime(2020, 8, 11, 2, 4, 28, 895, DateTimeKind.Utc).AddTicks(2079), null }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Answer",
                table: "Cards",
                column: "Answer");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Parent_Deck_Id",
                table: "Cards",
                column: "Parent_Deck_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Cards_Question",
                table: "Cards",
                column: "Question");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_Category_Name",
                table: "Decks",
                column: "Category_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Decks_Icon",
                table: "Decks",
                column: "Icon");

            migrationBuilder.CreateIndex(
                name: "IX_Sessions_User_Id",
                table: "Sessions",
                column: "User_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Statistics_Card_Id",
                table: "Statistics",
                column: "Card_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Subscribes_Publisher_Id",
                table: "Subscribes",
                column: "Publisher_Id");

            migrationBuilder.CreateIndex(
                name: "IX_UserDecks_Deck_Id",
                table: "UserDecks",
                column: "Deck_Id");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Avatar",
                table: "Users",
                column: "Avatar");

            migrationBuilder.CreateIndex(
                name: "IX_Users_Email",
                table: "Users",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Users_Role_Name",
                table: "Users",
                column: "Role_Name");

            migrationBuilder.CreateIndex(
                name: "IX_Users_UserName",
                table: "Users",
                column: "UserName",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Sessions");

            migrationBuilder.DropTable(
                name: "Statistics");

            migrationBuilder.DropTable(
                name: "Subscribes");

            migrationBuilder.DropTable(
                name: "UserDecks");

            migrationBuilder.DropTable(
                name: "Cards");

            migrationBuilder.DropTable(
                name: "Users");

            migrationBuilder.DropTable(
                name: "Decks");

            migrationBuilder.DropTable(
                name: "Roles");

            migrationBuilder.DropTable(
                name: "Categories");

            migrationBuilder.DropTable(
                name: "Files");
        }
    }
}
