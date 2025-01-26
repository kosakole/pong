using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Game
{
    public static class Images
    {
        public readonly static ImageSource Empty = LoadImage("Empty.png");
        public readonly static ImageSource Border = LoadImage("Border.png");
        public readonly static ImageSource Ball = LoadImage("Ball.png");
        public readonly static ImageSource Target = LoadImage("Target.png");

        private static ImageSource LoadImage(string fileName)
        {
            return new BitmapImage(new Uri($"Asserts/{fileName}", UriKind.Relative));
        }
    }

    
}
