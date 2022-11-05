using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Models
{
    public class DataModel
    {
        static public Dictionary<int, CadeteModel> CadeteList = ObtenerCadetes();
        static public Dictionary<int, PedidoModel> PedidoList = ObtenerPedidos();
        static public List<CadeteViewModel> CadeteVList = ObtenerViewCadetes();
        static public List<PedidoViewModel> PedidoVList = ObtenerViewPedidos();
        static public int IDCadetes { get; private set; }
        static public int IDPedidos { get; private set; }
        static private string direccion = "database/cadeteria.db";

        public DataModel()
        {
            // Unicamente utilizado para obtener el último ID
            SqliteConnection conexion = new SqliteConnection($"Data Source={direccion}");

            conexion.Open();

            SqliteCommand comando = new SqliteCommand("SELECT MAX(cadete.id_cadete), MAX(pedido.id_pedido) FROM cadete, pedido;", conexion);
            var reader = comando.ExecuteReader();

            IDCadetes = 0;
            IDPedidos = 0;

            while (reader.Read())
            {
                IDCadetes = reader.GetInt32(0);
                IDPedidos = reader.GetInt32(1);
            }

            conexion.Close();
        }

        static private List<CadeteViewModel> ObtenerViewCadetes()
        {
            List<CadeteViewModel> newList = new();

            /* rehacer con BD */

            return newList;
        }

        static private List<PedidoViewModel> ObtenerViewPedidos()
        {
            List<PedidoViewModel> newList = new();

            /* rehacer con BD */

            return newList;
        }

        static private Dictionary<int, CadeteModel> ObtenerCadetes()
        {
            Dictionary<int, CadeteModel> cadetes = new();

            try
            {
                SqliteConnection conexion = new SqliteConnection($"Data Source={direccion}");

                conexion.Open();

                SqliteCommand comando = new SqliteCommand("SELECT * FROM cadete;", conexion);
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    cadetes.Add(reader.GetInt32(0), new CadeteModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }

                conexion.Close();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerCadetes): " + ex.Message);
            }

            return cadetes;
        }

        static private Dictionary<int, PedidoModel> ObtenerPedidos()
        {
            Dictionary<int, PedidoModel> pedidos = new();

            try
            {
                SqliteConnection conexion = new SqliteConnection($"Data Source={direccion}");

                conexion.Open();

                SqliteCommand comando = new SqliteCommand("SELECT * FROM pedido INNER JOIN cliente USING (id_cliente);", conexion);
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(reader.GetInt32(0), new PedidoModel(reader.GetInt32(0), reader.GetString(1), reader.GetInt32(2), reader.GetString(4), reader.GetString(5), reader.GetString(6)));
                }

                conexion.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerPedidos): " + ex.Message);
            }

            return pedidos;
        }

        static public void ActualizarCadete(int id, string nom, string direc, string tel)
        {
            /* actualizar BD */
        }

        static public void IngresarCadete(string nom, string direc, string tel)
        {
            /* actualizar BD */
        }

        static private void ActualizarCadetes()
        {
            /* obtener datos de BD */
        }

        static public void RestaurarDatos()
        {
            /* obtener datos de BD para restaurar */
        }

        static public bool BorrarCadete(int id)
        {
            /* borrar de la BD */
        }

        static public void IngresarCadete(CadeteModel cadete)
        {
            /* actualizar BD */
        }

        static public void IngresarPedido(int nro, string det, int idc, string nomc, string direc, string tel)
        {
            IngresarPedido(new PedidoModel(nro, det, idc, nomc, direc, tel));
        }

        static public void IngresarPedido(string det, int idc, string nomc, string direc, string tel)
        {
            IngresarPedido(new PedidoModel(IDPedidos, det, idc, nomc, direc, tel));
        }

        static private void ActualizarPedidos()
        {
            PedidoList = ObtenerPedidos();
            PedidoVList = ObtenerViewPedidos();
        }

        static public void AsignarPedidoACadete(int idPedido, int idCadete)
        {
            PedidoList[idPedido].IniciarPedido();

            /* actualizar BD */

            ActualizarPedidos();
        }

        static public void BorrarPedido(int id)
        {
            PedidoList.Remove(id);

            ActualizarPedidos();
        }

        static public void IngresarPedido(PedidoModel pedido)
        {
            /* actualizar BD */
        }

        static public void EntregarPedido(int id)
        {
            PedidoList[id].EntregarPedido();

            ActualizarPedidos();
        }
    }
}
