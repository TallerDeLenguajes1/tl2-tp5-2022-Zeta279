using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadeteria.ViewModels
{
    public class ClienteViewModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public ClienteViewModel()
        {

        }

        public ClienteViewModel(int iD, string nombre, string direccion, string telefono)
        {
            ID = iD;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }
    }

    public class CrearClienteViewModel
    {
        [Required]
        [DisplayName("ID")]
        public int ID { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public string Telefono { get; set; }

        public CrearClienteViewModel()
        {

        }

        public CrearClienteViewModel(int iD, string nombre, string direccion, string telefono)
        {
            ID = iD;
            Nombre = nombre;
            Direccion = direccion;
            Telefono = telefono;
        }
    }
}
