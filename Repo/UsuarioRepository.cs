using Cadeteria.ViewModels;
using Cadeteria.Models;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Repo
{
    public interface IUsuarioRepository
    {
        UsuarioModel Logear(string usuario, string pass);
    }

    public class UsuarioRepository: IUsuarioRepository
    {
        private string ConnectionString;

        public UsuarioRepository()
        {
            ConnectionString = "Data Source=database/cadeteria.db";
        }

        public UsuarioModel Logear(string usuario, string pass)
        {
            UsuarioModel user = null;
            int idCadete = 0;

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
                    if (!reader.IsDBNull(5))
                    {
                        idCadete = reader.GetInt32(5);
                    }
                    if (reader.GetString(4) == "Administrador")
                    {
                        user = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), RolUsuario.Administrador, idCadete);
                    }
                    if (reader.GetString(4) == "Cadete")
                    {
                        user = new(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), RolUsuario.Cadete, idCadete);
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