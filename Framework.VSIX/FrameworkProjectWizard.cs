using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;
using System.IO;
using System.Text;
using System.Resources;
using Framework.VSIX.Resources;
using System.Reflection;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;
using Microsoft.ApplicationInsights.DataContracts;

namespace Framework.VSIX
{
	public class FrameworkProjectWizard : IWizard
	{
		private string solutionName;
		private bool showWindow;
		private bool skipInstall;
		private bool formCancel;

		private string framework;
		private string componentName;
		private string componentDescription;
		private string solutionDir;
		private string projectDir;
		private string logFile;
		private string commandString;

		TelemetryClient telemetry = new TelemetryClient
		{
			InstrumentationKey = Utility.AppInsightsKey
		};
		Dictionary<string, string> telProps = new Dictionary<string, string>();
		Guid telOpCtx = Guid.NewGuid();


		public void BeforeOpeningFile(ProjectItem projectItem)
		{
		}

		public void ProjectFinishedGenerating(Project project)
		{
			if (!formCancel)
			{
				StringBuilder outputText = new StringBuilder();

				DirectoryInfo projectDirInfo = new DirectoryInfo(projectDir);
				var genDir = projectDirInfo.Parent.FullName;
				using (var proc = new System.Diagnostics.Process())
				{
					proc.StartInfo.WorkingDirectory = genDir; 
					proc.StartInfo.FileName = @"cmd.exe";

					if (showWindow == false)
					{
						proc.StartInfo.Arguments = string.Format(@" /c  {0}", commandString);
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
							string result = reader.ReadToEnd();
							outputText.Append(result);
						}

						using (StreamReader reader = proc.StandardError)
						{
							string result = reader.ReadToEnd();
							outputText.Append(result);
						}
					}
					else
					{
						proc.StartInfo.Arguments = string.Format(@" /k  {0}", commandString);
						proc.Start();
					}

					proc.WaitForExit();

					using (StreamWriter sw = File.AppendText(logFile))
					{
						sw.Write(outputText);
					}
				}

				telemetry.TrackEvent("project-wizard", telProps, null);
				// Allow some time for flushing before shutdown.
				telemetry.Flush();
				System.Threading.Thread.Sleep(1000);

				string[] files = Directory.GetFiles(projectDir, "*.*", SearchOption.AllDirectories);

				foreach (string file in files)
				{
					if (!file.ToLower().Contains("node_modules") && 
						  !file.ToLower().Contains(@"\bin\") && 
							!file.ToLower().Contains(@"\obj\") && 
							!file.ToLower().Contains(@"\properties\"))
					{
						try
						{
							project.ProjectItems.AddFromFile(file);
						}
						catch { }
					}
				}
			}
		}

		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
		}

		public void RunFinished()
		{

		}

		public void RunStarted(object automationObject,
				Dictionary<string, string> replacementsDictionary,
				WizardRunKind runKind, object[] customParams)
		{
#if DEBUG
			TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
#endif

			try
			{
				telProps.Add("generator-version", Utility.InstalledGeneratorVersion.ToString());
				telemetry.Context.Operation.Id = telOpCtx.ToString();

				if (Utility.InstalledGeneratorVersion == new Version(0,0))
				{
					throw new Exception("Yeoman generator not found");
				}

				var projectForm = new NewProjectForm()
				{
					SolutionName = replacementsDictionary["$safeprojectname$"],
					GeneratorVersion = Utility.InstalledGeneratorVersion
				};
				projectForm.Initialize();
				projectForm.ShowDialog();

				formCancel = projectForm.FormCancel;

				if (!formCancel)
				{
					solutionName = projectForm.SolutionName;
					framework = projectForm.Framework;
					componentName = projectForm.ComponentName;
					componentDescription = projectForm.ComponentDescription;
					commandString = projectForm.CommandString;
					showWindow = projectForm.ShowWindow;
					skipInstall = projectForm.SkipInstall;

					telProps.Add("framework", framework);
					telProps.Add("skipInstall", skipInstall.ToString());
					telProps.Add("environment", projectForm.Environment);
					telProps.Add("componentType", projectForm.ComponentType);
					telProps.Add("extensionType", projectForm.ExtensionType);

					try
					{
						replacementsDictionary.Remove("$SolutionName$");
						replacementsDictionary.Remove("$Framework$");
						replacementsDictionary.Remove("$ComponentName$");
						replacementsDictionary.Remove("$ComponentDescription$");
						replacementsDictionary.Remove("$CommandString$");
					}
					catch { }

					replacementsDictionary.Add("$SolutionName$", solutionName);
					replacementsDictionary.Add("$Framework$", framework);
					replacementsDictionary.Add("$ComponentName$", componentName);
					replacementsDictionary.Add("$ComponentDescription$", componentDescription);
					replacementsDictionary.Add("$CommandString$", commandString);

					solutionDir = System.IO.Path.GetDirectoryName(replacementsDictionary["$destinationdirectory$"]);
					projectDir = String.Format(@"{0}\{1}", solutionDir, replacementsDictionary["$safeprojectname$"]);
					projectDir = String.Format(@"{0}\{1}", solutionDir, solutionName);

					logFile = String.Format(@"{0}\generator.log", projectDir);
				}
			}
			catch (Exception ex)
			{
				MessageBox.Show(ex.ToString());
			}
		}

		public bool ShouldAddProjectItem(string filePath)
		{
			return true;
		}
	}
}
