using Entidades.DataBase;
using Entidades.Exceptions;
using Entidades.Files;
using Entidades.Interfaces;
using System.ComponentModel.Design;
using System.Threading;

namespace Entidades.Modelos
{

    public delegate void DelegadoDemoraAtencion(double demora);
    public delegate void DelegadoPedidoEnCurso(IComestible menu);

    /// añado un delegado y un evento adicional para demostrar conocimiento
    public delegate void DelegadoInformarCola(int largo);
    public class Cocinero<T> where T : IComestible, new()
    {
        private int cantPedidosFinalizados;
        private string nombre;
        private double demoraPreparacionTotal;
        private CancellationTokenSource cancellation;
        private T pedidoEnPreparacion;
        private Task tarea;
        private Mozo<T> mozo;
        private Queue<T> pedidos;
        public event DelegadoPedidoEnCurso OnPedido;
        public event DelegadoDemoraAtencion OnDemora;
        public event DelegadoInformarCola CambioLargo;


        public Cocinero(string nombre)
        {
            this.nombre = nombre;
            this.mozo = new Mozo<T> { };
            this.pedidos = new Queue<T> { };
            this.mozo.OnPedido += TomarNuevoPedido;
        }

        //No hacer nada
        public bool HabilitarCocina
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running ||
                    this.tarea.Status == TaskStatus.WaitingToRun ||
                    this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.HabilitarCocina)
                {
                    this.cancellation = new CancellationTokenSource();
                    this.mozo.EmpezarATrabajar = true;
                    this.EmpezarACocinar();
                }
                else
                {
                    this.mozo.EmpezarATrabajar = false;
                    this.cancellation.Cancel();
                }
            }
        }

        //no hacer nada
        public double TiempoMedioDePreparacion { get => this.cantPedidosFinalizados == 0 ? 0 : this.demoraPreparacionTotal / this.cantPedidosFinalizados; }

        public string Nombre { get => nombre; }

        public int CantPedidosFinalizados { get => cantPedidosFinalizados; }

        public Queue<T> Pedidos { get { return this.pedidos; } }

        private void EmpezarACocinar()
        {
            tarea = Task.Run(() =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    if (pedidos.Count > 0 && OnPedido != null)// CREO QUE NO HACE FALTA VALIDARLO PORQUE YA LO VALIDAMOS ANTES ( si el count aumento es porque ya llamamos a tomar pedido y ya se valido)
                    {
                        pedidoEnPreparacion = pedidos.Dequeue();
                        NotificarCantidadEnCola();
                        OnPedido.Invoke(pedidoEnPreparacion);
                        EsperarProximoIngreso();
                        cantPedidosFinalizados++;
                        DataBaseManager.GuardarTicket<T>(nombre, pedidoEnPreparacion);
                    }
                }
            }, cancellation.Token);
        }
        private void EsperarProximoIngreso()    
        {
            if (OnDemora != null)
            { 
                int tiempoActual = 0;
                while (!cancellation.IsCancellationRequested && !pedidoEnPreparacion.Estado)
                {
                    OnDemora.Invoke(tiempoActual);
                    Thread.Sleep(1000);
                    tiempoActual += 1;
                }
                demoraPreparacionTotal += tiempoActual;

            }

        }
        private void TomarNuevoPedido(T menu)
        {
            if(OnPedido != null)
            {
                pedidos.Enqueue(menu);
                NotificarCantidadEnCola();
            }
        }

        private void NotificarCantidadEnCola()
        {
            if(CambioLargo != null)
            {
                CambioLargo.Invoke(pedidos.Count);
            }
        }
    }
}
