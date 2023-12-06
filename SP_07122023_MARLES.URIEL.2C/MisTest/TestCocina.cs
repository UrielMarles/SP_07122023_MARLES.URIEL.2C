using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Modelos;

namespace MisTest
{
    [TestClass]
    public class TestCocina
    {
        [TestMethod]
        [ExpectedException(typeof(FileManagerException))]
        public void AlGuardarUnArchivo_ConNombreInvalido_TengoUnaExcepcion()
        {
            string textoGuardado = "textoGuardado";
            string rutaGuardado = ".txt. 1||@#123 ÑÑÑÑnombre invalido xdxd";
            bool seAppendea = false;

            FileManager.Guardar(textoGuardado,rutaGuardado,seAppendea);
        }

        [TestMethod]

        public void AlInstanciarUnCocinero_SeEspera_PedidosCero()
        {

            string nombre = "UrielCocinero";
            Cocinero<Hamburguesa> uriel;
            uriel = new Cocinero<Hamburguesa>(nombre);

            Assert.AreEqual(uriel.CantPedidosFinalizados, 0);
        }
    }
}