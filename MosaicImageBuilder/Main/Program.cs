
﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Main
{
    class Program
    {

        //Single-threaded apartment #Required for use  of Dialogs
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine(@"
   _____                          .__            ________                                         __                 
  /     \    ____   ___________   |__|  ____    /  _____/   ____    ____    ____ _______ _____  _/  |_  ____ _______ 
 /  \ /  \  /  _ \ /  ___/\__  \  |  |_/ ___\  /   \  ___ _/ __ \  /    \ _/ __ \\_  __ \\__  \ \   __\/  _ \\_  __ \
/    Y    \(  <_> )\___ \  / __ \_|  |\  \___  \    \_\  \\  ___/ |   |  \\  ___/ |  | \/ / __ \_|  | (  <_> )|  | \/
\____|__  / \____//____  >(____  /|__| \___  >  \______  / \___  >|___|  / \___  >|__|   (____  /|__|  \____/ |__|   
        \/             \/      \/          \/          \/      \/      \/      \/             \/                     
");
            int progress = 0;
            string path = "";
            List<MosaicTiles> mosaictiles = new List<MosaicTiles>();
            List<MosaicImage> ImageBlocks = new List<MosaicImage>();

            Bitmap finalimage;

            Program pr = new Program();


            MessageBox.Show("Please choose the folder containing the tile images.", "Choose folder");
            FolderBrowserDialog dialog = new FolderBrowserDialog();

            //Save each tile's value in an entity object list    
            try
            {
                if (dialog.ShowDialog() == DialogResult.OK)
                {
                    Console.WriteLine("Busy processing Image Tiles \n");
                    path = dialog.SelectedPath;
                }
                DirectoryInfo info = new DirectoryInfo(path);
                int length = info.GetFiles("*.jpg").Length;



                foreach (FileInfo info2 in info.GetFiles("*.jpg"))
                {
                    ProcessTiles tiles = new ProcessTiles(mosaictiles, info2.FullName);
                    progress++;
                    pr.drawTextProgressBar(progress, length);
                }
               
            
                MessageBox.Show("Please choose the image to be processed.", "Choose file");
                OpenFileDialog dialog2 = new OpenFileDialog();

//Divide source image into blocks and saves values in entity object list
                try
                {
                    if (dialog2.ShowDialog() == DialogResult.OK)
                    {

                        path = dialog2.FileName;
                        Console.WriteLine("Busy processing Image \n");

                        ProcessImage image = new ProcessImage();
                        ImageBlocks = image.ProcessImages(path);
                    }

                }
                catch (Exception exception2)
                {
                    Console.WriteLine("The following exception occured. :" + exception2);
                    throw;
                }

            }
            catch (Exception exception)
            {
                Console.WriteLine("The following exception occured. :" + exception);
                throw;
            }


//Find best tile for each block and renders it
            try
            {
                Console.WriteLine("Busy Rendering Image \n");
                RenderImage ri = new RenderImage();
                finalimage= ri.Render(mosaictiles, ImageBlocks);

                MessageBox.Show("Choose folder where image should be saved.");
                //Saves rendered image
                SaveFileDialog sd = new SaveFileDialog();
                if (sd.ShowDialog() == DialogResult.OK)
                {
                    sd.DefaultExt = ".jpg";
                    sd.AddExtension = true;
                 
                    string savepath = sd.FileName;
                    finalimage.Save(savepath );
                }
            }

            catch (Exception exception3)
            {
                Console.WriteLine("The following exception occured. :" + exception3);
                throw;
            }
           finally
            {
                MessageBox.Show("Image Saved.");
                Console.WriteLine("Completed");
                Console.ReadKey();
            }
        
    
        }


        //ProgressBar
        public void drawTextProgressBar(int progress, int total)
        {
            //draw empty progress bar
            Console.CursorLeft = 0;
            Console.Write("["); //start
            Console.CursorLeft = 32;
            Console.Write("]"); //end
            Console.CursorLeft = 1;
            float onechunk = 30.0f / total;

            //draw filled part
            int position = 1;
            for (int i = 0; i < onechunk * progress; i++)
            {
                Console.BackgroundColor = ConsoleColor.Gray;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw unfilled part
            for (int i = position; i <= 31; i++)
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.CursorLeft = position++;
                Console.Write(" ");
            }

            //draw totals
            Console.CursorLeft = 35;
            Console.BackgroundColor = ConsoleColor.Black;
            Console.Write(progress.ToString() + " of " + total.ToString() + "    "); //blanks at the end remove any excess

            if (progress == total)
                Console.WriteLine("Completed \n");

        }
    }
}