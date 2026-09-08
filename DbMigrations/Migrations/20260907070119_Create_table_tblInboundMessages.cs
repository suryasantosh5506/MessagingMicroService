using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Create_table_tblInboundMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    CREATE TABLE [dbo].[tblInboundMessages](
                                        [Id] INT PRIMARY KEY Identity(1,1),
                                        [ProviderMessageId] varchar(255) NOT NULL,
                                        [FromNumber] VARCHAR(30) NOT NULL,
                                        [ToNumber] VARCHAR(30) NOT NULL,
                                        [Content] TEXT NOT NULL,
                                        [ProviderId] INT NOT NULL,
                                        [CreatedAt] DATETIME DEFAULT CURRENT_TIMESTAMP,
                                        
                                        CONSTRAINT [FK_tblInboundMessages_tblProviders] 
                                        FOREIGN KEY([ProviderId])
                                        REFERENCES [dbo].[tblProviders] ([Id])
                                    );
                                 """
            );  
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    IF OBJECT_ID('[dbo].[tblInboundMessages]', 'U') IS NOT NULL
                                    BEGIN
                                        DROP TABLE [dbo].[tblInboundMessages];
                                    END
                                 """);
        }
    }
}