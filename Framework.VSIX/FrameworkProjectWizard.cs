using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;
using System.IO;
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
        private Project currentProject;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            commandString = commandString.Replace("$SolutionName$", solutionName);
            commandString = commandString.Replace("$Framework$", framework);
            commandString = commandString.Replace("$ComponentName$", componentName);
            commandString = commandString.Replace("$ComponentDescription$", componentDescription);

            using (var proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.WorkingDirectory = solutionDir;
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

                    using (StreamWriter sw = File.AppendText(logFile))
                    {
                        var s = string.Empty;
                        while ((s = proc.StandardOutput.ReadLine()) != null)
                        {
                            sw.WriteLine(s);
                        }
                    }

                    proc.StandardInput.Flush();
                    proc.StandardInput.WriteLine("exit");
                    proc.StandardInput.Flush();
                }
                else
                {
                    proc.StartInfo.Arguments = string.Format(@" /k  {0}", commandString);
                    proc.Start();
                }

                proc.WaitForExit();
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
                inputForm.ShowDialog();

                solutionName = UserInputForm.SolutionName;
                framework = UserInputForm.Framework;
                componentName = UserInputForm.ComponentName;
                componentDescription = UserInputForm.ComponentDescription;
                commandString = UserInputForm.CommandString;
                showWindow = UserInputForm.ShowWindow;

                try
                {
                    replacementsDictionary.Remove("$SolutionName$");
                    replacementsDictionary.Remove("$Framework$");
                    replacementsDictionary.Remove("$ComponentName$");
                    replacementsDictionary.Remove("$ComponentDescription$");
                    replacementsDictionary.Remove("$CommandString$");
                    replacementsDictionary.Remove("$ShowWindow$");
                }
                catch { }

                replacementsDictionary.Add("$SolutionName$", solutionName);
                replacementsDictionary.Add("$Framework$", framework);
                replacementsDictionary.Add("$ComponentName$", componentName);
                replacementsDictionary.Add("$ComponentDescription$", componentDescription);
                replacementsDictionary.Add("$CommandString$", commandString);
                replacementsDictionary.Add("$ShowWindow$", showWindow);

                solutionDir = System.IO.Path.GetDirectoryName(replacementsDictionary["$destinationdirectory$"]);
                projectDir = String.Format(@"{0}\{1}", solutionDir, replacementsDictionary["$safeprojectname$"]);
                logFile = String.Format(@"{0}\generator.log", projectDir);
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
        private TextBox _solutionName;
        private ComboBox _framework;
        private TextBox _componentName;
        private TextBox _componentDescription;
        private TextBox _commandString;
        private Button button1;
        private Button button2;
        private CheckBox _showWindow;
        ResourceManager rm = new ResourceManager("Framework.VSIX.Resources.Global", Assembly.GetExecutingAssembly());


        public UserInputForm()
        {
            this.Size = new System.Drawing.Size(600, 450);
            this.Name = Global.Form_Project_Name;
            this.Text = Global.Form_Project_Title;
            this.Icon = new System.Drawing.Icon("Framework.VSIX.ico");

            TabControl tabCtrl = new TabControl();
            tabCtrl.Name = "ConfigTabControl";
            tabCtrl.Width = 600;
            tabCtrl.Height = 350;

            TabPage tabProps = new TabPage();
            tabProps.Name = "ConfigTabPageProps";
            tabProps.Text = Global.Form_PropertyTab_Title;
            tabProps.Height = 350;
            tabProps.Width = 600;
            tabProps.Click += TabProps_Click;

            Label _label1 = new Label();
            _label1.Location = new System.Drawing.Point(10, 40);
            _label1.Size = new System.Drawing.Size(200, 20);
            _label1.Text = Global.Form_SolutionName;
            tabProps.Controls.Add(_label1);

            _solutionName = new TextBox();
            _solutionName.Location = new System.Drawing.Point(10, 60);
            _solutionName.Size = new System.Drawing.Size(400, 20);
            _solutionName.TextChanged += _solutionName_TextChanged;
            tabProps.Controls.Add(_solutionName);

            Label _label2 = new Label();
            _label2.Location = new System.Drawing.Point(10, 90);
            _label2.Size = new System.Drawing.Size(200, 20);
            _label2.Text = Global.Form_Framework;
            tabProps.Controls.Add(_label2);

            _framework = new ComboBox();
            _framework.Location = new System.Drawing.Point(10, 110);
            _framework.Size = new System.Drawing.Size(150, 20);

            Dictionary<string, string> comboSource = new Dictionary<string, string>();
            comboSource.Add("none", "none");
            comboSource.Add("react", "react");
            comboSource.Add("knockout", "knockout");

            _framework.DataSource = new BindingSource(comboSource, null);
            _framework.DisplayMember = "Value";
            _framework.ValueMember = "Key";
            _framework.SelectedIndexChanged += _framework_SelectedIndexChanged;

            tabProps.Controls.Add(_framework);

            Label _label3 = new Label();
            _label3.Location = new System.Drawing.Point(10, 140);
            _label3.Size = new System.Drawing.Size(400, 20);
            _label3.Text = Global.Form_ComponentName;
            tabProps.Controls.Add(_label3);

            _componentName = new TextBox();
            _componentName.Location = new System.Drawing.Point(10, 160);
            _componentName.Size = new System.Drawing.Size(400, 20);
            _componentName.TextChanged += _componentName_TextChanged;
            tabProps.Controls.Add(_componentName);

            Label _label4 = new Label();
            _label4.Location = new System.Drawing.Point(10, 190);
            _label4.Size = new System.Drawing.Size(200, 20);
            _label4.Text = Global.Form_ComponentDescription;
            tabProps.Controls.Add(_label4);

            _componentDescription = new TextBox();
            _componentDescription.Location = new System.Drawing.Point(10, 210);
            _componentDescription.Size = new System.Drawing.Size(400, 20);
            _componentDescription.TextChanged += _componentDescription_TextChanged;
            tabProps.Controls.Add(_componentDescription);

            tabCtrl.TabPages.Add(tabProps);

            TabPage tabAdv = new TabPage();
            tabAdv.Name = "ConfigTabPageAdvanced";
            tabAdv.Text = Global.Form_AdvancedTab_Title;
            tabAdv.Height = 350;
            tabAdv.Width = 600;
            tabAdv.Click += TabAdv_Click;

            Label _label6 = new Label();
            _label6.Location = new System.Drawing.Point(10, 40);
            _label6.Size = new System.Drawing.Size(200, 20);
            _label6.Text = Global.Form_CommandString;
            tabAdv.Controls.Add(_label6);

            _commandString = new TextBox();
            _commandString.Location = new System.Drawing.Point(10, 60);
            _commandString.Size = new System.Drawing.Size(560, 200);
            _commandString.Multiline = true;
            _commandString.ScrollBars = ScrollBars.Vertical;
            _commandString.Text = string.Format(Global.Yeoman_Project_DefaultCommandString, solutionName, "none", componentName, componentDescription);
            tabAdv.Controls.Add(_commandString);

            Label _label7 = new Label();
            _label7.Location = new System.Drawing.Point(10, 265);
            _label7.Size = new System.Drawing.Size(500, 40);
            _label7.Text = Global.Form_AdvancedTab_CommandDescription;
            _label7.MaximumSize = new System.Drawing.Size(500, 40);
            _label7.AutoSize = true;
            tabAdv.Controls.Add(_label7);

            _showWindow = new CheckBox();
            _showWindow.Location = new System.Drawing.Point(10, 290);
            _showWindow.Checked = false;
            _showWindow.Text = Global.Form_ShowCommandWIndow;
            _showWindow.AutoSize = true;
            tabAdv.Controls.Add(_showWindow);

            tabCtrl.TabPages.Add(tabAdv);

            button1 = new Button();
            button1.Location = new System.Drawing.Point(10, 360);
            button1.Size = new System.Drawing.Size(100, 25);
            button1.Text = Global.Form_ButtonGenerate;
            button1.Click += button1_Click;
            button1.Enabled = false;
            this.Controls.Add(button1);

            button2 = new Button();
            button2.Location = new System.Drawing.Point(475, 360);
            button2.Size = new System.Drawing.Size(100, 25);
            button2.Text = Global.Form_ButtonCancel;
            button2.Click += Button2_Click;
            this.Controls.Add(button2);

            Label _label5 = new Label();
            _label5.Location = new System.Drawing.Point(10, 390);
            _label5.Size = new System.Drawing.Size(500, 20);
            _label5.Text = Global.Form_Footer_GeneratorText;
            this.Controls.Add(_label5);

            this.Controls.Add(tabCtrl);

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            this.Close();
            Application.Exit();
        }

        private void _componentDescription_TextChanged(object sender, EventArgs e)
        {
            componentDescription = _componentDescription.Text;
            _commandString.Text = string.Format(Global.Yeoman_Project_CommandString, solutionName, framework, componentName, componentDescription);
            SetSubmitState();
        }

        private void _componentName_TextChanged(object sender, EventArgs e)
        {
            componentName = _componentName.Text;
            _commandString.Text = string.Format(Global.Yeoman_Project_CommandString, solutionName, framework, componentName, componentDescription);
            SetSubmitState();
        }

        private void _framework_SelectedIndexChanged(object sender, EventArgs e)
        {
            framework = ((KeyValuePair<string, string>)_framework.SelectedItem).Key;
            _commandString.Text = string.Format(Global.Yeoman_Project_CommandString, solutionName, framework, componentName, componentDescription);
            SetSubmitState();
        }

        private void _solutionName_TextChanged(object sender, EventArgs e)
        {
            solutionName = _solutionName.Text;
            _commandString.Text = string.Format(Global.Yeoman_Project_CommandString, solutionName, framework, componentName, componentDescription);
            SetSubmitState();
        }

        private void TabAdv_Click(object sender, EventArgs e)
        {

        }

        private void TabProps_Click(object sender, EventArgs e)
        {

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

        protected void button1_Click(object sender, EventArgs e)
        {
            solutionName = _solutionName.Text;
            componentName = _componentName.Text;
            componentDescription = _componentDescription.Text;
            framework = ((KeyValuePair<string, string>)_framework.SelectedItem).Key;
            commandString = _commandString.Text;
            showWindow = _showWindow.Checked == true ? "true" : "false";
            this.Close();
        }

        protected void SetSubmitState()
        {
            if (!string.IsNullOrWhiteSpace(_solutionName.Text) && !string.IsNullOrWhiteSpace(_componentName.Text) && !string.IsNullOrWhiteSpace(_componentDescription.Text))
                button1.Enabled = true;
        }
    }
}
