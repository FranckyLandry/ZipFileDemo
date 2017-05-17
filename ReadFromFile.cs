using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Reflection;



namespace ZipFileDemo
{
    class Fileprocess
    {

        private string temp_File;

        public List<string> typeOf_variables = new List<string>(new string[] {
                                                    "public void","public int","public string","public List<string>","public List<int>",
                                                    "private void","private int","private string","private List<string>","private List<int>",
                                                    "protected void","protected int","protected string","protected List<string>","protected List<int>",
                                                     "public ArrayList","private ArrayList","protected ArrayList"
                                                });

        public string final_File;


        public Fileprocess()
        {
            temp_File = "temp.txt";
            final_File = "main_file.txt";
        }

        public Array aia()
        {
            return null;
        }


       

        public void ReadingSignature()
        {
            var list_type_methods = new List<string>(new string[] {
                                                    "public void","public int","public string","public List<string>","public List<int>",
                                                    "private void","private int","private string","private List<string>","private List<int>",
                                                    "protected void","protected int","protected string","protected List<string>","protected List<int>",
                                                     "public ArrayList","private ArrayList","protected ArrayList"
                                                });




            //foreach (var item in list_type_methods)
            //{
            //    Regex re = new Regex("["+item + "-\\s A-Za-z-("+typeof""+-]");
            //}
            //"[A - Za - z0 - 9\s]{ 1, }$")

          
        }


        public List<string> GetAllSignatureMehod(string strFileName)
        {
            List<string> methodNames = new List<string>();
            //var strMethodLines = File.ReadAllLines()
            var strMethodLines = File.ReadAllLines(strFileName)
                                        .Where(a => (a.Contains("protected") ||
                                                    a.Contains("private") ||
                                                    a.Contains("public")) &&
                                                    !a.Contains("_") && !a.Contains("class"));
            foreach (var item in strMethodLines)
            {
                if (item.IndexOf("(") != -1)
                {
                    methodNames.Add(item);
                    //string strTemp = String.Join("", item.Substring(0, item.IndexOf("(")).Reverse());
                    //methodNames.Add(String.Join("", strTemp.Substring(0, strTemp.IndexOf(" ")).Reverse()));
                }
            }
            return methodNames;
        }


        //public List<string> GetAllMethodNames(string strFileName)
        //{
        //    List<string> methodNames = new List<string>();
        //    //var strMethodLines = File.ReadAllLines()
        //    var strMethodLines = File.ReadAllLines(strFileName)
        //                                .Where(a => (a.Contains("protected") ||
        //                                            a.Contains("private") ||
        //                                            a.Contains("public")) &&
        //                                            !a.Contains("_") && !a.Contains("class"));
        //    foreach (var item in strMethodLines)
        //    {
        //        if (item.IndexOf("(") != -1)
        //        {
        //            string strTemp = String.Join("", item.Substring(0, item.IndexOf("(")).Reverse());
        //            methodNames.Add(String.Join("", strTemp.Substring(0, strTemp.IndexOf(" ")).Reverse()));
        //        }
        //    }
        //    return methodNames.Distinct().ToList();
        //}



        //public  IEnumerable<MethodInfo> GetMethodsBySig(this Type type, Type returnType, params Type[] parameterTypes)
        //{
        //    return type.GetMethods().Where((m) =>
        //    {
        //        if (m.ReturnType != returnType) return false;
        //        var parameters = m.GetParameters();
        //        if ((parameterTypes == null || parameterTypes.Length == 0))
        //            return parameters.Length == 0;
        //        if (parameters.Length != parameterTypes.Length)
        //            return false;
        //        for (int i = 0; i < parameterTypes.Length; i++)
        //        {
        //            if (parameters[i].ParameterType != parameterTypes[i])
        //                return false;
        //        }
        //        return true;
        //    });
        //}



        public void GetContent(string _DestinationFile, ZipArchiveEntry entry)
        {
             
            entry.ExtractToFile(temp_File);
            string[] content_temp = File.ReadAllLines(temp_File);


            if (!File.Exists(_DestinationFile))
            {
                File.Copy(temp_File, _DestinationFile);
               
            }
            else
            {
                File.AppendAllLines(_DestinationFile, content_temp);

            }
            File.Delete(temp_File);

        }



        public string Count_file(string name)
        {
            string count_file = name + "count.txt";
            return count_file;
        }

        public string DestinationFile(ZipArchiveEntry entry)
        {
            string dest_file = Path.ChangeExtension(entry.Name, ".txt");
            return dest_file;


        }



        public bool GetOnlyClass(ZipArchiveEntry entry)
        {
            if (entry.FullName.EndsWith(".cs", StringComparison.OrdinalIgnoreCase) &&
                                       (!(entry.FullName.Contains("Resources"))) &&
                                       (!(entry.FullName.Contains("AssemblyInfo"))) &&
                                       (!(entry.FullName.Contains("Settings"))) &&
                                       (!(entry.FullName.Contains("Form"))) &&
                                       (!(entry.FullName.Contains("Program"))) &&
                                       (!(entry.FullName.Contains("Temp"))))
                return true;
            else
                return false;

        }






