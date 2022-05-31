using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows;
using System.Windows.Forms;

using System.Diagnostics;

namespace OpenInputOSD {
	public partial class MainForm : Form {
		Overlay m_overlay = null;
		
		Color m_fill = Color.White;
		Color m_outline = Color.Black;
		Color m_text = Color.Red;
		
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
		
		void Button1Click(object sender, EventArgs e) {
			if(m_overlay == null){
				
				List<InputSource> inputSources = new List<InputSource>();
				
				if(checkBox1.Checked)
					inputSources.Add(
						new InputSourceKeyboard(
							new List<string>(textBox1.Text.Split(','))));
					
				if(checkBox2.Checked) inputSources.Add(new InputSourceMouse());
				
				if(checkBox3.Checked) inputSources.Add(new InputSourceGamepad());
				
				ConsolidatedInputManager cim = new ConsolidatedInputManager(inputSources);
				
				m_overlay = new Overlay((int) numericUpDown1.Value, (int) numericUpDown2.Value,
				                    	(int) numericUpDown3.Value, (int) numericUpDown4.Value,
				                    	(int) numericUpDown5.Value, (int) numericUpDown6.Value,
				                    	m_fill, m_outline, m_text, cim);
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
		
		void applyColorsToButtons() {
			button2.BackColor = m_fill;
			button2.ForeColor = Color.FromArgb(m_fill.ToArgb()^0xffffff);
			button3.BackColor = m_outline;
			button3.ForeColor = Color.FromArgb(m_outline.ToArgb()^0xffffff);
			button4.BackColor = m_text;
			button4.ForeColor = Color.FromArgb(m_text.ToArgb()^0xffffff);
		}
		
		void Button2Click(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_fill = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void Button3Click(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_outline = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
		
		void Button4Click(object sender, EventArgs e) {
			if(colorDialog1.ShowDialog() == DialogResult.OK){
				m_text = colorDialog1.Color;
				applyColorsToButtons();
			}
		}
	}
}
