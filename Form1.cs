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
        private ReadFromFile r_File = new ReadFromFile();
        private string temp_File = @"temp.txt";
        private string dest_file = null;
        public Form1()
        {
            InitializeComponent();
        }

        private bool isFolder = false;

        private void button1_Click(object sender, EventArgs e)
        {
            DialogResult result = openFileDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                string[] files = openFileDialog1.FileNames;
                textBox1.Text = string.Join(",", files);
                isFolder = false;
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            DialogResult result = folderBrowserDialog1.ShowDialog();
            if (result == DialogResult.OK)
            {
                textBox1.Text = folderBrowserDialog1.SelectedPath;
                isFolder = true;
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            DialogResult result=saveFileDialog1.ShowDialog();
            if(result==DialogResult.OK)
            {
                if(isFolder)
                {
                    ZipFile.CreateFromDirectory(textBox1.Text, saveFileDialog1.FileName);
                }
                else
                {
                    string[] files=textBox1.Text.Split(',');
                    ZipArchive zip = ZipFile.Open(saveFileDialog1.FileName, ZipArchiveMode.Create);
                    foreach(string file in files)
                    {
                        zip.CreateEntryFromFile(file, Path.GetFileName(file),CompressionLevel.Optimal);
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
                ZipArchive zip = ZipFile.OpenRead(openFileDialog1.FileName);

                string _SourcePath = @"" + openFileDialog1.FileName;

                string filenameWithoutPath = Path.GetFileNameWithoutExtension(openFileDialog1.FileName);
                
                foreach (ZipArchiveEntry entry in zip.Entries)
                {
                    
                    if (entry.FullName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase))
                    {
                      

                        dest_file = filenameWithoutPath + ".txt";
                        entry.ExtractToFile(temp_File);
                        string[] content_temp = File.ReadAllLines(temp_File);


                        if (!File.Exists(dest_file)) { 
                            File.Copy(temp_File, dest_file);
                            
                        }
                        else { 
                            File.AppendAllLines(dest_file, content_temp);
                            
                        }

                        File.Delete(temp_File);
  
                    }
                }
                r_File.Read_fileread(dest_file, filenameWithoutPath + "_to_count.txt", "main_file.txt", filenameWithoutPath);
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
