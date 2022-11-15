using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Repo
{
    public interface IPedidoRepository
    {
        List<PedidoViewModel> ObtenerTodo();
        PedidoViewModel Obtener(int id);
        bool Crear(string det, int idc);
        bool CrearPedidoConCliente(string det, string nom, string direc, string tel);
        bool Borrar(int id);
        bool AsignarCadete(int idPedido, int idCadete);
        bool CambiarEstado(int id, estado estado);
        void BorrarCadete(int id);
    }

    public class PedidoResitory: IPedidoRepository
    {
        private string ConnectionString;

        public PedidoResitory()
        {
            ConnectionString = "Data Source=database/cadeteria.db";
        }

        public List<PedidoViewModel> ObtenerTodo()
        {
            List<PedidoViewModel> pedidos = new();
            CadeteRepository cadeteRepo = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM pedido INNER JOIN cliente USING (id_cliente);";
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(new PedidoViewModel(reader.GetInt32(0), reader.GetString(1), new ClienteViewModel(reader.GetInt32(3), reader.GetString(5), reader.GetString(6), reader.GetString(7))));
                    if (reader.GetString(2) == "Pendiente")
                    {
                        pedidos[pedidos.Count - 1].Estado = estado.Pendiente;
                    }
                    if (reader.GetString(2) == "EnCurso")
                    {
                        pedidos[pedidos.Count - 1].Estado = estado.EnCurso;
                    }
                    if (reader.GetString(2) == "Entregado")
                    {
                        pedidos[pedidos.Count - 1].Estado = estado.Entregado;
                    }

                    if (!reader.IsDBNull(4))
                    {
                        pedidos[pedidos.Count - 1].IngresarCadete(cadeteRepo.Obtener(reader.GetInt32(4)).nombre);
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, ObtenerTodo): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        public PedidoViewModel Obtener(int id)
        {
            PedidoViewModel pedido = null;
            CadeteRepository cadeteRepo = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM pedido WHERE id_pedido = $id;";
                comando.Parameters.AddWithValue("$id", id);
                reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    pedido = new PedidoViewModel(reader.GetInt32(0), reader.GetString(1), new ClienteViewModel(reader.GetInt32(3), reader.GetString(5), reader.GetString(6), reader.GetString(7)));
                    if (reader.GetString(2) == "Pendiente")
                    {
                        pedido.Estado = estado.Pendiente;
                    }
                    if (reader.GetString(2) == "EnCurso")
                    {
                        pedido.Estado = estado.EnCurso;
                    }
                    if (reader.GetString(2) == "Entregado")
                    {
                        pedido.Estado = estado.Entregado;
                    }

                    if (!reader.IsDBNull(4))
                    {
                        pedido.IngresarCadete(cadeteRepo.Obtener(reader.GetInt32(4)).nombre);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, Obtener): " + ex.Message);
            }

            conexion.Close();

            return pedido;
        }

        public bool Crear(string det, int idc)
        {
            int resultado = 0, id;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                // Obtener ID
                comando.CommandText = "SELECT MAX(id_pedido) + 1 FROM pedido";
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

                // Ingresar pedido
                comando.CommandText = "INSERT INTO pedido VALUES ($id, $det, 'SinAsignar', $id_c, null)";
                comando.Parameters.AddWithValue("$id", id);
                comando.Parameters.AddWithValue("$det", det);
                comando.Parameters.AddWithValue("$id_c", idc);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, Crear): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool CrearPedidoConCliente(string det, string nom, string direc, string tel)
        {
            int resultado = 0, idp, idc;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                // Obtener ID de pedido
                comando.CommandText = "SELECT MAX(id_pedido) + 1 FROM pedido";
                reader = comando.ExecuteReader();
                reader.Read();

                if (reader.IsDBNull(0))
                {
                    idp = 1;
                }
                else
                {
                    idp = reader.GetInt32(0);
                }

                // Obtener ID de cliente
                comando.CommandText = "SELECT MAX(id_cliente) + 1 FROM cliente";
                reader.Close();
                reader = comando.ExecuteReader();
                reader.Read();

                if (reader.IsDBNull(0))
                {
                    idc = 1;
                }
                else
                {
                    idc = reader.GetInt32(0);
                }

                // Ingresar pedido
                comando.CommandText = "INSERT INTO pedido VALUES ($id, $det, 'SinAsignar', $id_c, null)";
                comando.Parameters.AddWithValue("$id", idp);
                comando.Parameters.AddWithValue("$det", det);
                comando.Parameters.AddWithValue("$id_c", idc);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, CrearPedidoConCliente): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool Borrar(int id)
        {
            int resultado = 0;
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "DELETE FROM pedido WHERE id_pedido = $id";
                comando.Parameters.AddWithValue("$id", id);

                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, Borrar): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool AsignarCadete(int idPedido, int idCadete)
        {
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();

            try
            {
                comando.CommandText = "UPDATE pedido SET id_cadete = $idc WHERE id_pedido = $idp AND estado != 'Entregado'";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$idc", idCadete);
                comando.Parameters.AddWithValue("$idp", idPedido);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, AsignarCadete): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool CambiarEstado(int id, estado estado)
        {
            string est = null;
            int resultado = 0;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();

            try
            {
                if(estado == estado.SinAsignar)
                {
                    est = "SinAsignar";
                }
                if(estado == estado.Pendiente)
                {
                    est = "Pendiente";
                }
                if(estado == estado.EnCurso)
                {
                    est = "EnCurso";
                }
                if(estado == estado.Entregado)
                {
                    est = "Entregado";
                }

                comando.CommandText = "UPDATE pedido SET estado = $est WHERE id_pedido = $idp;";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);
                comando.Parameters.AddWithValue("$est", est);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, CambiarEstado): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public void BorrarCadete(int id)
        {
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();

            try
            {
                comando.CommandText = "UPDATE pedido SET id_cadete = null, estado = 'SinAsignar' WHERE id_pedido = $idp AND estado != 'Entregado';";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();

                comando.Parameters.Clear();
                comando.CommandText = "UPDATE pedido SET id_cadete = null WHERE id_pedido = $idp AND estado = 'Entregado';";
                comando.Parameters.AddWithValue("$id", id);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, BorrarCadete): " + ex.Message);
            }

            conexion.Close();
        }
    }
}
