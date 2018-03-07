using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Shapes;
using BusinessLayer;

namespace AI_Application
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        // Entire file location string
        public string FileLocation { get; private set; }
        // Visited the cave previously?
        public bool[] VisitedCoords { get; private set; }
        public bool[] DeadEndCaves { get; private set; }
        public int[] EndNodeCoords { get; set; }

        // Reads the file and gets ready for search
        public ReadWindow(string fileLocation)
        {
            FileLocation = fileLocation;
            InitializeComponent();
        }

        private void SetUpRead()
        {
            // Calling the read constructor
            Read newRead = new Read(FileLocation);
            VisitedCoords = new Boolean[Read.CaveNum];
            DeadEndCaves = new bool[Read.CaveNum];

            // Setting all values to false
            for (int i = 0; i < Read.CaveNum; i++)
            {
                VisitedCoords[i] = false;
                DeadEndCaves[i] = false;
            }
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
        private void AddCavern(int xCoord, int yCoord, bool startOrEnd)
        {
            Ellipse elipse = new Ellipse();
            elipse.Width = 12;
            elipse.Height = 12;

            if (startOrEnd)
            {
                elipse.Fill = Brushes.Red;
            }
            else
            {
                elipse.Fill = Brushes.Black;
            }
            Canvas.SetLeft(elipse, xCoord * Read.MaxXCoord + 5);
            Canvas.SetTop(elipse, yCoord * Read.MaxYCoord + 5);

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

            SetUpRead();

            ClearCanvas();

            int currentCave = 0;
            AddCavern(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1], true);

            // Repeats until current cave has been fully mapped
            while (currentCave != (Read.CaveNum - 1))
            {
                // Will return true when MapCaves reaches the end node
                var newCave = MapCaves(currentCave);
                currentCave = newCave;
            }

            MessageBox.Show("FINISHED MAPPING CAVES");
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
        private int MapCaves(int currCaveNum)
        {
            int currentX = Read.CaveCoords[currCaveNum, 0];
            int currentY = Read.CaveCoords[currCaveNum, 1];

            // Search through caves connected to the cave number
            int count = 0;
            int caveConn = 0;
            // Visited node 1
            VisitedCoords[0] = true;
            bool[] caveConnected = new bool[Read.CaveNum];

            for (int i = 0; i < Read.CaveNum; i++)
            {
                caveConnected[i] = false;
            }

            // Loops through the caves connected to currCaveNum
            for (int i = Read.CaveNum * currCaveNum; i < Read.CaveNum * currCaveNum + Read.CaveNum; i++)
            {
                // If there is a cave connection
                if (Read.CaveConnections[i] == '1')
                {
                    int linkedX = Read.CaveCoords[count, 0];
                    int linkedY = Read.CaveCoords[count, 1];

                    // Only draw a new cave and line if the node hasn't been visited
                    if (!VisitedCoords[count])
                    {
                        AddLine(currentX, currentY, linkedX, linkedY);
                        
                        // If cavern is the end cavern (start cavern already been drawn)
                        if (count == Read.CaveNum - 1)
                        {
                            AddCavern(linkedX, linkedY, true);
                        }
                        else // If not the final cavern
                        {
                            AddCavern(linkedX, linkedY, false);
                        }
                    }

                    caveConn++;
                    caveConnected[count] = true;
                    // Switches active cave halfway through code
                }

                count++;
            }

            UpdateUI(currCaveNum + 1, caveConn, currentX, currentY);

            // If it already at the end node, end program
            // NOTE: READ.CaveNum holds the number of caves from 1, not 0
            if (currCaveNum == Read.CaveNum - 1)
            {
                return currCaveNum;
            }
            // Adding to a list of visited coordinates
            VisitedCoords[currCaveNum] = true;

            return CheckCaves(currCaveNum, caveConn, caveConnected);
            // IF THERE IS ONLY ONE NODE CONNECTED AND IT'S ALREADY BEEN VISITED

            // Return the next cave to go to
        }

        /// <summary>
        /// Process that updates the UI information on the RHS screen
        /// </summary>
        private void UpdateUI(int caveNum, int cavesConnected, int xCoord, int yCoord)
        {
            CaveConnectedBlock.Text = "Cave Number: " + caveNum;
            CaveNumBlock.Text = "Caves Connected: " + cavesConnected;
            CaveCoordinatesBlock.Text = "Cave Coordinates: "+xCoord+","+yCoord;
        }

        // Clears the canvas
        private void ClearCanvas()
        {
            CaveCanvas.Children.Clear();
        }

        /// <summary>
        /// Checks each of the caves connecting to the current cave and returns the next cave number
        /// </summary>
        /// <param name="currCaveNum">Current cave number active</param>
        /// <param name="caveConn">Number of cave connections to active cave</param>
        /// <param name="caveConnected">Boolean array of connected caves</param>
        /// <returns></returns>
        private int CheckCaves(int currCaveNum, int caveConn, bool[] caveConnected)
        {
            // If a connected cave is the final cave of the search
            if (caveConnected[Read.CaveNum - 1])
            {
                MessageBox.Show("FINAL CAVE CONNECTED");
                return Read.CaveNum - 1;
            }

            // If there is only one connected cave
            //  And not the first node
            if (caveConn == 1 && currCaveNum != 0)
            {
                DeadEndCaves[currCaveNum] = true;

                MessageBox.Show("DEAD END NODE");

                // Loop back through all the caves
                for (int i = 0; i < Read.CaveNum; i++)
                {
                    if (caveConnected[i])
                    {
                        MessageBox.Show("Leaving dead end node");
                        return i;
                    }
                }
            }

            // If the start node has no nodes connected
            if (currCaveNum == 0 && caveConn == 0)
            {
                MessageBox.Show("NO NODES CONNECTED");
                throw new Exception("There are no caves connected to the first cave.");
            }

            // If there is only a single cave connection
            //  To the first node
            if (caveConn == 1 && currCaveNum == 0)
            {
                MessageBox.Show("ONLY ONE CAVE CONNECTION");
                for (int i = 0; i < Read.CaveNum; i++)
                {
                    if (caveConnected[i])
                    {
                        currCaveNum = i;
                        MessageBox.Show("Returning " + currCaveNum);
                        return currCaveNum;
                    }
                }

                // If only one new connected cave (Came from previous node)
            }
            else if (caveConn == 2 && currCaveNum != 0)
            {
                MessageBox.Show("2 NODES - CAME FROM ONE");
                for (int i = 0; i < Read.CaveNum; i++)
                {
                    if (caveConnected[i])
                    {
                        // If it hasn't been visited and not a dead end
                        if (!VisitedCoords[i] && !DeadEndCaves[i])
                        {
                            currCaveNum = i;
                        }

                        // If there is a dead cave connected
                        // CAN ONLY BE A DEAD END IF IT'S BEEN VISITED
                        if (DeadEndCaves[i])
                        {
                            DeadEndCaves[currCaveNum] = true;
                        }

                        // POSSIBLE FOR INFINITE LOOP IF VISITED AND NOT DEAD END? 3 WAY LOOP? 
                        // if (VisitedCoords[i])
                    }
                }

                MessageBox.Show("In 2 Cave Connections and Not 1");
                return currCaveNum;
                // If Cave Connected greater than one and it's either the first node
                //  or another node with 3+ connections
            }
            else if (caveConn > 1)
            {
                MessageBox.Show("MULTIPLE CONNECTIONS - TO BE CODED");
                // If there is more than one cave connection available 
                //  (cave 1 has 2 connections or other caves have 3)
            }
            else
            {
                MessageBox.Show("ELSE");
            }

            return Read.CaveNum - 1;
        }
    }
}
