using Microsoft.EntityFrameworkCore.Migrations;

namespace TTDataAccessLibrary.Migrations
{
    public partial class TVP_AssetType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql = @"CREATE TYPE [dbo].[AssetType] AS TABLE(
	[AssetId] [int] NOT NULL,
	[Property] [nvarchar](20) NOT NULL,
	[PropertyValue] [bit] NOT NULL,
	[TimeStamp] [datetime2](7) NOT NULL
)";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"DROP TYPE [dbo].[AssetType]";
            migrationBuilder.Sql(sql);
        }
    }
}
