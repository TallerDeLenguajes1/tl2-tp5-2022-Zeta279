using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Cadeteria.ViewModels
{
    public class CadeteViewModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }

        public CadeteViewModel()
        {

        }
        public CadeteViewModel(int id, string nombre, string direccion, string telefono)
        {
            this.ID = id;
            this.Nombre = nombre;
            this.Direccion = direccion;
            this.Telefono = telefono;
        }

        static public bool operator ==(CadeteViewModel cad1, CadeteViewModel cad2)
        {
            return cad1.ID == cad2.ID;
        }

        static public bool operator !=(CadeteViewModel cad1, CadeteViewModel cad2)
        {
            return cad1.ID != cad2.ID;
        }
    }

    public class CrearCadeteViewModel
    {
        public int ID { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public long Telefono { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        public CrearCadeteViewModel()
        {

        }

        public CrearCadeteViewModel(string nombre, long telefono, string direccion)
        {
            Nombre = nombre;
            Telefono = telefono;
            Direccion = direccion;
        }
    }

    public class EditarCadeteViewModel
    {
        [Required]
        [NotNull]
        public int ID { get; set; }

        [Required]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public long Telefono { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }

        public EditarCadeteViewModel()
        {

        }
        public EditarCadeteViewModel(int iD, string nombre, long tel, string direccion)
        {
            ID = iD;
            Nombre = nombre;
            Telefono = tel;
            Direccion = direccion;
        }
    }
}
