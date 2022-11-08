using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;

namespace Cadeteria.Controllers
{
    public class ClienteController : Controller
    {
        // GET: ClienteController
        public ActionResult Index()
        {
            return View(DataModel.ClienteVList);
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
            if (ModelState.IsValid && DataModel.IngresarCliente(cliente.ID, cliente.Nombre, cliente.Direccion, cliente.Telefono))
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
            if (DataModel.CadeteList.ContainsKey(id))
            {
                var cliente = DataModel.ClienteList[id];
                return View(new ClienteViewModel(id, cliente.nombre, cliente.telefono, cliente.direccion));
            }
            return RedirectToAction("Error", new { error = "No se ha encontrado el cliente solicitado" });
        }

        // POST: ClienteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ClienteViewModel cliente)
        {
            if (ModelState.IsValid)
            {
                DataModel.ActualizarCliente(cliente.ID, cliente.Nombre, cliente.Direccion, cliente.Telefono);
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
            if (DataModel.BorrarCliente(id)) return RedirectToAction("Index");
            else return RedirectToAction("Error", new { error = "Ocurrió un error al intentar borrar el cliente" });
        }

        public ActionResult Error(string error)
        {
            ViewData["error"] = error;
            return View();
        }
    }
}
