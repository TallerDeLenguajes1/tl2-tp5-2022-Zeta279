using Cadeteria.ViewModels;

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

        public DataModel()
        {
            if (CadeteList.Count > 0) IDCadetes = CadeteList[CadeteList.Count - 1].id;
            else IDCadetes = 0;

            if (PedidoList.Count > 0) IDPedidos = PedidoList[PedidoList.Count - 1].Nro;
            else IDPedidos = 0;
        }

        static private List<CadeteViewModel> ObtenerViewCadetes()
        {
            List<CadeteViewModel> newList = new();

            foreach(var cadete in CadeteList)
            {
                newList.Add(new CadeteViewModel(cadete.Key, cadete.Value.nombre, cadete.Value.direccion, cadete.Value.telefono));
            }

            return newList;
        }

        static private List<PedidoViewModel> ObtenerViewPedidos()
        {
            List<PedidoViewModel> newList = new();

            foreach (var pedido in PedidoList)
            {
                if(pedido.Value.Cadete is not null)
                {
                    newList.Add(new PedidoViewModel(pedido.Key, pedido.Value.Detalles, new CadeteViewModel(pedido.Value.Cadete.id, pedido.Value.Cadete.nombre, pedido.Value.Cadete.direccion, pedido.Value.Cadete.telefono), pedido.Value.cliente, pedido.Value.EstaEnCurso(), pedido.Value.FueEntregado()));
                }
                else
                {
                    newList.Add(new PedidoViewModel(pedido.Key, pedido.Value.Detalles, null, pedido.Value.cliente, pedido.Value.EstaEnCurso(), pedido.Value.FueEntregado()));
                }
            }

            return newList;
        }

        static private Dictionary<int, CadeteModel> ObtenerCadetes()
        {
            string[] array;
            Dictionary<int, CadeteModel> cadetes = new();

            foreach (string s in File.ReadAllLines("CSV/cadetes.csv"))
            {
                if (s != "")
                {
                    try
                    {
                        array = s.Split(";");
                        cadetes.Add(Int32.Parse(array[0]), new CadeteModel(Int32.Parse(array[0]), array[1], array[2], long.Parse(array[3])));
                        IDCadetes = Int32.Parse(array[0]) + 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ha ocurrido un error (ObtenerCadetes): " + ex.Message);
                    }
                }
            }

            return cadetes;
        }

        static private Dictionary<int, PedidoModel> ObtenerPedidos()
        {
            string[] aux;
            Dictionary<int, PedidoModel> lista = new();

            foreach (var s in File.ReadAllLines("CSV/pedidos.csv"))
            {
                if (s != "")
                {
                    aux = s.Split(";");
                    try
                    {
                        lista.Add(Int32.Parse(aux[0]), new PedidoModel(Int32.Parse(aux[0]), aux[1], Int32.Parse(aux[2]), aux[3], aux[4], long.Parse(aux[5]), aux[6]));
                        if(Int32.Parse(aux[7]) == -1){
                            lista[Int32.Parse(aux[0])].AsignarCadete(null);
                            lista[Int32.Parse(aux[0])].EntregarPedido();
                        }
                        else if (Int32.Parse(aux[7]) != 0)
                        {
                            lista[Int32.Parse(aux[0])].IniciarPedido(CadeteList[Int32.Parse(aux[7])]);
                            if (Int32.Parse(aux[8]) == 0)
                            {
                                lista[Int32.Parse(aux[0])].EntregarPedido();
                            }
                        }
                        IDPedidos = Int32.Parse(aux[0]) + 1;
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine("Ha ocurrido un error (ObtenerPedidos): " + ex.Message);
                    }
                }
            }

            return lista;
        }

        static public void ActualizarCadete(int id, string nom, string direc, long tel)
        {

            CadeteList[id] = new CadeteModel(id, nom, direc, tel);

            ActualizarCadetes();
        }

        static public void IngresarCadete(string nom, string direc, long tel)
        {
            try{
                if(CadeteList.Count > 0) IDCadetes = CadeteList.Keys.Max() + 1;
                else IDCadetes = 1;
                IngresarCadete(new CadeteModel(IDCadetes, nom, direc, tel));
            }
            catch(Exception ex){
                Console.WriteLine("Ha ocurrido un error (IngresarCadete): " + ex.Message);
            }
        }

        static private void ActualizarCadetes()
        {
            File.WriteAllText("CSV/cadetes.csv", "");

            foreach (var cadetes in CadeteList)
            {
                IngresarCadete(cadetes.Value);
            }

            CadeteVList = ObtenerViewCadetes();
        }

        static public void RestaurarDatos()
        {
            File.WriteAllText("CSV/cadetes.csv", File.ReadAllText("CSV/Restaurar/cadetes.csv"));
            File.WriteAllText("CSV/pedidos.csv", File.ReadAllText("CSV/Restaurar/pedidos.csv"));

            CadeteList = ObtenerCadetes();
            PedidoList = ObtenerPedidos();
            CadeteVList = ObtenerViewCadetes();
            PedidoVList = ObtenerViewPedidos();
        }

        static public bool BorrarCadete(int id)
        {
            try
            {
                foreach(var pedido in PedidoList)
                {
                    if (pedido.Value.Cadete is not null && pedido.Value.Cadete.id == id)
                    {
                        pedido.Value.BorrarCadete();
                        break;
                    }
                }

                CadeteList.Remove(id);
                
                ActualizarCadetes();
                ActualizarPedidos();

                return true;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (BorrarCadete): " + ex.Message);
                return false;
            }
        }

        static public void IngresarCadete(CadeteModel cadete)
        {
            try
            {
                string csv = $"{cadete.id};{cadete.nombre};{cadete.direccion};{cadete.telefono}\n";

                File.AppendAllText("CSV/cadetes.csv", csv);

                if(!CadeteList.ContainsKey(cadete.id)) CadeteList.Add(cadete.id, cadete);
                CadeteVList = ObtenerViewCadetes();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IngresarCadete): " + ex.Message);
            }
        }

        static public void IngresarPedido(int nro, string det, int idc, string nomc, string direc, long tel, string datos)
        {
            IngresarPedido(new PedidoModel(nro, det, idc, nomc, direc, tel, datos));
        }

        static public void IngresarPedido(string det, int idc, string nomc, string direc, long tel, string datos)
        {
            var lista = PedidoList.Keys.ToList<int>();
            int aux = 0;
            lista.Sort();
            IDPedidos = lista.Min();

            foreach (var num in lista)
            {
                aux = num;
                if (num > IDPedidos + 1)
                {
                    IDPedidos += 1;
                    break;
                }
                else
                {
                    IDPedidos = num;
                }
            }

            if (IDPedidos == aux) IDPedidos += 1;

            IngresarPedido(new PedidoModel(IDPedidos, det, idc, nomc, direc, tel, datos));
        }

        static private void ActualizarPedidos()
        {
            File.WriteAllText("CSV/pedidos.csv", "");

            foreach (var pedido in PedidoList.Values)
            {
                IngresarPedido(pedido);
            }

            PedidoVList = ObtenerViewPedidos();
        }

        static public void AsignarPedidoACadete(int idPedido, int idCadete)
        {
            PedidoList[idPedido].IniciarPedido(CadeteList[idCadete]);

            ActualizarPedidos();
        }

        static public void BorrarPedido(int id)
        {
            PedidoList.Remove(id);

            ActualizarPedidos();
        }

        static private string PedidoACSV(PedidoModel pedido)
        {
            int pedEnCurso = 0, pedEntregado = 0, cadID;

            if (pedido.EstaEnCurso()) pedEnCurso = 1;

            if (pedido.FueEntregado()) pedEntregado = 1;

            if (!pedido.EstaEnCurso() && !pedido.FueEntregado()) cadID = 0;
            if (pedido.Cadete is null) cadID = -1;
            else cadID = pedido.Cadete.id;

            return $"{pedido.Nro};{pedido.Detalles};{pedido.cliente.id};{pedido.cliente.nombre};{pedido.cliente.direccion};{pedido.cliente.telefono};{pedido.cliente.DatosRef};{cadID};{pedEnCurso};{pedEntregado}\n";
        }

        static public void IngresarPedido(PedidoModel pedido)
        {
            try
            {
                File.AppendAllText("CSV/pedidos.csv", PedidoACSV(pedido));
                if (!PedidoList.ContainsKey(pedido.Nro)) PedidoList[pedido.Nro] = pedido;
                PedidoVList = ObtenerViewPedidos();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Ha ocurrido un error (IngresarPedido(PedidoModel)): " + ex.Message);
            }
        }

        static public void EntregarPedido(int id)
        {
            PedidoList[id].EntregarPedido();

            ActualizarPedidos();
        }
    }
}
