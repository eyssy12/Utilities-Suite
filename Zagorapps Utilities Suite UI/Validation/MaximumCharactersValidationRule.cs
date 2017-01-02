namespace Zagorapps.Utilities.Suite.UI.Validation
{
    using System.Globalization;
    using System.Windows.Controls;

    public class MaximumCharactersValidationRule : ValidationRule
    {
        public int MaximumCharactersAllowed { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = (value ?? string.Empty).ToString();

            if (this.MaximumCharactersAllowed < data.Length)
            {
                return new ValidationResult(false, "A maximum of " + this.MaximumCharactersAllowed + " characters are allowed.");
            }

            return ValidationResult.ValidResult;
        }
    }
}