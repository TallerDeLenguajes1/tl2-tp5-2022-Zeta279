using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Cadeteria.DataModels;
using Cadeteria.Repo;

namespace Cadeteria.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository PedidoRepo;

        public PedidoController(IPedidoRepository pedidoRepo)
        {
            PedidoRepo = pedidoRepo;

            var Cadetes = new CadeteRepository();
            var Clientes = new ClienteRepository();

            ViewData["cadetes"] = Cadetes.ObtenerTodo();
            ViewData["clientes"] = Clientes.ObtenerTodo();
        }

        public ActionResult Index()
        {
            return View(PedidoRepo.ObtenerTodo());
        }

        [HttpPost]
        public ActionResult Index(int idCadete, int idCliente)
        {
            List<PedidoViewModel> pedidosCadete;
            List<PedidoViewModel> pedidosCliente;

            if (idCadete != 0 && idCliente != 0)
            {
                pedidosCadete = PedidoDataModel.ObtenerPedidosDeCadete(idCadete);
                pedidosCliente = PedidoDataModel.ObtenerPedidosDeCliente(idCliente);
                List<PedidoViewModel> aux = new();

                foreach(var pedido in pedidosCadete)
                {
                    if (pedidosCliente.Contains(pedido)) aux.Add(pedido);
                }

                return View(aux);
            }
            else if(idCadete != 0)
            {
                return View(PedidoDataModel.ObtenerPedidosDeCadete(idCadete));
            }
            else if(idCliente != 0)
            {
                return View(PedidoDataModel.ObtenerPedidosDeCliente(idCliente));
            }
            return RedirectToAction("Index");
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CrearPedidoViewModel pedido)
        {
            var clienteRepo = new ClienteRepository();

            if (ModelState.IsValid)
            {
                if (pedido.NuevoCliente)
                {
                    if(pedido.NombreCliente != null && pedido.DireccionCliente != null && pedido.TelefonoCliente != null)
                    {
                        clienteRepo.Crear(pedido.NombreCliente, pedido.DireccionCliente, pedido.TelefonoCliente);
                    }
                    else
                    {
                        return RedirectToAction("Error", new { error = "No se ha podido crear el cliente" });
                    }
                }
                PedidoRepo.Crear(pedido.Detalles, pedido.IDCliente);
                return RedirectToAction("Index");
                
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido crear el pedido" });
            }
        }

        public ActionResult Details(int id)
        {
            var pedido = PedidoRepo.Obtener(id);
            if (pedido is not null)
            {
                return View(pedido);
            }
            return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Delete(int id)
        {
            if (PedidoRepo.Borrar(id))
            {
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Estado(int id)
        {
            var pedido = PedidoRepo.Obtener(id);

            if (pedido is not null && pedido.Estado == estado.EnCurso)
            {
                PedidoRepo.CambiarEstado(id, estado.Entregado);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Asignar(int id)
        {
            var pedido = PedidoRepo.Obtener(id);

            if (pedido is not null)
            {
                return View(pedido);
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Iniciar(int id)
        {
            var pedido = PedidoRepo.Obtener(id);

            if (pedido is not null && pedido.Estado == estado.Pendiente)
            {
                PedidoRepo.CambiarEstado(id, estado.EnCurso);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido iniciar el pedido" });
            }
        }

        [HttpPost]
        public ActionResult Asignar(int idp, int idc)
        {
            if(PedidoRepo.AsignarCadete(idp, idc))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido asignar el pedido" });
            }

        }

        public ActionResult Error(string error)
        {
            ViewData["error"] = error;

            return View();
        }
    }
}
