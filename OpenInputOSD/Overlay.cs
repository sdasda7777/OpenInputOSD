using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Windows.Forms;
using System.Reflection;

namespace OpenInputOSD{
	public partial class Overlay : Form {
		
		private Point m_backupLocation;
		private Size m_backupSize;
		private ConsolidatedInputManager m_cim;
		private int m_rows, m_columns;
		private Color m_cellFillUnpressedColor, m_cellFillPressedColor, m_cellOutlineColor;
		private int m_textSize, m_textWidth;
		private Color m_textFillUnpressedColor, m_textFillPressedColor, m_textOutlineColor;
		
		public Overlay(int posX, int posY, int width, int height,
		               int columns, int rows, 
		               Color cellFillUnpressedColor, Color cellFillPressedColor, Color cellOutlineColor,
		               int textSize, int textWidth, 
		               Color textFillUnpressedColor, Color textFillPressedColor, Color textOutlineColor,
		               ConsolidatedInputManager inputManager){
			
			InitializeComponent();
						
			m_rows = rows; m_columns = columns;
			m_cellFillUnpressedColor = cellFillUnpressedColor;
			m_cellFillPressedColor = cellFillPressedColor;
			m_cellOutlineColor = cellOutlineColor;
			m_textSize = textSize; m_textWidth = textWidth;
			m_textFillUnpressedColor = textFillUnpressedColor;
			m_textFillPressedColor = textFillPressedColor;
			m_textOutlineColor = textOutlineColor;
			
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
						
			Invalidate();
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
			// Source: https://stackoverflow.com/a/5963710
			Graphics unbufferedGraphics = panel1.CreateGraphics();
			BufferedGraphicsContext bgc = new BufferedGraphicsContext();
			BufferedGraphics bg = bgc.Allocate(
				unbufferedGraphics,
				new Rectangle(new Point(0,0), unbufferedGraphics.VisibleClipBounds.Size.ToSize()));
			Graphics g = bg.Graphics;
			
			// The transparency mask is set to magenta, so clearing must be done with magenta 
			g.Clear(Color.Magenta);
			
			SolidBrush cellFillUnpressedBrush = new SolidBrush(m_cellFillUnpressedColor);
			SolidBrush cellFillPressedBrush = new SolidBrush(m_cellFillPressedColor);
			Pen cellOutlinePen = new Pen(m_cellOutlineColor);
			SolidBrush textFillUnpressedBrush = new SolidBrush(m_textFillUnpressedColor);
			SolidBrush textFillPressedBrush = new SolidBrush(m_textFillPressedColor);
			Pen textOutlinePen = new Pen(m_textOutlineColor, m_textWidth);
			Font font = new Font(FontFamily.GenericSansSerif, m_textSize);
			
			int column = 0;
			int row = 0;
			int column_width = panel1.Width / m_columns;
			int row_height = panel1.Height / m_rows;
			
			foreach(InputSource inputSource in m_cim.GetInputSources()){
				foreach(var input in inputSource.getState()){
					int pressedRectangleHeight = (int)(row_height * input.Item2);
					//Debug.WriteLine(input.Item1 + ": " + input.Item2 + " (=" + stateHeight + ")");
					
					g.FillRectangle(cellFillUnpressedBrush,
					                column * column_width, row * row_height,
					                column_width, (row_height - pressedRectangleHeight));
					g.FillRectangle(cellFillPressedBrush,
					                column * column_width, row * row_height + (row_height - pressedRectangleHeight),
					               	column_width, pressedRectangleHeight);
					g.DrawRectangle(cellOutlinePen,
					                column * column_width, row * row_height,
					                column_width, row_height-1);
					
					SizeF strSize = g.MeasureString(input.Item1, font);
					
					GraphicsPath p = new GraphicsPath(); 
					p.AddString(input.Item1, 
					    FontFamily.GenericSansSerif,
					    (int) FontStyle.Regular,      // font style (bold, italic, etc.)
					    g.DpiY * font.Size / 72,       // em size
					    new PointF(column * column_width + column_width / 2 - strSize.Width / 2,
					               row * row_height + row_height / 2 - strSize.Height / 2),
					    new StringFormat());          // set options here (e.g. center alignment)
					g.DrawPath(textOutlinePen, p);
					g.FillPath((input.Item2 < 0.5 ? textFillUnpressedBrush : textFillPressedBrush), p);
					
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
			
			bg.Render(unbufferedGraphics);
		}
	}
}
