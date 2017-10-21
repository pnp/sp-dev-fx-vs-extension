using Framework.VSIX.Resources;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.IO;
using System.Text;

namespace Framework.VSIX
{
	public static class Utility
	{
		#region Generator Version

		public static Version gv1_0 = new Version(1, 0);
		public static Version gv1_1 = new Version(1, 1);
		public static Version gv1_3 = new Version(1, 3);
		private static Version installedGeneratorVersion = null;
		public static Version InstalledGeneratorVersion
		{
			get
			{
				if (installedGeneratorVersion == null)
				{
					installedGeneratorVersion = GetGeneratorVersion();
				}
				return installedGeneratorVersion;
			}
		}

		private static Version GetGeneratorVersion()
		{
			Version result = new Version(0, 0);
			string versionString = string.Empty;

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

					JObject packageJson = JObject.Parse(outputText.ToString());
					JToken version = packageJson["dependencies"]["@microsoft/generator-sharepoint"]["version"];
					versionString = version.Value<string>();

					if (!String.IsNullOrEmpty(versionString))
					{
						result = new Version(versionString);
					}
				}
			}
			catch (System.Exception ex)
			{
				//TODO: Log exception
			}

			return result;
		}

		#endregion

		#region Set Command line

		public static bool SetItemCommand(string Framework, string ComponentName,
																			string ComponentDescription, string ComponentType,
																			string ExtensionType, out string command)
		{
			command = String.Empty;
			bool result = true;

			if (String.IsNullOrEmpty(ComponentName))
				result = false;
			if (String.IsNullOrEmpty(ComponentDescription))
				result = false;
			if (String.IsNullOrEmpty(ComponentType))
				result = false;
			if (ComponentType == "webpart" && String.IsNullOrEmpty(Framework))
				result = false;
			if (ComponentType == "extension" && String.IsNullOrEmpty(ExtensionType))
				result = false;
			if (ComponentType == "extension" && ExtensionType == "FieldCustomizer" && String.IsNullOrEmpty(Framework))
				result = false;

			command = SetCommand(null, Framework, ComponentName, ComponentDescription,
													 ComponentType, ExtensionType, null, false, false);
			return result;
		}

		public static bool SetProjectCommand(string SolutionName, string Framework, string ComponentName,
																				 string ComponentDescription, string ComponentType, string ExtensionType,
																				 string Environment, bool SkipFeatureDeployment, bool SkipInstall,
																				 out string command)
		{
			command = String.Empty;
			bool result = true;

			if (String.IsNullOrEmpty(SolutionName))
				result = false;
			if (String.IsNullOrEmpty(Framework))
				result = false;
			if (String.IsNullOrEmpty(ComponentName))
				result = false;
			if (String.IsNullOrEmpty(ComponentDescription))
				result = false;
			if (String.IsNullOrEmpty(ComponentType))
				result = false;
			if (ComponentType == "extension" && String.IsNullOrEmpty(ExtensionType))
				result = false;
			if (Utility.InstalledGeneratorVersion >= gv1_3 && String.IsNullOrEmpty(Environment))
				result = false;

			command = SetCommand(SolutionName, Framework, ComponentName, ComponentDescription, 
													 ComponentType, ExtensionType, Environment, SkipFeatureDeployment, SkipInstall);
			return result;
		}

			private static string SetCommand(string SolutionName, string Framework, string ComponentName,
																			 string ComponentDescription, string ComponentType, string ExtensionType,
																			 string Environment, bool SkipFeatureDeployment, bool SkipInstall)
			{ 
			StringBuilder commandBuilder = new StringBuilder();
			commandBuilder.Append($"yo @microsoft/sharepoint");

			if (!String.IsNullOrEmpty(SolutionName))
				commandBuilder.Append($" --solutionName \"{SolutionName}\"");

			if (!String.IsNullOrEmpty(ComponentName))
				commandBuilder.Append($" --componentName \"{ComponentName}\"");

			if (!String.IsNullOrEmpty(ComponentDescription))
				commandBuilder.Append($" --componentDescription \"{ComponentDescription}\"");

			if (!String.IsNullOrEmpty(ComponentType))
				commandBuilder.Append($" --componentType \"{ComponentType}\"");

			if (!String.IsNullOrEmpty(Framework))
				commandBuilder.Append($" --framework \"{Framework}\"");

			if (SkipInstall)
				commandBuilder.Append(" --skip-install");

			if (Utility.InstalledGeneratorVersion >= gv1_3)
			{
				if (!String.IsNullOrEmpty(Environment))
					commandBuilder.Append($" --environment \"{Environment}\"");
			}

			if (Utility.InstalledGeneratorVersion >= gv1_1)
			{
				if (ComponentType=="extension")
				{
					if (!String.IsNullOrEmpty(ExtensionType))
						commandBuilder.Append($" --extensionType \"{ExtensionType}\"");
				}

				commandBuilder.AppendFormat(" skipFeatureDeployment {0}", SkipFeatureDeployment ? "true" : "false");
			}

			return commandBuilder.ToString();
		}
		#endregion

	}
}
