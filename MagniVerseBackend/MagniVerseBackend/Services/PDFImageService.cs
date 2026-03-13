using UglyToad.PdfPig;
using System.Drawing;

namespace MagniVerseBackend.Services
{
    public class PDFImageService
    {
        public List<string> ExtractImages(string pdfPath)
        {
            var savedImages = new List<string>();

            var outputFolder = Path.Combine(
                Directory.GetCurrentDirectory(),
                "ManualImages"
            );

            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            using (var document = PdfDocument.Open(pdfPath))
            {
                int imageIndex = 1;

                foreach (var page in document.GetPages())
                {
                    foreach (var image in page.GetImages())
                    {
                        var imageBytes = image.RawBytes.ToArray();

                        var imageName = $"manual_image_{imageIndex}.png";
                        var imagePath = Path.Combine(outputFolder, imageName);

                        File.WriteAllBytes(imagePath, imageBytes);

                        savedImages.Add(imageName);

                        imageIndex++;
                    }
                }
            }

            return savedImages;
        }
    }
}