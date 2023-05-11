using System;
using System.Drawing;
using System.IO;
using System.Windows.Media.Imaging;

namespace CarSharingApplication
{
    public static class ImageConvertor
    {

        public static Image Base64ToImage(string base64String)
        {
            // Convert base 64 string to byte[]
            byte[] imageBytes = Convert.FromBase64String(base64String);
            // Convert byte[] to Image
            using (var ms = new MemoryStream(imageBytes, 0, imageBytes.Length))
            {
                Image image = Image.FromStream(ms, true);
                return image;
            }
        }

        public static string ImageToBase64(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            using (MemoryStream ms = new MemoryStream())
            {
                // Convert Image to byte[]
                image.Save(ms, format);
                byte[] imageBytes = ms.ToArray();

                // Convert byte[] to base 64 string
                string base64String = Convert.ToBase64String(imageBytes);
                return base64String;
            }
        }

        public static BitmapImage Base64ToBitmapImage(string base64String)
        {
            var bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ImageConvertor.GetImageSource(base64String);
            bi.EndInit();
            return bi;
        }

        public static byte[] ImageToBytes(Image image, System.Drawing.Imaging.ImageFormat format)
        {
            byte[] imageBytes;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, format);
                imageBytes = ms.ToArray();
            }
            return imageBytes;
        }

        //public static string BitmapImageToBase64(BitmapImage image)
        //{
        //    byte[] bytes = new byte[1024];
        //    image.StreamSource.Read(bytes, 0, bytes.Length);
        //    return Convert.ToBase64String(bytes);
        //}

        private static MemoryStream GetImageSource(string base64String)
        {
            byte[] imageBytes = Convert.FromBase64String(base64String);
            MemoryStream ms = new MemoryStream(imageBytes, 0, imageBytes.Length);
            return ms;
        }
    }
}
