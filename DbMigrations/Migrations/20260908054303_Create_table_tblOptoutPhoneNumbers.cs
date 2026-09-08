using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DbMigrations.Migrations
{
    /// <inheritdoc />
    public partial class Create_table_tblOptoutPhoneNumbers : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                    CREATE TABLE [dbo].[tblOptoutPhoneNumbers]
                                    (
                                        [Id] INT IDENTITY(1,1) PRIMARY KEY,
                                        [PhoneNumber] VARCHAR(30) NOT NULL,
                                        [OptedOutAt] DATETIME NOT NULL DEFAULT (SYSUTCDATETIME()),
                                        [InboundMessageId] INT,
                                        [ProviderId] INT NOT NULL,
                                        
                                        CONSTRAINT [UQ_tblOptoutPhoneNumbers_PhoneNumber]
                                        UNIQUE ([PhoneNumber]),
                                        
                                        CONSTRAINT [FK_tblProviders_tblOptoutPhoneNumbers]
                                        FOREIGN KEY ([ProviderId])
                                        REFERENCES [dbo].[tblProviders] ([Id]),
                                        
                                        CONSTRAINT [FK_tblInboundMessages_tblOptoutPhoneNumbers]
                                        FOREIGN KEY ([InboundMessageId])
                                        REFERENCES [dbo].[tblInboundMessages] ([Id])
                                     )
                                 """);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("""
                                     IF OBJECT_ID('[dbo].[tblOptoutPhoneNumbers]','U') IS NOT NULL
                                     BEGIN
                                         DROP TABLE [dbo].[tblOptoutPhoneNumbers];
                                     END
                                 """);
        }
    }
}
