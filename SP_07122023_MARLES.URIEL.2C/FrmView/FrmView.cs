using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using Entidades.Modelos;


namespace FrmView
{
    public partial class FrmView : Form
    {
        private IComestible comida;
        Cocinero<Hamburguesa> hamburguesero;

        public FrmView()
        {
            InitializeComponent();
            this.hamburguesero = new Cocinero<Hamburguesa>("Ramon");
            //Alumno - agregar manejadores al cocinero
            this.hamburguesero.OnDemora += this.MostrarConteo;
            this.hamburguesero.OnPedido += this.MostrarComida;
            this.hamburguesero.CambioLargo += this.MostrarCola;
        }


        //Alumno: Realizar los cambios necesarios sobre MostrarComida de manera que se refleje
        //en el formulario los datos de la comida
        private void MostrarComida(IComestible comida)
        {
            if (this.InvokeRequired)
            {
                // Si no estamos en el hilo de la interfaz de usuario principal,
                // invocamos el método en ese hilo.
                this.Invoke(new Action(() => MostrarComida(comida)));
            }
            else
            {
                this.comida = comida;
                this.pcbComida.Load(comida.Imagen);
                this.rchElaborando.Text = comida.ToString();
            }

        }



        //Alumno: Realizar los cambios necesarios sobre MostrarConteo de manera que se refleje
        //en el fomrulario el tiempo transucurrido
        private void MostrarConteo(double tiempo)
        {

            if (this.InvokeRequired)
            {
                // Si no estamos en el hilo de la interfaz de usuario principal,
                // invocamos el método en ese hilo.
                this.Invoke(new Action(() => MostrarConteo(tiempo)));
            }
            else
            {
                this.lblTiempo.Text = $"{tiempo} segundos";
                this.lblTmp.Text = $"{this.hamburguesero.TiempoMedioDePreparacion.ToString("00.0")} segundos";
            }

        }

        private void MostrarCola(int cantPedidosCola)
        {

            if (this.InvokeRequired)
            {
                // Si no estamos en el hilo de la interfaz de usuario principal,
                // invocamos el método en ese hilo.
                this.Invoke(new Action(() => MostrarCola(cantPedidosCola)));
            }
            else
            {
                this.cantPedidos.Text = $"hay {cantPedidosCola} pedidos";
            }

        }

        private void btnAbrir_Click(object sender, EventArgs e)
        {
            if (!this.hamburguesero.HabilitarCocina)
            {
                this.hamburguesero.HabilitarCocina = true;
                this.btnAbrir.Image = Properties.Resources.close_icon;
            }
            else
            {
                this.hamburguesero.HabilitarCocina = false;
                this.btnAbrir.Image = Properties.Resources.open_icon;
            }

        }

        private void btnSiguiente_Click(object sender, EventArgs e)
        {
            if (this.comida != null)
            {

                comida.FinalizarPreparacion(this.hamburguesero.Nombre);
                this.rchFinalizados.Text += "\n" + comida.Ticket;
            }
            else
            {
                MessageBox.Show("El Cocinero no posee comidas", "Atencion", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
            }

        }

        private void FrmView_FormClosing(object sender, FormClosingEventArgs e)
        {
            FileManager.Serializar<Cocinero<Hamburguesa>>(hamburguesero, "HamburgueseroSerializado.txt");

        }
    }
}