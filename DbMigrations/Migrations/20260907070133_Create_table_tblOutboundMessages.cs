using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Create_table_tblOutboundMessages : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    CREATE TABLE [dbo].[tblOutboundMessages](
                                        [Id] INT PRIMARY KEY Identity(1,1),
                                        [ProviderMessageId] varchar(255) NOT NULL,
                                        [FromNumber] VARCHAR(30) NOT NULL,
                                        [ToNumber] VARCHAR(30) NOT NULL,
                                        [ProviderId] INT NOT NULL,
                                        [Status] VARCHAR(20) NOT NULL,
                                        [CreatedAt] DATETIME DEFAULT CURRENT_TIMESTAMP,
                                        [UpdatedAt] DATETIME DEFAULT CURRENT_TIMESTAMP,
                                        
                                        CONSTRAINT [tblOutboundMessages_tblProviders] 
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
                                    IF OBJECT_ID('[dbo].[tblOutboundMessages]', 'U') IS NOT NULL
                                    BEGIN
                                        DROP TABLE [dbo].[tblOutboundMessages];
                                    END
                                 """);
        }
    }
}
