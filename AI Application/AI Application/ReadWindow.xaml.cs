using System;
using System.Collections.Generic;
using System.Threading.Tasks;
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
        private int FinalXCoord { get; set; }
        private int FinalYCoord { get; set; }

        private bool ProgramActive { get; set; }
        private bool ClickThroughActive { get; set; }

        // Fronteir nodes and all the weightings of the nodes
        public List<int> FrontierNodes = new List<int>();
        public List<double> FronteirNodeScores = new List<double>();

        public bool[] VisitedNode;
        public bool[] CheckedNodeScore;

        // Entire file location string
        public string FileLocation { get; private set; }

        // Reads the file and gets ready for search
        public ReadWindow(string fileLocation)
        {
            FileLocation = fileLocation;
            InitializeComponent();

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
            FinalYCoord = Read.CaveCoords[Read.CaveNum -1, 0];

            FrontierNodes.Clear();
            FronteirNodeScores.Clear();

            // Sets up the canvas for a new cave
            VisitedNode = new bool[Read.CaveNum];
            CheckedNodeScore = new bool[Read.CaveNum];

            // Setting all values to false
            for (int i = 1; i < VisitedNode.Length; i++)
            {
                VisitedNode[i] = false;
                CheckedNodeScore[i] = false;
            }

            // Visited the start node
            VisitedNode[0] = true;
            CheckedNodeScore[0] = false;

            ProgramActive = false;
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
            ProgramActive = false;
            SetUpRead();
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
            var line = new Line
            {
                Visibility = Visibility.Visible,
                StrokeThickness = 2,
                Stroke = Brushes.Black,
                X1 = startXCoord * Read.MaxXCoord + 10,
                Y1 = startYCoord * Read.MaxYCoord + 10,
                X2 = endXCoord * Read.MaxXCoord + 10,
                Y2 = endYCoord * Read.MaxYCoord + 10
            };

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
            var elipse = new Ellipse
            {
                Visibility = Visibility.Visible,
                Width = 12,
                Height = 12,
                Fill = Brushes.Red
            };

            Canvas.SetLeft(elipse, xCoord * Read.MaxXCoord + 5);
            Canvas.SetTop(elipse, yCoord * Read.MaxYCoord + 5);

            CaveCanvas.Children.Add(elipse);
        }

        /// <summary>
        /// Adds the label to the cave
        /// </summary>
        /// <param name="x">Left position</param>
        /// <param name="y">Top position</param>
        /// <param name="text">Cave Number</param>
        private void AddLabel(double x, double y, string text)
        {
            var textBlock = new TextBlock
            {
                Text = text,
                Foreground = new SolidColorBrush(Colors.Blue),
                FontSize = 20
            };
            Canvas.SetLeft(textBlock, x * Read.MaxXCoord - 16);
            Canvas.SetTop(textBlock, y * Read.MaxYCoord - 8);
            CaveCanvas.Children.Add(textBlock);
        }

        /// <summary>
        /// Automates the process of going from the start cave to the end
        /// </summary>
        /// <param name="sender">Object that sent the request</param>
        /// <param name="e">Dummy object</param>
        private void AutomateButton_Click(object sender, RoutedEventArgs e)
        {
            // If the manual button has already been pressed
            if (ProgramActive == false)
            {
                ProgramActive = true;
                ClickThroughActive = false;

                int currentCave = 0;

                // Adds the starting node on the map
                AddCavern(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1]);
                AddLabel(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1], (currentCave + 1).ToString());

                // Repeats until current cave has been fully mapped
                while (currentCave != (Read.CaveNum - 1))
                {
                    // Return current cave (CURRENTLY RETURNING -1)
                    currentCave = MapCaves(currentCave, false);
                }

                // Found final node - IS THERE ANYTHING EXTRA TO BE DONE?
                MessageBox.Show("FINISHED MAPPING CAVES");
            }
            else
            {
                MessageBox.Show("Please ensure that you reset the program before attempt another automatic search.");
            }
        }

        /// <summary>
        /// Manual option for the program
        /// </summary>
        /// <param name="sender">Object that sent the request</param>
        /// <param name="e">Dummy object</param>
        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            // If the automatic button has already been pressed
            if ((ProgramActive && ClickThroughActive) || (ProgramActive == false))
            {
                ProgramActive = true;
                ClickThroughActive = true;

                int currentCave = 0;

                // Adds the starting node on the map
                AddCavern(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1]);
                AddLabel(Read.CaveCoords[currentCave, 0], Read.CaveCoords[currentCave, 1], (currentCave + 1).ToString());

                // Repeats until current cave has been fully mapped
                while (currentCave != (Read.CaveNum - 1))
                {
                    // Return current cave (CURRENTLY RETURNING -1)
                    currentCave = MapCaves(currentCave, true);
                }

                // Found final node - IS THERE ANYTHING EXTRA TO BE DONE?
                MessageBox.Show("FINISHED MAPPING CAVES");
            }
            else
            {
                MessageBox.Show("Please ensure that you reset the program before attempting a different mode.");
            }
        }

        // Maps caves based on the cave number
        private int MapCaves(int caveToCheck, bool manualButton)
        {
            // Lists the new node to check
            int newNodeToCheck = 1000;
            double tempNode = 1000;


            #region LoopThroughLevels
            int count = 0; // Can be used for the UI to show the total number of loops through the program?
            // Needs to be global in that case
            // Repeating through each cave connection for caveNum
            for (int i = Read.CaveNum * caveToCheck; i < Read.CaveNum * caveToCheck + Read.CaveNum; i++)
            {
                // If there's a cave connection
                if (Read.CaveConnections[i] == '1')
                {
                    // If node hasn't already been visited
                    if (VisitedNode[count] != true)
                    {
                        VisitedNode[count] = true;

                        AddCavern(Read.CaveCoords[count,0], Read.CaveCoords[count,1]);
                        AddLabel(Read.CaveCoords[count, 0], Read.CaveCoords[count, 1], (count+1).ToString());
                        AddLine(Read.CaveCoords[count,0], Read.CaveCoords[count,1], Read.CaveCoords[caveToCheck,0], Read.CaveCoords[caveToCheck,1]);

                        // If the cave connection is the goal node
                        if (count == Read.CaveNum - 1)
                        {
                            return Read.CaveNum - 1;
                        }

                        int innerCount = 0;

                        // Repeating through each cave connection for caveNum
                        // NOT COUNT?
                        for (int j = Read.CaveNum * count; j < Read.CaveNum * count + Read.CaveNum; j++)
                        {
                            // If there's a cave connection
                            if (Read.CaveConnections[j] == '1')
                            {
                                // If node hasn't been visited already
                                if (VisitedNode[innerCount] != true)
                                {
                                    VisitedNode[innerCount] = true;

                                    AddCavern(Read.CaveCoords[innerCount, 0], Read.CaveCoords[innerCount, 1]);
                                    AddLabel(Read.CaveCoords[innerCount, 0], Read.CaveCoords[innerCount, 1], (innerCount+1).ToString());
                                    AddLine(Read.CaveCoords[innerCount, 0], Read.CaveCoords[innerCount, 1], Read.CaveCoords[count, 0], Read.CaveCoords[count, 1]);

                                    // If the cave connection is the goal node
                                    if (innerCount == Read.CaveNum - 1)
                                    {
                                        return Read.CaveNum - 1;
                                    }

                                    int finalCount = 0;

                                    // Repeating through each cave connection for caveNum
                                    for (int k = Read.CaveNum * innerCount; k < Read.CaveNum * innerCount + Read.CaveNum; k++)
                                    {

                                        // If there's a cave connection
                                        if (Read.CaveConnections[k] == '1')
                                        {
                                            // If node hasn't been visited already
                                            if (VisitedNode[finalCount] != true)
                                            {
                                                VisitedNode[finalCount] = true;

                                                AddCavern(Read.CaveCoords[innerCount, 0], Read.CaveCoords[innerCount, 1]);
                                                AddLine(Read.CaveCoords[innerCount, 0], Read.CaveCoords[innerCount, 1], Read.CaveCoords[finalCount, 0], Read.CaveCoords[finalCount, 1]);
                                                AddLabel(Read.CaveCoords[finalCount, 0], Read.CaveCoords[finalCount, 1], (finalCount+1).ToString());
                                                AddCavern(Read.CaveCoords[finalCount, 0], Read.CaveCoords[finalCount, 1]);

                                                if (finalCount == Read.CaveNum - 1)
                                                {
                                                    return Read.CaveNum - 1;
                                                }

                                                FrontierNodes.Add(finalCount);
                                                FronteirNodeScores.Add(NodeScore(finalCount));
                                            }
                                        }

                                        finalCount++;
                                    }
                                }
                            }

                            innerCount++;
                        }
                    }
                }
                 count++;
            }

            // For the number of nodes in the _frontlinenodes
            for (int i = 0; i < FrontierNodes.Count; i++)
            {
                if (!CheckedNodeScore[FrontierNodes[i]])
                {
                    // Getting an evaluation back to check the new frontline nodes
                    var tempNumber = NodeScore(FrontierNodes[i]);

                    // If the new node is closer
                    if (tempNumber < tempNode)
                    {
                        tempNode = tempNumber;
                        newNodeToCheck = FrontierNodes[i];
                    }
                }
            }

            CheckedNodeScore[newNodeToCheck] = true;
            
            #endregion

            return newNodeToCheck;
        }

        // Returns the score of nodes
        private double NodeScore(int caveToCheck)
        {
            double _score;

            double xCoord = Read.CaveCoords[caveToCheck, 0];
            double yCoord = Read.CaveCoords[caveToCheck, 1];

            double goalXCoord = Read.CaveCoords[Read.CaveNum -1, 0];
            double goalYCood = Read.CaveCoords[Read.CaveNum -1, 1];

            _score = (goalXCoord - xCoord) + (goalYCood - yCoord);

            // Making square positive if it's negative
            if (_score < 0)
            {
                _score = _score * -1;
            }

            _score = Math.Sqrt(_score);
            return _score;
        }
    }
}
