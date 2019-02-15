using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class Conversion
    {
        //Convert RGB values to CIELAB values
        public double[] RGBtoCIELAB(int red, int green, int blue)
        {
            double[] LAB = new double[3];

            //Convert RBG values to 0..1
            double rfloat = Convert.ToDouble(red) / 0xff;
            double gfloat = Convert.ToDouble(green) / 0xff;
            double bfloat = Convert.ToDouble(blue) / 0xff;

            //sRBGtoXYZ
            rfloat = (rfloat <= 0.04045) ? (rfloat / 12.92) : Math.Pow((rfloat + 0.055) / 1.055, 2.4);
            gfloat = (gfloat <= 0.04045) ? (gfloat / 12.92) : Math.Pow((gfloat + 0.055) / 1.055, 2.4);
            bfloat = (bfloat <= 0.04045) ? (bfloat / 12.92) : Math.Pow((bfloat + 0.055) / 1.055, 2.4);
            rfloat *= 100.0;
            gfloat *= 100.0;
            bfloat *= 100.0;


            double x = (((rfloat * 0.4124) + (gfloat * 0.3576)) + (bfloat * 0.1805));
            double y = (((rfloat * 0.2126) + (gfloat * 0.7152)) + (bfloat * 0.0722));
            double z = (((rfloat * 0.0193) + (gfloat * 0.1192)) + (bfloat * 0.9505));

            //XYZtoCIELAB
            x = x / 95.047;
            y = y / 100.0;
            z = z / 108.883;
            x = (x <= 0.008856) ? ((7.787 * x) + 0.0) : Math.Pow(x, 0.33333333333333331);
            y = (y <= 0.008856) ? ((7.787 * y) + 0.0) : Math.Pow(y, 0.33333333333333331);
            z = (z <= 0.008856) ? ((7.787 * z) + 0.0) : Math.Pow(z, 0.33333333333333331);

            LAB[0] = (116 * y) - 16;
            LAB[1] = 500.0 * (x - y);
            LAB[2] = 200.0 * (y - z);


            return LAB;
        }
    }
}