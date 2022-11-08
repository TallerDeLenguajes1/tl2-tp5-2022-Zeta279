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
        public ClienteModel Cliente { get; set; }
        public string NombreCadete {get; set;}
        public estado Estado { get; set; }

        public PedidoViewModel()
        {

        }

        public PedidoViewModel(int num, string detalles, ClienteModel cliente, estado estado)
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
    }

    public class CrearPedidoViewModel
    {
        [AllowNull]
        [DisplayName("Detalles")]
        public string Detalles { get; set; }

        [Required]
        [DisplayName("ID")]
        public int IDCliente { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string NombreCliente { get; set; }

        [Required]
        [DisplayName("Dirección")]
        public string DireccionCliente { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public long TelefonoCliente { get; set; }

        public CrearPedidoViewModel()
        {

        }

        public CrearPedidoViewModel(string detalles, int iDCliente, string nombreCliente, string direccionCliente, long telefonoCliente)
        {
            Detalles = detalles;
            IDCliente = iDCliente;
            NombreCliente = nombreCliente;
            DireccionCliente = direccionCliente;
            TelefonoCliente = telefonoCliente;
        }
    }
}
