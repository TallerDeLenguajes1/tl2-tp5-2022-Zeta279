using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Repo
{
    public interface IPedidoRepository
    {
        List<PedidoModel> ObtenerTodo();
        PedidoModel Obtener(int id);
        List<PedidoModel> ObtenerPorCadete(int id);
        List<PedidoModel> ObtenerPorCliente(int id);
        bool Crear(PedidoModel pedido);
        bool CrearPedidoConCliente(PedidoModel pedido);
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

        public List<PedidoModel> ObtenerTodo()
        {
            List<PedidoModel> pedidos = new();
            CadeteRepository cadeteRepo = new();
            ClienteRepository clienteRepo = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM pedido;";
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.IsDBNull(4))
                    {
                        pedidos.Add(new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3))));
                    }
                    else
                    {
                        pedidos.Add(new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3)), cadeteRepo.Obtener(reader.GetInt32(4))));
                    }

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
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, ObtenerTodo): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        public PedidoModel Obtener(int id)
        {
            PedidoModel pedido = null;
            CadeteRepository cadeteRepo = new();
            ClienteRepository clienteRepo = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM pedido WHERE id_pedido = $id;";
                comando.Parameters.AddWithValue("$id", id);
                reader = comando.ExecuteReader();

                if (reader.Read())
                {
                    if (reader.IsDBNull(4))
                    {
                        pedido = new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3)));
                    }
                    else
                    {
                        pedido = new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3)), cadeteRepo.Obtener(reader.GetInt32(4)));
                    }

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, Obtener): " + ex.Message);
            }

            conexion.Close();

            return pedido;
        }

        public List<PedidoModel> ObtenerPorCadete(int id)
        {
            List<PedidoModel> pedidos = new();
            CadeteRepository cadeteRepo = new();
            ClienteRepository clienteRepo = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM pedido WHERE id_cadete = $id;";
                comando.Parameters.AddWithValue("$id", id);
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3)), cadeteRepo.Obtener(reader.GetInt32(4))));
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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, ObtenerPorCadete): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        public List<PedidoModel> ObtenerPorCliente(int id)
        {
            List<PedidoModel> pedidos = new();
            CadeteRepository cadeteRepo = new();
            ClienteRepository clienteRepo = new();

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();
            comando.Connection = conexion;
            SqliteDataReader reader;

            try
            {
                comando.CommandText = "SELECT * FROM pedido WHERE id_cliente = $id;";
                comando.Parameters.AddWithValue("$id", id);
                reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    if (reader.IsDBNull(4))
                    {
                        pedidos.Add(new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3))));
                    }
                    else
                    {
                        pedidos.Add(new PedidoModel(reader.GetInt32(0), reader.GetString(1), clienteRepo.Obtener(reader.GetInt32(3)), cadeteRepo.Obtener(reader.GetInt32(4))));
                    }

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
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, ObtenerPorCliente): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        public bool Crear(PedidoModel pedido)
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
                reader.Close();
                comando.CommandText = "INSERT INTO pedido VALUES ($id, $det, 'SinAsignar', $id_c, null)";
                comando.Parameters.AddWithValue("$id", id);
                comando.Parameters.AddWithValue("$det", pedido.Detalles);
                comando.Parameters.AddWithValue("$id_c", pedido.Cliente.ID);
                resultado = comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, Crear): " + ex.Message);
            }

            conexion.Close();

            return resultado > 0;
        }

        public bool CrearPedidoConCliente(PedidoModel pedido)
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
                reader.Close();
                comando.CommandText = "SELECT MAX(id_cliente) + 1 FROM cliente";
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

                // Ingresar cliente
                reader.Close();
                comando.CommandText = "INSERT INTO cliente VALUES ($idc, $nom, $direc, $tel)";
                comando.Parameters.AddWithValue("$idc", idc);
                comando.Parameters.AddWithValue("nom", pedido.Cliente.Nombre);
                comando.Parameters.AddWithValue("$direc", pedido.Cliente.Direccion);
                comando.Parameters.AddWithValue("$tel", pedido.Cliente.Telefono);
                comando.ExecuteNonQuery();

                // Ingresar pedido
                reader.Close();
                comando.CommandText = "INSERT INTO pedido VALUES ($idp, $det, 'SinAsignar', $idc, null)";
                comando.Parameters.Clear();
                comando.Parameters.AddWithValue("$idp", idp);
                comando.Parameters.AddWithValue("$det", pedido.Detalles);
                comando.Parameters.AddWithValue("$idc", idc);
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
                comando.CommandText = "UPDATE pedido SET id_cadete = $idc, estado = 'Pendiente' WHERE id_pedido = $idp AND estado != 'Entregado'";
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

                comando.CommandText = "UPDATE pedido SET estado = $est WHERE id_pedido = $id;";
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
