using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Create_table_tblProviders : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    CREATE TABLE [dbo].[tblProviders](
                                        [Id] INT PRIMARY KEY Identity(1,1),
                                        [MessageProvider] varchar(20) NOT NULL
                                    );
                                 """
                                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    IF OBJECT_ID('[dbo].[tblProviders]','U') IS NOT NULL
                                    BEGIN
                                        DROP TABLE [dbo].[tblProviders];
                                    END
                                 """
                                );
        }
    }
}
