using la_mia_pizzeria_static.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
using System.Collections.Generic;
using System.Linq;

namespace la_mia_pizzeria_static.Data
{
    public enum ResultType
    {
        OK,
        Exception,
        NotFound
    }

    public static class PizzaManager
    {
        public static int CountAllPizzas()
        {
            using PizzaDbContext db = new PizzaDbContext();
            return db.Pizzas.Count();
        }

        public static List<Pizza> GetAllPizzas()
        {
            using PizzaDbContext db = new PizzaDbContext();
            return db.Pizzas.ToList();
        }

        public static Pizza GetPizza(int id, bool includeReferences = true)
        {
            using PizzaDbContext db = new PizzaDbContext();
            if (includeReferences)
                return db.Pizzas.Where(x => x.Id == id).Include(p => p.Category).Include(p => p.Ingredients).FirstOrDefault();
            return db.Pizzas.FirstOrDefault(p => p.Id == id);
        }

        public static Pizza GetPizzaByName(string name)
        {
            using PizzaDbContext db = new PizzaDbContext();
            return db.Pizzas.FirstOrDefault(p => p.Name == name);
        }

        public static List<Pizza> GetPizzasByName(string name)
        {
            using PizzaDbContext db = new PizzaDbContext();

            return db.Pizzas.Where(p => p.Name == name).ToList();
        }

        public static List<Category> GetAllCategories()
        {
            using PizzaDbContext db = new PizzaDbContext();
            return db.Categories.ToList();
        }
        public static List<Ingredient> GetAllIngredients()
        {
            using PizzaDbContext db = new PizzaDbContext();
            return db.Ingredients.ToList();
        }


        public static void InsertPizza(Pizza pizza, List<string> selectedIngredients)
        {
            using PizzaDbContext db = new PizzaDbContext();
            pizza.Ingredients = new List<Ingredient>();
            if (selectedIngredients != null)
            {
                // Trasformiamo gli ID scelti in ingredienti da aggiungere tra i riferimenti in Pizza
                foreach (var ingredient in selectedIngredients)
                {
                    int id = int.Parse(ingredient);
                    // NON usiamo un GetIngredientById() perché userebbe un db context diverso
                    // e ciò causerebbe errore in fase di salvataggio - usiamo lo stesso context all'interno della stessa operazione
                    var ingredientFromDb = db.Ingredients.FirstOrDefault(x => x.Id == id);
                    if (ingredientFromDb != null)
                    {
                        pizza.Ingredients.Add(ingredientFromDb);
                    }
                }
            }
            db.Pizzas.Add(pizza);
            db.SaveChanges();
        }

