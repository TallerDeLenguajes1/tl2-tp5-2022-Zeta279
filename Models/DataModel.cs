using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Cadeteria.Models
{
    public class DataModel
    {
        static public Dictionary<int, CadeteModel> CadeteList;
        static public Dictionary<int, PedidoModel> PedidoList;
        static private Dictionary<int, int> PedidosConCadetes;
        static public List<CadeteViewModel> CadeteVList;
        static public List<PedidoViewModel> PedidoVList;
        static private string ConnectionString;
        static private int IDCadetes;
        static private int IDPedidos;

        static DataModel()
        {
            ConnectionString = $"Data Source=database/cadeteria.db";
            IDCadetes = 0;
            IDPedidos = 0;
            PedidosConCadetes = new();
            ObtenerIDs();
            ActualizarCadetes();
            ActualizarPedidos();
        }

        static private void ObtenerIDs()
        {
            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT MAX(cadete.id_cadete), MAX(pedido.id_pedido) FROM cadete, pedido;", conexion);
                var reader = comando.ExecuteReader();

                IDCadetes = 1;
                IDPedidos = 1;

                if (reader.Read() && !reader.IsDBNull(0) && !reader.IsDBNull(1))
                {
                    IDCadetes = reader.GetInt32(0) + 1;
                    IDPedidos = reader.GetInt32(1) + 1;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (DataModel): " + ex.Message);
            }

            conexion.Close();
        }

        static private List<CadeteViewModel> ObtenerViewCadetes()
        {
            List<CadeteViewModel> lista = new();

            foreach(var cadete in CadeteList.Values)
            {
                lista.Add(new CadeteViewModel(cadete.id, cadete.nombre, cadete.direccion, cadete.telefono));
            }

            return lista;
        }

        static private List<PedidoViewModel> ObtenerViewPedidos()
        {
            List<PedidoViewModel> lista = new();

            foreach(var pedido in PedidoList.Values)
            {
                lista.Add(new PedidoViewModel(pedido.Nro, pedido.Detalles, pedido.cliente, pedido.Estado));
                if(pedido.Estado != estado.SinAsignar && pedido.Estado != estado.EntregadoBorrado)
                {
                    lista[lista.Count - 1].IngresarCadete(CadeteList[PedidosConCadetes[pedido.Nro]].nombre);
                }
            }

            return lista;
        }

        static private Dictionary<int, CadeteModel> ObtenerCadetes()
        {
            Dictionary<int, CadeteModel> cadetes = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();

            if (IDCadetes == 0 || IDPedidos == 0) ObtenerIDs();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT * FROM cadete;", conexion);
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    cadetes.Add(reader.GetInt32(0), new CadeteModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerCadetes): " + ex.Message);
            }

            conexion.Close();

            return cadetes;
        }

        static private Dictionary<int, PedidoModel> ObtenerPedidos()
        {
            Dictionary<int, PedidoModel> pedidos = new();
            int id = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            if (IDCadetes == 0 || IDPedidos == 0) ObtenerIDs();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT * FROM pedido INNER JOIN cliente USING (id_cliente);", conexion);
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    pedidos.Add(id, new PedidoModel(id, reader.GetString(1), reader.GetInt32(3), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
                    if (reader.GetString(2) != "SinAsignar" && reader.GetString(3) != null)
                    {
                        CadeteList[reader.GetInt32(3)].IngresarPedido(pedidos[id]);
                        PedidosConCadetes[id] = reader.GetInt32(3);
                    }
                    if (reader.GetString(2) == "Pendiente")
                    {
                        pedidos[id].AsignarCadete();
                    }
                    if (reader.GetString(2) == "EnCurso")
                    {
                        pedidos[id].IniciarPedido();
                    }
                    if (reader.GetString(2) == "Entregado")
                    {
                        pedidos[id].EntregarPedido();
                    }
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerPedidos): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        static public bool IngresarCadete(string nom, string direc, string tel)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                SqliteCommand comando = new SqliteCommand();
                comando.Connection = conexion;
                comando.CommandText = "INSERT INTO cadete VALUES ($id, $nom, $direc, $tel, 1);";
                comando.Parameters.AddWithValue("$nom", nom);
                comando.Parameters.AddWithValue("$id", IDCadetes++);
                comando.Parameters.AddWithValue("$direc", direc);
                comando.Parameters.AddWithValue("$tel", tel);

                resultado = comando.ExecuteNonQuery();

                if (resultado > 0) ActualizarCadetes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IngresarCadete): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        static public bool IngresarPedido(string det, int idc, string nomc, string direc, string tel)
        {
            int resultado = 0;
            SqliteDataReader reader;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                // Obtener ID de cliente
                SqliteCommand comando = new();
                comando.CommandText = "SELECT MAX(id_cliente) FROM cliente";
                comando.Connection = conexion;
                reader = comando.ExecuteReader();
                reader.Read();

                idc = 1;
                if (!reader.IsDBNull(0))
                {
                    idc = reader.GetInt32(0) + 1;
                }
                reader.Close();

                // Ingresar el cliente en caso de no existir
                comando.CommandText = "SELECT * FROM cliente WHERE id_cliente = $id";
                comando.Parameters.AddWithValue("$id", idc);

                Console.WriteLine("Por ingresar cliente");
                if (comando.ExecuteScalar() == null)
                {
                    comando.Parameters.Clear();
                    comando.CommandText = "INSERT INTO cliente VALUES ($id, $nom, $direc, $tel)";
                    comando.Parameters.AddWithValue("$id", idc);
                    comando.Parameters.AddWithValue("$nom", nomc);
                    comando.Parameters.AddWithValue("$direc", direc);
                    comando.Parameters.AddWithValue("$tel", tel);
                    comando.ExecuteNonQuery();
                }
                Console.WriteLine("Cliente ingresado");

                // Ingresar pedido
                comando.Parameters.Clear();
                comando.CommandText = "INSERT INTO pedido VALUES ($id, $det, 'SinAsignar', $id_c, null)";
                comando.Parameters.AddWithValue("$id", IDPedidos++);
                comando.Parameters.AddWithValue("$det", det);
                comando.Parameters.AddWithValue("$id_c", idc);
                resultado = comando.ExecuteNonQuery();

                ActualizarPedidos();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IngresarPedido): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        static public bool ActualizarCadete(int id, string nom, string direc, string tel)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE cadete SET nombre = $nom, direccion = $direc, telefono = $tel WHERE id_cadete = $id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$nom", nom);
                comando.Parameters.AddWithValue("$direc", direc);
                comando.Parameters.AddWithValue("$tel", tel);
                comando.Parameters.AddWithValue("$id", id);
                resultado = comando.ExecuteNonQuery();

                if (resultado > 0) ActualizarCadetes();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ActualizarCadete): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        static public bool BorrarCadete(int id)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();

            conexion.Open();

            try
            {
                comando.CommandText = "DELETE FROM cadete WHERE id_cadete = $id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);
                resultado = comando.ExecuteNonQuery();

                if (resultado > 0) ActualizarCadetes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (BorrarCadete): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        static public void BorrarPedido(int id)
        {
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();

            try
            {
                comando.CommandText = "DELETE FROM pedido WHERE id_pedido = $id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);

                if (comando.ExecuteNonQuery() > 0) ActualizarPedidos();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (BorrarPedido): " + ex.Message);
            }

            conexion.Close();

            ActualizarPedidos();
        }

        static private void ActualizarCadetes()
        {
            CadeteList = ObtenerCadetes();
            CadeteVList = ObtenerViewCadetes();
        }

        static public void RestaurarDatos()
        {
            /* obtener datos de BD para restaurar */
        }        

        static private void ActualizarPedidos()
        {
            PedidoList = ObtenerPedidos();
            PedidoVList = ObtenerViewPedidos();
        }

        static public void AsignarPedidoACadete(int idPedido, int idCadete)
        {

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE pedido SET id_cadete = @idc, estado = 'Pendiente' WHERE id_pedido = @idp";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("@idc", idCadete);
                comando.Parameters.AddWithValue("@idp", idPedido);

                if(comando.ExecuteNonQuery() > 0)
                {
                    PedidoList[idPedido].AsignarCadete();
                    CadeteList[idCadete].IngresarPedido(PedidoList[idPedido]);
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (AsignarPedidoACadete): " + ex.Message);
            }

            conexion.Close();

            ActualizarPedidos();
        }

        static public void IniciarPedido(int id)
        {
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE pedido SET estado = 'EnCurso' WHERE id_pedido = @id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("@id", id);

                if (comando.ExecuteNonQuery() > 0) PedidoList[id].IniciarPedido();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IniciarPedido): " + ex.Message);
            }

            conexion.Close();

            ActualizarPedidos();
        }

        

        static public void EntregarPedido(int id)
        {
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE pedido SET estado = 'Entregado' WHERE id_pedido = @id";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("@id", id);

                if(comando.ExecuteNonQuery() > 0) PedidoList[id].EntregarPedido();

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (EntregarPedido): " + ex.Message);
            }

            conexion.Close();

            ActualizarPedidos();
        }
    }
}
