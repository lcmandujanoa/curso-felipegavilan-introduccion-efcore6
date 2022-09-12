using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EFCorePeliculas.Migrations
{
    public partial class VistaConteoPeliculas : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"CREATE VIEW [dbo].[PeliculasConConteos]
AS
SELECT Id, Titulo,
	(SELECT COUNT(*)
	FROM GeneroPelicula
	WHERE PeliculasId = Peliculas.Id) as CantidadGeneros,
	(SELECT COUNT(DISTINCT CineId)
	FROM PeliculaSalaDeCine
	INNER JOIN SalasDeCine
	ON SalasDeCine.Id = PeliculaSalaDeCine.SalasDeCineId
	WHERE PeliculaSalaDeCine.PeliculasId = Peliculas.Id) as CantidadCines,
	(SELECT COUNT(*)
	FROM PeliculasActores
	where PeliculaId = Peliculas.Id) as CantidadActores
FROM Peliculas;");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
			migrationBuilder.Sql("DROP VIEW [dbo].[PeliculasConConteos]");
        }
    }
}
