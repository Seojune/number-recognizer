using System;
using System.Drawing;
using System.IO;

namespace handwrite
{
    class Program
    {
        const int areaSize = 32;   //height, width of an area
        const int numArea = 8;    //number of areas

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
            Bitmap b = new Bitmap("Input.bmp");                  //open input.bmp
            double[,,] p = new double[numArea, numArea, 10];    //p(pixel=on|number)
            bool[,] input = new bool[numArea, numArea]; //input.bmp transformed into 8*8 array
            double[] posterior = new double[10];          //posterior
            double evidence = 0;
            double[] l = new double[10];                  //likelihood
            for (int i = 0; i <= 9; i++)                  //prior = 0.1
            {
                l[i] = 1;
            }

            //read from data.txt
            FileStream fs = new FileStream("data.txt", FileMode.Open, FileAccess.Read);
            StreamReader sr = new StreamReader(fs, System.Text.Encoding.Default);
            string str = sr.ReadToEnd();
            string[] splitS = str.Split('\n', ' ');
            double[] splitNum = new double[640];
            for (int i = 0; i <= 10 * numArea * numArea - 1; i++)
            {
                splitNum[i] = Convert.ToDouble(splitS[i]);
            }
            for (int num = 0; num <= 9; num++)
            {
                for (int y = 0; y <= numArea - 1; y++)
                {
                    for (int x = 0; x <= numArea - 1; x++)
                    {
                        p[x, y, num] = splitNum[x + numArea * y + numArea * numArea * num];
                    }
                }
            }

            //transform input.bmp into 8*8 array
            for (int x = 0; x <= numArea - 1; x++)
            {
                for (int y = 0; y <= numArea - 1; y++)
                {
                    input[x, y] = isPainted(b, x, y);
                }
            }

            //calculate likelihood of each
            for (int num = 0; num <= 9; num++)
            {
                for (int x = 0; x <= numArea - 1; x++)
                {
                    for (int y = 0; y <= numArea - 1; y++)
                    {
                        l[num] *= input[x, y] ? p[x, y, num] : 1 - p[x, y, num];
                    }
                }
            }

            //calculate evidence
            for (int i = 0; i <= 9; i++)
            {
                evidence += 0.1 * l[i];
            }

            //Output
            Console.WriteLine("POSTERIOR:");
            for (int num = 0; num <= 9; num++)
            {
                Console.Write("{0}: {1}%\n", num, 10 * l[num] / evidence);
            }
            int result = -1;
            double max = 0;                  //maximum value of posteriors
            for (int num = 0; num <= 9; num++)
            {
                if (max < l[num])
                {
                    max = l[num];
                    result = num;
                }
            }
            if (result == -1)
                Console.WriteLine("ERR");
            else
                Console.WriteLine("RESULT:{0}", result);
            Console.Read();
        }
    }
}