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
        public Read(string filepath)
        {
            string input = File.ReadAllText(filepath);

            // Getting cave number from beginning of string
            int caveNum = Int32.Parse(input[0].ToString());
            // Removing cave number and comma
            input = input.Remove(0, 2);

            string[,] caveCoords = new string[2,caveNum];

            for (int i = 0; i < caveNum; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (input[1] == ',')
                    {
                        caveCoords[j,i] += input[0].ToString();
                        input = input.Remove(0, 2);
                    }
                    else
                    {
                        caveCoords[j,i] += input[0].ToString() + input[1].ToString();
                        input = input.Remove(0, 3);
                    }
                }

                MessageBox.Show("Cave coordinates " + i + " are " + caveCoords[0,i]);
                MessageBox.Show("Cave coordinates " + i + " are " + caveCoords[1, i]);
            }

            // Replacing commas below the cave coordinates are checked and removed
            input = input.Replace(",", "");
            input = input.Remove(0,1);

            MessageBox.Show("CaveNum = " + caveNum);
            MessageBox.Show("Input is " + input);

            
        }
    }
}
