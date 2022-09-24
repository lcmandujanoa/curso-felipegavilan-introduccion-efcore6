using EFCorePeliculas.Servicios;

namespace EFCorePeliculas.Prueba
{
    [TestClass]
    public class ServicioUsuarioPruebas
    {
        [TestMethod]
        public void ObtenerUsuarioId_NoTraeValorVacioONulo()
        {
            //Preparación
            var servicioUsuario = new ServicioUsuario();

            //Prueba
            var resultado = servicioUsuario.ObtenerUsuarioId();

            //Verificación
            Assert.AreNotEqual("", resultado);
            Assert.IsNotNull(resultado);
        }
    }
}