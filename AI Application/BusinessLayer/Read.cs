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


            int caveNum = Int32.Parse(input[0].ToString());

            input = input.Replace(",", "");
            input = input.Remove(0,1);

            string[] caveCoords = new string[caveNum];

            for (int i = 0; i < (caveNum * 2); i++)
            {
                
            }

            MessageBox.Show("CaveNum = " + caveNum);
            MessageBox.Show("Input is " + input);

            
        }
    }
}
