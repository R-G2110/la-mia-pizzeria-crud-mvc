namespace la_mia_pizzeria_static.Models
{
    public class PizzaIngredient
    {
        public int Id { get; set; }

        // Foreign key for Pizza
        public int PizzaId { get; set; }
        public Pizza Pizza { get; set; }

        // Foreign key for Ingredient
        public int IngredientId { get; set; }
        public Ingredient Ingredient { get; set; }
    }
}
