using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace CSMWebCore.Services
{
    public class ImageConverter : IImageConverter
    {
        public Image byteArrayToImage(byte[] byteArray)
        {
            MemoryStream ms = new MemoryStream(byteArray);
            Image returnImage = Image.FromStream(ms);
            return returnImage;
        }

        public byte[] imageToByteArray(Image image)
        {
            MemoryStream ms = new MemoryStream();
            image.Save(ms, System.Drawing.Imaging.ImageFormat.Gif);
            return ms.ToArray();
        }
    }
}
