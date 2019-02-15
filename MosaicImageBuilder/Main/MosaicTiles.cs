using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class MosaicTiles
    {
        public int ID { get; set; }
        public int red { get; set; }
        public int green { get; set; }
        public int blue { get; set; }
        public string path { get; set; }


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