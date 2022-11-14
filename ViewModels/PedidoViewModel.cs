using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Cadeteria.Models;

namespace Cadeteria.ViewModels
{
    public class PedidoViewModel
    {
        public int NumPedido { get; set; }
        public string Detalles { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public string NombreCadete {get; set;}
        public estado Estado { get; set; }

        public PedidoViewModel()
        {

        }

        public PedidoViewModel(int num, string detalles, ClienteViewModel cliente, estado estado = estado.SinAsignar)
        {
            NumPedido = num;
            Detalles = detalles;
            Cliente = cliente;
            Estado = estado;
            NombreCadete = null;
        }

        public void IngresarCadete(string nombre)
        {
            NombreCadete = nombre;
        }

        public string ObtenerEstado()
        {
            if (Estado == estado.SinAsignar) return "Sin asignar";
            else if (Estado == estado.Pendiente) return "Pendiente";
            else if (Estado == estado.EnCurso) return "En curso";
            else if (Estado == estado.Entregado) return "Entregado";
            else return "Indefinido";
        }

        static public bool operator ==(PedidoViewModel ped1, PedidoViewModel ped2)
        {
            return ped1.NumPedido == ped2.NumPedido;
        }

        static public bool operator !=(PedidoViewModel ped1, PedidoViewModel ped2)
        {
            return ped1.NumPedido != ped2.NumPedido;
        }
    }

    public class CrearPedidoViewModel
    {
        [AllowNull]
        [DisplayName("Detalles")]
        public string Detalles { get; set; }

        [Required]
        [DisplayName("ID")]
        public int IDCliente { get; set; }

        public bool NuevoCliente { get; set; }

        [AllowNull]
        [DisplayName("Nombre")]
        public string? NombreCliente { get; set; }

        [AllowNull]
        [DisplayName("Dirección")]
        public string? DireccionCliente { get; set; }

        [AllowNull]
        [DisplayName("Teléfono")]
        public string? TelefonoCliente { get; set; }

        public CrearPedidoViewModel()
        {

        }

        public CrearPedidoViewModel(string detalles, int id)
        {
            Detalles = detalles;
            IDCliente = id;
        }
    }
}
