using la_mia_pizzeria_static.Data;
using la_mia_pizzeria_static.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;

namespace LaMiaPizzeria.Controllers
{
    [Authorize]
    public class PizzaController : Controller
    {
        private readonly ILogger<PizzaController> _logger;

        public PizzaController(ILogger<PizzaController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        [Authorize(Roles = "USER,ADMIN")]
        public IActionResult Index()
        {
            var pizzas = PizzaManager.GetAllPizzas();
            return View(pizzas);
        }

        [HttpGet]
        [Authorize(Roles = "USER,ADMIN")]
        public IActionResult GetPizza(int id)
        {
            try
            {
                var pizza = PizzaManager.GetPizza(id);
                if (pizza != null)
                    return View(pizza);
                else
                    return View("Errore", new ErrorViewModel($"La pizza {id} non è stata trovata!"));
            }
            catch (Exception e)
            {
                return View("Errore", new ErrorViewModel(e.Message));
            }
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult CreatePizza()
        {
            var categories = PizzaManager.GetAllCategories();
            var pizzaFormModel = new PizzaFormModel
            {
                Categories = categories,
                Pizza = new Pizza()
            };
            pizzaFormModel.CreateIngredients();
            return View("PizzaForm", pizzaFormModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult CreatePizza(PizzaFormModel pizzaFormModel)
        {
            if (ModelState.IsValid)
            {
                PizzaManager.InsertPizza(pizzaFormModel.Pizza, pizzaFormModel.SelectedIngredients);
                TempData["SuccessMessage"] = "Pizza creata con successo.";
                return RedirectToAction("Index");
            }
            pizzaFormModel.Categories = PizzaManager.GetAllCategories();
            pizzaFormModel.CreateIngredients();
            return View("PizzaForm", pizzaFormModel);
        }

        [HttpGet]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdatePizza(int id)
        {
            var pizza = PizzaManager.GetPizza(id);
            if (pizza == null)
                return NotFound();

            var categories = PizzaManager.GetAllCategories();
            var pizzaFormModel = new PizzaFormModel(pizza, categories);
            pizzaFormModel.CreateIngredients();

            return View("PizzaForm", pizzaFormModel);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult UpdatePizza(int id, PizzaFormModel pizzaFormModel)
        {
            if (ModelState.IsValid) // Verifica la validità del modello
            {
                // Il modello è valido, procedi con l'aggiornamento della pizza nel database
                bool updateResult = PizzaManager.UpdatePizza(id, pizzaFormModel.Pizza, pizzaFormModel.SelectedIngredients);
                if (updateResult)
                {
                    TempData["SuccessMessage"] = "Pizza modificata con successo.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Impossibile modificare la pizza.";
                    return NotFound();
                }
            }
            // Se il modello non è valido, ritorna la vista con gli errori di validazione
            pizzaFormModel.Categories = PizzaManager.GetAllCategories();
            pizzaFormModel.CreateIngredients();
            return View("PizzaForm", pizzaFormModel);
        }





        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "ADMIN")]
        public IActionResult DeletePizza(int id)
        {
            try
            {
                var deleted = PizzaManager.DeletePizza(id);
                if (deleted)
                {
                    TempData["SuccessMessage"] = "Pizza eliminata con successo.";
                    return RedirectToAction("Index");
                }
                else
                {
                    TempData["ErrorMessage"] = "Impossibile eliminare la pizza.";
                    return RedirectToAction("Index");
                }
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = "Si è verificato un errore durante l'eliminazione della pizza.";
                return RedirectToAction("Index");
            }
        }
    }
}
