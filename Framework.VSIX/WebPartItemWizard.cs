using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using System.Drawing;
using Microsoft.CSharp;
using EnvDTE;
using System.IO;
using System.Text;
using System.Globalization;
using System.Threading;
using System.Resources;
using System.Reflection;
using Framework.VSIX.Resources;
using Microsoft.ApplicationInsights;
using Microsoft.ApplicationInsights.Extensibility;

namespace Framework.VSIX
{
	public class WebPartItemWizard : IWizard
	{
		private AddItemForm inputForm;
		private string componentName;
		private string componentDescription;
		private string commandString;
		private bool showWindow;
		private bool formCancel;

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
		}

		public void ProjectItemFinishedGenerating(ProjectItem projectItem)
		{
			if (!formCancel)
			{
				string projectDir = projectItem.ContainingProject.FullName;
				projectDir = projectDir.Substring(0, projectDir.LastIndexOf("\\"));
				string logFile = String.Format(@"{0}\{1}.log", projectDir, componentName);

				StringBuilder outputText = new StringBuilder();

				using (var proc = new System.Diagnostics.Process())
				{
					proc.StartInfo.WorkingDirectory = projectDir;
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

				telemetry.TrackEvent("item-wizard", telProps, null);
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
							projectItem.ContainingProject.ProjectItems.AddFromFile(file);
						}
						catch { }
					}
				}
			}
		}

		public void RunFinished()
		{

		}

		public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
				WizardRunKind runKind, object[] customParams)
		{
#if DEBUG
			TelemetryConfiguration.Active.TelemetryChannel.DeveloperMode = true;
#endif

			try
			{
				telProps.Add("generator-version", Utility.InstalledGeneratorVersion.ToString());
				telemetry.Context.Operation.Id = telOpCtx.ToString();

				if (Utility.InstalledGeneratorVersion == new Version(0, 0))
				{
					throw new Exception("Yeoman generator not found");
				}

				inputForm = new AddItemForm()
				{
					GeneratorVersion = Utility.InstalledGeneratorVersion
				};
				inputForm.Initialize();
				inputForm.ShowDialog();

				formCancel = inputForm.FormCancel;

				if (!formCancel)
				{
					componentName = inputForm.ComponentName;
					componentDescription = inputForm.ComponentDescription;
					commandString = inputForm.CommandString;
					showWindow = inputForm.ShowWindow;

					telProps.Add("framework", inputForm.Framework);
					telProps.Add("componentType", inputForm.ComponentType);
					telProps.Add("extensionType", inputForm.ExtensionType);

					try
					{
						replacementsDictionary.Remove("$ComponentName$");
						replacementsDictionary.Remove("$ComponentDescription$");
						replacementsDictionary.Remove("$CommandString$");
					}
					catch { }

					replacementsDictionary.Add("$ComponentName$", componentName);
					replacementsDictionary.Add("$ComponentDescription$", componentDescription);
					replacementsDictionary.Add("$CommandString$", commandString);
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

	//public partial class WebPartInputForm : Form
	//{
	//    private static string componentName;
	//    private static string componentDescription;
	//    private static string framework = "none";
	//    private static string commandString;
	//    private static string showWindow;
	//    private static bool formCancel = false;
	//    private bool genVersion;
	//    private string _command = string.Empty;
	//    private ComboBox _framework;
	//    private TextBox _componentName;
	//    private TextBox _componentDescription;
	//    private TextBox _commandString;
	//    private Button button1;
	//    private Button button2;
	//    private CheckBox _showWindow;
	//    ResourceManager rm = new ResourceManager("Framework.VSIX.Resources.Global", Assembly.GetExecutingAssembly());


	//    public WebPartInputForm()
	//    {
	//        genVersion = Utility.CheckGeneratorVersion(Global.Yeoman_Generator_ExtensionsVersion);
	//        _command = genVersion == true ? Global.Yeoman_Item_CommandString_Extensions : Global.Yeoman_Item_CommandString;

	//        this.Size = new System.Drawing.Size(600, 450);
	//        this.Name = Global.Form_Item_Name;
	//        this.Text = Global.Form_Item_Title;
	//        this.Icon = Global.Extension;

	//        TabControl tabCtrl = new TabControl
	//        {
	//            Name = "ConfigTabControl",
	//            Width = 600,
	//            Height = 350
	//        };

	//        TabPage tabProps = new TabPage
	//        {
	//            Name = "ConfigTabPageProps",
	//            Text = Global.Form_PropertyTab_Title,
	//            Height = 350,
	//            Width = 600
	//        };
	//        tabProps.Click += TabProps_Click;

	//        Label _label3 = new Label
	//        {
	//            Location = new System.Drawing.Point(10, 20),
	//            Size = new System.Drawing.Size(200, 20),
	//            Text = Global.Form_ComponentName
	//        };
	//        tabProps.Controls.Add(_label3);

	//        _componentName = new TextBox
	//        {
	//            Location = new System.Drawing.Point(10, 40),
	//            Size = new System.Drawing.Size(400, 20)
	//        };
	//        _componentName.TextChanged += _componentName_TextChanged;
	//        tabProps.Controls.Add(_componentName);

	//        Label _label2 = new Label
	//        {
	//            Location = new System.Drawing.Point(10, 70),
	//            Size = new System.Drawing.Size(200, 20),
	//            Text = Global.Form_Framework
	//        };
	//        tabProps.Controls.Add(_label2);

	//        _framework = new ComboBox
	//        {
	//            Location = new System.Drawing.Point(10, 90),
	//            Size = new System.Drawing.Size(150, 20),
	//            Enabled = genVersion == true ? true : false
	//        };

	//        Dictionary<string, string> comboSource = new Dictionary<string, string>();
	//        comboSource.Add("none", "none");
	//        comboSource.Add("react", "react");
	//        comboSource.Add("knockout", "knockout");

	//        _framework.DataSource = new BindingSource(comboSource, null);
	//        _framework.DisplayMember = "Value";
	//        _framework.ValueMember = "Key";
	//        _framework.SelectedIndexChanged += _framework_SelectedIndexChanged;

	//        tabProps.Controls.Add(_framework);

	//        Label _label4 = new Label
	//        {
	//            Location = new System.Drawing.Point(10, 120),
	//            Size = new System.Drawing.Size(200, 20),
	//            Text = Global.Form_ComponentDescription
	//        };
	//        tabProps.Controls.Add(_label4);

	//        _componentDescription = new TextBox
	//        {
	//            Location = new System.Drawing.Point(10, 140),
	//            Size = new System.Drawing.Size(400, 20)
	//        };
	//        _componentDescription.TextChanged += _componentDescription_TextChanged;
	//        tabProps.Controls.Add(_componentDescription);

	//        tabCtrl.Controls.Add(tabProps);

	//        TabPage tabAdv = new TabPage
	//        {
	//            Name = "ConfigTabPageAdvanced",
	//            Text = Global.Form_AdvancedTab_Title,
	//            Height = 350,
	//            Width = 600
	//        };
	//        tabAdv.Click += TabAdv_Click;

	//        Label _label6 = new Label
	//        {
	//            Location = new System.Drawing.Point(10, 20),
	//            Size = new System.Drawing.Size(200, 20),
	//            Text = Global.Form_CommandString
	//        };
	//        tabAdv.Controls.Add(_label6);

	//        _commandString = new TextBox
	//        {
	//            Location = new System.Drawing.Point(10, 40),
	//            Size = new System.Drawing.Size(560, 200),
	//            Multiline = true,
	//            ScrollBars = ScrollBars.Vertical,
	//            Text = genVersion == true ? string.Format(_command, componentName, componentDescription, framework) : string.Format(_command, componentName, componentDescription)
	//        };
	//        tabAdv.Controls.Add(_commandString);

	//        Label _label7 = new Label
	//        {
	//            Location = new System.Drawing.Point(10, 245),
	//            Size = new System.Drawing.Size(500, 40),
	//            Text = Global.Form_AdvancedTab_CommandDescription,
	//            MaximumSize = new System.Drawing.Size(500, 40),
	//            AutoSize = true
	//        };
	//        tabAdv.Controls.Add(_label7);

	//        _showWindow = new CheckBox
	//        {
	//            Location = new System.Drawing.Point(10, 270),
	//            Checked = false,
	//            Text = Global.Form_ShowCommandWindow,
	//            AutoSize = true
	//        };
	//        tabAdv.Controls.Add(_showWindow);

	//        tabCtrl.TabPages.Add(tabAdv);

	//        this.Controls.Add(tabCtrl);

	//        button1 = new Button
	//        {
	//            Location = new System.Drawing.Point(10, 360),
	//            Size = new System.Drawing.Size(100, 25),
	//            Text = Global.Form_ButtonGenerate,
	//            Enabled = false
	//        };
	//        button1.Click += button1_Click;

	//        this.Controls.Add(button1);

	//        button2 = new Button
	//        {
	//            Location = new System.Drawing.Point(475, 360),
	//            Size = new System.Drawing.Size(100, 25),
	//            Text = Global.Form_ButtonCancel
	//        };
	//        button2.Click += Button2_Click;
	//        this.Controls.Add(button2);

	//        Label _label5 = new Label
	//        {
	//            Location = new System.Drawing.Point(10, 390),
	//            Size = new System.Drawing.Size(500, 20),
	//            Text = Global.Form_Footer_GeneratorText
	//        };
	//        this.Controls.Add(_label5);

	//    }

	//    private void SetCommandText()
	//    {
	//        _commandString.Text = genVersion == true ? string.Format(_command, componentName, componentDescription, framework) : string.Format(_command, componentName, componentDescription);
	//    }

	//    private void Button2_Click(object sender, EventArgs e)
	//    {
	//        formCancel = true;
	//        this.Close();
	//    }

	//    private void TabAdv_Click(object sender, EventArgs e)
	//    {

	//    }

	//    private void _framework_SelectedIndexChanged(object sender, EventArgs e)
	//    {
	//        framework = ((KeyValuePair<string, string>)_framework.SelectedItem).Key;
	//        SetCommandText();
	//        SetSubmitState();
	//    }

	//    private void _componentDescription_TextChanged(object sender, EventArgs e)
	//    {
	//        componentDescription = _componentDescription.Text;
	//        SetCommandText();
	//        SetSubmitState();
	//    }

	//    private void _componentName_TextChanged(object sender, EventArgs e)
	//    {
	//        componentName = _componentName.Text;
	//        SetCommandText();
	//        SetSubmitState();
	//    }

	//    private void TabProps_Click(object sender, EventArgs e)
	//    {

	//    }

	//    public static string Framework
	//    {
	//        get
	//        {
	//            return framework;
	//        }
	//        set
	//        {
	//            framework = value;
	//        }
	//    }

	//    public static string ComponentName
	//    {
	//        get
	//        {
	//            return componentName;
	//        }
	//        set
	//        {
	//            componentName = value;
	//        }
	//    }

	//    public static string ComponentDescription
	//    {
	//        get
	//        {
	//            return componentDescription;
	//        }
	//        set
	//        {
	//            componentDescription = value;
	//        }
	//    }

	//    public static string CommandString
	//    {
	//        get { return commandString; }
	//        set { commandString = value; }
	//    }

	//    public static string ShowWindow
	//    {
	//        get { return showWindow; }
	//        set { showWindow = value; }
	//    }

	//    public static bool FormCancel
	//    {
	//        get { return formCancel; }
	//        set { formCancel = value; }
	//    }

	//    protected void button1_Click(object sender, EventArgs e)
	//    {
	//        componentName = _componentName.Text;
	//        framework = ((KeyValuePair<string, string>)_framework.SelectedItem).Key;
	//        componentDescription = _componentDescription.Text;
	//        commandString = _commandString.Text;
	//        showWindow = _showWindow.Checked == true ? "true" : "false";
	//        this.Close();
	//    }

	//    protected void SetSubmitState()
	//    {
	//        if (!string.IsNullOrWhiteSpace(_componentName.Text) && !string.IsNullOrWhiteSpace(_componentDescription.Text))
	//            button1.Enabled = true;
	//    }
	//}
}
