using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Main
{
    class ProcessTiles
    {

        //Processes each tile image in directory and stores values in entity object list.
        public ProcessTiles(List<MosaicTiles> mosaic, string file)
        {

            Bitmap Bmp = new Bitmap(file);


            //32bit color
            BitmapData srcData = Bmp.LockBits(
            new Rectangle(0, 0, Bmp.Width, Bmp.Height),
            ImageLockMode.ReadOnly,
            PixelFormat.Format32bppArgb);


            int stride = srcData.Stride;

            IntPtr Scan0 = srcData.Scan0;

            int[] totals = new int[] { 0, 0, 0 };

            int width = Bmp.Width;
            int height = Bmp.Height;

            //Calculates average RGB values
            unsafe
            {
                byte* p = (byte*)(void*)Scan0;

                for (int y = 0; y < height; y++)
                {
                    for (int x = 0; x < width; x++)
                    {
                        for (int color = 0; color < 3; color++)
                        {
                            int idx = (y * stride) + x * 4 + color;

                            totals[color] += p[idx];
                        }
                    }
                }
            }
            int avgR = totals[2] / (width * height);
            int avgG = totals[1] / (width * height);
            int avgB = totals[0] / (width * height);

            int count = mosaic.Count;

            mosaic.Add(new MosaicTiles()
            {
                ID = count+1,
                red = avgR,
                green = avgG,
                blue = avgB,
                path = file
            });


        }
    }
}
