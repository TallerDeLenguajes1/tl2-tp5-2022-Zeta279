using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Cadeteria.Repo;

namespace Cadeteria.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository PedidoRepo;
        private readonly ICadeteRepository CadeteRepo;
        private readonly IClienteRepository ClienteRepo;

        public PedidoController(IPedidoRepository pedidoRepo, ICadeteRepository cadeteRepo, IClienteRepository clienteRepo)
        {
            PedidoRepo = pedidoRepo;
            CadeteRepo = cadeteRepo;
            ClienteRepo = clienteRepo;
        }

        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            ViewData["cadetes"] = CadeteRepo.ObtenerTodo();
            ViewData["clientes"] = ClienteRepo.ObtenerTodo();

            return View(PedidoRepo.ObtenerTodo());
        }

        [HttpPost]
        public ActionResult Index(int idCadete, int idCliente)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            List<PedidoViewModel> pedidosFiltrados = new();
            List<PedidoViewModel> pedidosCliente;
            List<PedidoViewModel> pedidosCadete;
            ViewData["cadetes"] = CadeteRepo.ObtenerTodo();
            ViewData["clientes"] = ClienteRepo.ObtenerTodo();

            if (idCadete == 0 && idCliente == 0)
            {
                return View(PedidoRepo.ObtenerTodo());
            }
            else
            {
                if (idCadete != 0 && idCliente != 0)
                {
                    pedidosCliente = PedidoRepo.ObtenerPorCliente(idCliente);
                    pedidosCadete = PedidoRepo.ObtenerPorCadete(idCadete);

                    foreach (var pedido in pedidosCliente)
                    {
                        if (pedidosCadete.Contains(pedido)) pedidosFiltrados.Add(pedido);
                    }
                }
                else
                {
                    if (idCadete != 0)
                    {
                        pedidosFiltrados = PedidoRepo.ObtenerPorCadete(idCadete);
                    }
                    else
                    {
                        pedidosFiltrados = PedidoRepo.ObtenerPorCliente(idCliente);
                    }
                }
            }
            
            return View(pedidosFiltrados);
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            ViewData["clientes"] = ClienteRepo.ObtenerTodo();

            return View();
        }

        [HttpPost]
        public ActionResult Create(CrearPedidoViewModel pedido)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (ModelState.IsValid)
            {
                if (pedido.NuevoCliente)
                {
                    if (pedido.NombreCliente != null && pedido.DireccionCliente != null && pedido.TelefonoCliente != null)
                    {
                        PedidoRepo.CrearPedidoConCliente(pedido.Detalles, pedido.NombreCliente, pedido.DireccionCliente, pedido.TelefonoCliente);
                    }
                    else
                    {
                        return RedirectToAction("Error", new { error = "No se ha podido crear el cliente" });
                    }
                }
                else
                {
                    PedidoRepo.Crear(pedido.Detalles, pedido.IDCliente);
                }
                return RedirectToAction("Index");
                
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido crear el pedido" });
            }
        }

        public ActionResult Details(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            var pedido = PedidoRepo.Obtener(id);
            if (pedido is not null)
            {
                return View(pedido);
            }
            return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (PedidoRepo.Borrar(id))
            {
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Estado(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            var pedido = PedidoRepo.Obtener(id);
            ViewData["cadetes"] = CadeteRepo.ObtenerTodo();

            if (pedido is not null)
            {
                return View(pedido);
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Iniciar(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (PedidoRepo.AsignarCadete(idp, idc))
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            ViewData["error"] = error;

            return View();
        }
    }
}
