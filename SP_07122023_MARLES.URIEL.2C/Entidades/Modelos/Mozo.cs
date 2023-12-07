﻿using Entidades.DataBase;
using Entidades.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades.Modelos
{
    public delegate void DelegadoNuevoPedido<T>(T menu) where T : IComestible;
    public class Mozo<T> where T : IComestible, new()
    {
        public event DelegadoNuevoPedido<T> OnPedido;
        private CancellationTokenSource cancellation;
        private T menu;
        private Task tarea;


        public bool EmpezarATrabajar
        {
            get
            {
                return this.tarea is not null && (this.tarea.Status == TaskStatus.Running || this.tarea.Status == TaskStatus.WaitingToRun || this.tarea.Status == TaskStatus.WaitingForActivation);
            }
            set
            {
                if (value && !this.EmpezarATrabajar)
                {
                    this.cancellation = new CancellationTokenSource();

                    this.TomarPedidos();
                }
                else
                {
                    this.cancellation.Cancel();
                }
            }
        }

        private void TomarPedidos()
        {
            tarea = Task.Run(() =>
            {
                while (!cancellation.IsCancellationRequested)
                {
                    NotificarNuevoPedido();
                    Thread.Sleep(5000);

                }
            }, cancellation.Token);
        }

        private void NotificarNuevoPedido()
        {
            if (OnPedido != null)
            {
                menu = new T();

                menu.IniciarPreparacion();

                OnPedido.Invoke(menu);
            }
        }
    }
}
