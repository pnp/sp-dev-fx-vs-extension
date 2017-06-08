using System;
using System.Diagnostics;
using System.Collections.Generic;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using System.Drawing;
using EnvDTE;
using System.IO;
using System.Text;
using System.Resources;
using System.Reflection;
using Framework.VSIX.Resources;

namespace Framework.VSIX
{
    public class WebPartItemWizard : IWizard
    {
        private string _componentName;
        private string _componentDescription;
        private string _commandString;
        private bool _showWindow;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
        }

        public void ProjectItemFinishedGenerating(ProjectItem
            projectItem)
        {
            string projectDir = projectItem.ContainingProject.FullName;
            projectDir = projectDir.Substring(0, projectDir.LastIndexOf("\\", StringComparison.Ordinal));
            string logFile = $@"{projectDir}\{_componentName}.log";

            _commandString = _commandString.Replace("$ComponentName$", _componentName);
            _commandString = _commandString.Replace("$ComponentDescription$", _componentDescription);

            StringBuilder outputText = new StringBuilder();

            using (var proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.WorkingDirectory = projectDir;
                proc.StartInfo.FileName = @"cmd.exe";

                if (!_showWindow)
                {
                    proc.StartInfo.Arguments = $@" /c  {_commandString}";
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
                    proc.StartInfo.Arguments = $@" /k  {_commandString}";
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

        public void RunFinished()
        {

        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary,
            WizardRunKind runKind, object[] customParams)
        {
            try
            {
                using (WebPartInputForm inputForm = new WebPartInputForm())
                {
                    if (inputForm.ShowDialog() != DialogResult.OK)
                    {
                        throw new WizardCancelledException();
                    }

                    replacementsDictionary["$ComponentName$"] = _componentName = inputForm.ComponentName;
                    replacementsDictionary["$ComponentDescription$"] = _componentDescription = inputForm.ComponentDescription;
                    replacementsDictionary["$CommandString$"] = _commandString = inputForm.CommandString;
                    _showWindow = inputForm.ShowWindow;
                }
            }
            catch (WizardCancelledException)
            {
                throw;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw new WizardCancelledException();
            }
        }

        public bool ShouldAddProjectItem(string filePath)
        {
            return true;
        }
    }

    public class WebPartInputForm : Form
    {
        private readonly TextBox _componentName;
        private readonly TextBox _componentDescription;
        private readonly TextBox _commandString;
        private readonly Button _button1;
        private readonly Button _button2;
        private readonly CheckBox _showWindow;
        private readonly ResourceManager _rm = new ResourceManager("Framework.VSIX.Resources.Global", Assembly.GetExecutingAssembly());

        public WebPartInputForm()
        {
            Size = new Size(600, 450);
            Name = Global.Form_Item_Name;
            Text = Global.Form_Item_Title;
            Icon = Global.Extension;

            TabControl tabCtrl = new TabControl
            {
                Name = "ConfigTabControl",
                Width = 600,
                Height = 350
            };
            Controls.Add(tabCtrl);

            TabPage tabProps = new TabPage
            {
                Name = "ConfigTabPageProps",
                Text = Global.Form_PropertyTab_Title,
                Height = 350,
                Width = 600
            };
            tabProps.Click += TabProps_Click;
            tabCtrl.Controls.Add(tabProps);

            Label label3 = new Label
            {
                Location = new Point(10, 20),
                Size = new Size(200, 20),
                Text = Global.Form_ComponentName
            };
            tabProps.Controls.Add(label3);

            _componentName = new TextBox
            {
                Location = new Point(10, 40),
                Size = new Size(400, 20)
            };
            _componentName.TextChanged += _componentName_TextChanged;
            tabProps.Controls.Add(_componentName);

            Label label4 = new Label
            {
                Location = new Point(10, 70),
                Size = new Size(200, 20),
                Text = Global.Form_ComponentDescription
            };
            tabProps.Controls.Add(label4);

            _componentDescription = new TextBox
            {
                Location = new Point(10, 90),
                Size = new Size(400, 20)
            };
            _componentDescription.TextChanged += _componentDescription_TextChanged;
            tabProps.Controls.Add(_componentDescription);

            TabPage tabAdv = new TabPage
            {
                Name = "ConfigTabPageAdvanced",
                Text = Global.Form_AdvancedTab_Title,
                Height = 350,
                Width = 600
            };
            tabAdv.Click += TabAdv_Click;
            tabCtrl.TabPages.Add(tabAdv);

            Label label6 = new Label
            {
                Location = new Point(10, 20),
                Size = new Size(200, 20),
                Text = Global.Form_CommandString
            };
            tabAdv.Controls.Add(label6);

            _commandString = new TextBox
            {
                Location = new Point(10, 40),
                Size = new Size(560, 200),
                Multiline = true,
                ScrollBars = ScrollBars.Vertical,
                Text = string.Format(Global.Yeoman_WebPartItem_CommandString, ComponentName, ComponentDescription)
            };
            tabAdv.Controls.Add(_commandString);

            Label label7 = new Label
            {
                Location = new Point(10, 245),
                Size = new Size(500, 40),
                Text = Global.Form_AdvancedTab_CommandDescription,
                MaximumSize = new Size(500, 40),
                AutoSize = true
            };
            tabAdv.Controls.Add(label7);

            _showWindow = new CheckBox
            {
                Location = new Point(10, 270),
                Checked = false,
                Text = Global.Form_ShowCommandWIndow,
                AutoSize = true
            };
            tabAdv.Controls.Add(_showWindow);

            _button1 = new Button
            {
                Location = new Point(10, 360),
                Size = new Size(100, 25),
                Text = Global.Form_ButtonGenerate,
                Enabled = false
            };
            _button1.Click += _button1_Click;
            Controls.Add(_button1);

            _button2 = new Button
            {
                Location = new Point(475, 360),
                Size = new Size(100, 25),
                Text = Global.Form_ButtonCancel
            };
            _button2.Click += _button2_Click;
            Controls.Add(_button2);

            Label label5 = new Label
            {
                Location = new Point(10, 390),
                Size = new Size(500, 20),
                Text = Global.Form_Footer_GeneratorText
            };
            Controls.Add(label5);
        }

        public string ComponentName
        {
            get => _componentName.Text;
            set => _componentName.Text = value;
        }

        public string ComponentDescription
        {
            get => _componentDescription.Text;
            set => _componentDescription.Text = value;
        }

        public string CommandString
        {
            get => _commandString.Text;
            set => _commandString.Text = value;
        }

        public bool ShowWindow
        {
            get => _showWindow.Checked;
            set => _showWindow.Checked = value;
        }

        private void TabProps_Click(object sender, EventArgs e)
        {
        }

        private void TabAdv_Click(object sender, EventArgs e)
        {

        }

        private void _componentDescription_TextChanged(object sender, EventArgs e)
        {
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_ComponentDescription, ComponentDescription);
            SetSubmitState();
        }

        private void _componentName_TextChanged(object sender, EventArgs e)
        {
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_ComponentName, ComponentName);
            SetSubmitState();
        }

        private void _button1_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.OK;
            Close();
        }

        private void _button2_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void SetSubmitState()
        {
            _button1.Enabled = !string.IsNullOrWhiteSpace(_componentName.Text) &&
                              !string.IsNullOrWhiteSpace(_componentDescription.Text);
        }
    }
}