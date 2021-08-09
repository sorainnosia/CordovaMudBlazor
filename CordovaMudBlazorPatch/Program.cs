using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Diagnostics;

namespace CordovaMudBlazorPatch
{
    class Program
    {
        static void Main(string[] args)
        {
            bool ignoreError = false;
            if (args == null || args.Length == 0)
            {
                Console.WriteLine("Usage :");
                Console.WriteLine("Place executable at the root of Cordova project, then use one of below command");
                Console.WriteLine("   CordovaMudBlazorPatch.exe /aaptoptionsignore");
                Console.WriteLine("   CordovaMudBlazorPatch.exe /fileperm");
                Console.WriteLine("   CordovaMudBlazorPatch.exe /mudblazorjs");
                Console.WriteLine("   CordovaMudBlazorPatch.exe /mudblazortable");
                Console.WriteLine("   CordovaMudBlazorPatch.exe /basecordova");
                return;
            }

            if (Directory.Exists("www") == false)
            {
                Console.WriteLine("Error: www directory not found");
                return;
            }

            if (args.Length > 1 && args[1].ToLower() == "/force")
                ignoreError = true;

            if (args[0].ToLower() == "/mudblazorjs") PatchMudBlazorJs();
            else if (args[0].ToLower() == "/mudblazortable") PatchMudBlazorTable();
            else if (args[0].ToLower() == "/basecordova") PatchBaseCordova();
            else if (args[0].ToLower() == "/dup") RemoveDuplicate();
            else if (args[0].ToLower() == "/fileperm") PatchFilePermission();
            else if (args[0].ToLower() == "/aaptoptionsignore") PatchAaptOption();
            else if (args[0].ToLower() == "/patchcsspath") PatchCssPath("www");
            else if (args[0].ToLower() == "/all")
            {
                RemoveDuplicate();
                int x = PatchMudBlazorJs();
                if (x == -1 && ignoreError == false) return;
                x = PatchMudBlazorTable();
                if (x == -1 && ignoreError == false) return;
                x = PatchBaseCordova();
                if (x == -1 && ignoreError == false) return;
                x = PatchFilePermission();
                if (x == -1 && ignoreError == false) return;
                x = PatchAaptOption();
                if (x == -1 && ignoreError == false) return;
                x = PatchCssPath("www");
                if (x == -1 && ignoreError == false) return;
                Console.WriteLine("Success: completed all commands");
            }
            else
                Console.WriteLine("Error: argument '" + args[0] + "' not recognized");
        }

        public static string RunCommand(string command)
        {
            ProcessStartInfo psi = null;
            Process p = null;
            try
            {
                psi = new ProcessStartInfo();
                psi.FileName = "cmd";
                psi.Arguments = "/c " + command;
                psi.CreateNoWindow = true;
                psi.WindowStyle = ProcessWindowStyle.Hidden;
                psi.UseShellExecute = false;
                psi.ErrorDialog = false;
                psi.RedirectStandardOutput = true;
                psi.RedirectStandardInput = true;
                psi.RedirectStandardError = true;
                psi.Verb = "runas";

                p = new Process();
                p.EnableRaisingEvents = true;
                p.StartInfo = psi;
                p.Start();

                string output = string.Empty;
                string standard_output = string.Empty;
                while ((standard_output = p.StandardOutput.ReadLine()) != null)
                {
                    output += standard_output + "\r\n";
                }

                return output;
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                if (p.StandardOutput != null)
                {
                    p.StandardOutput.Close();
                    p.StandardOutput.Dispose();
                }
                if (p.StandardError != null)
                {
                    p.StandardError.Close();
                    p.StandardError.Dispose();
                }
                if (p.StandardInput != null)
                {
                    p.StandardInput.Close();
                    p.StandardInput.Dispose();
                }
                if (p != null)
                {
                    p.Close();
                    p.Dispose();
                }
            }
            return string.Empty;
        }

        public static void RemoveDuplicate()
        {
            string cmd = "DEL /S /Q *.br *.gz";
            string output = RunCommand(cmd);
        }

