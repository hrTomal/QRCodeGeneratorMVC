using System.ComponentModel.DataAnnotations;

namespace QRCodeGeneratorMVC.Models
{
    public class DynamicImageModel
    {
        [Display(Name = "Enter Text")]
        public string JobTitle { get; set; }
    }
}
