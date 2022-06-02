using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace RecursivaChallenge.Migrations
{
    public partial class Init : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Socios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    Edad = table.Column<int>(type: "int", nullable: false),
                    Equipo = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    EstadoCivil = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false),
                    NivelDeEstudios = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Socios", x => x.Id);
                });

            migrationBuilder.Sql(
                @"CREATE OR ALTER PROCEDURE [dbo].[sp_InsertBulkCsv](
                @FilePath nvarchar(300),
                @FormatFileSql nvarchar(300)
                )
                AS
                BEGIN
                DECLARE @Sql nvarchar(1000)

                CREATE TABLE #TempSocios(Nombre nvarchar(max), Edad smallint, Equipo nvarchar(max), EstadoCivil nvarchar(max), NivelDeEstudios nvarchar(max)) 
                SET @Sql = CONCAT('BULK INSERT #TempSocios FROM ','''', @FilePath ,'''', ' WITH (FORMATFILE = ','''',@FormatFileSql,'''',')')
                exec(@Sql)

                INSERT INTO Socios(Nombre,Edad, Equipo, EstadoCivil, NivelDeEstudios)
                SELECT Nombre, Edad, Equipo, EstadoCivil, NivelDeEstudios FROM #TempSocios
                END"
            );

            migrationBuilder.Sql(
                @"CREATE OR ALTER PROCEDURE [dbo].[sp_TruncateTable]
                @NombreTabla nvarchar(100)
                AS
                BEGIN
                DECLARE @Sql NVARCHAR(1000)
                SET @Sql = 'TRUNCATE TABLE ' + @NombreTabla
                EXEC(@Sql)
                END"
            );
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Socios");

            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[sp_InsertBulkCsv]");
            migrationBuilder.Sql(@"DROP PROCEDURE [dbo].[sp_TruncateTable]");
        }
    }
}
