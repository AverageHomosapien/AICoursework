using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
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
        private int FinalXCoord { get; set; }
        private int FinalYCoord { get; set; }

        // Entire file location string
        public string FileLocation { get; private set; }

        // Reads the file and gets ready for search
        public ReadWindow(string fileLocation)
        {
            FileLocation = fileLocation;
            InitializeComponent();

            // Sets up the canvas for a new cave
            SetUpRead();
        }

        /// <summary>
        /// Sets up the canvas for a new read
        /// </summary>
        private void SetUpRead()
        {
            // Calling the read constructor
            Read newRead = new Read(FileLocation);
            CaveCanvas.Children.Clear();

            // Trying to get the coordinates from the cave coordinates
            FinalXCoord = Read.CaveCoords[Read.CaveNum - 1, 0];
            FinalYCoord = Read.CaveCoords[Read.CaveNum, 0];
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
            SetUpRead();
        }

        /// <summary>
        /// Adds a line from one cavern to another
        /// </summary>
        /// <param name="startXCoord">Starting X Coordinate</param>
        /// <param name="startYCoord">Starting Y Coordinate</param>
        /// <param name="endXCoord">Ending X Coordinate</param>
        /// <param name="endYCoord">Ending Y Coordinate</param>
        private void AddLine(int startXCoord, int startYCoord, int endXCoord, int endYCoord, bool activeLine)
        {
            Line line = new Line();

            line.Visibility = Visibility.Visible;
            line.StrokeThickness = 2;

            // If line is in current use
            if (activeLine)
            {
                line.Fill = Brushes.Red;
            }
            else
            {
                line.Fill = Brushes.Black;
            }

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
        /// <param name="startOrEnd">Start or ending coordinate</param>
        private void AddCavern(int xCoord, int yCoord, bool activeNode)
        {
            Ellipse elipse = new Ellipse();
            elipse.Width = 12;
            elipse.Height = 12;

            // If node is in current use
            if (activeNode)
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

            int currentCave = 0;

            // Adds the starting node on the map
            AddCavern(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1], true);

            // Repeats until current cave has been fully mapped
            while (currentCave != (Read.CaveNum - 1))
            {
                // Return current cave
                currentCave = MapCaves(currentCave);
            }

            MessageBox.Show("FINISHED MAPPING CAVES");
        }

        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Maps caves based on the cave number
        private int MapCaves(int caveToCheck)
        {
            // New list of connected cave numbers
            List<int> _levelOneCaves = new List<int>();

            // New list of connected cave numbers
            List<int> _levelTwoCaves = new List<int>();

            // List of bottom level caves
            List<int> _levelThreeCaves = new List<int>();

            // Connections for each level
            List<int> _caveConnections = new List<int>();

            // Contains a list of all of the frontline nodes to be evaluated
            List<int> _frontlineNodes = new List<int>();

            // List of all active nodes
            List<int> _activeNodes = new List<int>();

            // Lists the new node to check
            int newNodeToCheck = 1000;
            int tempNode = 1000;


            #region LoopThroughLevels
            int count = 0;
            // Repeating through each cave connection for caveNum
            for (int i = Read.CaveNum * caveToCheck; i < Read.CaveNum * caveToCheck + Read.CaveNum; i++)
            {
                // If there's a cave connection
                if (Read.CaveConnections[i] == '1')
                {
                    // If the cave connection is the goal node
                    if (count == Read.CaveNum - 1)
                    {
                        FoundEndNode();
                    }

                    // Repeating through each cave connection for caveNum
                    for (int j = Read.CaveNum * caveToCheck; j < Read.CaveNum * caveToCheck + Read.CaveNum; j++)
                    {
                        // If there's a cave connection
                        if (Read.CaveConnections[i] == '1')
                        {
                            // If the cave connection is the goal node
                            if (count == Read.CaveNum - 1)
                            {
                                FoundEndNode();
                            }

                            // Repeating through each cave connection for caveNum
                            for (int k = Read.CaveNum * caveToCheck; k < Read.CaveNum * caveToCheck + Read.CaveNum; k++)
                            {
                                // If there's a cave connection
                                if (Read.CaveConnections[i] == '1')
                                {
                                    if (count == Read.CaveNum - 1)
                                    {
                                        FoundEndNode();
                                    }
                                    _levelThreeCaves.Add(count);
                                    _frontlineNodes.Add(count);
                                }
                            }
                            _levelTwoCaves.Add(count);
                        }
                    }
                    // ADD TO CAVE CONNECTIONS? _caveConnections.Add);
                }
                count++;
            }

            // For the number of nodes in the _frontlinenodes
            for (int i = 0; i < _frontlineNodes.Count; i++)
            {
                // Getting an evaluation back to check the new frontline nodes
                int tempNumber = DjikstrasEvaluation(_frontlineNodes[i]);
                if (tempNumber < tempNode)
                {
                    newNodeToCheck = _frontlineNodes[i];
                }
            }

            #endregion

            return -1;
        }

        // Returns the djikstra's evaluation of a node
        private int DjikstrasEvaluation(int caveToCheck)
        {
            int xCoord = Read.CaveCoords[caveToCheck, 0];
            int yCoord = Read.CaveCoords[caveToCheck, 1];
            int outputNum;


            //return Math.Sqrt();
            return -1;
        }

        private void FoundEndNode()
        {

        }
    }
}
