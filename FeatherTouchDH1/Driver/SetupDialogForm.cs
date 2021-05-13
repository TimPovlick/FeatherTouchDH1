using ASCOM.FeatherTouchDH1;
using ASCOM.Utilities;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;


namespace ASCOM.FeatherTouchDH1
{
	[ComVisible(false)]                 // Form not registered for COM!
	public partial class SetupDialogForm : Form
	{
		/// <summary>
		/// A reference to the Focuser instance.
		/// </summary>
		readonly Focuser m_Focuser;

		readonly TraceLogger tl; // Holder for a reference to the driver's trace logger

		/// Singleton instance of our About Box.
		/// </summary>
		AboutFocuser m_About = new AboutFocuser();

		// Boolean flag used to determine when a character other than a number is entered.
		private bool nonNumberEntered = false;

		/// <summary>
		/// Verifies the key is a number, 0 through 9.
		/// </summary>
		/// <param name="e">The KeyPress event</param>
		/// <param name="AllowPeriod"></param>
		/// <returns>true if it is a number (period), false otherwise.</returns>
		private bool IsNumeric(KeyEventArgs e, bool AllowPeriod)
		{
			// Initialize the flag to false.
			nonNumberEntered = false;

			// Determine whether the keystroke is a number from the top of the keyboard.
			if (e.KeyCode < Keys.D0 || e.KeyCode > Keys.D9)
			{
				// Determine whether the keystroke is a number from the keypad.
				if (e.KeyCode < Keys.NumPad0 || e.KeyCode > Keys.NumPad9)
				{
					// Determine whether the keystroke is a backspace.
					if (e.KeyCode != Keys.Back)
					{
						if (AllowPeriod && e.KeyCode != Keys.OemPeriod)
						{
							// A non-numerical/period keystroke was pressed.
							// Set the flag to true and evaluate in KeyPress event.
							nonNumberEntered = true;
						}
						else
						{
							nonNumberEntered = true;
						}
					}
				}
			}

			// If shift key was pressed, it's not a number.
			if (Control.ModifierKeys == Keys.Shift)
			{
				nonNumberEntered = true;
			}

			return nonNumberEntered;
		}


		public SetupDialogForm(TraceLogger tlDriver, Focuser focuser)
		{
			InitializeComponent();

			m_Focuser = focuser;

			// Save the provided trace logger for use within the setup dialog
			tl = tlDriver;

			// Initialize current values of user settings from the ASCOM Profile
			InitUI();
		}

		private void SaveValues()
		{
			Cursor currentCursor = Cursor.Current;

			// Show busy cursor.
			if (currentCursor != null)
				Cursor.Current = Cursors.WaitCursor;
			//
			// Update the property settings with results from the dialog
			//
			string port = (string)cmbxCommPort.SelectedItem;
			Properties.Settings.Default.COMPort = port;
			Focuser.ComPort = port;

			// Focuser model and indexer.
			Properties.Settings.Default.FocuserModelIndex = cmbxModel.SelectedIndex;
			Properties.Settings.Default.FocuserModel = cmbxModel.Text;

			// Maximum focuser travel.
			Properties.Settings.Default.MaxStep = (int)nudMaxTravel.Value;

			// Maximum steps in one move.
			Properties.Settings.Default.MaxIncrement = (int)nudMaxIncrement.Value;

			Properties.Settings.Default.StepSize = Convert.ToSingle(tbxStepSizeMicrons.Text);

			Properties.Settings.Default.ReverseInOut = cbxReverseInOut.Checked;
			Properties.Settings.Default.NoCalibrationOnPowerUp = cbxDoNotCalOnPowerUp.Checked;

			// Focuser move resolution (Fine, Medium or Coarse).
			if (rbtnFineResolution.Checked)
			{
				Properties.Settings.Default.StepResolution = "Fine";
			}
			else if (rbtnMediumResolution.Checked)
			{
				Properties.Settings.Default.StepResolution = "Medium";
			}
			else
			{
				Properties.Settings.Default.StepResolution = "Coarse";
			}

			//
			// Temperature Compensation stuff.
			//
		 	Properties.Settings.Default.DeltaSteps =  Convert.ToInt32(tbxMoveSteps.Text);
			Properties.Settings.Default.DeltaT = Convert.ToSingle(tbxPerDegreesC.Text);

			if (radioButtonTempFinalDirectionIn.Checked)
			{
				Properties.Settings.Default.FinalDirectionIsOut = false;
			}
			else
			{
				Properties.Settings.Default.FinalDirectionIsOut = true;
			}

			// Backlash settings.
			Properties.Settings.Default.BacklashSteps = Convert.ToInt32(nudBacklashSteps.Value);
			Properties.Settings.Default.FinalDirectionIsOut = radioButtonTempFinalDirectionOut.Checked;

			// Logging enable/disable.
			tl.Enabled = chkTrace.Checked;

			Properties.Settings.Default.Save();

			// Return user's cursor.
			Cursor.Current = currentCursor;
		}

