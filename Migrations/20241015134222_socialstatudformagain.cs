using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace OfficePortal.Migrations
{
    /// <inheritdoc />
    public partial class socialstatudformagain : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_SocialStatusForm_SocialStatusRecordId",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_SocialStatusRecordId",
                table: "File");

            migrationBuilder.DropColumn(
                name: "SocialStatusRecordId",
                table: "File");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoiningDate",
                table: "SocialStatusForm",
                type: "datetime2",
                nullable: true,
                oldClrType: typeof(DateTime),
                oldType: "datetime2");

            migrationBuilder.CreateIndex(
                name: "IX_File_SocialStatusRecord_Id",
                table: "File",
                column: "SocialStatusRecord_Id");

            migrationBuilder.AddForeignKey(
                name: "FK_File_SocialStatusForm_SocialStatusRecord_Id",
                table: "File",
                column: "SocialStatusRecord_Id",
                principalTable: "SocialStatusForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_File_SocialStatusForm_SocialStatusRecord_Id",
                table: "File");

            migrationBuilder.DropIndex(
                name: "IX_File_SocialStatusRecord_Id",
                table: "File");

            migrationBuilder.AlterColumn<DateTime>(
                name: "JoiningDate",
                table: "SocialStatusForm",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified),
                oldClrType: typeof(DateTime),
                oldType: "datetime2",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "SocialStatusRecordId",
                table: "File",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_File_SocialStatusRecordId",
                table: "File",
                column: "SocialStatusRecordId");

            migrationBuilder.AddForeignKey(
                name: "FK_File_SocialStatusForm_SocialStatusRecordId",
                table: "File",
                column: "SocialStatusRecordId",
                principalTable: "SocialStatusForm",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
