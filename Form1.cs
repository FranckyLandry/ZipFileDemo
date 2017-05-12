using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.IO.Compression;

namespace ZipFileDemo
{
    public partial class Form1 : Form
    {
        private Fileprocess file_process = new Fileprocess();
        
        //private string dest_file = null;
        public Form1()
        {
            InitializeComponent();
        }

        private bool _IsFolder = false;

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] files = openFileDialog1.FileNames;
                textBox1.Text = string.Join(",", files);
                _IsFolder = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                _IsFolder = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result = saveFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                if (_IsFolder)
                {
                    ZipFile.CreateFromDirectory(textBox1.Text, saveFileDialog1.FileName);
                }
                else
                {
                    string[] files = textBox1.Text.Split(',');
                    ZipArchive zip = ZipFile.Open(saveFileDialog1.FileName, ZipArchiveMode.Create);
                    foreach (string file in files)
                    {
                        zip.CreateEntryFromFile(file, Path.GetFileName(file), CompressionLevel.Optimal);
                    }
                    zip.Dispose();
                }
                MessageBox.Show("ZIP file created successfully!");
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();

            if (result == DialogResult.OK)
            {
                var zip_Folder = ZipFile.OpenRead(openFileDialog1.FileName);

                var _SourcePath = @"" + openFileDialog1.FileName;

                var _FileOwner = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                

                foreach (ZipArchiveEntry entry in zip_Folder.Entries)
                {

                    if (file_process.GetOnlyClass(entry))
                    {
                        file_process.GetContent(file_process.DestinationFile(entry), entry);
                        file_process.ProcessingFile(file_process.DestinationFile(entry), file_process.Count_file(_FileOwner), file_process.final_File, _FileOwner);
                    }

                }
                //r_File.Read_fileread(dest_file, filenameWithoutPath + "_to_count.txt", "main_file.txt", filenameWithoutPath);
            }
        }

        private void button5_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox2.Text = openFileDialog1.FileName;
                DialogResult result2 = folderBrowserDialog1.ShowDialog();
                if (result2 == DialogResult.OK)
                {
                    ZipFile.ExtractToDirectory(openFileDialog1.FileName, folderBrowserDialog1.SelectedPath);

                    MessageBox.Show("ZIP file extracted successfully!");
                }
            }
        }



    }
}
