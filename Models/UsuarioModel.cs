namespace Cadeteria.Models
{
    public enum RolUsuario
    {
        Administrador,
        Encargado
    }
    public class UsuarioModel
    {
        public int ID { get; set; }
        public string Nombre { get; set; }
        public string Usuario { get; set; }
        public RolUsuario Rol { get; set; }

        public UsuarioModel()
        {

        }

        public UsuarioModel(int id, string name, string user, RolUsuario rol)
        {
            ID = id;
            Nombre = name;
            Usuario = user;
            Rol = rol;
        }
    }
}
