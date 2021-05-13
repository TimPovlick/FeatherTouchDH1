
namespace ASCOM.FeatherTouchDH1
{
	partial class SetupDialogForm
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
			this.components = new System.ComponentModel.Container();
			this.btnOK = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.label2 = new System.Windows.Forms.Label();
			this.btnAbout = new System.Windows.Forms.Button();
			this.btnApply = new System.Windows.Forms.Button();
			this.gbxConnection = new System.Windows.Forms.GroupBox();
			this.cmbxCommPort = new System.Windows.Forms.ComboBox();
			this.gbxFocuserSettings = new System.Windows.Forms.GroupBox();
			this.label12 = new System.Windows.Forms.Label();
			this.tbxStepSizeMicrons = new System.Windows.Forms.TextBox();
			this.nudMaxIncrement = new System.Windows.Forms.NumericUpDown();
			this.nudMaxTravel = new System.Windows.Forms.NumericUpDown();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.rbtnCoarseResolution = new System.Windows.Forms.RadioButton();
			this.rbtnMediumResolution = new System.Windows.Forms.RadioButton();
			this.rbtnFineResolution = new System.Windows.Forms.RadioButton();
			this.label4 = new System.Windows.Forms.Label();
			this.cbxDoNotCalOnPowerUp = new System.Windows.Forms.CheckBox();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.cbxReverseInOut = new System.Windows.Forms.CheckBox();
			this.label1 = new System.Windows.Forms.Label();
			this.cmbxModel = new System.Windows.Forms.ComboBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.tbxPerDegreesC = new System.Windows.Forms.TextBox();
			this.tbxMoveSteps = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.radioButtonOut = new System.Windows.Forms.RadioButton();
			this.radioButtonIn = new System.Windows.Forms.RadioButton();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.nudBacklashSteps = new System.Windows.Forms.NumericUpDown();
			this.radioButtonTempFinalDirectionOut = new System.Windows.Forms.RadioButton();
			this.radioButtonTempFinalDirectionIn = new System.Windows.Forms.RadioButton();
			this.label14 = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
			this.btnStaticSetPosition = new System.Windows.Forms.Button();
			this.nudPosition = new System.Windows.Forms.NumericUpDown();
			this.chkTrace = new System.Windows.Forms.CheckBox();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.gbxConnection.SuspendLayout();
			this.gbxFocuserSettings.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxIncrement)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxTravel)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.groupBox2.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudBacklashSteps)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPosition)).BeginInit();
			this.groupBox3.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnOK.BackColor = System.Drawing.Color.LawnGreen;
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOK.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnOK.ForeColor = System.Drawing.Color.Black;
			this.btnOK.Location = new System.Drawing.Point(504, 1130);
			this.btnOK.Margin = new System.Windows.Forms.Padding(6);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(118, 50);
			this.btnOK.TabIndex = 5;
			this.btnOK.Text = "OK";
			this.toolTip1.SetToolTip(this.btnOK, "Save and Close");
			this.btnOK.UseVisualStyleBackColor = false;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnCancel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(192)))), ((int)(((byte)(0)))), ((int)(((byte)(0)))));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnCancel.ForeColor = System.Drawing.Color.Black;
			this.btnCancel.Location = new System.Drawing.Point(504, 1261);
			this.btnCancel.Margin = new System.Windows.Forms.Padding(6);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(118, 50);
			this.btnCancel.TabIndex = 7;
			this.btnCancel.Text = "Cancel";
			this.toolTip1.SetToolTip(this.btnCancel, "Discard Changes");
			this.btnCancel.UseVisualStyleBackColor = false;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(14, 35);
			this.label2.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(150, 31);
			this.label2.TabIndex = 0;
			this.label2.Text = "Comm Port";
			// 
			// btnAbout
			// 
			this.btnAbout.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnAbout.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnAbout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
			this.btnAbout.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnAbout.Location = new System.Drawing.Point(504, 1325);
			this.btnAbout.Margin = new System.Windows.Forms.Padding(6);
			this.btnAbout.Name = "btnAbout";
			this.btnAbout.Size = new System.Drawing.Size(118, 50);
			this.btnAbout.TabIndex = 8;
			this.btnAbout.Text = "About";
			this.btnAbout.UseVisualStyleBackColor = false;
			this.btnAbout.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnAbout_MouseDown);
			// 
			// btnApply
			// 
			this.btnApply.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.btnApply.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.btnApply.BackColor = System.Drawing.Color.Teal;
			this.btnApply.DialogResult = System.Windows.Forms.DialogResult.Yes;
			this.btnApply.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.btnApply.ForeColor = System.Drawing.Color.Black;
			this.btnApply.Location = new System.Drawing.Point(504, 1199);
			this.btnApply.Margin = new System.Windows.Forms.Padding(6);
			this.btnApply.Name = "btnApply";
			this.btnApply.Size = new System.Drawing.Size(118, 50);
			this.btnApply.TabIndex = 6;
			this.btnApply.Text = "Apply";
			this.toolTip1.SetToolTip(this.btnApply, "Apply any changes immediately.");
			this.btnApply.UseVisualStyleBackColor = false;
			this.btnApply.Click += new System.EventHandler(this.btnApply_Click);
			// 
			// gbxConnection
			// 
			this.gbxConnection.Controls.Add(this.cmbxCommPort);
			this.gbxConnection.Controls.Add(this.label2);
			this.gbxConnection.Location = new System.Drawing.Point(59, 175);
			this.gbxConnection.Name = "gbxConnection";
			this.gbxConnection.Size = new System.Drawing.Size(480, 94);
			this.gbxConnection.TabIndex = 1;
			this.gbxConnection.TabStop = false;
			this.gbxConnection.Text = "Connection";
			// 
			// cmbxCommPort
			// 
			this.cmbxCommPort.BackColor = System.Drawing.Color.Black;
			this.cmbxCommPort.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::ASCOM.FeatherTouchDH1.Properties.Settings.Default, "COMPort", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cmbxCommPort.DisplayMember = "COM3";
			this.cmbxCommPort.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbxCommPort.ForeColor = System.Drawing.Color.Yellow;
			this.cmbxCommPort.FormattingEnabled = true;
			this.cmbxCommPort.Location = new System.Drawing.Point(236, 35);
			this.cmbxCommPort.Name = "cmbxCommPort";
			this.cmbxCommPort.Size = new System.Drawing.Size(201, 39);
			this.cmbxCommPort.TabIndex = 2;
			this.toolTip1.SetToolTip(this.cmbxCommPort, "Select the serial port connected to the focuser.COMM port to which the foucser is" +
        " connected");
			this.cmbxCommPort.ValueMember = global::ASCOM.FeatherTouchDH1.Properties.Settings.Default.COMPort;
			this.cmbxCommPort.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbxCommPort_KeyPress);
			// 
			// gbxFocuserSettings
			// 
			this.gbxFocuserSettings.Controls.Add(this.label12);
			this.gbxFocuserSettings.Controls.Add(this.tbxStepSizeMicrons);
			this.gbxFocuserSettings.Controls.Add(this.nudMaxIncrement);
			this.gbxFocuserSettings.Controls.Add(this.nudMaxTravel);
			this.gbxFocuserSettings.Controls.Add(this.label7);
			this.gbxFocuserSettings.Controls.Add(this.label6);
			this.gbxFocuserSettings.Controls.Add(this.rbtnCoarseResolution);
			this.gbxFocuserSettings.Controls.Add(this.rbtnMediumResolution);
			this.gbxFocuserSettings.Controls.Add(this.rbtnFineResolution);
			this.gbxFocuserSettings.Controls.Add(this.label4);
			this.gbxFocuserSettings.Controls.Add(this.cbxDoNotCalOnPowerUp);
			this.gbxFocuserSettings.Controls.Add(this.label5);
			this.gbxFocuserSettings.Controls.Add(this.label3);
			this.gbxFocuserSettings.Controls.Add(this.cbxReverseInOut);
			this.gbxFocuserSettings.Controls.Add(this.label1);
			this.gbxFocuserSettings.Controls.Add(this.cmbxModel);
			this.gbxFocuserSettings.Location = new System.Drawing.Point(59, 281);
			this.gbxFocuserSettings.Name = "gbxFocuserSettings";
			this.gbxFocuserSettings.Size = new System.Drawing.Size(480, 513);
			this.gbxFocuserSettings.TabIndex = 2;
			this.gbxFocuserSettings.TabStop = false;
			this.gbxFocuserSettings.Text = "Focuser Settings";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(14, 38);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(193, 31);
			this.label12.TabIndex = 0;
			this.label12.Text = "Focuser Model";
			// 
			// tbxStepSizeMicrons
			// 
			this.tbxStepSizeMicrons.BackColor = System.Drawing.Color.Black;
			this.tbxStepSizeMicrons.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxStepSizeMicrons.ForeColor = System.Drawing.Color.Yellow;
			this.tbxStepSizeMicrons.Location = new System.Drawing.Point(317, 228);
			this.tbxStepSizeMicrons.MaxLength = 5;
			this.tbxStepSizeMicrons.Name = "tbxStepSizeMicrons";
			this.tbxStepSizeMicrons.Size = new System.Drawing.Size(117, 38);
			this.tbxStepSizeMicrons.TabIndex = 4;
			this.toolTip1.SetToolTip(this.tbxStepSizeMicrons, "Steps in microns");
			this.tbxStepSizeMicrons.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxStepSizeMicrons_KeyDown);
			this.tbxStepSizeMicrons.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxStepSizeMicrons_KeyPress);
			// 
			// nudMaxIncrement
			// 
			this.nudMaxIncrement.BackColor = System.Drawing.Color.Black;
			this.nudMaxIncrement.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudMaxIncrement.ForeColor = System.Drawing.Color.Yellow;
			this.nudMaxIncrement.Location = new System.Drawing.Point(284, 168);
			this.nudMaxIncrement.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.nudMaxIncrement.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudMaxIncrement.Name = "nudMaxIncrement";
			this.nudMaxIncrement.Size = new System.Drawing.Size(153, 38);
			this.nudMaxIncrement.TabIndex = 3;
			this.toolTip1.SetToolTip(this.nudMaxIncrement, "Maximum number of steps allowed for a single focuser move operation");
			this.nudMaxIncrement.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// nudMaxTravel
			// 
			this.nudMaxTravel.BackColor = System.Drawing.Color.Black;
			this.nudMaxTravel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudMaxTravel.ForeColor = System.Drawing.Color.Yellow;
			this.nudMaxTravel.Location = new System.Drawing.Point(284, 90);
			this.nudMaxTravel.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.nudMaxTravel.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
			this.nudMaxTravel.Name = "nudMaxTravel";
			this.nudMaxTravel.Size = new System.Drawing.Size(153, 38);
			this.nudMaxTravel.TabIndex = 2;
			this.toolTip1.SetToolTip(this.nudMaxTravel, "The maximum number of steps for full range of focuser travel");
			this.nudMaxTravel.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(13, 234);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(283, 25);
			this.label7.TabIndex = 0;
			this.label7.Text = "Step Size (microns per step)";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(137, 416);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(206, 31);
			this.label6.TabIndex = 0;
			this.label6.Text = "Step Resolution";
			// 
			// rbtnCoarseResolution
			// 
			this.rbtnCoarseResolution.AutoSize = true;
			this.rbtnCoarseResolution.Location = new System.Drawing.Point(284, 457);
			this.rbtnCoarseResolution.Name = "rbtnCoarseResolution";
			this.rbtnCoarseResolution.Size = new System.Drawing.Size(112, 29);
			this.rbtnCoarseResolution.TabIndex = 9;
			this.rbtnCoarseResolution.TabStop = true;
			this.rbtnCoarseResolution.Text = "Coarse";
			this.rbtnCoarseResolution.UseVisualStyleBackColor = true;
			this.rbtnCoarseResolution.CheckedChanged += new System.EventHandler(this.rbtnCoarseResolution_CheckedChanged);
			// 
			// rbtnMediumResolution
			// 
			this.rbtnMediumResolution.AutoSize = true;
			this.rbtnMediumResolution.Location = new System.Drawing.Point(142, 457);
			this.rbtnMediumResolution.Name = "rbtnMediumResolution";
			this.rbtnMediumResolution.Size = new System.Drawing.Size(119, 29);
			this.rbtnMediumResolution.TabIndex = 8;
			this.rbtnMediumResolution.TabStop = true;
			this.rbtnMediumResolution.Text = "Medium";
			this.rbtnMediumResolution.UseVisualStyleBackColor = true;
			this.rbtnMediumResolution.CheckedChanged += new System.EventHandler(this.rbtnMediumResolution_CheckedChanged);
			// 
			// rbtnFineResolution
			// 
			this.rbtnFineResolution.AutoSize = true;
			this.rbtnFineResolution.Location = new System.Drawing.Point(44, 457);
			this.rbtnFineResolution.Name = "rbtnFineResolution";
			this.rbtnFineResolution.Size = new System.Drawing.Size(85, 29);
			this.rbtnFineResolution.TabIndex = 7;
			this.rbtnFineResolution.TabStop = true;
			this.rbtnFineResolution.Text = "Fine";
			this.rbtnFineResolution.UseVisualStyleBackColor = true;
			this.rbtnFineResolution.CheckedChanged += new System.EventHandler(this.rbtnFineResolution_CheckedChanged);
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(15, 293);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(246, 25);
			this.label4.TabIndex = 0;
			this.label4.Text = "Reverse In Out Direction";
			// 
			// cbxDoNotCalOnPowerUp
			// 
			this.cbxDoNotCalOnPowerUp.AutoSize = true;
			this.cbxDoNotCalOnPowerUp.Location = new System.Drawing.Point(351, 340);
			this.cbxDoNotCalOnPowerUp.Name = "cbxDoNotCalOnPowerUp";
			this.cbxDoNotCalOnPowerUp.Size = new System.Drawing.Size(28, 27);
			this.cbxDoNotCalOnPowerUp.TabIndex = 6;
			this.cbxDoNotCalOnPowerUp.UseVisualStyleBackColor = true;
			this.cbxDoNotCalOnPowerUp.CheckedChanged += new System.EventHandler(this.cbxDoNotCalOnPowerUp_CheckedChanged);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(15, 340);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(309, 50);
			this.label5.TabIndex = 0;
			this.label5.Text = "Do Not Calibrate Zero Point On\r\n                                  Power Up";
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(15, 168);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(259, 50);
			this.label3.TabIndex = 0;
			this.label3.Text = "Maximum Focuser Travel\r\n         Per Move (in steps):";
			// 
			// cbxReverseInOut
			// 
			this.cbxReverseInOut.AutoSize = true;
			this.cbxReverseInOut.Checked = global::ASCOM.FeatherTouchDH1.Properties.Settings.Default.ReverseInOut;
			this.cbxReverseInOut.Location = new System.Drawing.Point(351, 293);
			this.cbxReverseInOut.Name = "cbxReverseInOut";
			this.cbxReverseInOut.Size = new System.Drawing.Size(28, 27);
			this.cbxReverseInOut.TabIndex = 5;
			this.toolTip1.SetToolTip(this.cbxReverseInOut, "Reverses the meaning of IN and OUT for focuser travel");
			this.cbxReverseInOut.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(13, 90);
			this.label1.MinimumSize = new System.Drawing.Size(250, 0);
			this.label1.Name = "label1";
			this.label1.RightToLeft = System.Windows.Forms.RightToLeft.No;
			this.label1.Size = new System.Drawing.Size(250, 50);
			this.label1.TabIndex = 0;
			this.label1.Text = "Maximum Total Focuser\r\n             Travel (in steps)";
			// 
			// cmbxModel
			// 
			this.cmbxModel.BackColor = System.Drawing.Color.Black;
			this.cmbxModel.DataBindings.Add(new System.Windows.Forms.Binding("ValueMember", global::ASCOM.FeatherTouchDH1.Properties.Settings.Default, "FocuserModel", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.cmbxModel.DisplayMember = "FTF2016";
			this.cmbxModel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cmbxModel.ForeColor = System.Drawing.Color.Yellow;
			this.cmbxModel.FormattingEnabled = true;
			this.cmbxModel.Location = new System.Drawing.Point(236, 30);
			this.cmbxModel.Name = "cmbxModel";
			this.cmbxModel.Size = new System.Drawing.Size(201, 39);
			this.cmbxModel.TabIndex = 1;
			this.cmbxModel.Text = "AP27F0C3E";
			this.toolTip1.SetToolTip(this.cmbxModel, "Select the correct Feather Touch  focuser model.");
			this.cmbxModel.ValueMember = global::ASCOM.FeatherTouchDH1.Properties.Settings.Default.FocuserModel;
			this.cmbxModel.SelectedIndexChanged += new System.EventHandler(this.cmbxModel_SelectedIndexChanged);
			this.cmbxModel.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.cmbxModel_KeyPress);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.tbxPerDegreesC);
			this.groupBox1.Controls.Add(this.tbxMoveSteps);
			this.groupBox1.Controls.Add(this.label11);
			this.groupBox1.Controls.Add(this.radioButtonOut);
			this.groupBox1.Controls.Add(this.radioButtonIn);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Location = new System.Drawing.Point(57, 945);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(484, 159);
			this.groupBox1.TabIndex = 3;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Temperature Compensation";
			// 
			// tbxPerDegreesC
			// 
			this.tbxPerDegreesC.BackColor = System.Drawing.Color.Black;
			this.tbxPerDegreesC.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxPerDegreesC.ForeColor = System.Drawing.Color.Yellow;
			this.tbxPerDegreesC.Location = new System.Drawing.Point(125, 91);
			this.tbxPerDegreesC.Name = "tbxPerDegreesC";
			this.tbxPerDegreesC.Size = new System.Drawing.Size(81, 38);
			this.tbxPerDegreesC.TabIndex = 4;
			this.toolTip1.SetToolTip(this.tbxPerDegreesC, "Temperature Compensation does not appear to work.");
			this.tbxPerDegreesC.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxPerDegreesC_KeyDown);
			this.tbxPerDegreesC.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxPerDegreesC_KeyPress);
			// 
			// tbxMoveSteps
			// 
			this.tbxMoveSteps.BackColor = System.Drawing.Color.Black;
			this.tbxMoveSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tbxMoveSteps.ForeColor = System.Drawing.Color.Yellow;
			this.tbxMoveSteps.Location = new System.Drawing.Point(89, 35);
			this.tbxMoveSteps.Name = "tbxMoveSteps";
			this.tbxMoveSteps.Size = new System.Drawing.Size(117, 38);
			this.tbxMoveSteps.TabIndex = 1;
			this.toolTip1.SetToolTip(this.tbxMoveSteps, "Temperature Compensation does not appear to work.");
			this.tbxMoveSteps.KeyDown += new System.Windows.Forms.KeyEventHandler(this.tbxMoveSteps_KeyDown);
			this.tbxMoveSteps.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.tbxMoveSteps_KeyPress);
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(212, 44);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(151, 25);
			this.label11.TabIndex = 0;
			this.label11.Text = "Focuser Steps";
			// 
			// radioButtonOut
			// 
			this.radioButtonOut.AutoSize = true;
			this.radioButtonOut.Location = new System.Drawing.Point(378, 57);
			this.radioButtonOut.Name = "radioButtonOut";
			this.radioButtonOut.Size = new System.Drawing.Size(77, 29);
			this.radioButtonOut.TabIndex = 3;
			this.radioButtonOut.TabStop = true;
			this.radioButtonOut.Text = "Out";
			this.radioButtonOut.UseVisualStyleBackColor = true;
			// 
			// radioButtonIn
			// 
			this.radioButtonIn.AutoSize = true;
			this.radioButtonIn.Location = new System.Drawing.Point(377, 24);
			this.radioButtonIn.Name = "radioButtonIn";
			this.radioButtonIn.Size = new System.Drawing.Size(60, 29);
			this.radioButtonIn.TabIndex = 2;
			this.radioButtonIn.TabStop = true;
			this.radioButtonIn.Text = "In";
			this.radioButtonIn.UseVisualStyleBackColor = true;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(212, 100);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(253, 25);
			this.label10.TabIndex = 0;
			this.label10.Text = "Deg C Decrease In Temp";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(14, 100);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(105, 25);
			this.label9.TabIndex = 0;
			this.label9.Text = "For Every";
			// 
			// label8
			// 
			this.label8.AutoSize = true;
			this.label8.Location = new System.Drawing.Point(14, 44);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(65, 25);
			this.label8.TabIndex = 0;
			this.label8.Text = "Move";
			// 
			// groupBox2
			// 
			this.groupBox2.Controls.Add(this.nudBacklashSteps);
			this.groupBox2.Controls.Add(this.radioButtonTempFinalDirectionOut);
			this.groupBox2.Controls.Add(this.radioButtonTempFinalDirectionIn);
			this.groupBox2.Controls.Add(this.label14);
			this.groupBox2.Controls.Add(this.label13);
			this.groupBox2.Location = new System.Drawing.Point(57, 1137);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(379, 129);
			this.groupBox2.TabIndex = 4;
			this.groupBox2.TabStop = false;
			this.groupBox2.Text = "Backlash Settings";
			// 
			// nudBacklashSteps
			// 
			this.nudBacklashSteps.BackColor = System.Drawing.Color.Black;
			this.nudBacklashSteps.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudBacklashSteps.ForeColor = System.Drawing.Color.Yellow;
			this.nudBacklashSteps.Location = new System.Drawing.Point(187, 77);
			this.nudBacklashSteps.Name = "nudBacklashSteps";
			this.nudBacklashSteps.Size = new System.Drawing.Size(146, 38);
			this.nudBacklashSteps.TabIndex = 3;
			this.toolTip1.SetToolTip(this.nudBacklashSteps, "The number of steps for backlash compensation");
			// 
			// radioButtonTempFinalDirectionOut
			// 
			this.radioButtonTempFinalDirectionOut.AutoSize = true;
			this.radioButtonTempFinalDirectionOut.Location = new System.Drawing.Point(278, 38);
			this.radioButtonTempFinalDirectionOut.Name = "radioButtonTempFinalDirectionOut";
			this.radioButtonTempFinalDirectionOut.Size = new System.Drawing.Size(77, 29);
			this.radioButtonTempFinalDirectionOut.TabIndex = 2;
			this.radioButtonTempFinalDirectionOut.TabStop = true;
			this.radioButtonTempFinalDirectionOut.Text = "Out";
			this.radioButtonTempFinalDirectionOut.UseVisualStyleBackColor = true;
			// 
			// radioButtonTempFinalDirectionIn
			// 
			this.radioButtonTempFinalDirectionIn.AutoSize = true;
			this.radioButtonTempFinalDirectionIn.Location = new System.Drawing.Point(187, 36);
			this.radioButtonTempFinalDirectionIn.Name = "radioButtonTempFinalDirectionIn";
			this.radioButtonTempFinalDirectionIn.Size = new System.Drawing.Size(60, 29);
			this.radioButtonTempFinalDirectionIn.TabIndex = 1;
			this.radioButtonTempFinalDirectionIn.TabStop = true;
			this.radioButtonTempFinalDirectionIn.Text = "In";
			this.radioButtonTempFinalDirectionIn.UseVisualStyleBackColor = true;
			// 
			// label14
			// 
			this.label14.AutoSize = true;
			this.label14.Location = new System.Drawing.Point(11, 79);
			this.label14.Name = "label14";
			this.label14.Size = new System.Drawing.Size(161, 25);
			this.label14.TabIndex = 0;
			this.label14.Text = "Backlash Steps";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(15, 40);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(150, 25);
			this.label13.TabIndex = 0;
			this.label13.Text = "Final Direction";
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::ASCOM.FeatherTouchDH1.Properties.Resources.feathersmall;
			this.pictureBox1.Location = new System.Drawing.Point(44, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(508, 143);
			this.pictureBox1.TabIndex = 18;
			this.pictureBox1.TabStop = false;
			// 
			// toolTip1
			// 
			this.toolTip1.IsBalloon = true;
			// 
			// btnStaticSetPosition
			// 
			this.btnStaticSetPosition.BackColor = System.Drawing.Color.Teal;
			this.btnStaticSetPosition.ForeColor = System.Drawing.Color.White;
			this.btnStaticSetPosition.Location = new System.Drawing.Point(44, 43);
			this.btnStaticSetPosition.Name = "btnStaticSetPosition";
			this.btnStaticSetPosition.Size = new System.Drawing.Size(164, 47);
			this.btnStaticSetPosition.TabIndex = 19;
			this.btnStaticSetPosition.Text = "Sync";
			this.toolTip1.SetToolTip(this.btnStaticSetPosition, "Sets the Focuser position , no movement.");
			this.btnStaticSetPosition.UseVisualStyleBackColor = false;
			this.btnStaticSetPosition.Click += new System.EventHandler(this.btnStaticSetPosition_Click);
			// 
			// nudPosition
			// 
			this.nudPosition.BackColor = System.Drawing.Color.Black;
			this.nudPosition.Font = new System.Drawing.Font("Microsoft Sans Serif", 10.125F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nudPosition.ForeColor = System.Drawing.Color.Yellow;
			this.nudPosition.Location = new System.Drawing.Point(285, 45);
			this.nudPosition.Maximum = new decimal(new int[] {
            99999,
            0,
            0,
            0});
			this.nudPosition.Name = "nudPosition";
			this.nudPosition.Size = new System.Drawing.Size(169, 38);
			this.nudPosition.TabIndex = 20;
			this.toolTip1.SetToolTip(this.nudPosition, "Sets the Focuser position to this value.  No movement.");
			// 
			// chkTrace
			// 
			this.chkTrace.Checked = global::ASCOM.FeatherTouchDH1.Properties.Settings.Default.EnableLogging;
			this.chkTrace.CheckState = System.Windows.Forms.CheckState.Checked;
			this.chkTrace.DataBindings.Add(new System.Windows.Forms.Binding("Checked", global::ASCOM.FeatherTouchDH1.Properties.Settings.Default, "EnableLogging", true, System.Windows.Forms.DataSourceUpdateMode.OnPropertyChanged));
			this.chkTrace.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.chkTrace.Location = new System.Drawing.Point(197, 1301);
			this.chkTrace.Margin = new System.Windows.Forms.Padding(6);
			this.chkTrace.Name = "chkTrace";
			this.chkTrace.Size = new System.Drawing.Size(215, 33);
			this.chkTrace.TabIndex = 8;
			this.chkTrace.Text = "Enable Logging";
			this.chkTrace.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
			this.toolTip1.SetToolTip(this.chkTrace, "Check this box turn on logging.  The log file will be in the same directory as Fe" +
        "athreTouchDH1.dll");
			this.chkTrace.UseVisualStyleBackColor = true;
			// 
			// groupBox3
			// 
			this.groupBox3.Controls.Add(this.nudPosition);
			this.groupBox3.Controls.Add(this.btnStaticSetPosition);
			this.groupBox3.Location = new System.Drawing.Point(57, 807);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(483, 122);
			this.groupBox3.TabIndex = 21;
			this.groupBox3.TabStop = false;
			this.groupBox3.Text = "Sync Position";
			// 
			// SetupDialogForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.AutoSize = true;
			this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
			this.BackColor = System.Drawing.Color.Black;
			this.ClientSize = new System.Drawing.Size(649, 1450);
			this.ControlBox = false;
			this.Controls.Add(this.groupBox3);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.groupBox2);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.gbxFocuserSettings);
			this.Controls.Add(this.gbxConnection);
			this.Controls.Add(this.btnApply);
			this.Controls.Add(this.btnAbout);
			this.Controls.Add(this.chkTrace);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOK);
			this.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(192)))), ((int)(((byte)(0)))));
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
			this.Margin = new System.Windows.Forms.Padding(6);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "SetupDialogForm";
			this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
			this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
			this.Text = "FeatherTouchDH1 Setup";
			this.gbxConnection.ResumeLayout(false);
			this.gbxConnection.PerformLayout();
			this.gbxFocuserSettings.ResumeLayout(false);
			this.gbxFocuserSettings.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxIncrement)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudMaxTravel)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.groupBox2.ResumeLayout(false);
			this.groupBox2.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.nudBacklashSteps)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.nudPosition)).EndInit();
			this.groupBox3.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnOK;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox chkTrace;
		private System.Windows.Forms.CheckBox cbxReverseInOut;
		private System.Windows.Forms.ComboBox cmbxModel;
		private System.Windows.Forms.Button btnAbout;
		private System.Windows.Forms.Button btnApply;
		private System.Windows.Forms.GroupBox gbxConnection;
		private System.Windows.Forms.GroupBox gbxFocuserSettings;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.RadioButton rbtnCoarseResolution;
		private System.Windows.Forms.RadioButton rbtnMediumResolution;
		private System.Windows.Forms.RadioButton rbtnFineResolution;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.CheckBox cbxDoNotCalOnPowerUp;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.NumericUpDown nudMaxIncrement;
		private System.Windows.Forms.NumericUpDown nudMaxTravel;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.RadioButton radioButtonOut;
		private System.Windows.Forms.RadioButton radioButtonIn;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox tbxStepSizeMicrons;
		private System.Windows.Forms.TextBox tbxPerDegreesC;
		private System.Windows.Forms.TextBox tbxMoveSteps;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.NumericUpDown nudBacklashSteps;
		private System.Windows.Forms.RadioButton radioButtonTempFinalDirectionOut;
		private System.Windows.Forms.RadioButton radioButtonTempFinalDirectionIn;
		private System.Windows.Forms.Label label14;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.ToolTip toolTip1;
		private System.Windows.Forms.ComboBox cmbxCommPort;
		private System.Windows.Forms.Button btnStaticSetPosition;
		private System.Windows.Forms.NumericUpDown nudPosition;
		private System.Windows.Forms.GroupBox groupBox3;
	}
}