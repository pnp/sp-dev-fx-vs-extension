using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Drawing;
using Microsoft.VisualStudio.TemplateWizard;
using System.Windows.Forms;
using EnvDTE;
using System.IO;
using System.Linq;
using System.Text;
using System.Resources;
using Framework.VSIX.Resources;
using System.Reflection;

namespace Framework.VSIX
{
    public class FrameworkProjectWizard : IWizard
    {
        private string _solutionName;
        private string _componentType;
        private string _extensionType;
        private string _framework;
        private string _componentName;
        private string _componentDescription;
        private string _solutionDir;
        private string _projectDir;
        private string _logFile;
        private string _commandString;
        private bool _showWindow;

        public void BeforeOpeningFile(ProjectItem projectItem)
        {
        }

        public void ProjectFinishedGenerating(Project project)
        {
            _commandString = _commandString.Replace("$SolutionName$", _solutionName);
            _commandString = _commandString.Replace("$ComponentType$", _componentType);
            _commandString = _commandString.Replace("$ExtensionType$", _extensionType);
            _commandString = _commandString.Replace("$Framework$", _framework);
            _commandString = _commandString.Replace("$ComponentName$", _componentName);
            _commandString = _commandString.Replace("$ComponentDescription$", _componentDescription);

            StringBuilder outputText = new StringBuilder();

            using (var proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.WorkingDirectory = _projectDir;
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

                using (StreamWriter sw = File.AppendText(_logFile))
                {
                    sw.Write(outputText);
                }
            }

            string[] files = Directory.GetFiles(_projectDir, "*.*", SearchOption.AllDirectories);

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
                using (UserInputForm inputForm = new UserInputForm())
                {
                    inputForm.SolutionName = replacementsDictionary["$safeprojectname$"];
                    if (inputForm.ShowDialog() != DialogResult.OK)
                    {
                        throw new WizardCancelledException();
                    }

                    replacementsDictionary["$SolutionName$"] = _solutionName = inputForm.SolutionName;
                    replacementsDictionary["$ComponentType$"] = _componentType = inputForm.ComponentType;
                    replacementsDictionary["$ExtensionType$"] = _extensionType = inputForm.ExtensionType;
                    replacementsDictionary["$Framework$"] = _framework = inputForm.Framework;
                    replacementsDictionary["$ComponentName$"] = _componentName = inputForm.ComponentName;
                    replacementsDictionary["$ComponentDescription$"] = _componentDescription = inputForm.ComponentDescription;
                    replacementsDictionary["$CommandString$"] = _commandString = inputForm.CommandString;
                    _showWindow = inputForm.ShowWindow;

                    _solutionDir = Path.GetDirectoryName(replacementsDictionary["$destinationdirectory$"]);
                    _projectDir = $@"{_solutionDir}\{replacementsDictionary["$safeprojectname$"]}";
                    _logFile = $@"{_projectDir}\generator.log";
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

    public class UserInputForm : Form
    {
        private readonly Label _extensionTypeLabel;
        private readonly Label _frameworkLabel;
        private readonly TextBox _solutionName;
        private readonly ComboBox _componentType;
        private readonly ComboBox _extensionType;
        private readonly ComboBox _framework;
        private readonly TextBox _componentName;
        private readonly TextBox _componentDescription;
        private readonly TextBox _commandString;
        private readonly Button _button1;
        private readonly Button _button2;
        private readonly CheckBox _showWindow;
        private readonly CheckBox _skipInstall;
        private readonly ResourceManager _rm = new ResourceManager("Framework.VSIX.Resources.Global", Assembly.GetExecutingAssembly());

        public UserInputForm()
        {
            Size = new Size(600, 460);
            Name = Global.Form_Project_Name;
            Text = Global.Form_Project_Title;
            Icon = Global.Extension;

            TabControl tabCtrl = new TabControl
            {
                Name = "ConfigTabControl",
                Width = 590,
                Height = 360
            };
            Controls.Add(tabCtrl);

            TabPage tabProps = new TabPage
            {
                Name = "ConfigTabPageProps",
                Text = Global.Form_PropertyTab_Title,
                Height = 360,
                Width = 590
            };
            tabProps.Click += TabProps_Click;
            tabCtrl.TabPages.Add(tabProps);

            Label label1 = new Label
            {
                Location = new Point(10, 20),
                Size = new Size(200, 20),
                Text = Global.Form_SolutionName
            };
            tabProps.Controls.Add(label1);

            _solutionName = new TextBox
            {
                Name = "spfxSolutionName",
                Location = new Point(10, 40),
                Size = new Size(400, 20),
                Enabled = false
            };
            _solutionName.TextChanged += _solutionName_TextChanged;
            tabProps.Controls.Add(_solutionName);

            Label labelComponentType = new Label
            {
                Location = new Point(10, 70),
                Size = new Size(200, 20),
                Text = Global.Form_ComponentType
            };
            tabProps.Controls.Add(labelComponentType);

            _componentType = new ComboBox
            {
                Location = new Point(10, 90),
                Size = new Size(150, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            tabProps.Controls.Add(_componentType);

            Dictionary<string, string> componentTypeComboSource =
                new Dictionary<string, string>
                {
                    { "webpart", Global.Form_WebPart },
                    { "extension", Global.Form_Extension }
                };
            _componentType.DataSource = new BindingSource(componentTypeComboSource, null);
            _componentType.DisplayMember = "Value";
            _componentType.ValueMember = "Key";
            _componentType.SelectedItem = componentTypeComboSource.First();
            _componentType.SelectionChangeCommitted += _componentType_SelectionChangeCommitted;

            _extensionTypeLabel = new Label
            {
                Location = new Point(10, 120),
                Size = new Size(200, 20),
                Text = Global.Form_ExtensionType,
                Visible = false
            };
            tabProps.Controls.Add(_extensionTypeLabel);

            _extensionType = new ComboBox
            {
                Location = new Point(10, 140),
                Size = new Size(200, 20),
                DropDownStyle = ComboBoxStyle.DropDownList,
                Visible = false
            };
            tabProps.Controls.Add(_extensionType);

            Dictionary<string, string> extensionTypeComboSource =
                new Dictionary<string, string>
                {
                    { "ApplicationCustomizer", Global.Form_ApplicationCustomizer },
                    { "FieldCustomizer", Global.Form_FieldCustomizer },
                    { "ListViewCommandSet", Global.Form_ListViewCommandSet }
                };
            _extensionType.DataSource = new BindingSource(extensionTypeComboSource, null);
            _extensionType.DisplayMember = "Value";
            _extensionType.ValueMember = "Key";
            _extensionType.SelectedItem = extensionTypeComboSource.First();
            _extensionType.SelectionChangeCommitted += _extensionType_SelectionChangeCommitted;

            _frameworkLabel = new Label
            {
                Location = new Point(10, 120),
                Size = new Size(200, 20),
                Text = Global.Form_Framework
            };
            tabProps.Controls.Add(_frameworkLabel);

            _framework = new ComboBox
            {
                Location = new Point(10, 140),
                Size = new Size(150, 20),
                DropDownStyle = ComboBoxStyle.DropDownList
            };
            tabProps.Controls.Add(_framework);

            Dictionary<string, string> comboSource =
                new Dictionary<string, string>
                {
                    { "none", "none" },
                    { "react", "react" },
                    { "knockout", "knockout" }
                };

            _framework.DataSource = new BindingSource(comboSource, null);
            _framework.DisplayMember = "Value";
            _framework.ValueMember = "Key";
            _framework.SelectedItem = comboSource.First();
            _framework.SelectionChangeCommitted += _framework_SelectionChangeCommitted;

            Label label3 = new Label
            {
                Location = new Point(10, 170),
                Size = new Size(400, 20),
                Text = Global.Form_ComponentName
            };
            tabProps.Controls.Add(label3);

            _componentName = new TextBox
            {
                Location = new Point(10, 190),
                Size = new Size(400, 20)
            };
            _componentName.TextChanged += _componentName_TextChanged;
            tabProps.Controls.Add(_componentName);

            Label label4 = new Label
            {
                Location = new Point(10, 220),
                Size = new Size(200, 20),
                Text = Global.Form_ComponentDescription
            };
            tabProps.Controls.Add(label4);

            _componentDescription = new TextBox
            {
                Location = new Point(10, 240),
                Size = new Size(400, 20)
            };
            _componentDescription.TextChanged += _componentDescription_TextChanged;
            tabProps.Controls.Add(_componentDescription);

            _skipInstall = new CheckBox
            {
                Location = new Point(10, 280),
                Checked = false,
                Text = Global.Form_SkipInstall,
                AutoSize = true
            };
            _skipInstall.CheckedChanged += _skipInstall_CheckedChanged;
            tabProps.Controls.Add(_skipInstall);

            TabPage tabAdv = new TabPage
            {
                Name = "ConfigTabPageAdvanced",
                Text = Global.Form_AdvancedTab_Title,
                Height = 360,
                Width = 590
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
                Text = string.Format(Global.Yeoman_Project_CommandString, SolutionName, ComponentType, Framework,
                    ComponentName, ComponentDescription)
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
                Location = new Point(10, 370),
                Size = new Size(100, 25),
                Text = Global.Form_ButtonGenerate,
                Enabled = false
            };
            _button1.Click += _button1_Click;
            Controls.Add(_button1);

            _button2 = new Button
            {
                Location = new Point(475, 370),
                Size = new Size(100, 25),
                Text = Global.Form_ButtonCancel
            };
            _button2.Click += _button2_Click;
            Controls.Add(_button2);

            Label label5 = new Label
            {
                Location = new Point(10, 400),
                Size = new Size(500, 20),
                Text = Global.Form_Footer_GeneratorText
            };
            Controls.Add(label5);
        }

        public string SolutionName
        {
            get => _solutionName.Text;
            set => _solutionName.Text = value;
        }

        public string ComponentType
        {
            get => (string)_componentType.SelectedValue;
            set => _componentType.SelectedValue = value;
        }

        public string ExtensionType
        {
            get => ComponentType == "extension" ? (string)_extensionType.SelectedValue : string.Empty;
            set => _extensionType.SelectedValue = value;
        }

        public string Framework
        {
            get => ComponentType == "webpart" ? (string)_framework.SelectedValue : string.Empty;
            set => _framework.SelectedValue = value;
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

        public bool SkipInstall
        {
            get => _skipInstall.Checked;
            set => _skipInstall.Checked = value;
        }

        private void _skipInstall_CheckedChanged(object sender, EventArgs e)
        {
            CommandString = SkipInstall
                ? $@"{CommandString} {Global.Yeoman_SkipInstall_Flag}"
                : CommandString.Replace($@" {Global.Yeoman_SkipInstall_Flag}", string.Empty);
            SetSubmitState();
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

        private void _framework_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_Framework, Framework);
            SetSubmitState();
        }

        private void _solutionName_TextChanged(object sender, EventArgs e)
        {
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_SolutionName, SolutionName);
            SetSubmitState();
        }

        private void _componentType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            switch (ComponentType)
            {
                case "webpart":
                    _extensionTypeLabel.Hide();
                    _extensionType.Hide();
                    _frameworkLabel.Show();
                    _framework.Show();
                    break;
                case "extension":
                    _frameworkLabel.Hide();
                    _framework.Hide();
                    _extensionTypeLabel.Show();
                    _extensionType.Show();
                    break;
            }

            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_ComponentType, ComponentType);
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_ExtensionType, ExtensionType, true);
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_Framework, Framework, true);
            SetSubmitState();
        }

        private void _extensionType_SelectionChangeCommitted(object sender, EventArgs e)
        {
            CommandString = ParameterHelper.AddOrUpdateCommandParameter(CommandString, Global.Yeoman_ExtensionType, ExtensionType);
            SetSubmitState();
        }

        private void TabAdv_Click(object sender, EventArgs e)
        {

        }

        private void TabProps_Click(object sender, EventArgs e)
        {

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
            _button1.Enabled = !string.IsNullOrWhiteSpace(_solutionName.Text) &&
                               !string.IsNullOrWhiteSpace(_componentName.Text) &&
                               !string.IsNullOrWhiteSpace(_componentDescription.Text);
        }
    }
}