using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMath.Migrations
{
    public partial class FillLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("INSERT INTO [dbo].[MainDataAirportLinks](MainDataSequence, AirportsSequence)\r\n" +
                                 "SELECT [d].[Sequence] AS MainDataSequence, [a].[Sequence] AS AirportsSequence\r\n" +
                                 "FROM [dbo].[MainData] AS d\r\n" +
                                 "JOIN [dbo].[AIRPORT] AS a\r\n" +
                                 "ON [a].[IATA_Code] IN (SELECT VALUE FROM STRING_SPLIT([d].[Origin], ','))\r\n" +
                                 "ORDER BY [d].[Sequence]");

            migrationBuilder.Sql("INSERT INTO [dbo].[MainDataAirportLinks](MainDataSequence, AirportsSequence)\r\n" +
                                 "SELECT [d].[Sequence] AS MainDataSequence, [a].[Sequence] AS AirportsSequence\r\n" +
                                 "FROM [dbo].[MainData] AS d\r\n" +
                                 "JOIN [dbo].[AIRPORT] AS a\r\n" +
                                 "ON [a].[IATA_Code] IN (SELECT VALUE FROM STRING_SPLIT([d].[Dest], ','))\r\n" +
                                 "WHERE NOT EXISTS (SELECT * FROM [dbo].[MainDataAirportLinks] as e WHERE [e].[MainDataSequence] = [d].[Sequence] AND  [e].[AirportsSequence] = [a].[Sequence])\r\n" +
                                 "ORDER BY [d].[Sequence]");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"DELETE FROM [dbo].[MainDataAIRPORT]");
        }
    }
}