        public static int PatchCssPath(string directory)
        {
            int count = 0;
            string[] dirs = Directory.GetDirectories(directory);
            if (dirs != null && dirs.Length > 0)
            {
                foreach (string dir in dirs)
                {
                    count += PatchCssPath(dir);
                }
            }
            
            string[] files = Directory.GetFiles(directory, "*.css");
            if (files != null && files.Length > 0)
            {
                foreach (string file in files)
                {
                    string str = File.ReadAllText(file);
                    string[] tags = Common.CommonStringOps.TagMatch(str, "url(", ")");
                    int temp = 0;
                    if (tags != null && tags.Length > 0)
                    {
                        foreach (string t in tags)
                        {
                            if (t.IndexOf("\n") >= 0 || t.IndexOf("\r") >= 0 || t.IndexOf("\t") >= 0)
                                continue;

                            string tt = t.ToLower();
                            if (tt.IndexOf("#") >= 0) tt = tt.Substring(0, tt.IndexOf("#"));
                            if (tt.IndexOf("?") >= 0) tt = tt.Substring(0, tt.IndexOf("?"));
                            if (tt.ToLower().EndsWith(".woff") || tt.ToLower().EndsWith(".woff2") || tt.ToLower().EndsWith(".eot") || tt.ToLower().EndsWith(".svg") || tt.ToLower().EndsWith(".ttf") || tt.ToLower().EndsWith(".otf"))
                            {
                                string slash = "";
                                if (t.StartsWith("/") == false) slash = "/";
                                if (t.StartsWith("/android_asset/www") == false)
                                {
                                    str = str.Replace("url(" + t + ")", "url(/android_asset/www" + slash + t + ")");
                                    temp++;
                                }
                            }
                        }
                        if (temp > 0) count++;
                    }

                    File.WriteAllText(file, str);
                    if (temp > 0)
                        Console.WriteLine("Success: Successfully patch css path in file '" + file + "'");
                }
            }
            return count;
        }

        public static int PatchFilePermission()
        {
            int count = 0;
            string file = Path.Combine("platforms", "android", "app", "src", "main", "AndroidManifest.xml");
            if (File.Exists(file) == false)
            {
                Console.WriteLine("'" + file + "' does not exist");
                return -1;
            }
            string text = File.ReadAllText(file);
            int i = text.IndexOf(Strings.AndroidPermissionRead);
            if (i < 0)
            {
                int ii = text.IndexOf(Strings.Application);
                if (ii < 0)
                {
                    Console.WriteLine("Error: android application tag does not exist in AndroidManifest.xml");
                    return -1;
                }
                count++;
                text = text.Insert(ii, Strings.UsePermissionRead + "\r\n\t");
            }
            else
                Console.WriteLine("Info : already has Permission Read option in AndroidManifest.xml");

            i = text.IndexOf(Strings.AndroidPermissionWrite);
            if (i < 0)
            {
                int ii = text.IndexOf(Strings.Application);
                if (ii < 0)
                {
                    Console.WriteLine("Error: android application tag does not exist in AndroidManifest.xml");
                    return -1;
                }
                count++;
                text = text.Insert(ii, Strings.UsePermissionWrite + "\r\n\t");
            }
            else
                Console.WriteLine("Info : already has Permission Write option in AndroidManifest.xml");

            i = text.IndexOf(Strings.LegacyStorage);
            if (i < 0)
            {
                int ii = text.IndexOf(Strings.Application);
                if (ii < 0)
                {
                    Console.WriteLine("Error: android application tag does not exist in AndroidManifest.xml");
                    return -1;
                }
                count++;
                text = text.Insert(ii + Strings.Application.Length, Strings.LegacyStorage + " ");
            }
            else
                Console.WriteLine("Info : already has Legacy Storage option in AndroidManifest.xml");

            File.WriteAllText(file, text);

            file = Path.Combine("www", "index.html");
            if (File.Exists(file) == false)
                Console.WriteLine("Error: '" + file + "' does not exist");
            else
            {
                text = File.ReadAllText(file);
                if (text.IndexOf(Strings.ScriptPermission) < 0)
                {
                    int ii = text.IndexOf(Strings.EndBody, StringComparison.OrdinalIgnoreCase);
                    if (ii < 0)
                        Console.WriteLine("Error: index.html end of body tag not found");
                    else
                    {
                        count++;
                        text = text.Replace(Strings.EndBody, Strings.ScriptPermission + "\r\n" + Strings.EndBody, StringComparison.OrdinalIgnoreCase);
                    }
                }
                if (text.IndexOf(Strings.InstantiateStreaming) < 0) //Need to be included before webassembly.js
                {
                    i = text.IndexOf(Strings.ScriptTag, StringComparison.OrdinalIgnoreCase);
                    if (i < 0)
                    {
                        Console.WriteLine("Error: index.html needs to have at least one <script> tag");
                        return -1;
                    }

                    text = text.Insert(i, Strings.InstantiateStreaming + "\r\n\t");
                    count++;
                }
            }

            File.WriteAllText(file, text);
            if (count > 0)
            {
                Console.WriteLine("Success: Successfully adding file permission feature");
                return count;
            }
            return -1;
        }

