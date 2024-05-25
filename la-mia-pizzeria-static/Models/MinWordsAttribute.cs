using System;
using System.ComponentModel.DataAnnotations;

namespace la_mia_pizzeria_static.Models
{
    public class MinWordsAttribute : ValidationAttribute
    {
        private readonly int _minWords;

        public MinWordsAttribute(int minWords)
        {
            _minWords = minWords;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value != null)
            {
                var description = value.ToString();
                var wordCount = description.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries).Length;
                if (wordCount < _minWords)
                {
                    return new ValidationResult($"La descrizione deve contenere almeno {_minWords} parole.");
                }
            }
            return ValidationResult.Success;
        }
    }
}
