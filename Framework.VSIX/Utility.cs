using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Diagnostics;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using Framework.VSIX.Resources;

namespace Framework.VSIX
{
    public class Utility
    {
        public static string GetGeneratorVersion()
        {
            string result = string.Empty;

            try
            {
                StringBuilder outputText = new StringBuilder();

                using (var proc = new System.Diagnostics.Process())
                {
                    proc.StartInfo.FileName = @"cmd.exe";
                    proc.StartInfo.Arguments = string.Format(@" /c  {0}", Global.Yeoman_Generator_VersionCheck);
                    proc.StartInfo.RedirectStandardInput = true;
                    proc.StartInfo.RedirectStandardOutput = true;
                    proc.StartInfo.RedirectStandardError = true;
                    proc.StartInfo.CreateNoWindow = true;
                    proc.StartInfo.UseShellExecute = false;
                    proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
                    proc.Start();

                    proc.StandardInput.Flush();
                    proc.StandardInput.WriteLine("exit");
                    proc.StandardInput.Flush();

                    using (StreamReader reader = proc.StandardOutput)
                    {
                        string _val = reader.ReadToEnd();
                        outputText.Append(_val);
                    }

                    result = Regex.Replace(outputText.ToString(), "[^0-9.]", "");
                }
            }
            catch (System.Exception ex)
            {
                //TODO: Log exception
            }

            return result;
        }

        public static int CompareGeneratorVersions(string baseVersion, string currentVersion)
        {
            int result = 0;

            if (!string.IsNullOrEmpty(baseVersion) && !string.IsNullOrEmpty(currentVersion))
            {
                var bv = new Version(baseVersion);
                var cv = new Version(currentVersion);

                result = cv.CompareTo(bv);
            }

            return result;
        }

        public static bool CheckGeneratorVersion(string baseVersion)
        {
            string curVersion = GetGeneratorVersion();
            int curVal = CompareGeneratorVersions(baseVersion, curVersion);
            bool result = curVal >= 0 ? true : false;
            return result;
        }

    }
}
