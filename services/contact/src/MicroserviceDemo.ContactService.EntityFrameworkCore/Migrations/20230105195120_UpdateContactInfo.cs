using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MicroserviceDemo.ContactService.Migrations
{
    /// <inheritdoc />
    public partial class UpdateContactInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ConcurrencyStamp",
                table: "ContactInfo",
                type: "character varying(40)",
                maxLength: 40,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreationTime",
                table: "ContactInfo",
                type: "timestamp without time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<Guid>(
                name: "CreatorId",
                table: "ContactInfo",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "DeleterId",
                table: "ContactInfo",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletionTime",
                table: "ContactInfo",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ExtraProperties",
                table: "ContactInfo",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "ContactInfo",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "LastModificationTime",
                table: "ContactInfo",
                type: "timestamp without time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LastModifierId",
                table: "ContactInfo",
                type: "uuid",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ConcurrencyStamp",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "CreationTime",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "CreatorId",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "DeleterId",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "DeletionTime",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "ExtraProperties",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "LastModificationTime",
                table: "ContactInfo");

            migrationBuilder.DropColumn(
                name: "LastModifierId",
                table: "ContactInfo");
        }
    }
}
