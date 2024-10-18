using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficePortal.Migrations
{
    /// <inheritdoc />
    public partial class commentsreply : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "CommentId",
                table: "Reply_Comment_Model",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Reply_Comment_Model_CommentId",
                table: "Reply_Comment_Model",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reply_Comment_Model_Comment_CommentId",
                table: "Reply_Comment_Model",
                column: "CommentId",
                principalTable: "Comment",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reply_Comment_Model_Comment_CommentId",
                table: "Reply_Comment_Model");

            migrationBuilder.DropIndex(
                name: "IX_Reply_Comment_Model_CommentId",
                table: "Reply_Comment_Model");

            migrationBuilder.DropColumn(
                name: "CommentId",
                table: "Reply_Comment_Model");
        }
    }
}
