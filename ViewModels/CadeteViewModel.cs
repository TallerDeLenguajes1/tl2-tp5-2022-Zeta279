﻿using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;

namespace Cadeteria.ViewModels
{
    public class CadeteViewModel
    {
        public int CadeteId { get; set; }
        public string nombre { get; set; }
        public string direccion { get; set; }
        public string telefono { get; set; }

        public CadeteViewModel()
        {

        }
        public CadeteViewModel(int id, string nombre, string direccion, string telefono)
        {
            this.CadeteId = id;
            this.nombre = nombre;
            this.direccion = direccion;
            this.telefono = telefono;
        }

        static public bool operator ==(CadeteViewModel cad1, CadeteViewModel cad2)
        {
            return cad1.CadeteId == cad2.CadeteId;
        }

        static public bool operator !=(CadeteViewModel cad1, CadeteViewModel cad2)
        {
            return cad1.CadeteId != cad2.CadeteId;
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
