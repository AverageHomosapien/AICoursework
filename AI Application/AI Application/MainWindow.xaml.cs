using System;
using System.IO;
using System.Windows;
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

        /// <summary>
        /// Button to read file clicked
        /// </summary>
        /// <param name="sender">Names the object that sent the command</param>
        /// <param name="e">Dummy object</param>
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
            
            // Open new window
            ReadWindow window = new ReadWindow();
            Close();
            window.Show();

        }

        // Quits the program
        private void QuitButton_Click(object sender, RoutedEventArgs e)
        {
            System.Windows.Application.Current.Shutdown();
        }
    }
}
