using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Cadeteria.Repo;
using AutoMapper;

namespace Cadeteria.Controllers
{
    public class PedidoController : Controller
    {
        private readonly IPedidoRepository PedidoRepo;
        private readonly ICadeteRepository CadeteRepo;
        private readonly IClienteRepository ClienteRepo;
        private readonly IMapper mapper;

        public PedidoController(IPedidoRepository pedidoRepo, ICadeteRepository cadeteRepo, IClienteRepository clienteRepo, IMapper map)
        {
            PedidoRepo = pedidoRepo;
            CadeteRepo = cadeteRepo;
            ClienteRepo = clienteRepo;
            mapper = map;
        }

        public ActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            // Utilizados para filtrar
            ViewData["clientes"] = mapper.Map<List<ClienteViewModel>>(ClienteRepo.ObtenerTodo());
            ViewData["cadetes"] = mapper.Map<List<CadeteViewModel>>(CadeteRepo.ObtenerTodo());

            if(HttpContext.Session.GetInt32("rol") == 2)
            {
                return View(mapper.Map<List<PedidoViewModel>>(PedidoRepo.ObtenerPorCadete((int)HttpContext.Session.GetInt32("id_cadete"))));
            }
            else
            {
                return View(mapper.Map<List<PedidoViewModel>>(PedidoRepo.ObtenerTodo()));
            }
        }

        [HttpPost]
        public ActionResult Index(int idCadete, int idCliente)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["clientes"] = mapper.Map<List<ClienteViewModel>>(ClienteRepo.ObtenerTodo());
            ViewData["cadetes"] = mapper.Map<List<CadeteViewModel>>(CadeteRepo.ObtenerTodo());

            List<PedidoModel> pedidosFiltrados = new();
            List<PedidoModel> pedidosCliente;
            List<PedidoModel> pedidosCadete;

            if (idCadete == 0 && idCliente == 0)
            {
                return RedirectToAction("Index");
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
            
            return View(mapper.Map<List<PedidoViewModel>>(pedidosFiltrados));
        }

        [HttpGet]
        public ActionResult Create()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["clientes"] = mapper.Map<List<ClienteViewModel>>(ClienteRepo.ObtenerTodo());

            return View();
        }

        [HttpPost]
        public ActionResult Create(CrearPedidoViewModel pedido)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid)
            {
                if (pedido.NuevoCliente)
                {
                    if (pedido.ClienteNombre != null && pedido.ClienteDireccion != null && pedido.ClienteTelefono != null)
                    {
                        PedidoRepo.CrearPedidoConCliente(mapper.Map<PedidoModel>(pedido));
                    }
                    else
                    {
                        return RedirectToAction("Error", new { error = "No se ha podido crear el cliente" });
                    }
                }
                else
                {
                    PedidoRepo.Crear(mapper.Map<PedidoModel>(pedido));
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
                return View(mapper.Map<PedidoViewModel>(pedido));
            }

            return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Delete(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
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

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            var pedido = PedidoRepo.Obtener(id);
            ViewData["cadetes"] = mapper.Map<List<CadeteViewModel>>(CadeteRepo.ObtenerTodo());

            if (pedido is not null)
            {
                return View(mapper.Map<PedidoViewModel>(pedido));
            }

            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        [HttpPost]
        public ActionResult Asignar(int idp, int idc)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
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