        public void Save_File(string[] content, string destination)
        {

            try
            {
                File.AppendAllLines(destination, content);

            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            finally
            {
                Console.WriteLine("Executing finally block.");
            }

        }

        public void ProcessingFile(string source_file, string count_file, string finalFile, string owner)
        {
            String line;
            var temp_store = new KeyValuePair<string, int>();
            List<Tuple<string, int>> wordCounted_splited = new List<Tuple<string, int>>();
            List<string> temp_word_splited_list = new List<string>();
            int counter = 0;
         

            try
            {

               
                StreamReader sr = new StreamReader(source_file);


                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    Console.WriteLine(line);
                    //Read the next line
                    line = sr.ReadLine();
                    if (line != null)
                    {
                        line.Split();

                        foreach (string w in line.Split())
                        {
                            if (!temp_word_splited_list.Contains(w))
                            {

                                temp_word_splited_list.Add(w);

                                counter = 1;

                                wordCounted_splited.Add(new Tuple<string, int>(w, counter));


                            }
                            else if (temp_word_splited_list.Contains(w))
                            {
                                for (int i = 0; i < wordCounted_splited.Count(); i++)
                                {
                                    if (wordCounted_splited[i].Item1.Equals(w))
                                    {

                                        counter = wordCounted_splited[i].Item2;
                                        counter++;
                                        wordCounted_splited.RemoveAt(i);
                                        temp_store = new KeyValuePair<string, int>(w, counter);
                                        wordCounted_splited.Add(new Tuple<string, int>(temp_store.Key, temp_store.Value));

                                        break;
                                    }
                                }

                            }



                        }
                    }


                }

                sr.Close();
            }
            catch (Exception)
            {

                throw;
            }
            finally
            {
                try
                {

                    //Pass the filepath and filename to the StreamWriter Constructor
                    StreamWriter sw = new StreamWriter(count_file);

                    for (int i = 0; i < wordCounted_splited.Count(); i++)
                    {
                        sw.WriteLine(wordCounted_splited[i]);

                    }


                    //Close the file
                    sw.Close();
                    //System.Threading.Thread.Sleep(1);
                    Get_Features_Value(wordCounted_splited, finalFile, owner);


                }
                catch (Exception e)
                {
                    Console.WriteLine("Exception: " + e.Message);
                }
                finally
                {

                    Console.WriteLine("Executing finally block.");

                }

            }
        }


        private ArrayList ReadFeatures()
        {
             var features = new ArrayList();
             String line;

            try
            {
                //Pass the file path and file name to the StreamReader constructor
                StreamReader sr = new StreamReader(@"features.txt");

                //Read the first line of text
                line = sr.ReadLine();

                //Continue to read until you reach end of file
                while (line != null)
                {
                    //write the lie to console window
                    foreach (string item in line.Split(','))
                    {
                        features.Add(item);
                    }
                    break;


                }

                //close the file
                sr.Close();


            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
            return features;

        }


        public void Get_Features_Value(List<Tuple<string, int>> counted_features, string file_features_to_save, string owner)
        {
            try
            {
                List<Tuple<string, int>> content = new List<Tuple<string, int>>();


                ArrayList features = ReadFeatures();

                foreach (string f in features)
                {
                    for (int i = 0; i < counted_features.Count(); i++)
                    {
                        if (counted_features[i].Item1 == f)
                        {
                            //content.Add(new Tuple<string, int>(counted_features[i].Item1, counted_features[i].Item2));
                            content.Add(counted_features[i]);
                        }
                    }
                }
                // if features exist in the file then insert its value
                for (int i = 0; i < features.Count; i++)
                {
                    //System.Threading.Thread.Sleep(1);
                    string temp = features[i].ToString();
                    for (int j = 0; j < content.Count; j++)
                    {
                        //System.Threading.Thread.Sleep(1);
                        if (features[i].ToString() == content[j].Item1)
                        {
                            features.Insert(features.IndexOf(temp), content[j].Item2);
                            features.Remove(temp);
                        }

                    }
                    // if not found the value changes to zero
                    if (features.Contains(temp))
                    {
                        features.Insert(features.IndexOf(temp), 0);
                        features.Remove(temp);
                        temp = null;
                    }

                }


                switch (CheckFile_is_empty())
                {
                    case false:
                        bool first = false;

                        for (int i = 0; i < features.Count; i++)
                        {
                            //System.Threading.Thread.Sleep(1);

                            if (first == false)
                            {
                                File.AppendAllText(file_features_to_save, Environment.NewLine);
                                i--;

                            }
                            else
                            {

                                File.AppendAllText(file_features_to_save, features[i] + ",");
                            }
                            first = true;
                        }
                        features.Add(owner);
                        int lastIndex = features.Count - 1;
                        //System.Threading.Thread.Sleep(1);
                        File.AppendAllText(file_features_to_save, (features[lastIndex]).ToString());

                        break;
                    case true:

                        foreach (int val in features)
                        {
                            File.AppendAllText(file_features_to_save, val + ",");
                        }
                        features.Add(owner);
                        int lastI = features.Count - 1;

                        File.AppendAllText(file_features_to_save, features[lastI] + "");

                        break;
                }

                features.Clear();
            }
            catch (Exception e)
            {
                Console.WriteLine("Exception: " + e.Message);
            }
        }
        public bool CheckFile_is_empty()
        {
            //MethodBase b = MethodInfo.GetCurrentMethod();


            string text = File.ReadAllText(@"main_file.txt");

            if (text != "")
            {
                return false;
            }
            else
                return true;



        }



    }
}
