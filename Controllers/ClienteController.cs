using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Cadeteria.Repo;

namespace Cadeteria.Controllers
{
    public class ClienteController : Controller
    {
        private readonly IClienteRepository ClienteRepo;

        public ClienteController(IClienteRepository clienteRepo)
        {
            ClienteRepo = clienteRepo;
        }

        // GET: ClienteController
        public ActionResult Index()
        {
            return View(ClienteRepo.ObtenerTodo());
        }

        // GET: ClienteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ClienteController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CrearClienteViewModel cliente)
        {
            if (ModelState.IsValid && ClienteRepo.Crear(cliente.Nombre, cliente.Direccion, cliente.Telefono))
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
            var cliente = ClienteRepo.Obtener(id);

            if (cliente is not null)
            {
                return View(cliente);
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
            if (ModelState.IsValid)
            {
                ClienteRepo.Actualizar(cliente.ID, cliente.Nombre, cliente.Direccion, cliente.Telefono);
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
            if (ClienteRepo.Borrar(id)) return RedirectToAction("Index");
            else return RedirectToAction("Error", new { error = "Ocurrió un error al intentar borrar el cliente" });
        }

        public ActionResult Error(string error)
        {
            ViewData["error"] = error;
            return View();
        }
    }
}
