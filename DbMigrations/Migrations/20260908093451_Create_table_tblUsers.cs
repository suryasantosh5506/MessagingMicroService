using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Create_table_tblUsers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    CREATE TABLE [dbo].[tblUsers](
                                        [Id] [INT] IDENTITY(1,1) NOT NULL,
                                        [Email] VARCHAR(100) NOT NULL,
                                        [PasswordHash] VARCHAR(255) NOT NULL,
                                        [CreatedAt] DATETIME NOT NULL DEFAULT(SYSUTCDATETIME()),
                                        
                                        CONSTRAINT [UQ_tblUsers_Email] UNIQUE ([Email])
                                    );
                                 """
                                );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                     IF OBJECT_ID('dbo.tblUsers') IS NOT NULL
                                     BEGIN
                                        DROP TABLE [dbo].[tblUsers];
                                     END
                                 """
                                );
        }
    }
}
