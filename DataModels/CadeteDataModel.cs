using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.DataModels
{
    public class CadeteDataModel
    {
        static private string ConnectionString;

        static CadeteDataModel()
        {
            ConnectionString = $"Data Source=database/cadeteria.db";
        }

        static public List<CadeteViewModel> ObtenerCadetes(int id = 0)
        {
            List<CadeteViewModel> cadetes = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                if (id <= 0) comando.CommandText = "SELECT * FROM cadete";
                else
                {
                    comando.CommandText = "SELECT * FROM cadete WHERE id_cadete = $id";
                    comando.Parameters.AddWithValue("$id", id);
                }
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    cadetes.Add(new CadeteViewModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerCadetes): " + ex.Message);
            }

            conexion.Close();

            return cadetes;
        }
    }
}