        public static bool UpdatePizza(int id, Pizza updatedPizza, List<string> selectedIngredients)
        {
            try
            {
                using (var db = new PizzaDbContext())
                {
                    var existingPizza = GetPizza(id);
                    if (existingPizza == null)
                    {
                        Console.WriteLine($"Pizza with ID {id} not found.");
                        return false;
                    }

                    Console.WriteLine($"Updating pizza with ID {id}");

                    // Aggiorna i campi della pizza esistente con quelli della pizza aggiornata
                    existingPizza.Name = updatedPizza.Name;
                    existingPizza.Price = updatedPizza.Price;
                    existingPizza.ImagePath = updatedPizza.ImagePath;
                    existingPizza.Description = updatedPizza.Description;
                    existingPizza.CategoryId = updatedPizza.CategoryId;

                    // Aggiorna gli ingredienti
                    existingPizza.Ingredients.Clear();
                    if (selectedIngredients != null && selectedIngredients.Count > 0)
                    {
                        foreach (var ingredientId in selectedIngredients)
                        {
                            var ingredient = GetIngredient(int.Parse(ingredientId));
                            if (ingredient != null)
                            {
                                existingPizza.Ingredients.Add(ingredient);
                            }
                        }
                    }

                    // Salva le modifiche nel database
                    db.SaveChanges();

                    Console.WriteLine($"Pizza with ID {id} updated successfully.");

                    return true;
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while updating pizza with ID {id}: {ex.Message}");
                // Gestisci l'eccezione se si verifica un errore durante il salvataggio
                return false;
            }
        }




        public static ResultType UpdatePizzaWithEnum(int id, Pizza pizza) // solo a scopo didattico
        {
            try
            {
                using PizzaDbContext db = new PizzaDbContext();
                var pizzaDaModificare = db.Pizzas.FirstOrDefault(p => p.Id == id);
                if (pizzaDaModificare == null)
                    return ResultType.NotFound;
                pizzaDaModificare.Name = pizza.Name;
                pizzaDaModificare.Description = pizza.Description;
                pizzaDaModificare.Price = pizza.Price;

                db.SaveChanges();
                return ResultType.OK;
            }
            catch (Exception ex)
            {
                return ResultType.Exception;
            }
        }

        public static bool DeletePizza(int id)
        {
            try
            {
                //using PizzaDbContext db = new PizzaDbContext();
                var pizzaDaCancellare = GetPizza(id, false); // db.Pizzas.FirstOrDefault(p => p.Id == id);
                if (pizzaDaCancellare == null)
                    return false;

                using PizzaDbContext db = new PizzaDbContext();
                db.Remove(pizzaDaCancellare);
                db.SaveChanges();
                return true;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public static Ingredient GetIngredient(int id)
        {
            using PizzaDbContext db = new PizzaDbContext();
            return db.Ingredients.FirstOrDefault(x => x.Id == id);
        }

        public static bool SaveChanges()
        {
            try
            {
                using PizzaDbContext db = new PizzaDbContext();
                db.SaveChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        public static void SeedPizza()
        {
            using (PizzaDbContext db = new PizzaDbContext())
            {
                if (db.Pizzas.Count() == 0)
                {
                    db.Pizzas.AddRange(
                        new Pizza("Margherita", "Una pizza classica con mozzarella, pomodoro, basilico", "~/img/margherita.jpg", 5.99m),
                        new Pizza("Marinara", "Semplice e deliziosa, con pomodoro, aglio, origano e olio extravergine di oliva.", "~/img/marinara.jpg", 4.99m),
                        new Pizza("Diavola", "Una pizza piccante con salame piccante, peperoncino e mozzarella.", "~/img/diavola.jpg", 6.49m),
                        new Pizza("Mortadella, Stracchino e Pesto", "Una combinazione di mortadella, stracchino e pesto", "~/img/mortadella&stracchino.jpg", 7.99m),
                        new Pizza("Capricciosa", "Una pizza classica con prosciutto cotto, funghi, carciofi, olive e mozzarella.", "~/img/capricciosa.jpg", 8.99m),
                        new Pizza("Salsiccia e Friarielli", "Una pizza tipica della tradizione napoletana, con salsiccia e friarielli, condita con mozzarella di bufala.", "~/img/salsiccia&friarielli.jpg", 8.99m)
                    );
                    db.SaveChanges();
                }
            }
        }
        public static void SeedCategory()
        {
            using (PizzaDbContext db = new PizzaDbContext())
            {
                if (db.Categories.Count() == 0)
                {
                    db.Categories.AddRange(
                        new Category("Pizze classiche"),
                        new Category("Pizze bianche"),
                        new Category("Pizze vegetariane"),
                        new Category("Pizze di mare"),
                        new Category("Pizze speciali")
                    );
                    db.SaveChanges();
                }
            }
        }
        public static void SeedIngredient()
        {
            using (PizzaDbContext db = new PizzaDbContext())
            {
                if (db.Ingredients.Count() == 0)
                {
                    db.Ingredients.AddRange(
                        new Ingredient("Farina 00"),
                        new Ingredient("Acqua"),
                        new Ingredient("Sale"),
                        new Ingredient("Lievito madre"),
                        new Ingredient("Pomodoro"),
                        new Ingredient("Mozzarella"),
                        new Ingredient("Olio EVO"),
                        new Ingredient("Basilico"),
                        new Ingredient("Aglio"),
                        new Ingredient("Origano"),
                        new Ingredient("Salame piccante"),
                        new Ingredient("Peperoncino"),
                        new Ingredient("Mortadella"),
                        new Ingredient("Stracchino"),
                        new Ingredient("Pesto"),
                        new Ingredient("Carciofi"),
                        new Ingredient("Prosciutto cotto"),
                        new Ingredient("Funghi"),
                        new Ingredient("Olive"),
                        new Ingredient("Salsiccia"),
                        new Ingredient("Friarielli"),
                        new Ingredient("Mozzarella di bufala")
                    );
                    db.SaveChanges();
                }
            }
        }
    }

    

        
        
}