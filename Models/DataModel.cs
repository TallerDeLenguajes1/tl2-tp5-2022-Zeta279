using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Cadeteria.Models
{
    public class DataModel
    {
        static public Dictionary<int, CadeteModel> CadeteList;
        static public Dictionary<int, PedidoModel> PedidoList;
        static public Dictionary<int, ClienteModel> ClienteList;
        static private Dictionary<int, int> PedidosConCadetes;
        static public List<CadeteViewModel> CadeteVList;
        static public List<PedidoViewModel> PedidoVList;
        static public List<ClienteViewModel> ClienteVList;
        static private string ConnectionString;

        static DataModel()
        {
            ConnectionString = $"Data Source=database/cadeteria.db";
            PedidosConCadetes = new();
            ActualizarCadetes();
            ActualizarClientes();
            ActualizarPedidos();
        }

        static private int ObtenerIDCadete()
        {
            int id = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT MAX(cadete.id_cadete) FROM cadete;", conexion);
                var reader = comando.ExecuteReader();
                reader.Read();

                if (!reader.IsDBNull(0))
                {
                    id = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (DataModel): " + ex.Message);
            }

            conexion.Close();

            return id;
        }

        static private int ObtenerIDCliente()
        {
            int id = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT MAX(id_cliente) FROM cliente;", conexion);
                var reader = comando.ExecuteReader();
                reader.Read();

                if (!reader.IsDBNull(0))
                {
                    id = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (DataModel): " + ex.Message);
            }

            conexion.Close();

            return id;
        }

        static private int ObtenerIDPedido()
        {
            int id = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT MAX(pedido.id_pedido) FROM pedido;", conexion);
                var reader = comando.ExecuteReader();
                reader.Read();

                if (!reader.IsDBNull(0))
                {
                    id = reader.GetInt32(0);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (DataModel): " + ex.Message);
            }

            conexion.Close();

            return id;
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
                if(PedidosConCadetes.ContainsKey(pedido.Nro))
                {
                    lista[lista.Count - 1].IngresarCadete(CadeteList[PedidosConCadetes[pedido.Nro]].nombre);
                }
            }

            return lista;
        }

        static private List<ClienteViewModel> ObtenerViewClientes()
        {
            List<ClienteViewModel> lista = new();

            foreach(var cliente in ClienteList.Values)
            {
                lista.Add(new ClienteViewModel(cliente.id, cliente.nombre, cliente.direccion, cliente.telefono));
            }

            return lista;
        }

        static private Dictionary<int, CadeteModel> ObtenerCadetes()
        {
            Dictionary<int, CadeteModel> cadetes = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();

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

            foreach(var cadete in CadeteList)
            {
                cadete.Value.EliminarPedidos();
            }

            PedidosConCadetes.Clear();

            try
            {
                SqliteCommand comando = new SqliteCommand("SELECT * FROM pedido INNER JOIN cliente USING (id_cliente);", conexion);
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    id = reader.GetInt32(0);
                    pedidos.Add(id, new PedidoModel(id, reader.GetString(1), reader.GetInt32(3), reader.GetString(5), reader.GetString(6), reader.GetString(7)));

                    // Asignar pedido al cadete
                    if (!reader.IsDBNull(4))
                    {
                        CadeteList[reader.GetInt32(4)].IngresarPedido(pedidos[id]);
                        PedidosConCadetes[id] = reader.GetInt32(4);
                    }

                    // Asignar estado
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

        static public Dictionary<int, ClienteModel> ObtenerClientes()
        {
            Dictionary<int, ClienteModel> lista = new();

            SqliteConnection conexion = new(ConnectionString);
            SqliteCommand comando = new();
            SqliteDataReader reader;

            conexion.Open();

            try
            {
                comando.Connection = conexion;
                comando.CommandText = "SELECT *  FROM cliente;";
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    lista.Add(reader.GetInt32(0), new ClienteModel(reader.GetInt32(0), reader.GetString(1), reader.GetString(2), reader.GetString(3)));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerClientes): " + ex.Message);
            }

            conexion.Close();

            return lista;
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
                comando.Parameters.AddWithValue("$id", ObtenerIDCadete() + 1);
                comando.Parameters.AddWithValue("$nom", nom);
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

        static public bool IngresarPedido(string det, int idc)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            SqliteDataReader reader;

            conexion.Open();

            try
            {
                comando.CommandText = "INSERT INTO pedido VALUES ($id, $det, 'SinAsignar', $id_c, null)";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", ObtenerIDPedido() + 1);
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

        static public bool IngresarCliente(int id, string nom, string direc, string tel)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);

            conexion.Open();

            try
            {
                SqliteCommand comando = new SqliteCommand();
                comando.Connection = conexion;
                comando.CommandText = "INSERT INTO cliente VALUES ($id, $nom, $direc, $tel);";
                comando.Parameters.AddWithValue("$id", ObtenerIDCliente() + 1);
                comando.Parameters.AddWithValue("$nom", nom);
                comando.Parameters.AddWithValue("$direc", direc);
                comando.Parameters.AddWithValue("$tel", tel);

                resultado = comando.ExecuteNonQuery();

                if (resultado > 0) ActualizarClientes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IngresarCliente): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        static public bool IngresarPedidoConCliente(string detPedido, string nomCliente, string direcCliente, string telCliente)
        {
            int resultado = 0, idCliente;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new SqliteCommand();

            idCliente = ObtenerIDCliente() + 1;

            conexion.Open();

            try
            {
                comando.Connection = conexion;
                comando.CommandText = "INSERT INTO cliente VALUES ($id, $nom, $direc, $tel);";
                comando.Parameters.AddWithValue("$id", idCliente);
                comando.Parameters.AddWithValue("$nom", nomCliente);
                comando.Parameters.AddWithValue("$direc", direcCliente);
                comando.Parameters.AddWithValue("$tel", telCliente);

                resultado = comando.ExecuteNonQuery();

                if(resultado > 0) ActualizarClientes();

                comando.Parameters.Clear();
                comando.CommandText = "INSERT INTO pedido VALUES ($id, $det, 'SinAsignar', $id_c, null)";
                comando.Parameters.AddWithValue("$id", ObtenerIDPedido() + 1);
                comando.Parameters.AddWithValue("$det", detPedido);
                comando.Parameters.AddWithValue("$id_c", idCliente);
                resultado = comando.ExecuteNonQuery();


                if (resultado > 0) ActualizarPedidos();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IngresarPedidoConCliente): " + ex.Message);
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

        static public bool ActualizarCliente(int id, string nom, string direc, string tel)
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

                if (resultado > 0) ActualizarClientes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ActualizarCliente): " + ex.Message);
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
                comando.CommandText = "UPDATE pedido SET id_cadete = null WHERE id_cadete = $id AND estado = 'Entregado';";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                comando.CommandText = "UPDATE pedido SET id_cadete = null, estado = 'SinAsignar' WHERE id_cadete = $id AND estado != 'EntregadoBorrado';";
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();

                ActualizarPedidos();

                comando.Parameters.Clear();
                comando.CommandText = "DELETE FROM cadete WHERE id_cadete = $id;";
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

        static public bool BorrarCliente(int id)
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

                ActualizarPedidos();

                comando.Parameters.Clear();
                comando.CommandText = "DELETE FROM cliente WHERE id_cliente = $id;";
                comando.Parameters.AddWithValue("$id", id);
                resultado = comando.ExecuteNonQuery();

                if (resultado > 0) ActualizarClientes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (BorrarCliente): " + ex.Message);
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
            try
            {
                string sql = File.ReadAllText("database/restaurar.txt");

                SqliteConnection conexion = new SqliteConnection(ConnectionString);
                conexion.Open();

                SqliteCommand comando = new(sql, conexion);
                comando.ExecuteNonQuery();

                conexion.Close();

                ActualizarCadetes();
                ActualizarClientes();
                ActualizarPedidos();
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error al restaurar la base de datos: " + ex.Message);
            }
            
        }

        static private void ActualizarPedidos()
        {
            PedidoList = ObtenerPedidos();
            PedidoVList = ObtenerViewPedidos();
        }

        static private void ActualizarClientes()
        {
            ClienteList = ObtenerClientes();
            ClienteVList = ObtenerViewClientes();
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

        static public bool IniciarPedido(int id)
        {
            bool resultado = false;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();

            try
            {
                comando.CommandText = "UPDATE pedido SET estado = 'EnCurso' WHERE id_pedido = $id AND id_cadete IS NOT NULL";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);

                if (comando.ExecuteNonQuery() > 0)
                {
                    resultado = true;
                    PedidoList[id].IniciarPedido();
                }

            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IniciarPedido): " + ex.Message);
            }

            conexion.Close();

            ActualizarPedidos();

            return resultado;
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
