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

namespace AI_Application
{
    /// <summary>
    /// Interaction logic for ReadWindow.xaml
    /// </summary>
    public partial class ReadWindow : Window
    {
        public ReadWindow()
        {
            InitializeComponent();
            // Adding a line from (0,10) to (120,70)
            AddLine(0,10,120,70);
            // Adding a dot at (100,200)
            AddCavern(100,200);
        }

        private void MapCaves(int caveNum)
        {


            //CaveCanvas.Children.Add();
        }

        private void AddLine(int startXCoord, int startYCoord, int endXCoord, int endYCoord)
        {
            Line line = new Line();
            // code - https://stackoverflow.com/questions/5969503/drawing-lines-in-code-using-c-sharp-and-wpf
            line.Visibility = Visibility.Visible;
            line.StrokeThickness = 2;
            line.Stroke = Brushes.Black;



            // Coordinate for starting point of line
            line.X1 = startXCoord;
            line.Y1 = startYCoord;

            // Coordinate for ending point of line
            line.X2 = endXCoord;
            line.Y2 = endYCoord;

            // Adding the line to the canvas
            CaveCanvas.Children.Add(line);
        }

        private void AddCavern(int xCoord, int yCoord)
        {
            Ellipse elipse = new Ellipse();
            elipse.Width = 12;
            elipse.Height = 12;

            elipse.Fill = Brushes.Black;
            Canvas.SetLeft(elipse, xCoord);
            Canvas.SetTop(elipse, yCoord);

            CaveCanvas.Children.Add(elipse);
        }

        // Returns user to the main menu
        private void MenuButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow window = new MainWindow();
            Close();
            window.Show();
        }

        // Automatic step through of the cave
        private void AutomateButton_Click(object sender, RoutedEventArgs e)
        {

        }

        // Manual step through of the cave
        private void ManualButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
