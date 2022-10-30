using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;
using TP5.Models;

namespace TP5.ViewModels
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

    class CrearPedidoViewModel
    {
        [AllowNull]
        public string Detalles { get; set; }

        [AllowNull]
        public CadeteViewModel Cadete { get; set; }

        [Required]
        public ClienteModel Cliente { get; set; }

        public bool EnCurso { get; set; }
        public bool Entregado { get; set; }

        public CrearPedidoViewModel()
        {

        }

        public CrearPedidoViewModel(string detalles, CadeteViewModel cadete, ClienteModel cliente)
        {
            Detalles = detalles;
            Cadete = cadete;
            Cliente = cliente;
            EnCurso = false;
            Entregado = false;
        }
    }
}
