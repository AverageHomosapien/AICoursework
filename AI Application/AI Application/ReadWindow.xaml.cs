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

        // Plotting scale for the X and Y coordinates
        private double XCoordModifier { get;  set; }
        private double YCoordModifier { get; set; }

        public ReadWindow(string fileLocation)
        {
            InitializeComponent();
            FileLocation = fileLocation;

            Read readFile = new Read(FileLocation);

            XCoordModifier = readFile.MaxXCoord;
            YCoordModifier = readFile.MaxYCoord;

            MessageBox.Show("XCoordModifier is " + XCoordModifier);
            MessageBox.Show("YCoordModifier is " + YCoordModifier);

            // Adding a line from (0,10) to (120,70)
            AddLine(0,0,14,8);
            // Adding a dot at (100,200)
            AddCavern(2,6);
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
            line.X1 = startXCoord * XCoordModifier + 10;
            line.Y1 = startYCoord * YCoordModifier + 10;

            // Coordinate for ending point of line
            line.X2 = endXCoord * XCoordModifier + 10;
            line.Y2 = endYCoord * YCoordModifier + 10;

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
            Canvas.SetLeft(elipse, xCoord * XCoordModifier + 10);
            Canvas.SetTop(elipse, yCoord * YCoordModifier + 10);

            CaveCanvas.Children.Add(elipse);
        }

        // Automatic step through of the cave
        private void AutomateButton_Click(object sender, RoutedEventArgs e)
        {
            Read readFile = new Read(FileLocation);

            XCoordModifier = readFile.MaxXCoord;
            YCoordModifier = readFile.MaxYCoord;
        }

        // Manual step through of the cave
        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {
            Read readFile = new Read(FileLocation);

            XCoordModifier = readFile.MaxXCoord;
            YCoordModifier = readFile.MaxYCoord;
        }

        private void MapCaves(int caveNum)
        {


        }
    }
}
