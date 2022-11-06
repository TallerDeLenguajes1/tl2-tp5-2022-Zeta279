using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;

namespace Cadeteria.Controllers
{
    public class CadeteController : Controller
    {
        // GET: CadeteController
        public ActionResult Index()
        {
            return View(DataModel.CadeteVList);
        }

        public ActionResult Error(string error)
        {
            ViewData["error"] = error;
            return View();
        }

        // GET: CadeteController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: CadeteController/Create
        [HttpPost]
        public ActionResult Create(CrearCadeteViewModel cadete)
        {
            if (ModelState.IsValid)
            {
                DataModel.IngresarCadete(cadete.Nombre, cadete.Direccion, cadete.Tel.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido crear el cadete" });
            }
        }

        // GET: CadeteController/Edit/5
        public ActionResult Edit(int id)
        {
            if (DataModel.CadeteList.ContainsKey(id))
            {
                var cadete = DataModel.CadeteList[id];
                return View(new EditarCadeteViewModel(id, cadete.nombre, long.Parse(cadete.telefono), cadete.direccion));
            }
            return RedirectToAction("Error", new {error = "No se ha encontrado el cadete solicitado"});
        }

        // POST: CadeteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarCadeteViewModel cadete)
        {
            if (ModelState.IsValid)
            {
                DataModel.ActualizarCadete(cadete.ID, cadete.Nombre, cadete.Direccion, cadete.Tel.ToString());
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha editar el cadete" });
            }

        }

        // GET: CadeteController/Delete/5
        public ActionResult Delete(int id)
        {
            if (DataModel.BorrarCadete(id)) return RedirectToAction("Index");
            else return RedirectToAction("Error", new { error = "Ocurrió un error al intentar borrar el cadete" });
        }
    }
}
