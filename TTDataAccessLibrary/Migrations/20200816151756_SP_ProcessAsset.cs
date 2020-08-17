using Microsoft.EntityFrameworkCore.Migrations;

namespace TTDataAccessLibrary.Migrations
{
    public partial class SP_ProcessAsset : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            string sql =
@"CREATE PROCEDURE [dbo].[sp_ProcessAsset]
@AssetTable AssetType READONLY
AS
BEGIN
	SET NOCOUNT ON;
	SET XACT_ABORT ON;

	with MatchingCTE([AssetId], [Name], [Bit], [TimeStamp])
	as
	(SELECT  source.AssetId, 
		source.Name, 
		CASE 
			when csv.PropertyValue = 1 AND ((source.TypeBitField & csv.BitFlag) = 0) then csv.BitFlag + source.TypeBitField
			when csv.PropertyValue = 0 AND ((source.TypeBitField & csv.BitFlag) = csv.BitFlag) then source.TypeBitField - csv.BitFlag
			else source.TypeBitField 
		END as [Bit],
		csv.TimeStamp

	FROM [dbo].[Assets] as [source]
	Inner join (
				SELECT t.[AssetId], t2.Property, t2.PropertyValue, t.[TimeStamp], f.BitFlag
				FROM (
						SELECT [AssetId], MAX([TimeStamp]) as [TimeStamp], MAX(Property) as [Property]
						FROM @AssetTable
						GROUP BY [AssetId]) as t
				LEFT JOIN [dbo].[Flags] f
				ON t.Property = f.Name
				CROSS APPLY (
						SELECT TOP(1) *
						FROM @AssetTable
						WHERE AssetId = t.[AssetId] and [TimeStamp] = t.[TimeStamp]) as t2
	) as csv
	on source.AssetId = csv.AssetId)

	MERGE [dbo].[Assets] AS TARGET
	USING MatchingCTE AS SOURCE
	ON (TARGET.[AssetId] = SOURCE.[AssetId])
	WHEN MATCHED AND TARGET.[TimeStamp] < SOURCE.[TimeStamp]
	THEN UPDATE SET 
		TARGET.[TimeStamp] = SOURCE.[TimeStamp], 
		TARGET.[TypeBitField] = SOURCE.[Bit];

	with NotMatchingCTE([AssetId], [Name], [TypeBitField], [TimeStamp])
	as
	(
		SELECT 
			source.[AssetId], 
			'', 
			0,
			source.TimeStamp 
		FROM @AssetTable as [source]
		Left Join [dbo].[Assets] as [target]
		on target.AssetId = source.AssetId
		where target.AssetId is null
	)
	select * from NotMatchingCTE;
END
GO";
            migrationBuilder.Sql(sql);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            string sql = @"DROP PROCEDURE [dbo].[sp_ProcessAsset]";
            migrationBuilder.Sql(sql);
        }
    }
}
