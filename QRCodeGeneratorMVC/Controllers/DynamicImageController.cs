using Microsoft.AspNetCore.Mvc;
using QRCodeGeneratorMVC.Models;
using System.Drawing;
using System.Drawing.Imaging;
using System.Net;

namespace QRCodeGeneratorMVC.Controllers
{
    public class DynamicImageController : Controller
    {
        public IActionResult CreateDynamicImage()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CreateDynamicImage(DynamicImageModel dynamicImage)
        {
            // Background image
            string backgroundImageUrl = "https://userhub.com.bd/wp-content/uploads/2018/08/partner-bd-jobs.png";
            Image backgroundImage = LoadImageFromUrl(backgroundImageUrl);

            // Logo image
            string toplogoImageUrl = "https://thumbs.dreamstime.com/b/tiger-running-holding-box-both-hands-logo-mascot-delivery-vector-illustration-tiger-running-holding-box-both-hands-logo-199419145.jpg";
            Image topLogoImage = LoadImageFromUrl(toplogoImageUrl);
            
            // Logo image
            string logoImageUrl = "https://bdjobs.com/images/logo.png";
            Image logoImage = LoadImageFromUrl(logoImageUrl);
            
            // Logo image
            string qrImageUrl = "https://cdn2.hubspot.net/hubfs/477837/Imported_Blog_Media/QR_Code-1.jpg";
            Image qrImage = LoadImageFromUrl(qrImageUrl);

            // Text
            string text = dynamicImage.JobTitle; //"Software Engineer";
            Font font = new Font("Arial", 30, FontStyle.Bold);
            Brush textBrush = new SolidBrush(Color.Navy);

            int totalWidth = 1000;
            int totalHeight = 600;

            var qrImageHeight = 120;
            var qrImageWidth = 120;
            
            var leftTopImageHeight = 200;
            var leftTopImageWidth = 200;
            
            var leftBottomImageHeight = 100;
            var leftBottomImageWidth = 250;

            // Create a new image with the same dimensions as the background
            using (Bitmap resultImage = new Bitmap(totalWidth, totalHeight))
            {
                using (Graphics graphics = Graphics.FromImage(resultImage))
                {
                    graphics.Clear(Color.White);

                    var padding = 30;

                    // Draw the background image to cover the entire canvas
                    using (ImageAttributes attributes = new ImageAttributes())
                    {
                        // Set the opacity (0.5 for 50% transparency, adjust as needed)
                        attributes.SetColorMatrix(new ColorMatrix { Matrix33 = 0.5f });
                        graphics.DrawImage(backgroundImage, new Rectangle(0, 0, totalWidth, totalHeight),
                            0, 0, backgroundImage.Width, backgroundImage.Height, GraphicsUnit.Pixel, attributes);
                    }

                    // Draw the logo in the left top corner
                    graphics.DrawImage(topLogoImage, new Rectangle(0 + padding, 0 + padding, leftTopImageWidth, leftTopImageHeight));
                    
                    // Draw the image in the left bottom corner
                    graphics.DrawImage(logoImage, new Rectangle(0 + padding, totalHeight - leftBottomImageHeight - padding, leftBottomImageWidth, leftBottomImageHeight));
                    
                    // Draw the image in the right bottom corner
                    graphics.DrawImage(qrImage, new Rectangle(totalWidth - qrImageWidth - padding, totalHeight - qrImageHeight - padding, qrImageWidth, qrImageHeight));

                    // Draw the text in the right top corner
                    SizeF textSize = graphics.MeasureString(text, font);
                    float textX = resultImage.Width - textSize.Width - padding; 
                    float textY = 100; //Top Padding
                    graphics.DrawString(text, font, textBrush, new PointF(textX, textY));
                }

                // Save the result image to a file or stream
                //resultImage.Save("output.jpg", ImageFormat.Jpeg);

                string base64Image = ConvertImageToBase64(resultImage);
                ViewBag.dynamicImage = base64Image;                
            }
            return View();
        }

        static Image LoadImageFromUrl(string url)
        {
            using (WebClient webClient = new WebClient())
            {
                byte[] data = webClient.DownloadData(url);
                using (MemoryStream stream = new MemoryStream(data))
                {
                    return Image.FromStream(stream);
                }
            }
        }

        static string ConvertImageToBase64(Image image)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Save the image to the memory stream
                image.Save(ms, ImageFormat.Jpeg);

                // Convert the byte array to Base64
                byte[] imageBytes = ms.ToArray();

                return string.Format("data:image/png;base64,{0}", Convert.ToBase64String(imageBytes));
                //return Convert.ToBase64String(imageBytes);
            }
        }
    }
}
