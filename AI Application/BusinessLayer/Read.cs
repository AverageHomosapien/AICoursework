using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessLayer
{
    public class Read
    {

        private int CaveNum { get; set; }
        private int[,] CaveCoords { get; set; }
        private string ReadableText { get; set; }

        // Reads in text
        public Read(string filepath)
        {
            string input = File.ReadAllText(filepath);

            // Converts text into readable format
            ReadableText = ConvertText(input);
        }

        private string ConvertText(string input)
        {
            // Getting cave number from beginning of string
            string caveStr;

            // Checking the number of caves
            if (input[1] == ',')
            {
                caveStr = input[0].ToString();
                CaveNum = Int32.Parse(caveStr);
                // Removing cave number and comma
                input = input.Remove(0, 2);
            }
            else
            {
                caveStr = input[0].ToString() + input[1].ToString();
                CaveNum = Int32.Parse(caveStr);
                // Removing cave number and comma
                input = input.Remove(0, 3);
            }

            // Creating cave coord array
            CaveCoords = new Int32[2, CaveNum];

            // Loops through cave coordinates
            for (int i = 0; i < CaveNum; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (input[1] == ',')
                    {
                        CaveCoords[j, i] = Int32.Parse(input[0].ToString());
                        input = input.Remove(0, 2);
                    }
                    else
                    {
                        var tempStr = input[0].ToString() + input[1].ToString();
                        CaveCoords[j, i] += Int32.Parse(tempStr);
                        input = input.Remove(0, 3);
                    }
                }
            }

            // Replacing commas below the cave coordinates are checked and removed
            input = input.Replace(",", "");
            input = input.Remove(0, 1);

            return input;
        }
    }
}
