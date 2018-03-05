using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;
using BusinessLayer;

namespace AI_Application
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public string FileLocation { get; private set; }
        public int[] VisitedCoords { get; private set; }

        // Reads the file and gets ready for search
        public ReadWindow(string fileLocation)
        {
            InitializeComponent();
            FileLocation = fileLocation;
            // Calling the read constructor
            Read newRead = new Read(fileLocation);
            VisitedCoords = new int[Read.CaveNum];
        }

        // Returns user to the main menu
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            Close();
            window.Show();
        }

        // Request to clear the caverns
        private void ClearCaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
        }

        /// <summary>
        /// Adds a line from one cavern to another
        /// </summary>
        /// <param name="startXCoord">Starting X Coordinate</param>
        /// <param name="startYCoord">Starting Y Coordinate</param>
        /// <param name="endXCoord">Ending X Coordinate</param>
        /// <param name="endYCoord">Ending Y Coordinate</param>
        private void AddLine(int startXCoord, int startYCoord, int endXCoord, int endYCoord)
        {
            Line line = new Line();

            line.Visibility = Visibility.Visible;
            line.StrokeThickness = 2;
            line.Stroke = Brushes.Black;

            // Coordinate for starting point of line
            line.X1 = startXCoord * Read.MaxXCoord + 10;
            line.Y1 = startYCoord * Read.MaxYCoord + 10;

            // Coordinate for ending point of line
            line.X2 = endXCoord * Read.MaxXCoord + 10;
            line.Y2 = endYCoord * Read.MaxYCoord + 10;

            // Adding the line to the canvas
            CaveCanvas.Children.Add(line);
        }

        /// <summary>
        /// Adds caverns on the cavern map
        /// </summary>
        /// <param name="xCoord">X Coordinate of the Cavern</param>
        /// <param name="yCoord">Y Coordinate of the Cavern</param>
        private void AddCavern(int xCoord, int yCoord)
        {
            Ellipse elipse = new Ellipse();
            elipse.Width = 12;
            elipse.Height = 12;

            elipse.Fill = Brushes.Black;
            Canvas.SetLeft(elipse, xCoord * Read.MaxXCoord + 5);
            Canvas.SetTop(elipse, yCoord * Read.MaxYCoord + 5);

            MessageBox.Show("Added elipse name");
            //elipse.Name = caveNum.ToString();

            CaveCanvas.Children.Add(elipse);
        }

        /// <summary>
        /// Automates the process of going from the start cave to the end
        /// </summary>
        /// <param name="sender">Object that sent the request</param>
        /// <param name="e">Dummy object</param>
        private void AutomateButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();

            int currentCave = 0;
            int newCave;
            AddCavern(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1]);
            bool cavesMapped = false;

            // Repeats until current cave has been fully mapped
            while (currentCave != 20)
            {
                // Will return true when MapCaves reaches the end node
                newCave = MapCaves(currentCave);
                MessageBox.Show("Current cave is " + newCave);
                currentCave = newCave;

                if (currentCave == Read.CaveNum)
                {
                    // NEED TO BREAK LOOP
                }

                /* Checking for end of loop
                if (newCave == Read.CaveNum)
                {
                    cavesMapped = true;
                }*/

                // Breaking infinite loop
                //cavesMapped = true;
            }
        }

        /// <summary>
        /// Manual process for going through each cavern with a click
        /// </summary>
        /// <param name="sender">Object that sent the request</param>
        /// <param name="e">Dummy object</param>
        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            
        }

        // Maps caves based on the cave number
        private int MapCaves(int caveNum)
        {
            MessageBox.Show("Entering map caves");
            int currentX = Read.CaveCoords[caveNum, 0];
            int currentY = Read.CaveCoords[caveNum, 1];

            // Search through caves connected to the cave number
            int count = 0;
            int caveConn = 0;
            bool[] caveConnected = new bool[Read.CaveNum];

            for (int i = 0; i < Read.CaveNum; i++)
            {
                caveConnected[i] = false;
            }

            for (int i = Read.CaveNum * caveNum; i < Read.CaveNum * caveNum + Read.CaveNum; i++)
            {
                // If there is a cave connection
                if (Read.CaveConnections[i] == '1')
                {
                    MessageBox.Show("Inner hit is" + count);
                    int linkedX = Read.CaveCoords[count, 0];
                    int linkedY = Read.CaveCoords[count, 1];
                    
                    // Adds a line from the current 
                    AddLine(currentX,currentY,linkedX,linkedY);
                    //ADDED COUNT
                    AddCavern(linkedX,linkedY);

                    caveConn++;
                    caveConnected[count] = true;
                }

                count++;
            }

            MessageBox.Show("Count is " + count);

            // If there is only one connected cave
            // May need to change code location
            if (caveConn == 1)
            {
                MessageBox.Show("ONLY ONE CAVE CONNECTION");
                for (int i = 0; i < Read.CaveNum; i++)
                {
                    if (caveConnected[i])
                    {
                        caveNum = i;
                        MessageBox.Show("Returning " + caveNum);
                        return caveNum;
                    }
                }
            }else if (caveConn > 1)
            {
                // DO SOMETHING IF THERE'S MORE THAN ONE CONNECTED CAVE
            // If there's no cave connected to 1, change the connection to 0
            }else if (caveConn == 0)
            {
                caveNum = 2;
                // Creates infinite loop
                return caveNum;
            }

            // Return the next cave to go to
            return 20;
        }

        /// <summary>
        /// Process that updates the UI information on the RHS screen
        /// </summary>
        private void UpdateUI()
        {

        }

        /// <summary>
        /// Clears the cavern
        /// </summary>
        private void ClearCanvas()
        {
            CaveCanvas.Children.Clear();
        }
    }
}
