﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PostgresDemo.Database.Migrations
{
    /// <inheritdoc />
    public partial class ChangedIsCompleteToIsCompleted : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsComplete",
                table: "TodoItems",
                newName: "IsCompleted");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsCompleted",
                table: "TodoItems",
                newName: "IsComplete");
        }
    }
}
