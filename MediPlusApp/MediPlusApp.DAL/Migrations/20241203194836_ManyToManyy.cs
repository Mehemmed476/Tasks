using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MediPlusApp.DAL.Migrations
{
    /// <inheritdoc />
    public partial class ManyToManyy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedDate",
                table: "HospitalDoctors",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "DeletedDate",
                table: "HospitalDoctors",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsActive",
                table: "HospitalDoctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "HospitalDoctors",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "ModifiedDate",
                table: "HospitalDoctors",
                type: "datetime2",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CreatedDate",
                table: "HospitalDoctors");

            migrationBuilder.DropColumn(
                name: "DeletedDate",
                table: "HospitalDoctors");

            migrationBuilder.DropColumn(
                name: "IsActive",
                table: "HospitalDoctors");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "HospitalDoctors");

            migrationBuilder.DropColumn(
                name: "ModifiedDate",
                table: "HospitalDoctors");
        }
    }
}
