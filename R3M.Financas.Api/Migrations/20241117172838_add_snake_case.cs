using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace R3M.Financas.Api.Migrations
{
    /// <inheritdoc />
    public partial class add_snake_case : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimentations_Categories_CategoryId",
                table: "Movimentations");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimentations_Institutions_InstitutionId",
                table: "Movimentations");

            migrationBuilder.DropForeignKey(
                name: "FK_Movimentations_Periods_PeriodId",
                table: "Movimentations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Periods",
                table: "Periods");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Movimentations",
                table: "Movimentations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Categories",
                table: "Categories");

            migrationBuilder.RenameTable(
                name: "Periods",
                newName: "periods");

            migrationBuilder.RenameTable(
                name: "Movimentations",
                newName: "movimentations");

            migrationBuilder.RenameTable(
                name: "Institutions",
                newName: "institutions");

            migrationBuilder.RenameTable(
                name: "Categories",
                newName: "categories");

            migrationBuilder.RenameColumn(
                name: "Start",
                table: "periods",
                newName: "start");

            migrationBuilder.RenameColumn(
                name: "End",
                table: "periods",
                newName: "end");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "periods",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "periods",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatenDate",
                table: "periods",
                newName: "updaten_date");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "periods",
                newName: "insert_date");

            migrationBuilder.RenameIndex(
                name: "IX_Periods_Description",
                table: "periods",
                newName: "ix_periods_description");

            migrationBuilder.RenameColumn(
                name: "Value",
                table: "movimentations",
                newName: "value");

            migrationBuilder.RenameColumn(
                name: "Description",
                table: "movimentations",
                newName: "description");

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "movimentations",
                newName: "date");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "movimentations",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatenDate",
                table: "movimentations",
                newName: "updaten_date");

            migrationBuilder.RenameColumn(
                name: "PeriodId",
                table: "movimentations",
                newName: "period_id");

            migrationBuilder.RenameColumn(
                name: "InstitutionId",
                table: "movimentations",
                newName: "institution_id");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "movimentations",
                newName: "insert_date");

            migrationBuilder.RenameColumn(
                name: "CategoryId",
                table: "movimentations",
                newName: "category_id");

            migrationBuilder.RenameIndex(
                name: "IX_Movimentations_PeriodId",
                table: "movimentations",
                newName: "ix_movimentations_period_id");

            migrationBuilder.RenameIndex(
                name: "IX_Movimentations_InstitutionId",
                table: "movimentations",
                newName: "ix_movimentations_institution_id");

            migrationBuilder.RenameIndex(
                name: "IX_Movimentations_CategoryId",
                table: "movimentations",
                newName: "ix_movimentations_category_id");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "institutions",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Balance",
                table: "institutions",
                newName: "balance");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "institutions",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatenDate",
                table: "institutions",
                newName: "updaten_date");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "institutions",
                newName: "insert_date");

            migrationBuilder.RenameColumn(
                name: "InitialBalance",
                table: "institutions",
                newName: "initial_balance");

            migrationBuilder.RenameIndex(
                name: "IX_Institutions_Name",
                table: "institutions",
                newName: "ix_institutions_name");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "categories",
                newName: "name");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "categories",
                newName: "id");

            migrationBuilder.RenameColumn(
                name: "UpdatenDate",
                table: "categories",
                newName: "updaten_date");

            migrationBuilder.RenameColumn(
                name: "ParentId",
                table: "categories",
                newName: "parent_id");

            migrationBuilder.RenameColumn(
                name: "InsertDate",
                table: "categories",
                newName: "insert_date");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_Name",
                table: "categories",
                newName: "ix_categories_name");

            migrationBuilder.RenameIndex(
                name: "IX_Categories_ParentId",
                table: "categories",
                newName: "ix_categories_parent_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_periods",
                table: "periods",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_movimentations",
                table: "movimentations",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_institutions",
                table: "institutions",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_categories",
                table: "categories",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_categories_categories_parent_id",
                table: "categories",
                column: "parent_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_movimentations_categories_category_id",
                table: "movimentations",
                column: "category_id",
                principalTable: "categories",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_movimentations_institutions_institution_id",
                table: "movimentations",
                column: "institution_id",
                principalTable: "institutions",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "fk_movimentations_periods_period_id",
                table: "movimentations",
                column: "period_id",
                principalTable: "periods",
                principalColumn: "id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_categories_categories_parent_id",
                table: "categories");

            migrationBuilder.DropForeignKey(
                name: "fk_movimentations_categories_category_id",
                table: "movimentations");

            migrationBuilder.DropForeignKey(
                name: "fk_movimentations_institutions_institution_id",
                table: "movimentations");

            migrationBuilder.DropForeignKey(
                name: "fk_movimentations_periods_period_id",
                table: "movimentations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_periods",
                table: "periods");

            migrationBuilder.DropPrimaryKey(
                name: "pk_movimentations",
                table: "movimentations");

            migrationBuilder.DropPrimaryKey(
                name: "pk_institutions",
                table: "institutions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_categories",
                table: "categories");

            migrationBuilder.RenameTable(
                name: "periods",
                newName: "Periods");

            migrationBuilder.RenameTable(
                name: "movimentations",
                newName: "Movimentations");

            migrationBuilder.RenameTable(
                name: "institutions",
                newName: "Institutions");

            migrationBuilder.RenameTable(
                name: "categories",
                newName: "Categories");

            migrationBuilder.RenameColumn(
                name: "start",
                table: "Periods",
                newName: "Start");

            migrationBuilder.RenameColumn(
                name: "end",
                table: "Periods",
                newName: "End");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Periods",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Periods",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updaten_date",
                table: "Periods",
                newName: "UpdatenDate");

            migrationBuilder.RenameColumn(
                name: "insert_date",
                table: "Periods",
                newName: "InsertDate");

            migrationBuilder.RenameIndex(
                name: "ix_periods_description",
                table: "Periods",
                newName: "IX_Periods_Description");

            migrationBuilder.RenameColumn(
                name: "value",
                table: "Movimentations",
                newName: "Value");

            migrationBuilder.RenameColumn(
                name: "description",
                table: "Movimentations",
                newName: "Description");

            migrationBuilder.RenameColumn(
                name: "date",
                table: "Movimentations",
                newName: "Date");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Movimentations",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updaten_date",
                table: "Movimentations",
                newName: "UpdatenDate");

            migrationBuilder.RenameColumn(
                name: "period_id",
                table: "Movimentations",
                newName: "PeriodId");

            migrationBuilder.RenameColumn(
                name: "institution_id",
                table: "Movimentations",
                newName: "InstitutionId");

            migrationBuilder.RenameColumn(
                name: "insert_date",
                table: "Movimentations",
                newName: "InsertDate");

            migrationBuilder.RenameColumn(
                name: "category_id",
                table: "Movimentations",
                newName: "CategoryId");

            migrationBuilder.RenameIndex(
                name: "ix_movimentations_period_id",
                table: "Movimentations",
                newName: "IX_Movimentations_PeriodId");

            migrationBuilder.RenameIndex(
                name: "ix_movimentations_institution_id",
                table: "Movimentations",
                newName: "IX_Movimentations_InstitutionId");

            migrationBuilder.RenameIndex(
                name: "ix_movimentations_category_id",
                table: "Movimentations",
                newName: "IX_Movimentations_CategoryId");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Institutions",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "balance",
                table: "Institutions",
                newName: "Balance");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Institutions",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updaten_date",
                table: "Institutions",
                newName: "UpdatenDate");

            migrationBuilder.RenameColumn(
                name: "insert_date",
                table: "Institutions",
                newName: "InsertDate");

            migrationBuilder.RenameColumn(
                name: "initial_balance",
                table: "Institutions",
                newName: "InitialBalance");

            migrationBuilder.RenameIndex(
                name: "ix_institutions_name",
                table: "Institutions",
                newName: "IX_Institutions_Name");

            migrationBuilder.RenameColumn(
                name: "name",
                table: "Categories",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "id",
                table: "Categories",
                newName: "Id");

            migrationBuilder.RenameColumn(
                name: "updaten_date",
                table: "Categories",
                newName: "UpdatenDate");

            migrationBuilder.RenameColumn(
                name: "parent_id",
                table: "Categories",
                newName: "ParentId");

            migrationBuilder.RenameColumn(
                name: "insert_date",
                table: "Categories",
                newName: "InsertDate");

            migrationBuilder.RenameIndex(
                name: "ix_categories_name",
                table: "Categories",
                newName: "IX_Categories_Name");

            migrationBuilder.RenameIndex(
                name: "ix_categories_parent_id",
                table: "Categories",
                newName: "IX_Categories_ParentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Periods",
                table: "Periods",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Movimentations",
                table: "Movimentations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Institutions",
                table: "Institutions",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Categories",
                table: "Categories",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Categories_Categories_ParentId",
                table: "Categories",
                column: "ParentId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentations_Categories_CategoryId",
                table: "Movimentations",
                column: "CategoryId",
                principalTable: "Categories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentations_Institutions_InstitutionId",
                table: "Movimentations",
                column: "InstitutionId",
                principalTable: "Institutions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Movimentations_Periods_PeriodId",
                table: "Movimentations",
                column: "PeriodId",
                principalTable: "Periods",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
