namespace Zagorapps.Utilities.Suite.UI.Validation
{
    using System.Globalization;
    using System.Windows.Controls;

    public class MinimumCharactersValidationRule : ValidationRule
    {
        public int MinimumCharactersRequired { get; set; }

        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = (value ?? string.Empty).ToString();

            if (this.MinimumCharactersRequired >= data.Length)
            {
                return new ValidationResult(false, "A minimum of " + this.MinimumCharactersRequired + " characters are required");
            }

            return ValidationResult.ValidResult;
        }
    }
}