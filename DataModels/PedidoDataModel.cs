using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using System.Data;

namespace Cadeteria.DataModels
{
    public class PedidoDataModel
    {
        static private string ConnectionString;

        static PedidoDataModel()
        {
            ConnectionString = "Data Source=database/cadeteria.db";
        }

        static public List<PedidoViewModel> ObtenerPedidos(int id = 0)
        {
            List<PedidoViewModel> pedidos = new();
            string estado;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                if (id <= 0) comando.CommandText = "SELECT * FROM pedido";
                else
                {
                    comando.CommandText = "SELECT * FROM pedido WHERE id_pedido = $id";
                    comando.Parameters.AddWithValue("$id", id);
                }
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(new PedidoViewModel(reader.GetInt32(0), reader.GetString(1), new Models.ClienteModel(reader.GetInt32(3), reader.GetString(6), reader.GetString(7), reader.GetString(8))));
                    estado = reader.GetString(2);
                    if (estado == "SinAsignar") pedidos[pedidos.Count - 1].Estado = Models.estado.SinAsignar;
                    if (estado == "Pendiente") pedidos[pedidos.Count - 1].Estado = Models.estado.Pendiente;
                    if (estado == "EnCurso") pedidos[pedidos.Count - 1].Estado = Models.estado.EnCurso;
                    if (estado == "Entregado") pedidos[pedidos.Count - 1].Estado = Models.estado.Entregado;
                    if (!reader.IsDBNull(4))
                    {
                        pedidos[pedidos.Count - 1].IngresarCadete(CadeteDataModel.ObtenerCadetes(reader.GetInt32(4))[0].nombre);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerCadetes): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        static public List<PedidoViewModel> ObtenerPedidosDeCliente(int idCliente)
        {
            List<PedidoViewModel> pedidos = new();
            string estado;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "SELECT * FROM pedido INNER JOIN cliente USING (id_cliente) WHERE id_cliente = $id";
                comando.Parameters.AddWithValue("$id", idCliente);
                
                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(new PedidoViewModel(reader.GetInt32(0), reader.GetString(1), new Models.ClienteModel(reader.GetInt32(3), reader.GetString(5), reader.GetString(6), reader.GetString(7))));
                    estado = reader.GetString(2);
                    if (estado == "SinAsignar") pedidos[pedidos.Count - 1].Estado = Models.estado.SinAsignar;
                    if (estado == "Pendiente") pedidos[pedidos.Count - 1].Estado = Models.estado.Pendiente;
                    if (estado == "EnCurso") pedidos[pedidos.Count - 1].Estado = Models.estado.EnCurso;
                    if (estado == "Entregado") pedidos[pedidos.Count - 1].Estado = Models.estado.Entregado;
                    if (!reader.IsDBNull(4))
                    {
                        pedidos[pedidos.Count - 1].IngresarCadete(CadeteDataModel.ObtenerCadetes(reader.GetInt32(4))[0].nombre);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerPedidosDeCliente): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }

        static public List<PedidoViewModel> ObtenerPedidosDeCadete(int idCadete)
        {
            List<PedidoViewModel> pedidos = new();
            string estado;

            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            SqliteCommand comando = new();
            conexion.Open();
            comando.Connection = conexion;

            try
            {
                comando.CommandText = "SELECT * FROM pedido INNER JOIN cliente USING (id_cliente) WHERE id_cadete = $id";
                comando.Parameters.AddWithValue("$id", idCadete);

                var reader = comando.ExecuteReader();

                while (reader.Read())
                {
                    pedidos.Add(new PedidoViewModel(reader.GetInt32(0), reader.GetString(1), new Models.ClienteModel(reader.GetInt32(3), reader.GetString(5), reader.GetString(6), reader.GetString(7))));
                    estado = reader.GetString(2);
                    if (estado == "SinAsignar") pedidos[pedidos.Count - 1].Estado = Models.estado.SinAsignar;
                    if (estado == "Pendiente") pedidos[pedidos.Count - 1].Estado = Models.estado.Pendiente;
                    if (estado == "EnCurso") pedidos[pedidos.Count - 1].Estado = Models.estado.EnCurso;
                    if (estado == "Entregado") pedidos[pedidos.Count - 1].Estado = Models.estado.Entregado;
                    if (!reader.IsDBNull(4))
                    {
                        pedidos[pedidos.Count - 1].IngresarCadete(CadeteDataModel.ObtenerCadetes(reader.GetInt32(4))[0].nombre);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (ObtenerPedidosDeCadete): " + ex.Message);
            }

            conexion.Close();

            return pedidos;
        }
    }
}
