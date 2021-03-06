using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenInputOSD {
	public class InputSourceMouse : InputSource {
		[DllImport("user32.dll")]
        private static extern int GetSystemMetrics(int index);
		
		private List<MuTuple<int, MuTuple<string, float>>> m_buttons;
		private List<MuTuple<string, float>> m_state;
		
		public InputSourceMouse(string inputTrackingString) {
			
			List<MuTuple<string, string>> splitAndParsed = InputSource.splitAndParseAliases(inputTrackingString);
			
			
			//System.Windows.Forms.Cursor.Position;
		}
		
		public override void Dispose(){}
		
		public override List<MuTuple<string, float>> getState(){
			return new List<MuTuple<string, float>>();
		}
	}
}
