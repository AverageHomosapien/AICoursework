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

        // Reads the file and gets ready for search
        public ReadWindow(string fileLocation)
        {
            InitializeComponent();
            FileLocation = fileLocation;
            Read newRead = new Read(fileLocation);
        }

        // Returns user to the main menu
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            Close();
            window.Show();
        }

        // Adds a line from one point to another point
        private void AddLine(int startXCoord, int startYCoord, int endXCoord, int endYCoord)
        {
            Line line = new Line();
            // code - https://stackoverflow.com/questions/5969503/drawing-lines-in-code-using-c-sharp-and-wpf
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

        // Adds caverns on the page
        private void AddCavern(int xCoord, int yCoord)
        {
            Ellipse elipse = new Ellipse();
            elipse.Width = 12;
            elipse.Height = 12;

            elipse.Fill = Brushes.Black;
            Canvas.SetLeft(elipse, xCoord * Read.MaxXCoord + 10);
            Canvas.SetTop(elipse, yCoord * Read.MaxYCoord + 10);

            CaveCanvas.Children.Add(elipse);
        }

        // Automatic step through of the cave
        private void AutomateButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();

            // Adding a line from (0,10) to (120,70)
            AddLine(0, 0, 14, 8);
            // Adding a dot at (100,200)
            AddCavern(4, 6);
        }

        // Manual step through of the cave
        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            ClearCanvas();

            int currentCave = 0;
            bool cavesMapped = false;

            // Repeats until current cave has been fully mapped
            while (cavesMapped == false)
            {

                // Will return true when MapCaves reaches the end node
                currentCave = MapCaves(currentCave);

                // Checking for end of loop
                if (currentCave == Read.CaveNum)
                {
                    cavesMapped = true;
                }
                
                // Breaking infinite loop
                cavesMapped = true;
            }

            // Adding a line from (0,10) to (120,70)
            AddLine(0, 8, 14, 8);
            // Adding a dot at (100,200)
            AddCavern(2, 6);

            
        }

        private int MapCaves(int caveNum)
        {
            // Search through caves connected to the cave number
            for (int i = Read.CaveNum * caveNum; i < Read.CaveNum * caveNum + 7; i++)
            {
                
            }

            // Should return false - just to stop infinite loops
            return caveNum;
        }

        private void UpdateUI()
        {

        }

        private void ClearCaveButton_OnClick(object sender, RoutedEventArgs e)
        {
            ClearCanvas();
        }

        private void ClearCanvas()
        {
            CaveCanvas.Children.Clear();
        }
    }
}