		/// <summary>
		/// This method opens a browser to the ASCOM website.
		/// Not used, keep for future needs.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BrowseToAscom(object sender, EventArgs e) // Click on ASCOM logo event handler
		{
			try
			{
				System.Diagnostics.Process.Start("https://ascom-standards.org/");
			}
			catch (System.ComponentModel.Win32Exception noBrowser)
			{
				if ((uint)noBrowser.ErrorCode == 0x80004005)
					MessageBox.Show(noBrowser.Message);
			}
			catch (System.Exception other)
			{
				MessageBox.Show(other.Message);
			}
		}

		private void InitUI()
		{
			chkTrace.Checked = Properties.Settings.Default.EnableLogging;

			//
			// Set the list of serial ports to those that are currently available.
			//
			cmbxCommPort.Items.Clear();
			cmbxCommPort.Items.AddRange(new ASCOM.Utilities.Serial().AvailableCOMPorts);

			if (cmbxCommPort.Items.Contains(Focuser.ComPort))
			{
				cmbxCommPort.SelectedItem = Focuser.ComPort;
			}
			else if (cmbxCommPort.Items.Count > 0)
			{
				cmbxCommPort.SelectedItem = cmbxCommPort.Items[0];
			}
			else
			{
				MessageBox.Show("ERROR! No serial COM port detected.");
			}

			// Focuser model numbers
			foreach (Focuser.FocuserDataType model in m_Focuser.FocuserModels.Values)
			{
				cmbxModel.Items.Add(model.ModelNumber);
			}

			// Select the focuser model.
			cmbxModel.SelectedIndex = Properties.Settings.Default.FocuserModelIndex;

			nudMaxTravel.Value = Convert.ToDecimal(Properties.Settings.Default.MaxStep);
			nudMaxIncrement.Value = Properties.Settings.Default.MaxIncrement;
			tbxStepSizeMicrons.Text = Properties.Settings.Default.StepSize.ToString();
			cbxReverseInOut.Checked = Properties.Settings.Default.ReverseInOut;
			cbxDoNotCalOnPowerUp.Checked = Properties.Settings.Default.NoCalibrationOnPowerUp;

			//
			// Focuser move resolution (Fine, Medium or Coarse).
			//
			if (Properties.Settings.Default.StepResolution == "Fine")
			{
				rbtnFineResolution.Checked = true;
				rbtnMediumResolution.Checked = false;
				rbtnCoarseResolution.Checked = false;
			}
			else if (Properties.Settings.Default.StepResolution == "Medium")
			{
				rbtnMediumResolution.Checked = true;
				rbtnFineResolution.Checked = false;
				rbtnCoarseResolution.Checked = false;
			}
			else
			{
				rbtnCoarseResolution.Checked = true;
				rbtnFineResolution.Checked = false;
				rbtnMediumResolution.Checked = false;
			}

			//
			// Temperature Compensation settings.
			//
			tbxMoveSteps.Text = Properties.Settings.Default.DeltaSteps.ToString();
			tbxPerDegreesC.Text = Properties.Settings.Default.DeltaT.ToString();

			if (Properties.Settings.Default.FinalDirectionIsOut)
			{
				radioButtonTempFinalDirectionIn.Checked = false;
				radioButtonTempFinalDirectionOut.Checked = true;
			}
			else
			{
				radioButtonTempFinalDirectionIn.Checked = true;
				radioButtonTempFinalDirectionOut.Checked = false;
			}

			//
			// Backlash settings.
			//
			nudBacklashSteps.Value =  Properties.Settings.Default.BacklashSteps;

			if (Properties.Settings.Default.BacklashFinalDirectionIsOut)
			{
				radioButtonIn.Checked = false;
				radioButtonOut.Checked = true;
			}
			else
			{
				radioButtonIn.Checked = true;
				radioButtonOut.Checked = false;
			}

			// Logging enable/disable.
			chkTrace.Checked = Properties.Settings.Default.EnableLogging = chkTrace.Checked;
		}

