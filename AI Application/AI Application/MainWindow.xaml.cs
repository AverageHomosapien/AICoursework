using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using BusinessLayer;

namespace AI_Application
{
    

    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        private void ReadButton_Click(object sender, RoutedEventArgs e)
        {
            var filePath = @"C:\Users\hamca\Documents\GitHub\AICoursework\AI Application\TestingFiles\";
            var fileName = @"input.cav.text";

            // Checking to use the standard pathname.
            if (FilePathBox.Text != "")
            {
                filePath = FilePathBox.Text;
            }

            // Checking to use the standard filename.
            if (FileNameBox.Text != "")
            {
                fileName = FileNameBox.Text;
            }

            // Calling readfile with correct filepath
            var fileLoc = filePath + fileName;
            
            try
            {
                // Checking to see file exists
                if (!File.Exists(fileLoc))
                {
                    throw new Exception
                        ("Please ensure that the file you are attempting to search exists.");
                }

                var readFile = new Read(fileLoc);
            }
            catch (Exception fileCheck)
            {
                MessageBox.Show(fileCheck.Message, "error");
            }
            

        }
    }
}
