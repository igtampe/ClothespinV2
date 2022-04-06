using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Clothespin2.Data.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Data = table.Column<byte[]>(type: "bytea", nullable: true),
                    Type = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.ID);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Username = table.Column<string>(type: "text", nullable: false),
                    Password = table.Column<string>(type: "text", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Username);
                });

            migrationBuilder.CreateTable(
                name: "Wearable",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Type = table.Column<int>(type: "integer", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false),
                    Color = table.Column<string>(type: "text", nullable: false),
                    OwnerUsername = table.Column<string>(type: "text", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wearable", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Wearable_User_OwnerUsername",
                        column: x => x.OwnerUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Dress",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Distinguisher = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Dress", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Dress_Wearable_ID",
                        column: x => x.ID,
                        principalTable: "Wearable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pants",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Distinguisher = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pants", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Pants_Wearable_ID",
                        column: x => x.ID,
                        principalTable: "Wearable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shirt",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Distinguisher = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shirt", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Shirt_Wearable_ID",
                        column: x => x.ID,
                        principalTable: "Wearable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Shoes",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Distinguisher = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Shoes", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Shoes_Wearable_ID",
                        column: x => x.ID,
                        principalTable: "Wearable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Outfit",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false),
                    OwnerUsername = table.Column<string>(type: "text", nullable: true),
                    DressID = table.Column<Guid>(type: "uuid", nullable: true),
                    ShirtID = table.Column<Guid>(type: "uuid", nullable: true),
                    PantsID = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoesID = table.Column<Guid>(type: "uuid", nullable: true),
                    Deleted = table.Column<bool>(type: "boolean", nullable: false),
                    ImageURL = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outfit", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Outfit_Dress_DressID",
                        column: x => x.DressID,
                        principalTable: "Dress",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Outfit_Pants_PantsID",
                        column: x => x.PantsID,
                        principalTable: "Pants",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Outfit_Shirt_ShirtID",
                        column: x => x.ShirtID,
                        principalTable: "Shirt",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Outfit_Shoes_ShoesID",
                        column: x => x.ShoesID,
                        principalTable: "Shoes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Outfit_User_OwnerUsername",
                        column: x => x.OwnerUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "LogItem",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    OwnerUsername = table.Column<string>(type: "text", nullable: true),
                    Date = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Note = table.Column<string>(type: "text", nullable: false),
                    OutfitID = table.Column<Guid>(type: "uuid", nullable: true),
                    ShirtID = table.Column<Guid>(type: "uuid", nullable: true),
                    PantsID = table.Column<Guid>(type: "uuid", nullable: true),
                    DressID = table.Column<Guid>(type: "uuid", nullable: true),
                    ShoesID = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LogItem", x => x.ID);
                    table.ForeignKey(
                        name: "FK_LogItem_Dress_DressID",
                        column: x => x.DressID,
                        principalTable: "Dress",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogItem_Outfit_OutfitID",
                        column: x => x.OutfitID,
                        principalTable: "Outfit",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogItem_Pants_PantsID",
                        column: x => x.PantsID,
                        principalTable: "Pants",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogItem_Shirt_ShirtID",
                        column: x => x.ShirtID,
                        principalTable: "Shirt",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogItem_Shoes_ShoesID",
                        column: x => x.ShoesID,
                        principalTable: "Shoes",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_LogItem_User_OwnerUsername",
                        column: x => x.OwnerUsername,
                        principalTable: "User",
                        principalColumn: "Username");
                });

            migrationBuilder.CreateTable(
                name: "Outerwear",
                columns: table => new
                {
                    ID = table.Column<Guid>(type: "uuid", nullable: false),
                    Distinguisher = table.Column<int>(type: "integer", nullable: false),
                    Size = table.Column<string>(type: "text", nullable: false),
                    Region = table.Column<string>(type: "text", nullable: false),
                    LogItemID = table.Column<Guid>(type: "uuid", nullable: true),
                    State = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Outerwear", x => x.ID);
                    table.ForeignKey(
                        name: "FK_Outerwear_LogItem_LogItemID",
                        column: x => x.LogItemID,
                        principalTable: "LogItem",
                        principalColumn: "ID");
                    table.ForeignKey(
                        name: "FK_Outerwear_Wearable_ID",
                        column: x => x.ID,
                        principalTable: "Wearable",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "OuterwearOutfit (Dictionary<string, object>)",
                columns: table => new
                {
                    OuterwearLayersID = table.Column<Guid>(type: "uuid", nullable: false),
                    OutfitsID = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_OuterwearOutfit (Dictionary<string, object>)", x => new { x.OuterwearLayersID, x.OutfitsID });
                    table.ForeignKey(
                        name: "FK_OuterwearOutfit (Dictionary<string, object>)_Outerwear_Oute~",
                        column: x => x.OuterwearLayersID,
                        principalTable: "Outerwear",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_OuterwearOutfit (Dictionary<string, object>)_Outfit_Outfits~",
                        column: x => x.OutfitsID,
                        principalTable: "Outfit",
                        principalColumn: "ID",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LogItem_DressID",
                table: "LogItem",
                column: "DressID");

            migrationBuilder.CreateIndex(
                name: "IX_LogItem_OutfitID",
                table: "LogItem",
                column: "OutfitID");

            migrationBuilder.CreateIndex(
                name: "IX_LogItem_OwnerUsername",
                table: "LogItem",
                column: "OwnerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_LogItem_PantsID",
                table: "LogItem",
                column: "PantsID");

            migrationBuilder.CreateIndex(
                name: "IX_LogItem_ShirtID",
                table: "LogItem",
                column: "ShirtID");

            migrationBuilder.CreateIndex(
                name: "IX_LogItem_ShoesID",
                table: "LogItem",
                column: "ShoesID");

            migrationBuilder.CreateIndex(
                name: "IX_Outerwear_LogItemID",
                table: "Outerwear",
                column: "LogItemID");

            migrationBuilder.CreateIndex(
                name: "IX_OuterwearOutfit (Dictionary<string, object>)_OutfitsID",
                table: "OuterwearOutfit (Dictionary<string, object>)",
                column: "OutfitsID");

            migrationBuilder.CreateIndex(
                name: "IX_Outfit_DressID",
                table: "Outfit",
                column: "DressID");

            migrationBuilder.CreateIndex(
                name: "IX_Outfit_OwnerUsername",
                table: "Outfit",
                column: "OwnerUsername");

            migrationBuilder.CreateIndex(
                name: "IX_Outfit_PantsID",
                table: "Outfit",
                column: "PantsID");

            migrationBuilder.CreateIndex(
                name: "IX_Outfit_ShirtID",
                table: "Outfit",
                column: "ShirtID");

            migrationBuilder.CreateIndex(
                name: "IX_Outfit_ShoesID",
                table: "Outfit",
                column: "ShoesID");

            migrationBuilder.CreateIndex(
                name: "IX_Wearable_OwnerUsername",
                table: "Wearable",
                column: "OwnerUsername");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "OuterwearOutfit (Dictionary<string, object>)");

            migrationBuilder.DropTable(
                name: "Outerwear");

            migrationBuilder.DropTable(
                name: "LogItem");

            migrationBuilder.DropTable(
                name: "Outfit");

            migrationBuilder.DropTable(
                name: "Dress");

            migrationBuilder.DropTable(
                name: "Pants");

            migrationBuilder.DropTable(
                name: "Shirt");

            migrationBuilder.DropTable(
                name: "Shoes");

            migrationBuilder.DropTable(
                name: "Wearable");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