        public static int PatchAaptOption()
        {
            string file = Path.Combine("platforms", "android", "app", "build.gradle");
            if (File.Exists(file) == false)
            {
                Console.WriteLine("Error: '" + file + "' does not exist");
                return -1;
            }
            
            int count = 0;
            string text = File.ReadAllText(file);
            int i = text.IndexOf(Strings.AaptOptionsIgnoreProperty);
            if (i >= 0)
            {
                int j = Math.Min(text.IndexOf("\n", i), text.IndexOf("\r", i));
                if (j >= 0)
                {
                    string temp = text.Substring(i + Strings.AaptOptionsIgnoreProperty.Length + 1, j - (i + Strings.AaptOptionsIgnoreProperty.Length + 1)).Trim();
                    if (temp == Strings.AaptOptionsIgnorePattern)
                        Console.WriteLine("Info: ignoreAssetsPattern already exist");
                    else
                    {
                        if (temp.StartsWith("\"") && temp.EndsWith("\"")) temp = temp.Substring(1, temp.Length - 2).Trim();

                        List<string> strs = temp.Split(new char[] { ':' }).ToList();
                        List<string> strs2 = Strings.AaptOptionsIgnorePattern.Split(new char[] { ':' }).ToList();

                        foreach (string s in strs2)
                        {
                            bool exist = false;
                            foreach (string ss in strs)
                            {
                                if (s == ss) exist = true;
                            }
                            if (exist == false) strs.Add(s);
                        }
                        text = text.Replace(temp, string.Join(":", strs));
                        File.WriteAllText(file, text);
                        count++;
                        Console.WriteLine("Success: Successfully amended ignoreAssetsPattern");
                    }
                }
            }
            else
            {
                int k = text.IndexOf(Strings.DefaultConfig);
                if (k < 0)
                    Console.WriteLine("Info: defaultConfig does not exist in build.gradle, fail to inject aaptOptions ignoreAssetsPattern");
                else
                {
                    text = text.Remove(k, Strings.DefaultConfig.Length);
                    text = text.Insert(k, Strings.AaptOption + "\r\n\r\n\t" + Strings.DefaultConfig);
                    File.WriteAllText(file, text);
                    count++;
                    Console.WriteLine("Success: Successfully appended ignoreAssetsPattern");
                }
            }
            return count;
        }

