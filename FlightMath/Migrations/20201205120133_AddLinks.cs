using Microsoft.EntityFrameworkCore.Migrations;

namespace FlightMath.Migrations
{
    public partial class AddLinks : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "MainDataAirportLinks",
                schema: "dbo",
                columns: table => new
                {
                    MainDataSequence = table.Column<decimal>(type: "numeric(9,0)", nullable: false),
                    AirportsSequence = table.Column<decimal>(type: "numeric(18,0)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FlightPoint", x => new { x.MainDataSequence, x.AirportsSequence });
                    table.ForeignKey("FK_MainDataAirportLinks_MainData_MainDataId",
                        x => x.MainDataSequence,
                        principalSchema: "dbo",
                        principalTable: "MainData",
                        principalColumn: "Sequence");
                    table.ForeignKey("FK_MainDataAirportLinks_Airport_AirportsSequence",
                        x => x.AirportsSequence,
                        principalSchema: "dbo",
                        principalTable: "AIRPORT",
                        principalColumn: "Sequence");
                });

            migrationBuilder.CreateIndex(
                name: "IX_MainDataSequenceLinks",
                schema: "dbo",
                table: "MainDataAirportLinks",
                column: "MainDataSequence");

            migrationBuilder.CreateIndex(
                name: "IX_AirportsSequenceLinks",
                schema: "dbo",
                table: "MainDataAirportLinks",
                column: "AirportsSequence");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataSequenceAirportsSequenceLinks",
                schema: "dbo",
                table: "MainDataAirportLinks",
                columns: new[] { "AirportsSequence", "MainDataSequence" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_MainDataSequence",
                schema: "dbo",
                table: "MainData",
                column: "Sequence");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataAWBSeq",
                schema: "dbo",
                table: "MainData",
                column: "AWBSeq");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataPrefix",
                schema: "dbo",
                table: "MainData",
                column: "Prefix");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataSerial",
                schema: "dbo",
                table: "MainData",
                column: "Serial");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataPCWeight",
                schema: "dbo",
                table: "MainData",
                column: "PC_Weight");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataFlights",
                schema: "dbo",
                table: "MainData",
                column: "Flights");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataOrigin",
                schema: "dbo",
                table: "MainData",
                column: "Origin");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataDates",
                schema: "dbo",
                table: "MainData",
                column: "Dates");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataDest",
                schema: "dbo",
                table: "MainData",
                column: "Dest");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataCarrier",
                schema: "dbo",
                table: "MainData",
                column: "Carrier");

            migrationBuilder.CreateIndex(
                name: "IX_MainDataActualKGs",
                schema: "dbo",
                table: "MainData",
                column: "ActualKGs");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTSequence",
                schema: "dbo",
                table: "AIRPORT",
                column: "Sequence");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTIATACode",
                schema: "dbo",
                table: "AIRPORT",
                column: "IATA_Code");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTICAOCode",
                schema: "dbo",
                table: "AIRPORT",
                column: "ICAO_Code");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTAirportName",
                schema: "dbo",
                table: "AIRPORT",
                column: "Airport_Name");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTCity",
                schema: "dbo",
                table: "AIRPORT",
                column: "City");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTCountryId",
                schema: "dbo",
                table: "AIRPORT",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTLatitude",
                schema: "dbo",
                table: "AIRPORT",
                column: "Latitude");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTLongitude",
                schema: "dbo",
                table: "AIRPORT",
                column: "Longitude");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTAltitude",
                schema: "dbo",
                table: "AIRPORT",
                column: "Altitude");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTUTCOffset",
                schema: "dbo",
                table: "AIRPORT",
                column: "UTC_Offset");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTDST",
                schema: "dbo",
                table: "AIRPORT",
                column: "DST");

            migrationBuilder.CreateIndex(
                name: "IX_AIRPORTBaseId",
                schema: "dbo",
                table: "AIRPORT",
                column: "Base_Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MainDataAIRPORT",
                schema: "dbo");
        }
    }
}
