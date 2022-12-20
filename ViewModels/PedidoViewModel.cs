using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using Cadeteria.Models;

namespace Cadeteria.ViewModels
{
    public class PedidoViewModel
    {
        public int Nro { get; set; }
        public string Detalles { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public CadeteViewModel Cadete { get; set; }
        public estado Estado { get; set; }

        public PedidoViewModel()
        {

        }

        public PedidoViewModel(int num, string detalles, ClienteViewModel cliente, CadeteViewModel cadete = null, estado estado = estado.SinAsignar)
        {
            Nro = num;
            Detalles = detalles;
            Cliente = cliente;
            Estado = estado;
            Cadete = cadete;
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
            return ped1.Nro == ped2.Nro;
        }

        static public bool operator !=(PedidoViewModel ped1, PedidoViewModel ped2)
        {
            return ped1.Nro != ped2.Nro;
        }
    }

    public class CrearPedidoViewModel
    {
        [AllowNull]
        [DisplayName("Detalles")]
        public string Detalles { get; set; }

        [Required]
        [DisplayName("ID")]
        public int ClienteID { get; set; }

        public bool NuevoCliente { get; set; }

        [AllowNull]
        [DisplayName("Nombre")]
        public string? ClienteNombre { get; set; }

        [AllowNull]
        [DisplayName("Dirección")]
        public string? ClienteDireccion { get; set; }

        [AllowNull]
        [DisplayName("Teléfono")]
        public string? ClienteTelefono { get; set; }

        public CrearPedidoViewModel()
        {

        }

        public CrearPedidoViewModel(string detalles, int id)
        {
            Detalles = detalles;
            ClienteID = id;
        }
    }
}
