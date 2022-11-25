using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Session;
using Cadeteria.ViewModels;
using Cadeteria.Repo;

namespace Cadeteria.Controllers
{
    public class LoggingController : Controller
    {
        private readonly IUsuarioRepository UsuarioRepo;

        public LoggingController(IUsuarioRepository UserRepo)
        {
            UsuarioRepo = UserRepo;
        }

        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Verificar(string usuario, string pass)
        {
            UsuarioViewModel user = UsuarioRepo.Logear(usuario, pass);

            if(user is not null)
            {
                HttpContext.Session.SetString("name", user.Nombre);

                if(user.Rol == RolUsuario.Administrador)
                {
                    HttpContext.Session.SetInt32("rol", 1);
                }
                if(user.Rol == RolUsuario.Encargado)
                {
                    /* Lo mismo que el administrador, salvo dar de alta pedidos y modificar pedidos */
                    HttpContext.Session.SetInt32("rol", 2);
                }

                return RedirectToAction("Index", "Cadete");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha encontrado el usuario ingresado" });
            }
        }

        public ActionResult Error(string error)
        {
            ViewData["error"] = error;
            return View();
        }
    }
}