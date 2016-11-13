namespace File.Organiser.UI.Validation
{
    using System.Globalization;
    using System.Windows.Controls;

    public class MinimumCharacterValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            string data = (value ?? "").ToString();

            if (this.MinimumCharactersRequired >= data.Length)
            {
                return new ValidationResult(false, "A minimum of " + this.MinimumCharactersRequired + " characters are required");
            }

            return ValidationResult.ValidResult;
        }

        public int MinimumCharactersRequired { get; set; }
    }
}