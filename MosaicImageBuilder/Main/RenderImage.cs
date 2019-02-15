using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

namespace Main
{
    class RenderImage
    {
        //Returns Bitmap finalimage
        public Bitmap Render(List<MosaicTiles> tiles, List<MosaicImage> blocks)
        {

            string Path;
           List<Mosaic>  mosaic = new List<Mosaic>();
            Program pr = new Program();

            //Find best Tile image for each block
            foreach(var block in blocks)
                {

               Path= GetBestTile(tiles, block.X, block.Y,block.CIELab);

                mosaic.Add(new Mosaic()
                {
                    X = block.X,
                    Y = block.Y,
                Path= Path
                });
            }

            int count = 0;
            Bitmap finalimage = new Bitmap(401,401) ;

            //Draws new image
            foreach (var image in mosaic)
            {
                
                Image im = Image.FromFile(image.Path);
                Bitmap img = resizeImage(im);
                
                Graphics g = Graphics.FromImage(finalimage);
                g.DrawImage(img, image.X * 20, image.Y * 20);
                pr.drawTextProgressBar(count+1, 400);

count++;

            }

            
            return finalimage;
         

        }





        //Calculates best tile image for each block
        public string GetBestTile(List<MosaicTiles> tiles,int X, int Y, double[] CIELab)
        {
            double difference;
            double bestDifference = double.MaxValue;
            string path = "";

            foreach (var tile in tiles)
            {

                difference = Distance(tile.CIELab[0], CIELab[0]) + Distance(tile.CIELab[1], CIELab[1]) + Distance(tile.CIELab[2], CIELab[2]);
                if(difference < bestDifference)
                {
                    bestDifference = difference;
                    path = tile.path;
                }


            }

            return path;
        }

        private static double Distance(double a, double b)
        {
            return (a - b) * (a - b);
        }

        //Resizes tiles to appropriate block size
        public Bitmap resizeImage(Image image)
        {
            Rectangle destRect = new Rectangle(0, 0, 20, 20);
            Bitmap bitmap = new Bitmap(20, 20);
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
