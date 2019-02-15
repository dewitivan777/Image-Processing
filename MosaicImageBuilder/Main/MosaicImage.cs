using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class MosaicImage
    {
        public int ID { get; set; }
        public int X { get; set; }
        public int Y { get; set; }
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }



        public double[] CIELab
        {
            get
            {
                Conversion cv = new Conversion();
                return cv.RGBtoCIELAB(red, green, blue);
            }
        }
    }
}