        public static int PatchBaseCordova()
        {
            string file = Path.Combine("www", "index.html");
            if (File.Exists(file))
            {
                string text = File.ReadAllText(file);
                int count = 0;
                int i =text.IndexOf(Strings.BaseTag, StringComparison.OrdinalIgnoreCase);
                if (i >= 0)
                {
                    int j = text.IndexOf(">", i);
                    string basetype = text.Substring(i, j - i + 1);

                    text = text.Replace(basetype, Strings.CordovaBase);
                    count++;
                }

                i = text.IndexOf(Strings.ScriptTag, StringComparison.OrdinalIgnoreCase);
                if (i < 0)
                {
                    Console.WriteLine("Error: index.html needs to have at least one <script> tag");
                    return -1;
                }

                if (text.IndexOf(Strings.SourceCordovaJs) < 0)
                    text = text.Insert(i, Strings.IncludeCordovaJs + "\r\n\t");
                else
                    Console.WriteLine("Info: cordova.js is already included in index.html");

                File.WriteAllText(file, text);
                Console.WriteLine("Success: Successfully patching '" + file + "'");
                return count;
            }
            return -1;
        }

        public static int PatchMudBlazorTable()
        {
            string file = Path.Combine("www", "_content", "MudBlazor", "MudBlazor.css");
            string file2 = Path.Combine("www", "_content", "MudBlazor", "MudBlazor.min.css");

            int count = 0;
            if (File.Exists(file2))
            {
                string text = File.ReadAllText(file2);
                text = text.Replace(Strings.MBMinifyCenter, Strings.MBMinifyCenterComment);
                text = text.Replace(Strings.MBMinifyNo50, Strings.MBMinify50);
                
                File.WriteAllText(file2, text);
                count++;
            }

            if (File.Exists(file))
            {
                string text = File.ReadAllText(file);
                text = text.Replace(Strings.MBNoMinifyCenter, Strings.MBNoMinifyCenterComment);
                text = text.Replace(Strings.MBNoMinifyNo50, Strings.MBNoMinify50);

                File.WriteAllText(file, text);
                count++;
            }

            if (count > 0)
            {
                Console.WriteLine("Success: Successfully patching '" + file + "'");
                return count;
            }
            return -1;
        }
        public static int PatchMudBlazorJs()
        {
            string file = Path.Combine("www", "_content", "MudBlazor", "MudBlazor.js");
            string file2 = Path.Combine("www", "_content", "MudBlazor", "MudBlazor.min.js");

            int count = 0;
            if (File.Exists(file))
            {
                string text = File.ReadAllText(file);

                text = text.Replace("anchorRect?.top", "(anchorRect == null ? 0 : anchorRect.top)");
                text = text.Replace("anchorRect?.absoluteTop", "(anchorRect == null ? 0 : anchorRect.absoluteTop)");
                text = text.Replace("anchorRect?.height", "(anchorRect == null ? 0 : anchorRect.height)");
                text = text.Replace("anchorRect?.width", "(anchorRect == null ? 0 : anchorRect.width)");
                text = text.Replace("anchorRect?.left", "(anchorRect == null ? 0 : anchorRect.left)");
                text = text.Replace("anchorRect?.absoluteLeft", "(anchorRect == null ? 0 : anchorRect.absoluteLeft)");

                File.WriteAllText(file, text);
                Console.WriteLine("Success: Successfully patching '" + file + "'");
                count++;
            }
            if (File.Exists(file2))
            {
                string text = File.ReadAllText(file2);

                text = text.Replace("anchorRect?.top", "(anchorRect == null ? 0 : anchorRect.top)");
                text = text.Replace("anchorRect?.absoluteTop", "(anchorRect == null ? 0 : anchorRect.absoluteTop)");
                text = text.Replace("anchorRect?.height", "(anchorRect == null ? 0 : anchorRect.height)");
                text = text.Replace("anchorRect?.width", "(anchorRect == null ? 0 : anchorRect.width)");
                text = text.Replace("anchorRect?.left", "(anchorRect == null ? 0 : anchorRect.left)");
                text = text.Replace("anchorRect?.absoluteLeft", "(anchorRect == null ? 0 : anchorRect.absoluteLeft)");

                File.WriteAllText(file2, text);
                Console.WriteLine("Success: Successfully patching '" + file2 + "'");
                count++;
            }

            if (count == 0)
            {
                Console.WriteLine("Error: MudBlazor.min.js or MudBlazor.js not found");
                return -1;
            }
            return count;
        }
    }
}
