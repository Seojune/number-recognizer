/*
 * this program is for creating data.txt file from input bitmaps.
 */

using System;
using System.Drawing;
using System.IO;

namespace data
{


    class Program
    {
        const int areaSize = 32;  //height, width of an area
        const int areaNum = 8;    //number of areas
        static bool isPainted(Bitmap b, int x, int y) //checking whether an area is painted
        {
            for (int i = areaSize * x; i <= areaSize * (x + 1) - 1; i++)
            {
                for (int j = areaSize * y; j <= areaSize * (y + 1) - 1; j++)
                {
                    if (b.GetPixel(i, j) == Color.FromArgb(0, 0, 0)) return true;
                }
            }
            return false;
        }

        static void Main(string[] args)
        {
            int num;
            int m = Convert.ToInt16(Console.ReadLine());              //Set number
            StreamWriter dw = new StreamWriter(@"data.txt");          //open data.txt
            int[,,] n = new int[areaNum, areaNum, 10];
            for (int i = 1; i <= m; i++) //set no.1 to no.m
            {
                for (num = 0; num <= 9; num++) //digit 0~9
                {
                    Bitmap bmp = new Bitmap(num + "_" + i + ".bmp");
                    for (int x = 0; x <= areaNum - 1; x++)
                    {
                        for (int y = 0; y <= areaNum - 1; y++)
                        {
                            n[x, y, num] += Convert.ToInt32(isPainted(bmp, x, y));  //add to array if that area is painted
                        }
                    }

                }
            }

            //Output
            for (num = 0; num <= 9; num++)
            {
                for (int y = 0; y <= areaNum - 1; y++)
                {
                    for (int x = 0; x <= areaNum - 1; x++)
                    {
                        dw.Write((double)n[x, y, num] / m + " ");
                    }
                }
            }
            dw.Close();
        }
    }
}