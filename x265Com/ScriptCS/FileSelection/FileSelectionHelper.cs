﻿//using System.Windows.Controls;
using System.Windows.Forms;

namespace x265Com.ScriptCS.FileSelection
{
    public class FileSelectionHelper
    {
        //private static void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        //{
        //    // Create an instance of the open file dialog box.
        //    OpenFileDialog openFileDialog1 = new OpenFileDialog();
            
        //    // Set filter options and filter index.
        //    openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
        //    openFileDialog1.FilterIndex = 1;

        //    openFileDialog1.Multiselect = true;

        //    // Call the ShowDialog method to show the dialog box.
        //    bool? userClickedOK = openFileDialog1.ShowDialog();

        //    // Process input if the user clicked OK.
        //    if (userClickedOK == true)
        //    {
        //        // Open the selected file to read.
        //        System.IO.Stream fileStream = openFileDialog1.File.OpenRead();

        //        using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
        //        {
        //            // Read the first line from the file and write it the textbox.
        //            tbResults.Text = reader.ReadLine();
        //        }
        //        fileStream.Close();
        //    }
        //}
        public static void OpenFileDialog_Click()
        {
            // Create an instance of the open file dialog box.
            OpenFileDialog _OpenFileDialog = new OpenFileDialog();

            // Set filter options and filter index.
            //openFileDialog1.Filter = "Text Files (.txt)|*.txt|All Files (*.*)|*.*";
            //openFileDialog1.FilterIndex = 1;

            _OpenFileDialog.Multiselect = true;

            // Call the ShowDialog method to show the dialog box.
            DialogResult _DialogResult = _OpenFileDialog.ShowDialog();
            if(_DialogResult == DialogResult.OK || _DialogResult == DialogResult.Cancel)
            {
                return;
            }
            // Process input if the user clicked OK.
            //if (userClickedOK == true)
            //{
                // Open the selected file to read.
               // System.IO.Stream fileStream = _OpenFileDialog.File.OpenRead();
                /*
                using (System.IO.StreamReader reader = new System.IO.StreamReader(fileStream))
                {
                    // Read the first line from the file and write it the textbox.
                    tbResults.Text = reader.ReadLine();
                }
                fileStream.Close();
                */
            //}
        }
    }
}