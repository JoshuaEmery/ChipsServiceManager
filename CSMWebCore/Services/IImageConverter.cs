using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Drawing;

namespace CSMWebCore.Services
{
    public interface IImageConverter
    {
        byte[] imageToByteArray(Image image);
        Image byteArrayToImage(byte[] byteArray);

    }
}
