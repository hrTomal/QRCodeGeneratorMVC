using System.ComponentModel.DataAnnotations;

namespace QRCodeGeneratorMVC.Models
{
    public class QRCodeModel
    {
        [Display(Name = "Enter QRCode Text")]
        public string QRCodeText { get; set; }
    }
}
