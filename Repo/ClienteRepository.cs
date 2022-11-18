using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Reflection.PortableExecutable;

namespace Cadeteria.Repo
{
    public interface IClienteRepository
    {
        List<ClienteViewModel> ObtenerTodo();
        ClienteViewModel Obtener(int id);
        bool Crear(string nom, string direc, string tel);
        bool Actualizar(int id, string nom, string direc, string tel);
        bool Borrar(int id);
    }

    public class ClienteRepository: IClienteRepository
    {
        private string ConnectionString;

        public ClienteRepository()
        {
            ConnectionString = "Data Source=database/cadeteria.db";
        }

        public List<ClienteViewModel> ObtenerTodo()
        {
            List<ClienteViewModel> clientes = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "SELECT * FROM cliente";
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    clientes.Add(new ClienteViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ClienteRepo, ObtenerTodo): " + ex.Message);
            }

            conexion.Close();

            return clientes;
        }

        public ClienteViewModel Obtener(int id)
        {
            ClienteViewModel cliente = null;
            SqliteConnection conexion = new(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM cliente WHERE id_cliente = $id";
                comando.Parameters.AddWithValue("$id", id);
                reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    cliente = new ClienteViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ClienteRepo, Obtener): " + ex.Message);
            }

            conexion.Close();

            return cliente;
        }

        public bool Crear(string nom, string direc, string tel)
        {
            int resultado = 0, id;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new SqliteCommand();
            SqliteDataReader reader;
            comando.Connection = conexion;

            try
            {
                // Obtener ID
                comando.CommandText = "SELECT MAX(id_cliente) + 1 FROM cliente";
                reader = comando.ExecuteReader();
                reader.Read();

                if (reader.IsDBNull(0))
                {
                    id = 1;
                }
                else
                {
                    id = reader.GetInt32(0);
                }

                // Ingresar cliente
                reader.Close();
                comando.CommandText = "INSERT INTO cliente VALUES ($id, $nom, $direc, $tel);";
                comando.Parameters.AddWithValue("id", id);
                comando.Parameters.AddWithValue("$nom", nom);
                comando.Parameters.AddWithValue("$direc", direc);
                comando.Parameters.AddWithValue("$tel", tel);

                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ClienteRepo, Crear): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool Actualizar(int id, string nom, string direc, string tel)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE cliente SET nombre = $nom, direccion = $direc, telefono = $tel WHERE id_cliente = $id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$nom", nom);
                comando.Parameters.AddWithValue("$direc", direc);
                comando.Parameters.AddWithValue("$tel", tel);
                comando.Parameters.AddWithValue("$id", id);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ClienteRepo, Actualizar): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool Borrar(int id)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();

            conexion.Open();

            try
            {
                comando.CommandText = "DELETE FROM pedido WHERE id_cliente = $id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                comando.CommandText = "DELETE FROM cliente WHERE id_cliente = $id;";
                comando.Parameters.AddWithValue("$id", id);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ClienteRepo, Borrar): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }
    }
}
