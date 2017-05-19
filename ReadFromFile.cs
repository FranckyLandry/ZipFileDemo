using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.IO.Compression;
using System.Text.RegularExpressions;
using System.Reflection;



namespace ZipFileDemo
/*
    * regular expression
    * 
    * \(             # Escaped parenthesis, means "starts with a '(' character"
(          # Parentheses in a regex mean "put (capture) the stuff 
          #     in between into the Groups array" 
  [^)]    # Any character that is not a ')' character
  *       # Zero or more occurrences of the aforementioned "non ')' char"
)          # Close the capturing group
   \)             # "Ends with a ')' character"
    * 
    * 
    * 
    * */
{
    class Fileprocess
    {
       

        private string temp_File;

        public List<string> typeOf_statement = new List<string>(new string[] {
                                                    "if","foreach","while","for","switch"
                                                    });

        public string final_File;


        public Fileprocess()
        {
            temp_File = "temp.txt";
            final_File = "main_file.txt";
        }







        public ArrayList GetAllSignatureMethod(string strFileName)
        {
            ArrayList methodNames = new ArrayList();

            var strMethodLines = File.ReadAllLines(strFileName)
                                        .Where(a => (a.Contains("protected") ||
                                                    a.Contains("private") ||
                                                    a.Contains("public")) &&
                                                    !a.Contains("class"));
            foreach (var item in strMethodLines)
            {
                if (item.IndexOf("(") != -1)
                {
                    methodNames.Add(item);
                    
                    //s.Split(new char[] { '{', '}' }, StringSplitOptions.RemoveEmptyEntries)

                    //File.ReadAllLines(strFileName).Where(a => (a.StartsWith(item)));
                    //string strTemp = String.Join("", item.Substring(0, item.IndexOf("(")).Reverse());
                    //methodNames.Add(String.Join("", strTemp.Substring(0, strTemp.IndexOf(" ")).Reverse()));
                }
            }
            return methodNames;
        }



        public List<string> GetMethodContent(ArrayList signature)
        {
            var temp2 = new List<string>();
           
            try
            {
               
                StreamReader sr = new StreamReader("Armor.txt");

                string line;
                
              
                line = sr.ReadLine();


                while ((line = sr.ReadLine()) != null)
                {

                    foreach (var item in signature)
                    {
                        if (line.Contains(item.ToString()))
                        {
                            line = sr.ReadLine();
                            //File.ReadAllLines()
                           var a = Regex.Match(line, @"\{([^}]*)\}").Groups[1].Value;


                        }

                    }

                }




                //while ((line = sr.ReadLine()) != null)
                //{

                //    foreach (var item in signature)
                //    {
                //        if (line.StartsWith(item.ToString()))
                //        {
                //            temp2.Add(line);
                //            line = sr.ReadLine();
                //            while ((line != signature.IndexOf(item) + 1.ToString()) && (line != null))
                //            {
                //                temp2.Add(line);
                //                line = sr.ReadLine();

                //            }

                //        }

                //    }

                //}

                sr.Close();
                

            }
            catch (Exception e)
            {
                // Let the user know what went wrong.
                Console.WriteLine("The file could not be read:");
                Console.WriteLine(e.Message);
            }

            return temp2;

        }


        public int GetNberOfMethod(ArrayList allSignatureMethod)
        {
            int nberOfMethod = allSignatureMethod.Count;
            return nberOfMethod;
        }

        public List<Tuple<string,int,string,string>> Layer(ArrayList allsignatureMethod,string owner)
        {

            List<Tuple<string, int,string,string>> mydictionary = new List<Tuple<string, int,string,string>>(); 

            var returnType ="";

            foreach ( string item in allsignatureMethod)
            {
                if (item.IndexOf("(")!=-1)
                {
                    string strTemp = String.Join("", item.Substring(0, item.IndexOf("(")).Reverse());
                    char [] type = strTemp.Split(' ')[1].ToCharArray();
                    Array.Reverse(type);
                    returnType = new string(type);
                    
                  

                    var methodParamters = Regex.Match(item, @"\(([^)]*)\)").Groups[1].Value;
                    MatchCollection collection = Regex.Matches(methodParamters, @"[\S]+");
                    var nberOfParameter = collection.Count / 2;
                    string strtemp1 = String.Join("", item.Substring(0, item.IndexOf("(")).Reverse());
                    string nameMethod = String.Join("", strTemp.Substring(0, strTemp.IndexOf(" ")).Reverse());

                    mydictionary.Add(Tuple.Create(returnType, nberOfParameter, nameMethod,owner));
                    
                }
            }
            
            return mydictionary;
        }


        public void SaveLayerProcess(List<Tuple<string, int, string, string>> layercollection,string detination)
        {
            foreach (var layer in layercollection)
            {
               
                File.AppendAllText(detination, Environment.NewLine+ "" + layer.Item1 + "," + layer.Item2 + "," + layer.Item3 + "," + layer.Item4 );
            }
        }





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


            string text = File.ReadAllText(@"main_file1.txt");

            if (text != "")
            {
                return false;
            }
            else
                return true;



        }



    }
}
