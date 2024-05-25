using Microsoft.Extensions.Hosting;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Category
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        public string Name { get; set; }
        public List<Pizza> Pizzas { get; set; }

        // Costruttore vuoto
        public Category() { }
        public Category(string nome)
        {
            this.Name = nome;
        }
    }
}
