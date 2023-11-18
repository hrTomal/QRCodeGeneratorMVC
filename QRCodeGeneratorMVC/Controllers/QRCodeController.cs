using Microsoft.AspNetCore.Mvc;
using QRCodeGeneratorMVC.Models;
using QRCoder;
using System.Drawing;
using System.Drawing.Imaging;

namespace QRCodeGeneratorMVC.Controllers
{
    public class QRCodeController : Controller
    {
        public IActionResult CreateQRCode()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateQRCode(QRCodeModel qRCode)
        {
            QRCodeGenerator QrGenerator = new QRCodeGenerator();
            QRCodeData QrCodeInfo = QrGenerator.CreateQrCode(qRCode.QRCodeText, QRCodeGenerator.ECCLevel.Q);
            QRCode qrCode = new QRCode(QrCodeInfo);
            Bitmap QrBitmap = qrCode.GetGraphic(60);

            byte[] BitmapArray;
            using (MemoryStream ms = new MemoryStream())
            {
                QrBitmap.Save(ms, ImageFormat.Png);
                BitmapArray =  ms.ToArray();
            }

            string QrUri = string.Format("data:image/png;base64,{0}", Convert.ToBase64String(BitmapArray));
            ViewBag.QrCodeUri = QrUri;
            return View();
        }

        
    }
}