		/// <summary>
		/// Sets various parameters based on user input.
		/// </summary>
		void SetFocuserParameters()
		{
			// Load Step Resolution
			string strStepResolution;
			if (rbtnCoarseResolution.Checked)
				strStepResolution = "Coarse";
			else if (rbtnMediumResolution.Checked)
				strStepResolution = "Medium";
			else
				strStepResolution = "Fine";

			Focuser.ModelNumberEnum modelEnum = (Focuser.ModelNumberEnum)cmbxModel.SelectedItem;

			m_Focuser.DetermineFocuserLength(modelEnum);
			m_Focuser.DetermineStepResolution(strStepResolution);

			tbxStepSizeMicrons.ReadOnly = true;
			Focuser.FocuserDataType model = m_Focuser.FocuserModels[modelEnum];
			tbxStepSizeMicrons.Text = (model.MicronsPerStep * m_Focuser.StepResolution).ToString();
			nudMaxTravel.Text = (model.StepCount / m_Focuser.StepResolution).ToString();
			nudMaxIncrement.Value = Convert.ToInt32(nudMaxTravel.Text);

			// TODO:  Some sort of UI that needs to be implement?
			// TODO:  Calibrate Zero?
			//cmdCalibrate.Visible = false;
			//cmdCalibrate.Enabled = false;
			//cmdInit.Visible = false;
			//cmdInit2.Visible = true;

			switch (modelEnum)
			{
				case Focuser.ModelNumberEnum.FTF2008BCR:
				case Focuser.ModelNumberEnum.FTF2008:
					break;

				case Focuser.ModelNumberEnum.FTF2015:
				case Focuser.ModelNumberEnum.FTF2020:
				case Focuser.ModelNumberEnum.FTF2025:
				case Focuser.ModelNumberEnum.FTF3545:
				case Focuser.ModelNumberEnum.AP27FTMU:
				case Focuser.ModelNumberEnum.AP4FOC3E:
				case Focuser.ModelNumberEnum.AP27FOC3E:
					if (Convert.ToInt32(nudMaxTravel.Text) <= UInt16.MaxValue)
						nudMaxIncrement.Value = Convert.ToDecimal(nudMaxTravel.Text);
					else
						nudMaxIncrement.Value = UInt16.MaxValue;

					break;

				case Focuser.ModelNumberEnum.Other:
					tbxStepSizeMicrons.Text = "6.4";
					nudMaxTravel.Text = Properties.Settings.Default.MaxStep.ToString();
					nudMaxIncrement.Text = Properties.Settings.Default.MaxIncrement.ToString();

					tbxStepSizeMicrons.ReadOnly = false;
					//cmdCalibrate.Visible = true;
					//cmdCalibrate.Enabled = true;
					//cmdInit2.Visible = false;
					//cmdInit.Visible = true;

					break;
			}
		}

		private void btnApply_Click(object sender, EventArgs e)
		{
			SaveValues();
		}

		private void btnOK_Click(object sender, EventArgs e)
		{
			SaveValues();
		}

