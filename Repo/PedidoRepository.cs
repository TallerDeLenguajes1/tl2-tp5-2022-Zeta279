﻿using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Repo
{
    public interface IPedidoRepository
    {
        List<PedidoViewModel> ObtenerTodo();
        PedidoViewModel Obtener(int id);
        bool Borrar(int id);
        void AsignarCadete(int idPedido, int idCadete);
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

        public bool Borrar(int id)
        {
            int resultado = 0;
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();

            try
            {
                comando.CommandText = "DELETE FROM pedido WHERE id_pedido = $id";
                comando.Connection = conexion;
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

        public void AsignarCadete(int idPedido, int idCadete)
        {
            SqliteConnection conexion = new SqliteConnection(ConnectionString);
            conexion.Open();
            SqliteCommand comando = new();

            try
            {
                comando.CommandText = "UPDATE pedido SET id_cadete = $idc WHERE id_pedido = $idp AND estado != 'Entregado'";
                comando.Connection = conexion;
                comando.Parameters.AddWithValue("$idc", idCadete);
                comando.Parameters.AddWithValue("$idp", idPedido);
                comando.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (PedidoRepo, AsignarCadete): " + ex.Message);
            }

            conexion.Close();
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