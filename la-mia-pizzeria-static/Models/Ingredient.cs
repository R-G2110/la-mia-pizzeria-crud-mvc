using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Ingredient
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        [MaxLength(100, ErrorMessage = "Il campo Nome non può superare i 100 caratteri.")]
        public string Name { get; set; }

        public List<Pizza> Pizzas { get; set; }

        // Costruttore vuoto
        public Ingredient() { }
        public Ingredient(string nome)
        {
            this.Name = nome;
        }
    }
}