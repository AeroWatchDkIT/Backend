namespace PalletSyncApi.Classes
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Text.RegularExpressions;

    [AttributeUsage(AttributeTargets.Property | AttributeTargets.Field | AttributeTargets.Parameter, AllowMultiple = false)]
    public class RegexAttribute : ValidationAttribute
    {
        private readonly string pattern;

        public RegexAttribute(string pattern)
        {
            this.pattern = pattern ?? throw new ArgumentNullException(nameof(pattern));
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                // You might want to handle null values based on your requirements
                return ValidationResult.Success;
            }

            string valueAsString = value.ToString();

            if (!Regex.IsMatch(valueAsString, pattern))
            {
                return new ValidationResult(ErrorMessage ?? $"The field {validationContext.DisplayName} is not in the correct format.");
            }

            return ValidationResult.Success;
        }
    }
}

//Got this straight from chatgpt 
