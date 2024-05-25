using la_mia_pizzeria_static.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections.Generic;

namespace la_mia_pizzeria_static.Models
{
    public class PizzaFormModel
    {
        public List<Category>? Categories { get; set; }
        public Pizza Pizza { get; set; }
        public List<SelectListItem>? Ingredients { get; set; }
        public List<string>? SelectedIngredients { get; set; }

        public PizzaFormModel() { }

        public PizzaFormModel(Pizza pizza, List<Category>? categories)
        {
            Pizza = pizza;
            Categories = categories;
            SelectedIngredients = new List<string>();
            if (Pizza.Ingredients != null)
                foreach (var i in Pizza.Ingredients)
                    SelectedIngredients.Add(i.Id.ToString());
        }

        public void CreateIngredients()
        {
            this.Ingredients = new List<SelectListItem>();
            if (this.SelectedIngredients == null)
                this.SelectedIngredients = new List<string>();
            var ingredientsFromDB = PizzaManager.GetAllIngredients();
            foreach (var ingredient in ingredientsFromDB)
            {
                bool isSelected = this.SelectedIngredients.Contains(ingredient.Id.ToString());
                this.Ingredients.Add(new SelectListItem
                {
                    Text = ingredient.Name,
                    Value = ingredient.Id.ToString(),
                    Selected = isSelected
                });
            }
        }
    }
}
