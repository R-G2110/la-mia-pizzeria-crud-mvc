using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class Pizza
    {
        [Key] public int Id { get; set; }

        [Required(ErrorMessage = "Il campo Nome è obbligatorio.")]
        [MinLength(3, ErrorMessage = "Il campo Nome deve avere almeno 3 caratteri.")]
        [MaxLength(50, ErrorMessage = "Il campo Nome non può superare i 50 caratteri.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "Il campo Descrizione è obbligatorio.")]
        [MaxLength(250, ErrorMessage = "Il campo Descrizione non può superare i 250 caratteri.")]
        [MinWords(5)]
        public string Description { get; set; }

        [Required(ErrorMessage = "Il campo Prezzo è obbligatorio.")]
        [Range(0.01, double.MaxValue, ErrorMessage = "Il campo Prezzo deve essere maggiore di zero")]
        public decimal Price { get; set; }

        public string? ImagePath { get; set; }

        public int? CategoryId { get; set; }
        public Category? Category { get; set; }

        public List<Ingredient>? Ingredients { get; set; }

        public Pizza() { }
        public Pizza(string nome, string descrizione, string foto, decimal prezzo) : this()
        {
            Name = nome;
            Description = descrizione;
            ImagePath = foto;
            Price = prezzo;
        }
    }
}