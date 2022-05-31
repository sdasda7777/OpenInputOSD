using System;
using System.Drawing;
using System.Windows.Forms;

using System.Diagnostics;

namespace OpenInputOSD{
	public partial class Overlay : Form {
		
		private Point m_backupLocation;
		private Size m_backupSize;
		private ConsolidatedInputManager m_cim;
		private int m_rows, m_columns;
		private Color m_fillColor, m_outlineColor, m_textColor;
		
		public Overlay(int posX, int posY, int width, int height, int columns, int rows,
						Color fillColor, Color outlineColor, Color textColor,
		               ConsolidatedInputManager inputManager){
			
			InitializeComponent();
			
			m_rows = rows; m_columns = columns;
			m_fillColor = fillColor; m_outlineColor = outlineColor; m_textColor = textColor;
			
			//Debug.WriteLine("("+posX+","+posY+","+width+","+height+")");
			
			StartPosition = FormStartPosition.Manual;
			m_backupLocation = new Point(posX, posY);
			OverlayLocationChanged(null, null);
			m_backupSize = new Size(width, height);
			OverlayResize(null, null);
			
			FormBorderStyle = FormBorderStyle.None;
			TopMost = true;

			BackColor = Color.Magenta;
			TransparencyKey = Color.Magenta;
			panel1.BackColor = Color.Magenta;
			
			Paint += OverlayPaintEventHandler;
			
			m_cim = inputManager;
			m_cim.OnManagerStateChanged += OverlayCIMStateChanged;
		}
		
		public void Dispose(){
			m_cim.Dispose();
		}
		
		void OverlayLocationChanged(object sender, EventArgs ea){
			if(this.Location.X != m_backupLocation.X ||
			  this.Location.Y != m_backupLocation.Y)
				this.Location = new Point(m_backupLocation.X, m_backupLocation.Y);
		}
		
		void OverlayResize(object sender, EventArgs ea){
			if(this.Size.Width != m_backupSize.Width ||
			  this.Size.Height != m_backupSize.Height)
				this.Size = new Size(m_backupSize.Width, m_backupSize.Height);
			
			this.WindowState = FormWindowState.Normal;
		}
		
		void OverlayCIMStateChanged(){
			OverlayPaintEventHandler(null, null);
		}
		
		void OverlayPaintEventHandler(object sender, PaintEventArgs pea){
			Graphics g = panel1.CreateGraphics();
			g.FillRectangle(new SolidBrush(Color.Magenta), 0, 0, panel1.Width, panel1.Height);
			
			SolidBrush fillBrush = new SolidBrush(m_fillColor);
			Pen outlinePen = new Pen(m_outlineColor);
			SolidBrush textBrush = new SolidBrush(m_textColor);
			Font font = new Font(FontFamily.GenericSansSerif, 12);
			
			int column = 0;
			int row = 0;
			int column_width = panel1.Width / m_columns;
			int row_height = panel1.Height / m_rows;
			
			foreach(InputSource inputSource in m_cim.GetInputSources()){
				foreach(var input in inputSource.getState()){
					int stateHeight = (int)(row_height * input.Item2);
					//Debug.WriteLine(input.Item1 + ": " + input.Item2 + " (=" + stateHeight + ")");
					
					g.FillRectangle(fillBrush,
					                column * column_width, (row + 1) * row_height - stateHeight,
					               	column_width, stateHeight);
					g.DrawRectangle(outlinePen,
					                column * column_width, row * row_height,
					                column_width, row_height-1);
					
					SizeF strSize = g.MeasureString(input.Item1, font);
					g.DrawString(input.Item1, font, textBrush,
					             column * column_width + column_width / 2 - strSize.Width / 2,
					             row * row_height + row_height / 2 - strSize.Height / 2);
					
					++column;
					if(column >= m_columns){
						++row;
						column = 0;
					}
				}
				
				if(column != 0){
					++row;
					column = 0;
				}
			}
		}
	}
}