		/// <summary>
		/// This method is called when the user clicks the Cancel button.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e">MouseEventArgs</param>
		private void btnCancel_Click(object sender, EventArgs e)
		{
			// Restore the previous property settings.
			m_Focuser.ReadProfile();
		}

		private void tbxStepSizeMicrons_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Check for the flag being set in the KeyDown event.
			if (nonNumberEntered)
			{
				// Stop the character from being entered into the control since it is non-numerical.
				e.Handled = true;
			}
		}
		private void tbxStepSizeMicrons_KeyDown(object sender, KeyEventArgs e)
		{
			IsNumeric(e, true);
		}

		private void tbxMoveSteps_KeyDown(object sender, KeyEventArgs e)
		{
			IsNumeric(e, false);
		}

		private void tbxMoveSteps_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Check for the flag being set in the KeyDown event.
			if (nonNumberEntered)
			{
				// Stop the character from being entered into the control since it is non-numerical.
				e.Handled = true;
			}
		}

		private void tbxPerDegreesC_KeyDown(object sender, KeyEventArgs e)
		{
			IsNumeric(e, false);
		}

		private void tbxPerDegreesC_KeyPress(object sender, KeyPressEventArgs e)
		{
			// Check for the flag being set in the KeyDown event.
			if (nonNumberEntered)
			{
				// Stop the character from being entered into the control since it is non-numerical.
				e.Handled = true;
			}
		}

		/// <summary>
		/// Called when the user is trying to change the model number.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmbxModel_KeyPress(object sender, KeyPressEventArgs e)
		{
			// THe model number is Read-Only!
		 	e.Handled = true;
		}

		private void cbxDoNotCalOnPowerUp_CheckedChanged(object sender, EventArgs e)
		{
			// cbxDoNotCalOnPowerUp.Checked = true;
		}

		/// <summary>
		/// Don't allow the user to edit the COMM Port entries.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmbxCommPort_KeyPress(object sender, KeyPressEventArgs e)
		{
			e.Handled = true;
		}

		/// <summary>
		/// The user has selected a different Focuser.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cmbxModel_SelectedIndexChanged(object sender, EventArgs e)
		{
			// Focuser model and indexer.
			Focuser.ModelNumberEnum ModelNumber = (Focuser.ModelNumberEnum)cmbxModel.SelectedItem;
			Properties.Settings.Default.FocuserModel = ModelNumber.ToString();

			Focuser.FocuserDataType model;
			if (m_Focuser.m_FocuserTypes.TryGetValue(ModelNumber, out model))
			{
				SetFocuserParameters();
			}
		}

		private void rbtnFineResolution_CheckedChanged(object sender, EventArgs e)
		{
			if (rbtnFineResolution.Checked)
				SetFocuserParameters();
		}

		private void rbtnMediumResolution_CheckedChanged(object sender, EventArgs e)
		{
			if (rbtnMediumResolution.Checked)
				SetFocuserParameters();
		}
		private void rbtnCoarseResolution_CheckedChanged(object sender, EventArgs e)
		{
			if (rbtnCoarseResolution.Checked)
				SetFocuserParameters();
		}
		private void btnAbout_MouseDown(object sender, MouseEventArgs e)
		{
			m_About.ShowDialog();
		}

		private void btnStaticSetPosition_Click(object sender, EventArgs e)
		{
			Cursor currentCursor = Cursor.Current;

			try
			{
				if (currentCursor != null)
					Cursor.Current = Cursors.WaitCursor;

				btnStaticSetPosition.Enabled = false;
				Refresh();

				SaveValues();

				if (!m_Focuser.Connected)
					m_Focuser.Connected = true;	// Connect and stay connected.

				m_Focuser.SetFocuserPosition(nudPosition.Value.ToString());
			}
			catch (Exception ex)
			{
				MessageBox.Show(string.Format(ex.Message), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
			finally
			{
				btnStaticSetPosition.Enabled = true;
				Cursor.Current = currentCursor;
			}
		}
	}
}