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


namespace Framework.VSIX
{
    public class FrameworkProjectWizard : IWizard
    {
        private UserInputForm inputForm;
        private string solutionName;
        private string framework;
        private string componentName;
        private string componentDescription;
        private string solutionDir;
        private string projectDir;
        private string logFile;
        private string commandString;
        private string showWindow;
        private string skipInstall;
        private bool formCancel;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (!formCancel)
            {
                commandString = commandString.Replace("$SolutionName$", solutionName);
                commandString = commandString.Replace("$Framework$", framework);
                commandString = commandString.Replace("$ComponentName$", componentName);
                commandString = commandString.Replace("$ComponentDescription$", componentDescription);

                StringBuilder outputText = new StringBuilder();

                using (var proc = new System.Diagnostics.Process())
                {
                    proc.StartInfo.WorkingDirectory = projectDir;
                    proc.StartInfo.FileName = @"cmd.exe";

                    if (showWindow == "false")
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

                string[] files = Directory.GetFiles(projectDir, "*.*", SearchOption.AllDirectories);

                foreach (string file in files)
                {
                    if (!file.ToLower().Contains("node_modules") && !file.ToLower().Contains(@"\bin\") && !file.ToLower().Contains(@"\obj\") && !file.ToLower().Contains(@"\properties\"))
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

        public void ProjectItemFinishedGenerating(ProjectItem
            projectItem)
        {
        }

        public void RunFinished()
        {

        }

        public void RunStarted(object automationObject,
            Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            try
            {
                inputForm = new UserInputForm();
                inputForm.Controls["ConfigTabControl"].Controls["ConfigTabPageProps"].Controls["spfxSolutionName"].Text = replacementsDictionary["$safeprojectname$"];
                inputForm.ShowDialog();
                formCancel = UserInputForm.FormCancel;

                if (!formCancel)
                {
                    solutionName = UserInputForm.SolutionName;
                    framework = UserInputForm.Framework;
                    componentName = UserInputForm.ComponentName;
                    componentDescription = UserInputForm.ComponentDescription;
                    commandString = UserInputForm.CommandString;
                    showWindow = UserInputForm.ShowWindow;
                    skipInstall = UserInputForm.SkipInstall;

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

    public partial class UserInputForm : Form
    {
        private static string solutionName;
        private static string framework = "none";
        private static string componentName;
        private static string componentDescription;
        private static string commandString;
        private static string showWindow;
        private static string skipInstall;
        private static string skipInstallCommand = string.Empty;
        private static bool formCancel = false;
        private string _command = string.Empty;
        private TextBox _solutionName;
        private ComboBox _framework;
        private TextBox _componentName;
        private TextBox _componentDescription;
        private TextBox _commandString;
        private Button button1;
        private Button button2;
        private CheckBox _showWindow;
        private CheckBox _skipInstall;
        ResourceManager rm = new ResourceManager("Framework.VSIX.Resources.Global", Assembly.GetExecutingAssembly());


        public UserInputForm()
        {
            bool genVersion = Utility.CheckGeneratorVersion(Global.Yeoman_Generator_ExtensionsVersion);
            _command = genVersion == true ? Global.Yeoman_Project_CommandString_Extensions : Global.Yeoman_Project_CommandString;

            this.Size = new System.Drawing.Size(600, 450);
            this.Name = Global.Form_Project_Name;
            this.Text = Global.Form_Project_Title;
            this.Icon = Global.Extension;

            TabControl tabCtrl = new TabControl {
                Name = "ConfigTabControl",
                Width = 600,
                Height = 350
            };

            TabPage tabProps = new TabPage {
                Name = "ConfigTabPageProps",
                Text = Global.Form_PropertyTab_Title,
                Height = 350,
                Width = 600
            };
            tabProps.Click += TabProps_Click;

            Label _label1 = new Label {
                Location = new System.Drawing.Point(10, 20),
                Size = new System.Drawing.Size(200, 20),
                Text = Global.Form_SolutionName
            };
            tabProps.Controls.Add(_label1);

            _solutionName = new TextBox {
                Name = "spfxSolutionName",
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(400, 20),
                
                Enabled = false
            };
            _solutionName.TextChanged += _solutionName_TextChanged;
            tabProps.Controls.Add(_solutionName);

            Label _label2 = new Label
            {
                Location = new System.Drawing.Point(10, 70),
                Size = new System.Drawing.Size(200, 20),
                Text = Global.Form_Framework
            };
            tabProps.Controls.Add(_label2);

            _framework = new ComboBox
            {
                Location = new System.Drawing.Point(10, 90),
                Size = new System.Drawing.Size(150, 20)
            };

            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            comboSource.Add("none", "none");
            comboSource.Add("react", "react");
            comboSource.Add("knockout", "knockout");

            _framework.DataSource = new BindingSource(comboSource, null);
            _framework.DisplayMember = "Value";
            _framework.ValueMember = "Key";
            _framework.SelectedIndexChanged += _framework_SelectedIndexChanged;

            tabProps.Controls.Add(_framework);

            Label _label3 = new Label
            {
                Location = new System.Drawing.Point(10, 120),
                Size = new System.Drawing.Size(400, 20),
                Text = Global.Form_ComponentName
            };
            tabProps.Controls.Add(_label3);

            _componentName = new TextBox
            {
                Location = new System.Drawing.Point(10, 140),
                Size = new System.Drawing.Size(400, 20)
            };
            _componentName.TextChanged += _componentName_TextChanged;
            tabProps.Controls.Add(_componentName);

            Label _label4 = new Label
            {
                Location = new System.Drawing.Point(10, 170),
                Size = new System.Drawing.Size(200, 20),
                Text = Global.Form_ComponentDescription
            };
            tabProps.Controls.Add(_label4);

            _componentDescription = new TextBox
            {
                Location = new System.Drawing.Point(10, 190),
                Size = new System.Drawing.Size(400, 20)
            };
            _componentDescription.TextChanged += _componentDescription_TextChanged;
            tabProps.Controls.Add(_componentDescription);

            _skipInstall = new CheckBox
            {
                Location = new System.Drawing.Point(10, 230),
                Checked = false,
                Text = Global.Form_SkipInstall,
                AutoSize = true
            };
            _skipInstall.CheckedChanged += _skipInstall_CheckedChanged;
            tabProps.Controls.Add(_skipInstall);

            tabCtrl.TabPages.Add(tabProps);

            TabPage tabAdv = new TabPage
            {
                Name = "ConfigTabPageAdvanced",
                Text = Global.Form_AdvancedTab_Title,
                Height = 350,
                Width = 600
            };
            tabAdv.Click += TabAdv_Click;

            Label _label6 = new Label
            {
                Location = new System.Drawing.Point(10, 20),
                Size = new System.Drawing.Size(200, 20),
                Text = Global.Form_CommandString
            };
            tabAdv.Controls.Add(_label6);

            _commandString = new TextBox
            {
                Location = new System.Drawing.Point(10, 40),
                Size = new System.Drawing.Size(560, 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = string.Format(_command, solutionName, "none", componentName, componentDescription, skipInstallCommand)
            };
            tabAdv.Controls.Add(_commandString);

            Label _label7 = new Label
            {
                Location = new System.Drawing.Point(10, 245),
                Size = new System.Drawing.Size(500, 40),
                Text = Global.Form_AdvancedTab_CommandDescription,
                MaximumSize = new System.Drawing.Size(500, 40),
                AutoSize = true
            };
            tabAdv.Controls.Add(_label7);

            _showWindow = new CheckBox
            {
                Location = new System.Drawing.Point(10, 270),
                Checked = false,
                Text = Global.Form_ShowCommandWIndow,
                AutoSize = true
            };
            tabAdv.Controls.Add(_showWindow);

            tabCtrl.TabPages.Add(tabAdv);

            button1 = new Button
            {
                Location = new System.Drawing.Point(10, 360),
                Size = new System.Drawing.Size(100, 25),
                Text = Global.Form_ButtonGenerate,
                Enabled = false
            };
            button1.Click += button1_Click;
            this.Controls.Add(button1);

            button2 = new Button {
                Location = new System.Drawing.Point(475, 360),
                Size = new System.Drawing.Size(100, 25),
                Text = Global.Form_ButtonCancel
            };
            button2.Click += Button2_Click;
            this.Controls.Add(button2);

            Label _label5 = new Label
            {
                Location = new System.Drawing.Point(10, 390),
                Size = new System.Drawing.Size(500, 20),
                Text = Global.Form_Footer_GeneratorText,
            };
            this.Controls.Add(_label5);

            this.Controls.Add(tabCtrl);

        }

        private void _skipInstall_CheckedChanged(object sender, EventArgs e)
        {
            skipInstallCommand = this._skipInstall.Checked == true ? Global.Form_SkipInstall_Flag : string.Empty;
            SetCommandText();
            SetSubmitState();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            formCancel = true;
            this.Close();
        }

        private void _componentDescription_TextChanged(object sender, EventArgs e)
        {
            componentDescription = _componentDescription.Text;
            SetCommandText();
            SetSubmitState();
        }

        private void _componentName_TextChanged(object sender, EventArgs e)
        {
            componentName = _componentName.Text;
            SetCommandText();
            SetSubmitState();
        }

        private void _framework_SelectedIndexChanged(object sender, EventArgs e)
        {
            framework = ((KeyValuePair<string, string>)_framework.SelectedItem).Key;
            SetCommandText();
            SetSubmitState();
        }

        private void _solutionName_TextChanged(object sender, EventArgs e)
        {
            solutionName = _solutionName.Text;
            SetCommandText();
            SetSubmitState();
        }

        private void TabAdv_Click(object sender, EventArgs e)
        {

        }

        private void TabProps_Click(object sender, EventArgs e)
        {

        }

        private void SetCommandText()
        {
            _commandString.Text = string.Format(_command, solutionName, framework, componentName, componentDescription, skipInstallCommand);
        }

        public static string SolutionName
        {
            get
            {
                return solutionName;
            }
            set
            {
                solutionName = value;
            }
        }

        public static string Framework
        {
            get
            {
                return framework;
            }
            set
            {
                framework = value;
            }
        }

        public static string ComponentName
        {
            get
            {
                return componentName;
            }
            set
            {
                componentName = value;
            }
        }

        public static string ComponentDescription
        {
            get
            {
                return componentDescription;
            }
            set
            {
                componentDescription = value;
            }
        }

        public static string CommandString
        {
            get { return commandString; }
            set { commandString = value; }
        }

        public static string ShowWindow
        {
            get { return showWindow; }
            set { showWindow = value; }
        }

        public static string SkipInstall
        {
            get { return skipInstall; }
            set { skipInstall = value; }
        }

        public static bool FormCancel
        {
            get { return formCancel;  }
            set { formCancel = value; }
        }

        protected void button1_Click(object sender, EventArgs e)
        {
            solutionName = _solutionName.Text;
            componentName = _componentName.Text;
            componentDescription = _componentDescription.Text;
            framework = ((KeyValuePair<string, string>)_framework.SelectedItem).Key;
            commandString = _commandString.Text;
            showWindow = _showWindow.Checked == true ? "true" : "false";
            skipInstall = _skipInstall.Checked == true ? "true" : "false";
            this.Close();
        }

        protected void SetSubmitState()
        {
            if (!string.IsNullOrWhiteSpace(_solutionName.Text) && !string.IsNullOrWhiteSpace(_componentName.Text) && !string.IsNullOrWhiteSpace(_componentDescription.Text))
                button1.Enabled = true;
        }
    }
}
