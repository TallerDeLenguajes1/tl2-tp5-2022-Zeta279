using Microsoft.AspNetCore.Mvc;
using Cadeteria.Models;
using Cadeteria.ViewModels;

namespace Cadeteria.Controllers
{
    public class PedidoController : Controller
    {
        public ActionResult Index()
        {
            return View(DataModel.PedidoVList);
        }

        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Create(CrearPedidoViewModel pedido)
        {
            if (ModelState.IsValid)
            {
                DataModel.IngresarPedido(pedido.Detalles, pedido.IDCliente);
                return RedirectToAction("Index");
            }
            else
            {
                return RedirectToAction("Error", new { error = "No se ha podido crear el pedido" });
            }
        }

        public ActionResult Details(int id)
        {
            if (DataModel.PedidoList.ContainsKey(id))
            {
                foreach(var pedido in DataModel.PedidoVList)
                {
                    if (pedido.NumPedido == id) return View(pedido);
                }
            }
            return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Delete(int id)
        {
            if (DataModel.PedidoList.ContainsKey(id))
            {
                DataModel.BorrarPedido(id);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Estado(int id)
        {
            if (DataModel.PedidoList.ContainsKey(id))
            {
                if (DataModel.PedidoList[id].Estado == estado.Pendiente) DataModel.IniciarPedido(id);
                DataModel.EntregarPedido(id);
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Asignar(int id)
        {
            if (DataModel.PedidoList.ContainsKey(id)) return View(DataModel.PedidoList[id]);
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido solicitado" });
        }

        public ActionResult Iniciar(int id)
        {
            if (DataModel.PedidoList.ContainsKey(id))
            {
                if (!DataModel.IniciarPedido(id)) return RedirectToAction("Error", new { error = "No se ha podido iniciar el pedido" });
                return RedirectToAction("Index");
            }
            else return RedirectToAction("Error", new { error = "No se ha encontrado el pedido" });
        }

        [HttpPost]
        public ActionResult Asignar(int idp, int idc)
        {
            DataModel.AsignarPedidoACadete(idp, idc);
            return RedirectToAction("Index");
        }

        public ActionResult Error(string error)
        {
            ViewData["error"] = error;

            return View();
        }
    }
}
