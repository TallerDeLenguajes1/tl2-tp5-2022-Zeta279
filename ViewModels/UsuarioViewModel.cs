using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Cadeteria.ViewModels
{
    public enum RolUsuario{
        Administrador,
        Encargado
    }

    public class UsuarioViewModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public RolUsuario Rol {get; set; }

        public UsuarioViewModel()
        {

        }

        public UsuarioViewModel(int id, string name, string user, RolUsuario rol)
        {
            ID = id;
            Nombre = name;
            Usuario = user;
            Rol = rol;
        }
    }
}
