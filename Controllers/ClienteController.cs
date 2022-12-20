using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Cadeteria.Repo;
using AutoMapper;

namespace Cadeteria.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepository ClienteRepo;
        private readonly IMapper mapper;

        public ClienteController(IClienteRepository clienteRepo, IMapper map)
        {
            ClienteRepo = clienteRepo;
            mapper = map;
        }

        // GET: ClienteController
        public ActionResult Index()
        {
            var clientes = new List<ClienteViewModel>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var cliente in ClienteRepo.ObtenerTodo())
            {
                clientes.Add(mapper.Map<ClienteViewModel>(cliente));
            }

            return View(clientes);
        }

        // GET: ClienteController/Create
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

            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearClienteViewModel cliente)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            if (ModelState.IsValid && ClienteRepo.Crear(mapper.Map<ClienteModel>(cliente)))
            {
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido crear el cliente" });
            }
        }

        // GET: ClienteController/Edit/5
        public ActionResult Edit(int id)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            var cliente = ClienteRepo.Obtener(id);

            if (cliente is not null)
            {
                return View(mapper.Map<EditarClienteViewModel>(cliente));
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha encontrado el cliente solicitado" });
            }
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClienteViewModel cliente)
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
                ClienteRepo.Actualizar(mapper.Map<ClienteModel>(cliente));
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido editar el cliente" });
            }
        }

        // GET: ClienteController/Delete/5
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

            if (ClienteRepo.Borrar(id)) return RedirectToAction("Index");
            else return RedirectToAction("Error", new { error = "Ocurrió un error al intentar borrar el cliente" });
        }

        public ActionResult Error(string error)
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewData["error"] = error;
            return View();
        }
    }
}
