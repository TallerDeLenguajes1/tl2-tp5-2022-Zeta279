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
        public CadeteViewModel Cadete { get; set; }
        public ClienteModel Cliente { get; set; }
        public bool EnCurso { get; set; }
        public bool Entregado { get; set; }

        public PedidoViewModel()
        {

        }

        public PedidoViewModel(int num, string detalles, CadeteViewModel cadete, bool enCurso, bool entregado)
        {
            NumPedido = num;
            Detalles = detalles;
            Cadete = cadete;
            EnCurso = enCurso;
            Entregado = entregado;
        }

        public PedidoViewModel(int num, string detalles, CadeteViewModel cadete, ClienteModel cliente, bool enCurso, bool entregado)
        {
            NumPedido = num;
            Detalles = detalles;
            Cadete = cadete;
            Cliente = cliente;
            EnCurso = enCurso;
            Entregado = entregado;
        }

        public PedidoViewModel(string detalles, CadeteViewModel cadete, ClienteModel cliente)
        {
            Detalles = detalles;
            Cadete = cadete;
            Cliente = cliente;
            EnCurso = false;
            Entregado = false;
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

        [AllowNull]
        [DisplayName("Datos de referencia")]
        public string DatosRefCliente { get; set; }

        public CrearPedidoViewModel()
        {

        }

        public CrearPedidoViewModel(string detalles, int iDCliente, string nombreCliente, string direccionCliente, long telefonoCliente, string datosRefCliente)
        {
            Detalles = detalles;
            IDCliente = iDCliente;
            NombreCliente = nombreCliente;
            DireccionCliente = direccionCliente;
            TelefonoCliente = telefonoCliente;
            DatosRefCliente = datosRefCliente;
        }
    }
}
