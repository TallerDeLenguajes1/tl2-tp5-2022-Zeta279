using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;
using Microsoft.Data.Sqlite;
using Cadeteria.Repo;
using Microsoft.AspNetCore.Session;
using AutoMapper;

namespace Cadeteria.Controllers
{
    public class CadeteController : Controller
    {
        private readonly ICadeteRepository CadeteRepo;
        private readonly IMapper mapper;

        public CadeteController(ICadeteRepository cadeteRepo, IMapper map)
        {
            CadeteRepo = cadeteRepo;
            mapper = map;
        }

        // GET: CadeteController
        public ActionResult Index()
        {
            List<CadeteViewModel> cadetes = new List<CadeteViewModel>();

            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            foreach (var cadete in CadeteRepo.ObtenerTodo())
            {
                cadetes.Add(mapper.Map<CadeteViewModel>(cadete));
            }

            return View(cadetes);
        }

        public ActionResult Sesion()
        {
            return View();
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

        // GET: CadeteController/Create
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

        // POST: CadeteController/Create
        [HttpPost]
        public ActionResult Create(CrearCadeteViewModel cadete)
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
                CadeteRepo.Crear(mapper.Map<CadeteModel>(cadete));
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            var cadete = CadeteRepo.Obtener(id);

            if (cadete is not null)
            {
                return View(mapper.Map<EditarCadeteViewModel>(cadete));
            }
            else
            {
                return RedirectToAction("Error", new {error = "No se ha encontrado el cadete solicitado"});
            }
        }

        // POST: CadeteController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(EditarCadeteViewModel cadete)
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
                CadeteRepo.Actualizar(mapper.Map<CadeteModel>(cadete));
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
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("name")))
            {
                return RedirectToAction("Index", "Logging");
            }

            if (HttpContext.Session.GetInt32("rol") != 1)
            {
                return RedirectToAction("Index", "Home");
            }

            if (CadeteRepo.Borrar(id)) return RedirectToAction("Index");
            else return RedirectToAction("Error", new { error = "Ocurrió un error al intentar borrar el cadete" });
        }
    }
}
