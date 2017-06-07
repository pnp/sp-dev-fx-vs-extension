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

namespace Framework.VSIX
{
    public class WebPartItemWizard : IWizard
    {
        private WebPartInputForm inputForm;
        private string componentName;
        private string componentDescription;
        private string commandString;
        private string showWindow;
        private bool formCancel;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {           
        }

        public void ProjectItemFinishedGenerating(ProjectItem
            projectItem)
        {
            if (!formCancel)
            {
                string projectDir = projectItem.ContainingProject.FullName;
                projectDir = projectDir.Substring(0, projectDir.LastIndexOf("\\"));
                string logFile = String.Format(@"{0}\{1}.log", projectDir, componentName);

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
            try
            {
                inputForm = new WebPartInputForm();
                inputForm.ShowDialog();
                formCancel = UserInputForm.FormCancel;

                if (!formCancel)
                {
                    componentName = WebPartInputForm.ComponentName;
                    componentDescription = WebPartInputForm.ComponentDescription;
                    commandString = WebPartInputForm.CommandString;
                    showWindow = WebPartInputForm.ShowWindow;

                    try
                    {
                        replacementsDictionary.Remove("$ComponentName$");
                        replacementsDictionary.Remove("$ComponentDescription$");
                        replacementsDictionary.Remove("$CommandString$");
                        replacementsDictionary.Remove("$ShowWindow$");
                    }
                    catch { }

                    replacementsDictionary.Add("$ComponentName$", componentName);
                    replacementsDictionary.Add("$ComponentDescription$", componentDescription);
                    replacementsDictionary.Add("$CommandString$", commandString);
                    replacementsDictionary.Add("$ShowWindow$", showWindow);
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

    public partial class WebPartInputForm : Form
    {
        private static string componentName;
        private static string componentDescription;
        private static string commandString;
        private static string showWindow;
        private static bool formCancel = false;
        private TextBox _componentName;
        private TextBox _componentDescription;
        private TextBox _commandString;
        private Button button1;
        private Button button2;
        private CheckBox _showWindow;
        ResourceManager rm = new ResourceManager("Framework.VSIX.Resources.Global", Assembly.GetExecutingAssembly());


        public WebPartInputForm()
        {
            this.Size = new System.Drawing.Size(600, 450);
            this.Name = Global.Form_Item_Name;
            this.Text = Global.Form_Item_Title;
            this.Icon = Global.Extension;

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

            Label _label3 = new Label();
            _label3.Location = new System.Drawing.Point(10, 20);
            _label3.Size = new System.Drawing.Size(200, 20);
            _label3.Text = Global.Form_ComponentName;
            tabProps.Controls.Add(_label3);

            _componentName = new TextBox();
            _componentName.Location = new System.Drawing.Point(10, 40);
            _componentName.Size = new System.Drawing.Size(400, 20);
            _componentName.TextChanged += _componentName_TextChanged;
            tabProps.Controls.Add(_componentName);

            Label _label4 = new Label();
            _label4.Location = new System.Drawing.Point(10, 70);
            _label4.Size = new System.Drawing.Size(200, 20);
            _label4.Text = Global.Form_ComponentDescription;
            tabProps.Controls.Add(_label4);

            _componentDescription = new TextBox();
            _componentDescription.Location = new System.Drawing.Point(10, 90);
            _componentDescription.Size = new System.Drawing.Size(400, 20);
            _componentDescription.TextChanged += _componentDescription_TextChanged;
            tabProps.Controls.Add(_componentDescription);

            tabCtrl.Controls.Add(tabProps);

            TabPage tabAdv = new TabPage();
            tabAdv.Name = "ConfigTabPageAdvanced";
            tabAdv.Text = Global.Form_AdvancedTab_Title;
            tabAdv.Height = 350;
            tabAdv.Width = 600;
            tabAdv.Click += TabAdv_Click;

            Label _label6 = new Label();
            _label6.Location = new System.Drawing.Point(10, 20);
            _label6.Size = new System.Drawing.Size(200, 20);
            _label6.Text = Global.Form_CommandString;
            tabAdv.Controls.Add(_label6);

            _commandString = new TextBox();
            _commandString.Location = new System.Drawing.Point(10, 40);
            _commandString.Size = new System.Drawing.Size(560, 200);
            _commandString.Multiline = true;
            _commandString.ScrollBars = ScrollBars.Vertical;
            _commandString.Text = string.Format(Global.Yeoman_Item_CommandString, componentName, componentDescription, "webpart");
            tabAdv.Controls.Add(_commandString);

            Label _label7 = new Label();
            _label7.Location = new System.Drawing.Point(10, 245);
            _label7.Size = new System.Drawing.Size(500, 40);
            _label7.Text = Global.Form_AdvancedTab_CommandDescription;
            _label7.MaximumSize = new System.Drawing.Size(500, 40);
            _label7.AutoSize = true;
            tabAdv.Controls.Add(_label7);

            _showWindow = new CheckBox();
            _showWindow.Location = new System.Drawing.Point(10, 270);
            _showWindow.Checked = false;
            _showWindow.Text = Global.Form_ShowCommandWIndow;
            _showWindow.AutoSize = true;
            tabAdv.Controls.Add(_showWindow);

            tabCtrl.TabPages.Add(tabAdv);

            this.Controls.Add(tabCtrl);

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

        }

        private void Button2_Click(object sender, EventArgs e)
        {
            formCancel = true;
            this.Close();
        }

        private void TabAdv_Click(object sender, EventArgs e)
        {
            
        }

        private void _componentDescription_TextChanged(object sender, EventArgs e)
        {
            componentDescription = _componentDescription.Text;
            _commandString.Text = string.Format(Global.Yeoman_Item_CommandString, componentName, componentDescription, "webpart");
            SetSubmitState();
        }

        private void _componentName_TextChanged(object sender, EventArgs e)
        {
            componentName = _componentName.Text;
            _commandString.Text = string.Format(Global.Yeoman_Item_CommandString, componentName, componentDescription, "webpart");
            SetSubmitState();
        }

        private void TabProps_Click(object sender, EventArgs e)
        {
            
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

        public static bool FormCancel
        {
            get { return formCancel; }
            set { formCancel = value; }
        }

        protected void button1_Click(object sender, EventArgs e)
        {
            componentName = _componentName.Text;
            componentDescription = _componentDescription.Text;
            commandString = _commandString.Text;
            showWindow = _showWindow.Checked == true ? "true" : "false";
            this.Close();
        }

        protected void SetSubmitState()
        {
            if (!string.IsNullOrWhiteSpace(_componentName.Text) && !string.IsNullOrWhiteSpace(_componentDescription.Text))
                button1.Enabled = true;
        }
    }
}
