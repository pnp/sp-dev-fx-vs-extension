using Framework.VSIX.Resources;

namespace Framework.VSIX
{
  partial class NewProjectForm
  {
    /// <summary>
    /// Required designer variable.
    /// </summary>
    private System.ComponentModel.IContainer components = null;

    /// <summary>
    /// Clean up any resources being used.
    /// </summary>
    /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
    protected override void Dispose(bool disposing)
    {
      if (disposing && (components != null))
      {
        components.Dispose();
      }
      base.Dispose(disposing);
    }

    #region Windows Form Designer generated code

    /// <summary>
    /// Required method for Designer support - do not modify
    /// the contents of this method with the code editor.
    /// </summary>
    private void InitializeComponent()
    {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NewProjectForm));
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabProps = new System.Windows.Forms.TabPage();
            this.lblIsDomainIsolatedInfo = new System.Windows.Forms.Label();
            this.lblSkipFeatureDeploymentInfo = new System.Windows.Forms.Label();
            this.cbxDomainIsolated = new System.Windows.Forms.CheckBox();
            this.cboPackageManager = new System.Windows.Forms.ComboBox();
            this.lblPackageManager = new System.Windows.Forms.Label();
            this.cbxPlusBeta = new System.Windows.Forms.CheckBox();
            this.cboEnvironment = new System.Windows.Forms.ComboBox();
            this.lblEnvironment = new System.Windows.Forms.Label();
            this.cbxSkipFeatureDeployment = new System.Windows.Forms.CheckBox();
            this.cboExtensionType = new System.Windows.Forms.ComboBox();
            this.lblExtensionType = new System.Windows.Forms.Label();
            this.cboComponentType = new System.Windows.Forms.ComboBox();
            this.lblComponentType = new System.Windows.Forms.Label();
            this.cbxSkipInstall = new System.Windows.Forms.CheckBox();
            this.txtComponentDescription = new System.Windows.Forms.TextBox();
            this.lblComponentDescription = new System.Windows.Forms.Label();
            this.txtComponentName = new System.Windows.Forms.TextBox();
            this.lblComponentName = new System.Windows.Forms.Label();
            this.cboFramework = new System.Windows.Forms.ComboBox();
            this.lblFramework = new System.Windows.Forms.Label();
            this.txtSolutionName = new System.Windows.Forms.TextBox();
            this.lblSolutionName = new System.Windows.Forms.Label();
            this.tabAdv = new System.Windows.Forms.TabPage();
            this.cbxShowWindow = new System.Windows.Forms.CheckBox();
            this.lblCommandDescription = new System.Windows.Forms.Label();
            this.lblCommandString = new System.Windows.Forms.Label();
            this.txtCommandString = new System.Windows.Forms.TextBox();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.lblFooter = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblYeomanVersion = new System.Windows.Forms.Label();
            this.tabControl1.SuspendLayout();
            this.tabProps.SuspendLayout();
            this.tabAdv.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabProps);
            this.tabControl1.Controls.Add(this.tabAdv);
            this.tabControl1.Location = new System.Drawing.Point(-2, 0);
            this.tabControl1.Margin = new System.Windows.Forms.Padding(0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(585, 552);
            this.tabControl1.TabIndex = 0;
            // 
            // tabProps
            // 
            this.tabProps.Controls.Add(this.lblIsDomainIsolatedInfo);
            this.tabProps.Controls.Add(this.lblSkipFeatureDeploymentInfo);
            this.tabProps.Controls.Add(this.cbxDomainIsolated);
            this.tabProps.Controls.Add(this.cboPackageManager);
            this.tabProps.Controls.Add(this.lblPackageManager);
            this.tabProps.Controls.Add(this.cbxPlusBeta);
            this.tabProps.Controls.Add(this.cboEnvironment);
            this.tabProps.Controls.Add(this.lblEnvironment);
            this.tabProps.Controls.Add(this.cbxSkipFeatureDeployment);
            this.tabProps.Controls.Add(this.cboExtensionType);
            this.tabProps.Controls.Add(this.lblExtensionType);
            this.tabProps.Controls.Add(this.cboComponentType);
            this.tabProps.Controls.Add(this.lblComponentType);
            this.tabProps.Controls.Add(this.cbxSkipInstall);
            this.tabProps.Controls.Add(this.txtComponentDescription);
            this.tabProps.Controls.Add(this.lblComponentDescription);
            this.tabProps.Controls.Add(this.txtComponentName);
            this.tabProps.Controls.Add(this.lblComponentName);
            this.tabProps.Controls.Add(this.cboFramework);
            this.tabProps.Controls.Add(this.lblFramework);
            this.tabProps.Controls.Add(this.txtSolutionName);
            this.tabProps.Controls.Add(this.lblSolutionName);
            this.tabProps.Location = new System.Drawing.Point(4, 22);
            this.tabProps.Margin = new System.Windows.Forms.Padding(4, 13, 4, 13);
            this.tabProps.Name = "tabProps";
            this.tabProps.Padding = new System.Windows.Forms.Padding(4, 13, 4, 13);
            this.tabProps.Size = new System.Drawing.Size(577, 526);
            this.tabProps.TabIndex = 0;
            this.tabProps.Text = global::Framework.VSIX.Resources.Global.Form_PropertyTab_Title;
            this.tabProps.UseVisualStyleBackColor = true;
            // 
            // lblIsDomainIsolatedInfo
            // 
            this.lblIsDomainIsolatedInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblIsDomainIsolatedInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblIsDomainIsolatedInfo.Location = new System.Drawing.Point(24, 464);
            this.lblIsDomainIsolatedInfo.Name = "lblIsDomainIsolatedInfo";
            this.lblIsDomainIsolatedInfo.Size = new System.Drawing.Size(520, 30);
            this.lblIsDomainIsolatedInfo.TabIndex = 22;
            this.lblIsDomainIsolatedInfo.Text = "label1";
            // 
            // lblSkipFeatureDeploymentInfo
            // 
            this.lblSkipFeatureDeploymentInfo.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSkipFeatureDeploymentInfo.ForeColor = System.Drawing.SystemColors.ControlDark;
            this.lblSkipFeatureDeploymentInfo.Location = new System.Drawing.Point(24, 376);
            this.lblSkipFeatureDeploymentInfo.Name = "lblSkipFeatureDeploymentInfo";
            this.lblSkipFeatureDeploymentInfo.Size = new System.Drawing.Size(520, 30);
            this.lblSkipFeatureDeploymentInfo.TabIndex = 21;
            this.lblSkipFeatureDeploymentInfo.Text = "label1";
            // 
            // cbxDomainIsolated
            // 
            this.cbxDomainIsolated.CheckAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbxDomainIsolated.Location = new System.Drawing.Point(10, 433);
            this.cbxDomainIsolated.Name = "cbxDomainIsolated";
            this.cbxDomainIsolated.Size = new System.Drawing.Size(538, 30);
            this.cbxDomainIsolated.TabIndex = 12;
            this.cbxDomainIsolated.Text = "domainIsolated";
            this.cbxDomainIsolated.TextAlign = System.Drawing.ContentAlignment.TopLeft;
            this.cbxDomainIsolated.UseVisualStyleBackColor = true;
            this.cbxDomainIsolated.CheckedChanged += new System.EventHandler(this.cbxDomainIsolated_CheckedChanged);
            // 
            // cboPackageManager
            // 
            this.cboPackageManager.FormattingEnabled = true;
            this.cboPackageManager.Location = new System.Drawing.Point(313, 246);
            this.cboPackageManager.Name = "cboPackageManager";
            this.cboPackageManager.Size = new System.Drawing.Size(150, 21);
            this.cboPackageManager.TabIndex = 7;
            this.cboPackageManager.SelectedIndexChanged += new System.EventHandler(this.cboPackageManager_SelectedIndexChanged);
            // 
            // lblPackageManager
            // 
            this.lblPackageManager.AutoSize = true;
            this.lblPackageManager.Location = new System.Drawing.Point(316, 226);
            this.lblPackageManager.Name = "lblPackageManager";
            this.lblPackageManager.Size = new System.Drawing.Size(95, 13);
            this.lblPackageManager.TabIndex = 18;
            this.lblPackageManager.Text = "Package Manager";
            // 
            // cbxPlusBeta
            // 
            this.cbxPlusBeta.AutoSize = true;
            this.cbxPlusBeta.Location = new System.Drawing.Point(319, 407);
            this.cbxPlusBeta.Name = "cbxPlusBeta";
            this.cbxPlusBeta.Size = new System.Drawing.Size(66, 17);
            this.cbxPlusBeta.TabIndex = 11;
            this.cbxPlusBeta.Text = "plusbeta";
            this.cbxPlusBeta.UseVisualStyleBackColor = true;
            // 
            // cboEnvironment
            // 
            this.cboEnvironment.FormattingEnabled = true;
            this.cboEnvironment.Location = new System.Drawing.Point(187, 90);
            this.cboEnvironment.Name = "cboEnvironment";
            this.cboEnvironment.Size = new System.Drawing.Size(361, 21);
            this.cboEnvironment.TabIndex = 3;
            // 
            // lblEnvironment
            // 
            this.lblEnvironment.AutoSize = true;
            this.lblEnvironment.Location = new System.Drawing.Point(187, 70);
            this.lblEnvironment.Name = "lblEnvironment";
            this.lblEnvironment.Size = new System.Drawing.Size(66, 13);
            this.lblEnvironment.TabIndex = 15;
            this.lblEnvironment.Text = "Environment";
            // 
            // cbxSkipFeatureDeployment
            // 
            this.cbxSkipFeatureDeployment.AutoSize = true;
            this.cbxSkipFeatureDeployment.Location = new System.Drawing.Point(10, 355);
            this.cbxSkipFeatureDeployment.Name = "cbxSkipFeatureDeployment";
            this.cbxSkipFeatureDeployment.Size = new System.Drawing.Size(138, 17);
            this.cbxSkipFeatureDeployment.TabIndex = 9;
            this.cbxSkipFeatureDeployment.Text = "skip-feature-deployment";
            this.cbxSkipFeatureDeployment.UseVisualStyleBackColor = true;
            // 
            // cboExtensionType
            // 
            this.cboExtensionType.FormattingEnabled = true;
            this.cboExtensionType.Location = new System.Drawing.Point(10, 296);
            this.cboExtensionType.Name = "cboExtensionType";
            this.cboExtensionType.Size = new System.Drawing.Size(150, 21);
            this.cboExtensionType.TabIndex = 8;
            // 
            // lblExtensionType
            // 
            this.lblExtensionType.AutoSize = true;
            this.lblExtensionType.Location = new System.Drawing.Point(10, 276);
            this.lblExtensionType.Name = "lblExtensionType";
            this.lblExtensionType.Size = new System.Drawing.Size(83, 13);
            this.lblExtensionType.TabIndex = 11;
            this.lblExtensionType.Text = "Extension Type:";
            this.lblExtensionType.Visible = false;
            // 
            // cboComponentType
            // 
            this.cboComponentType.FormattingEnabled = true;
            this.cboComponentType.Location = new System.Drawing.Point(10, 246);
            this.cboComponentType.Name = "cboComponentType";
            this.cboComponentType.Size = new System.Drawing.Size(150, 21);
            this.cboComponentType.TabIndex = 6;
            // 
            // lblComponentType
            // 
            this.lblComponentType.AutoSize = true;
            this.lblComponentType.Location = new System.Drawing.Point(10, 226);
            this.lblComponentType.Name = "lblComponentType";
            this.lblComponentType.Size = new System.Drawing.Size(85, 13);
            this.lblComponentType.TabIndex = 9;
            this.lblComponentType.Text = "ComponentType";
            // 
            // cbxSkipInstall
            // 
            this.cbxSkipInstall.AutoSize = true;
            this.cbxSkipInstall.Location = new System.Drawing.Point(10, 407);
            this.cbxSkipInstall.Name = "cbxSkipInstall";
            this.cbxSkipInstall.Size = new System.Drawing.Size(72, 17);
            this.cbxSkipInstall.TabIndex = 10;
            this.cbxSkipInstall.Text = "skipInstall";
            this.cbxSkipInstall.UseVisualStyleBackColor = true;
            // 
            // txtComponentDescription
            // 
            this.txtComponentDescription.Location = new System.Drawing.Point(10, 190);
            this.txtComponentDescription.Name = "txtComponentDescription";
            this.txtComponentDescription.Size = new System.Drawing.Size(538, 20);
            this.txtComponentDescription.TabIndex = 5;
            // 
            // lblComponentDescription
            // 
            this.lblComponentDescription.AutoSize = true;
            this.lblComponentDescription.Location = new System.Drawing.Point(10, 170);
            this.lblComponentDescription.Name = "lblComponentDescription";
            this.lblComponentDescription.Size = new System.Drawing.Size(117, 13);
            this.lblComponentDescription.TabIndex = 6;
            this.lblComponentDescription.Text = "Component Description";
            // 
            // txtComponentName
            // 
            this.txtComponentName.Location = new System.Drawing.Point(10, 140);
            this.txtComponentName.Name = "txtComponentName";
            this.txtComponentName.Size = new System.Drawing.Size(538, 20);
            this.txtComponentName.TabIndex = 4;
            // 
            // lblComponentName
            // 
            this.lblComponentName.AutoSize = true;
            this.lblComponentName.Location = new System.Drawing.Point(10, 120);
            this.lblComponentName.Name = "lblComponentName";
            this.lblComponentName.Size = new System.Drawing.Size(95, 13);
            this.lblComponentName.TabIndex = 4;
            this.lblComponentName.Text = "Component Name:";
            // 
            // cboFramework
            // 
            this.cboFramework.DisplayMember = "Value";
            this.cboFramework.FormattingEnabled = true;
            this.cboFramework.Location = new System.Drawing.Point(10, 90);
            this.cboFramework.Name = "cboFramework";
            this.cboFramework.Size = new System.Drawing.Size(150, 21);
            this.cboFramework.TabIndex = 2;
            this.cboFramework.ValueMember = "Key";
            // 
            // lblFramework
            // 
            this.lblFramework.AutoSize = true;
            this.lblFramework.Location = new System.Drawing.Point(10, 70);
            this.lblFramework.Name = "lblFramework";
            this.lblFramework.Size = new System.Drawing.Size(62, 13);
            this.lblFramework.TabIndex = 2;
            this.lblFramework.Text = "Framework:";
            // 
            // txtSolutionName
            // 
            this.txtSolutionName.Enabled = false;
            this.txtSolutionName.Location = new System.Drawing.Point(10, 40);
            this.txtSolutionName.Name = "txtSolutionName";
            this.txtSolutionName.Size = new System.Drawing.Size(538, 20);
            this.txtSolutionName.TabIndex = 1;
            // 
            // lblSolutionName
            // 
            this.lblSolutionName.AutoSize = true;
            this.lblSolutionName.Location = new System.Drawing.Point(10, 20);
            this.lblSolutionName.Name = "lblSolutionName";
            this.lblSolutionName.Size = new System.Drawing.Size(79, 13);
            this.lblSolutionName.TabIndex = 0;
            this.lblSolutionName.Text = "Solution Name:";
            // 
            // tabAdv
            // 
            this.tabAdv.Controls.Add(this.cbxShowWindow);
            this.tabAdv.Controls.Add(this.lblCommandDescription);
            this.tabAdv.Controls.Add(this.lblCommandString);
            this.tabAdv.Controls.Add(this.txtCommandString);
            this.tabAdv.Location = new System.Drawing.Point(4, 22);
            this.tabAdv.Margin = new System.Windows.Forms.Padding(4, 13, 4, 13);
            this.tabAdv.Name = "tabAdv";
            this.tabAdv.Padding = new System.Windows.Forms.Padding(4, 13, 4, 13);
            this.tabAdv.Size = new System.Drawing.Size(577, 526);
            this.tabAdv.TabIndex = 1;
            this.tabAdv.Text = "Advanced";
            this.tabAdv.UseVisualStyleBackColor = true;
            // 
            // cbxShowWindow
            // 
            this.cbxShowWindow.AutoSize = true;
            this.cbxShowWindow.Location = new System.Drawing.Point(10, 270);
            this.cbxShowWindow.Name = "cbxShowWindow";
            this.cbxShowWindow.Size = new System.Drawing.Size(145, 17);
            this.cbxShowWindow.TabIndex = 16;
            this.cbxShowWindow.Text = "Show Command Window";
            this.cbxShowWindow.UseVisualStyleBackColor = true;
            // 
            // lblCommandDescription
            // 
            this.lblCommandDescription.AutoSize = true;
            this.lblCommandDescription.Location = new System.Drawing.Point(10, 245);
            this.lblCommandDescription.Name = "lblCommandDescription";
            this.lblCommandDescription.Size = new System.Drawing.Size(325, 13);
            this.lblCommandDescription.TabIndex = 2;
            this.lblCommandDescription.Text = "Add switches and parameters to the Yeoman Generator commands.";
            // 
            // lblCommandString
            // 
            this.lblCommandString.AutoSize = true;
            this.lblCommandString.Location = new System.Drawing.Point(10, 20);
            this.lblCommandString.Name = "lblCommandString";
            this.lblCommandString.Size = new System.Drawing.Size(87, 13);
            this.lblCommandString.TabIndex = 1;
            this.lblCommandString.Text = "Command String:";
            // 
            // txtCommandString
            // 
            this.txtCommandString.Location = new System.Drawing.Point(10, 40);
            this.txtCommandString.Multiline = true;
            this.txtCommandString.Name = "txtCommandString";
            this.txtCommandString.Size = new System.Drawing.Size(560, 200);
            this.txtCommandString.TabIndex = 15;
            // 
            // btnGenerate
            // 
            this.btnGenerate.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnGenerate.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnGenerate.Location = new System.Drawing.Point(12, 565);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(0);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(75, 23);
            this.btnGenerate.TabIndex = 13;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.Generate_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCancel.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCancel.Location = new System.Drawing.Point(477, 565);
            this.btnCancel.Margin = new System.Windows.Forms.Padding(0);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 14;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.Cancel_Click);
            // 
            // lblFooter
            // 
            this.lblFooter.Location = new System.Drawing.Point(12, 595);
            this.lblFooter.Name = "lblFooter";
            this.lblFooter.Size = new System.Drawing.Size(520, 30);
            this.lblFooter.TabIndex = 3;
            this.lblFooter.Text = "lblFooter";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 628);
            this.pictureBox1.MaximumSize = new System.Drawing.Size(30, 73);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(20, 35);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // lblYeomanVersion
            // 
            this.lblYeomanVersion.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.lblYeomanVersion.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblYeomanVersion.ForeColor = System.Drawing.SystemColors.HotTrack;
            this.lblYeomanVersion.Location = new System.Drawing.Point(37, 631);
            this.lblYeomanVersion.Name = "lblYeomanVersion";
            this.lblYeomanVersion.Size = new System.Drawing.Size(450, 30);
            this.lblYeomanVersion.TabIndex = 5;
            this.lblYeomanVersion.Text = "label1";
            this.lblYeomanVersion.TextAlign = System.Drawing.ContentAlignment.BottomLeft;
            // 
            // NewProjectForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(583, 684);
            this.Controls.Add(this.lblYeomanVersion);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.lblFooter);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.tabControl1);
            this.Icon = global::Framework.VSIX.Resources.Global.Extension;
            this.Margin = new System.Windows.Forms.Padding(4, 13, 4, 13);
            this.Name = "NewProjectForm";
            this.Text = "SharePoint Framework Project Configuration";
            this.tabControl1.ResumeLayout(false);
            this.tabProps.ResumeLayout(false);
            this.tabProps.PerformLayout();
            this.tabAdv.ResumeLayout(false);
            this.tabAdv.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

    }

    #endregion

    private System.Windows.Forms.TabControl tabControl1;
    private System.Windows.Forms.TabPage tabProps;
    private System.Windows.Forms.TabPage tabAdv;
    private System.Windows.Forms.TextBox txtSolutionName;
    private System.Windows.Forms.Label lblSolutionName;
    private System.Windows.Forms.ComboBox cboFramework;
    private System.Windows.Forms.Label lblFramework;
    private System.Windows.Forms.TextBox txtComponentName;
    private System.Windows.Forms.Label lblComponentName;
    private System.Windows.Forms.TextBox txtComponentDescription;
    private System.Windows.Forms.Label lblComponentDescription;
    private System.Windows.Forms.CheckBox cbxSkipInstall;
    private System.Windows.Forms.TextBox txtCommandString;
    private System.Windows.Forms.ComboBox cboComponentType;
    private System.Windows.Forms.Label lblComponentType;
    private System.Windows.Forms.ComboBox cboExtensionType;
    private System.Windows.Forms.Label lblExtensionType;
    private System.Windows.Forms.Label lblCommandString;
    private System.Windows.Forms.Button btnGenerate;
    private System.Windows.Forms.Button btnCancel;
    private System.Windows.Forms.Label lblFooter;
    private System.Windows.Forms.Label lblCommandDescription;
    private System.Windows.Forms.CheckBox cbxShowWindow;
    private System.Windows.Forms.CheckBox cbxSkipFeatureDeployment;
		private System.Windows.Forms.ComboBox cboEnvironment;
		private System.Windows.Forms.Label lblEnvironment;
        private System.Windows.Forms.CheckBox cbxPlusBeta;
        private System.Windows.Forms.ComboBox cboPackageManager;
        private System.Windows.Forms.Label lblPackageManager;
        private System.Windows.Forms.CheckBox cbxDomainIsolated;
        private System.Windows.Forms.Label lblSkipFeatureDeploymentInfo;
        private System.Windows.Forms.Label lblIsDomainIsolatedInfo;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblYeomanVersion;
    }
}