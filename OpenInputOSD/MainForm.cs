using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

namespace OpenInputOSD {
	public partial class MainForm : Form {
		Overlay m_overlay = null;
		
		Color m_cellFillUnpressed = Color.Transparent;
		Color m_cellFillPressed = Color.White;
		Color m_cellOutline = Color.Black;
		Color m_textFillUnpressed = Color.White;
		Color m_textFillPressed = Color.Red;
		Color m_textOutline = Color.Black;
		
		public MainForm() {
			InitializeComponent();
			
			numericUpDown1.Minimum = 0;
			numericUpDown1.Maximum = (decimal) SystemInformation.VirtualScreen.Width;
			numericUpDown2.Minimum = 0;
			numericUpDown2.Maximum = (decimal) SystemInformation.VirtualScreen.Height;
			
			numericUpDown3.Minimum = 0;
			numericUpDown3.Maximum = (decimal) SystemInformation.VirtualScreen.Width;
			numericUpDown3.Value = numericUpDown3.Maximum / 4;
			numericUpDown4.Minimum = 0;
			numericUpDown4.Maximum = (decimal) SystemInformation.VirtualScreen.Height;
			numericUpDown4.Value = numericUpDown4.Maximum / 4;
			
			applyColorsToButtons();
		}
		
		void b_StartStopOverlay(object sender, EventArgs e) {
			if(m_overlay == null){
				
				List<InputSource> inputSources = new List<InputSource>();
				
				if(checkBox1.Checked)
					inputSources.Add(
						new InputSourceKeyboard(
							new List<string>(textBox1.Text.Split(','))));
					
				if(checkBox2.Checked)
					inputSources.Add(
						new InputSourceMouse(
							new List<string>(textBox2.Text.Split(','))));
				
				if(checkBox3.Checked) inputSources.Add(new InputSourceGamepad());
				
				ConsolidatedInputManager cim = new ConsolidatedInputManager(inputSources);
				
				m_overlay = new Overlay((int) numericUpDown1.Value, (int) numericUpDown2.Value,
				                    	(int) numericUpDown3.Value, (int) numericUpDown4.Value,
				                    	(int) numericUpDown5.Value, (int) numericUpDown6.Value,
				                    	m_cellFillUnpressed, m_cellFillPressed, m_cellOutline,
				                    	(int) numericUpDown7.Value, (int) numericUpDown8.Value,
				                    	m_textFillUnpressed, m_textFillPressed, m_textOutline, 
				                    	cim);
				m_overlay.Show();
				
				button1.Text = "Stop Overlay";
			}else{
				//m_overlay.
				m_overlay.Close();
				m_overlay.Dispose();
				m_overlay = null;
				button1.Text = "Start Overlay";
			}
		}
		
		bool colorIsDark(Color c){
			/* Values taken from Miral's (https://stackoverflow.com/users/43534/miral)
				reply to https://stackoverflow.com/a/3943023 */
			return (c.R * 0.299 + c.G * 0.587 + c.B * 0.114) > 186;
		}
		
		void applyColorsToButtons() {
			button2.BackColor = m_cellFillUnpressed;
			button3.BackColor = m_cellFillPressed;
			button4.BackColor = m_cellOutline;
			button5.BackColor = m_textFillUnpressed;
			button6.BackColor = m_textFillPressed;
			button7.BackColor = m_textOutline;
			
			button2.ForeColor = (colorIsDark(m_cellFillUnpressed) ? Color.Black : Color.White);
			button3.ForeColor = (colorIsDark(m_cellFillPressed) ? Color.Black : Color.White);
			button4.ForeColor = (colorIsDark(m_cellOutline) ? Color.Black : Color.White);
			button5.ForeColor = (colorIsDark(m_textFillUnpressed) ? Color.Black : Color.White);
			button6.ForeColor = (colorIsDark(m_textFillPressed) ? Color.Black : Color.White);
			button7.ForeColor = (colorIsDark(m_textOutline) ? Color.Black : Color.White);
		}
		
		void b_CellFillUnpressed(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_cellFillUnpressed = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void b_CellFillPressed(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_cellFillPressed = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void b_CellOutline(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_cellOutline = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void b_TextFillUnpressed(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_textFillUnpressed = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void b_TextFillPressed(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_textFillPressed = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void b_TextOutline(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_textOutline = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
	}
}
