using Microsoft.EntityFrameworkCore.Migrations;

namespace JWTAuthentication.Migrations.ApplicationDb2
{
    public partial class south3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedBought_AspNetUsers_BuyerId",
                table: "AssociatedBought");

            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedSell_AspNetUsers_SellerIdId",
                table: "AssociatedSell");

            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedShared_AspNetUsers_SharedIdId",
                table: "AssociatedShared");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_AspNetUsers_sellerId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders");

            migrationBuilder.DropUniqueConstraint(
                name: "Uid",
                table: "AspNetUsers");

            migrationBuilder.AddUniqueConstraint(
                name: "Uid",
                table: "User",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedBought_User_BuyerId",
                table: "AssociatedBought",
                column: "BuyerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedSell_User_SellerIdId",
                table: "AssociatedSell",
                column: "SellerIdId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedShared_User_SharedIdId",
                table: "AssociatedShared",
                column: "SharedIdId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_User_sellerId",
                table: "OrderItems",
                column: "sellerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_User_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "User",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedBought_User_BuyerId",
                table: "AssociatedBought");

            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedSell_User_SellerIdId",
                table: "AssociatedSell");

            migrationBuilder.DropForeignKey(
                name: "FK_AssociatedShared_User_SharedIdId",
                table: "AssociatedShared");

            migrationBuilder.DropForeignKey(
                name: "FK_OrderItems_User_sellerId",
                table: "OrderItems");

            migrationBuilder.DropForeignKey(
                name: "FK_Orders_User_CustomerId",
                table: "Orders");

            migrationBuilder.DropUniqueConstraint(
                name: "Uid",
                table: "User");

            migrationBuilder.AddUniqueConstraint(
                name: "Uid",
                table: "AspNetUsers",
                column: "Uid");

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedBought_AspNetUsers_BuyerId",
                table: "AssociatedBought",
                column: "BuyerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedSell_AspNetUsers_SellerIdId",
                table: "AssociatedSell",
                column: "SellerIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_AssociatedShared_AspNetUsers_SharedIdId",
                table: "AssociatedShared",
                column: "SharedIdId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_OrderItems_AspNetUsers_sellerId",
                table: "OrderItems",
                column: "sellerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Orders_AspNetUsers_CustomerId",
                table: "Orders",
                column: "CustomerId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
