using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BusinessLayer
{
    public class Read
    {

        public static int CaveNum { get; private set; }
        public static int[,] CaveCoords { get; private set; }
        public static string CaveConnections { get; private set; }
        public static double MaxXCoord { get; private set; }
        public static double MaxYCoord { get; private set; }

        // Called in the readWindow initiation
        public Read(string filepath)
        {
            string input = File.ReadAllText(filepath);
            
            // Converts text into readable format
            CaveConnections = ConvertText(input);

            // Scales return to 480 size - total size of box is 500
            MaxXCoord = XCanvasScale();
            MaxYCoord = YCanvasScale();
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
            CaveCoords = new Int32[CaveNum, 2];

            // Loops through cave coordinates
            for (int i = 0; i < CaveNum; i++)
            {
                for (int j = 0; j < 2; j++)
                {
                    if (input[1] == ',')
                    {
                        CaveCoords[i, j] = Int32.Parse(input[0].ToString());
                        input = input.Remove(0, 2);
                    }
                    else
                    {
                        var tempStr = input[0].ToString() + input[1].ToString();
                        CaveCoords[i, j] += Int32.Parse(tempStr);
                        input = input.Remove(0, 3);
                    }
                }
            }
            
            // Replacing commas below the cave coordinates are checked and removed
            input = input.Replace(",", "");

            return input;
        }

        // Getting the biggest X coordinate to scale the cavern display accordingly
        private int HighestXCoord()
        {
            int _maxXCoord = 0;
            for (int i = 0; i < CaveNum; i++)
            {
                if (CaveCoords[i, 0] > _maxXCoord)
                {
                    _maxXCoord = CaveCoords[i, 0];
                }
            }
            
            return _maxXCoord;
        }

        // Getting the biggest Y coordinate to scale the cavern display accordingly
        private int HighestYCoord()
        {
            int _maxYCoord = 0;
            for (int i = 0; i < CaveNum; i++)
            {
                if (CaveCoords[i, 1] > _maxYCoord)
                {
                    _maxYCoord = CaveCoords[i, 1];
                }
            }
            
            return _maxYCoord;
        }

        // Returns the scale of the X canvas
        private double XCanvasScale()
        {
            int _maxXCoord = HighestXCoord();

            double _xScale = (480 / _maxXCoord);

            return _xScale;
        }

        // Returns the scale of the Y canvas
        private double YCanvasScale()
        {
            int _maxYCoord = HighestYCoord();

            double _yScale = (480 / _maxYCoord);

            return _yScale;
        }


    }
}
