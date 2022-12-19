using Cadeteria.Models;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Repo
{
    public interface ICadeteRepository
    {
        List<CadeteModel> ObtenerTodo();
        CadeteModel Obtener(int id);
        bool Crear(CadeteModel cadete);
        bool Actualizar(CadeteModel cadete);
        bool Borrar(int id);
    }

    public class CadeteRepository: ICadeteRepository
    {
        private string ConnectionString;

        public CadeteRepository()
        {
            ConnectionString = "Data Source=database/cadeteria.db";
        }

        public List<CadeteModel> ObtenerTodo()
        {
            List<CadeteModel> cadetes = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "SELECT * FROM cadete";
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    cadetes.Add(new CadeteModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (CadeteRepo, ObtenerTodo): " + ex.Message);
            }

            conexion.Close();

            return cadetes;
        }

        public CadeteModel Obtener(int id)
        {
            CadeteModel cadete = null;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "SELECT * FROM cadete WHERE id_cadete = $id";
                comando.Parameters.AddWithValue("$id", id);
                var reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    cadete = new CadeteModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (CadeteRepo, ObtenerTodo): " + ex.Message);
            }

            conexion.Close();

            return cadete;
        }

        public bool Crear(CadeteModel cadete)
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
                comando.CommandText = "SELECT MAX(id_cadete) + 1 FROM cadete";
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

                // Ingresar cadete
                reader.Close();
                comando.CommandText = "INSERT INTO cadete VALUES ($id, $nom, $direc, $tel, 1);";
                comando.Parameters.AddWithValue("id", cadete.ID);
                comando.Parameters.AddWithValue("$nom", cadete.Nombre);
                comando.Parameters.AddWithValue("$direc", cadete.Direccion);
                comando.Parameters.AddWithValue("$tel", cadete.Telefono);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (CadeteRepo, Crear): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool Actualizar(CadeteModel cadete)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE cadete SET nombre = $nom, direccion = $direc, telefono = $tel WHERE id_cadete = $id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$nom", cadete.Nombre);
                comando.Parameters.AddWithValue("$direc", cadete.Direccion);
                comando.Parameters.AddWithValue("$tel", cadete.Telefono);
                comando.Parameters.AddWithValue("$id", cadete.ID);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (CadeteRepo, Actualizar): " + ex.Message);
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
                comando.CommandText = "UPDATE pedido SET id_cadete = null WHERE id_cadete = $id AND estado = 'Entregado';";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                comando.CommandText = "UPDATE pedido SET id_cadete = null, estado = 'SinAsignar' WHERE id_cadete = $id AND estado != 'Entregado';";
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                comando.CommandText = "DELETE FROM cadete WHERE id_cadete = $id;";
                comando.Parameters.AddWithValue("$id", id);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (CadeteRepo, Borrar): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }
    }
}
