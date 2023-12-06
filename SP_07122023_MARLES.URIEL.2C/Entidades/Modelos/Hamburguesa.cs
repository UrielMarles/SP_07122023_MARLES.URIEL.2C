using Entidades.Enumerados;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.MetodosDeExtension;
using System.Text;
using Entidades.DataBase;

namespace Entidades.Modelos
{
    public class Hamburguesa : IComestible
    {

        private static int costoBase;
        private bool esDoble;
        private double costo;
        private bool estado;
        private string imagen;
        private List<EIngrediente> ingredientes;
        private Random random;
        public string Imagen { get { return imagen; } }
        public bool Estado { get { return estado; } }

        public string Ticket => $"{this}\nTotal a pagar:{this.costo}";

        
        static Hamburguesa() => Hamburguesa.costoBase = 1500;


        public Hamburguesa() : this(false) { }

        public Hamburguesa(bool esDoble)
        {
            this.esDoble = esDoble;
            this.random = new Random();
        }

        



        private void AgregarIngredientes()
        {
            ingredientes = random.IngredientesAleatorios();
        }

        private string MostrarDatos()
        {
            StringBuilder stringBuilder = new StringBuilder();
            stringBuilder.AppendLine($"Hamburguesa {(this.esDoble ? "Doble" : "Simple")}");
            stringBuilder.AppendLine("Ingredientes: ");
            this.ingredientes.ForEach(i => stringBuilder.AppendLine(i.ToString()));
            return stringBuilder.ToString();

        }



        public override string ToString() => this.MostrarDatos();

        public void FinalizarPreparacion(string cocinero)
        {
            this.costo = ingredientes.CalcularCostoIngredientes(costoBase);
            this.estado = true;
        }

        public void IniciarPreparacion()
        {
            if (!this.estado)
            {
                Random random = new Random();
                int numeroAleatorio = random.Next(1, 9);
                imagen = DataBaseManager.GetImagenComida($"Hamburguesa_{numeroAleatorio}");
                AgregarIngredientes();
            }
        }
    }
}