
﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    class ProcessImage
    {

        //Divides Image into blocks. Values stored in entity object list
        public List<MosaicImage> ProcessImages(string file)
        {
            Image image = Image.FromFile(file);
            Bitmap bitmap = this.resizeImage(image);
            List<MosaicImage> ImageBlocks = new List<MosaicImage>();

            int widthThird = (int)((double)bitmap.Width / 20);
            int heightThird = (int)((double)bitmap.Height / 20);
            Bitmap[,] bmps = new Bitmap[20, 20];
            for (int i = 0; i < 20; i++)
                for (int j = 0; j < 20; j++)
                {
                    bmps[i, j] = new Bitmap(widthThird, heightThird);
                    Graphics g = Graphics.FromImage(bmps[i, j]);
                    g.DrawImage(bitmap, new Rectangle(0, 0, widthThird, heightThird), new Rectangle(j * widthThird, i * heightThird, widthThird, heightThird), GraphicsUnit.Pixel);
                    g.Dispose();


                    BitmapData srcData = bmps[i, j].LockBits(
           new Rectangle(0, 0, bmps[i, j].Width, bmps[i, j].Height),
           ImageLockMode.ReadOnly,
           PixelFormat.Format32bppArgb);


                    int stride = srcData.Stride;

                    IntPtr Scan0 = srcData.Scan0;

                    int[] totals = new int[] { 0, 0, 0 };

                    int width = bmps[i, j].Width;
                    int height = bmps[i, j].Height;
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

                    int count = ImageBlocks.Count;

                    ImageBlocks.Add(new MosaicImage()
                    {
                        ID = count + 1,
                        X = j,
                        Y = i,
                        red = avgR,
                        green = avgG,
                        blue = avgB,

                    });


                    Program pr = new Program();
                    pr.drawTextProgressBar(count + 1, 400);

                }



            return ImageBlocks;
        }


        //Resize source image to 400x400px
        public Bitmap resizeImage(Image image)
        {
            Rectangle destRect = new Rectangle(0, 0, 400, 400);
            Bitmap bitmap = new Bitmap(400, 400);
            bitmap.SetResolution(image.HorizontalResolution, image.VerticalResolution);
            using (Graphics graphics = Graphics.FromImage(bitmap))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;
                using (ImageAttributes attributes = new ImageAttributes())
                {
                    attributes.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, attributes);
                }
            }
            return bitmap;
        }


    }

}
