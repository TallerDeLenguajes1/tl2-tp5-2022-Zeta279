using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Cadeteria.ViewModels
{
    public class CadeteViewModel
    {
        public int CadeteId { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public long telefono { get; set; }

        public CadeteViewModel()
        {

        }
        public CadeteViewModel(int id, string nombre, string direccion, long telefono)
        {
            this.CadeteId = id;
            this.nombre = nombre;
            this.direccion = direccion;
            this.telefono = telefono;
        }
    }

    public class CrearCadeteViewModel
    {
        [Required]
        [DisplayName("Nombre")]
        public string Nombre { get; set; }

        [Required]
        [DisplayName("Teléfono")]
        public long Tel { get; set; }

        [Required]
        [StringLength(80)]
        [DisplayName("Dirección")]
        public string Direccion { get; set; }
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
        public long Tel { get; set; }

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
            Tel = tel;
            Direccion = direccion;
        }
    }
}
