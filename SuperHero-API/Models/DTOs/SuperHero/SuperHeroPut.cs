using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SuperHero_API.Models.DTOs.SuperHero
{
    public class SuperHeroPut
    {
        private const string RequiredErrorTemplate = "Se debe indicar el {0} para el usuario";
        private const string StringMaxLengthErrorTemplate = "{0} debe poseer un máximo de {1} caracteres.";
        private const string RegularExpressionErrorTemplate = "{0} unicamente admite letras.";


        [Required(ErrorMessage = RequiredErrorTemplate)]
        [Display(Name = "Name")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = StringMaxLengthErrorTemplate)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = RegularExpressionErrorTemplate)]
        public string Name { get; set; } = String.Empty;

        [Required(ErrorMessage = RequiredErrorTemplate)]
        [Display(Name = "First Name")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = StringMaxLengthErrorTemplate)]
        [RegularExpression(@"^(([A-za-z]+[\s]{1}[A-za-z]+)|([A-Za-z]+))$", ErrorMessage = RegularExpressionErrorTemplate)]
        public string FirstName { get; set; } = String.Empty;

        [Required(ErrorMessage = RequiredErrorTemplate)]
        [Display(Name = "Last Name")]
        [StringLength(20, MinimumLength = 1, ErrorMessage = StringMaxLengthErrorTemplate)]
        public string LastName { get; set; } = String.Empty;

        [Required(ErrorMessage = RequiredErrorTemplate)]
        [Display(Name = "Place")]
        [StringLength(100, MinimumLength = 1, ErrorMessage = StringMaxLengthErrorTemplate)]
        public string Place { get; set; } = String.Empty;
    }
}
