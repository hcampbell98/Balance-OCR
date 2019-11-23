using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Tesseract;

namespace tessOCR
{
    class GrabBalance
    {
        public static string GetBalance(string url)
        {
            Console.WriteLine("OCR Processing Image");
            System.Net.WebRequest request = System.Net.WebRequest.Create(url);
            System.Net.WebResponse response = request.GetResponse();
            System.IO.Stream responseStream = response.GetResponseStream();

            Bitmap b = new Bitmap(responseStream);
            Bitmap grayscale = MakeGrayscale3(b);

            Rectangle r = new Rectangle(218, 60, 147, 30);

            var cropped = RemoveBlack(cropImage(grayscale, r));
            //cropped.Save("cropped.png");
            return GetText(cropped);
        }

        private static Bitmap RemoveBlack(Bitmap myBitmap)
        {
            const float limit = 0.1f;
            for (int i = 0; i < myBitmap.Width; i++)
            {
                for (int j = 0; j < myBitmap.Height; j++)
                {
                    Color c = myBitmap.GetPixel(i, j);
                    if (c.GetBrightness() > limit)
                    {
                        myBitmap.SetPixel(i, j, Color.White);
                    }
                }
            }
            return myBitmap;
        }

        private static string GetText(Bitmap imgsource)
        {
            var ocrtext = string.Empty;
            using (TesseractEngine engine = new TesseractEngine(@"./tessdata", "Bebas", EngineMode.Default))
            {
                using (var img = PixConverter.ToPix(imgsource))
                {
                    using (var page = engine.Process(img))
                    {
                        ocrtext = page.GetText();
                    }
                }
            }

            return ocrtext;
        }

        private static Bitmap cropImage(Image img, Rectangle cropArea)
        {
            Bitmap bmpImage = new Bitmap(img);
            return bmpImage.Clone(cropArea, bmpImage.PixelFormat);
        }

        private static Bitmap MakeGrayscale3(Bitmap original)
        {
            //create a blank bitmap the same size as original
            Bitmap newBitmap = new Bitmap(original.Width, original.Height);

            //get a graphics object from the new image
            using (Graphics g = Graphics.FromImage(newBitmap))
            {

                //create the grayscale ColorMatrix
                ColorMatrix colorMatrix = new ColorMatrix(
                   new float[][]
                   {
             new float[] {.3f, .3f, .3f, 0, 0},
             new float[] {.59f, .59f, .59f, 0, 0},
             new float[] {.11f, .11f, .11f, 0, 0},
             new float[] {0, 0, 0, 1, 0},
             new float[] {0, 0, 0, 0, 1}
                   });

                //create some image attributes
                using (ImageAttributes attributes = new ImageAttributes())
                {

                    //set the color matrix attribute
                    attributes.SetColorMatrix(colorMatrix);

                    //draw the original image on the new image
                    //using the grayscale color matrix
                    g.DrawImage(original, new Rectangle(0, 0, original.Width, original.Height),
                                0, 0, original.Width, original.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return newBitmap;
        }
    }
}
