using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Repo
{
    public interface IUsuarioRepository
    {
        UsuarioViewModel Logear(string usuario, string pass);
    }

    public class UsuarioRepository: IUsuarioRepository
    {
        private string ConnectionString;

        public UsuarioRepository()
        {
            ConnectionString = "Data Source=database/cadeteria.db";
        }

        public UsuarioViewModel Logear(string usuario, string pass)
        {
            UsuarioViewModel user = null;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "SELECT * FROM usuario WHERE usuario = $user AND password = $pass";
                comando.Parameters.AddWithValue("$user", usuario);
                comando.Parameters.AddWithValue("$pass", pass);
                var reader = comando.ExecuteReader();

                if(reader.Read() && !reader.IsDBNull(0))
                {
                    if(reader.GetString(4) == "Administrador")
                    {
                        user = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), RolUsuario.Administrador);
                    }
                    if(reader.GetString(4) == "Encargado")
                    {
                        user = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), RolUsuario.Encargado);
                    }
                }
                
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (UsuarioRepo, Logear): " + ex.Message);
            }

            conexion.Close();

            return user;
        }
    }
}