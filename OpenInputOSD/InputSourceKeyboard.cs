using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

using System.Diagnostics;

namespace OpenInputOSD {
	public class InputSourceKeyboard : InputSource {
		[DllImport("User32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		private System.Windows.Forms.Timer m_timer1;
		private List<MuTuple<int, MuTuple<string, float>>> m_keys;
		private List<MuTuple<string, float>> m_state;
		
		public InputSourceKeyboard(List<string> keyNames) {
			m_state = new List<MuTuple<string, float>>();
			
			m_keys = new List<MuTuple<int, MuTuple<string, float>>>();
			foreach(string kn in keyNames){
				System.Windows.Forms.Keys key;
				if(Enum.TryParse<System.Windows.Forms.Keys>(kn, out key)){
					Debug.WriteLine("key parsed");
					
					MuTuple<string, float> kstate = new MuTuple<string, float>(kn, 0);
					
					m_state.Add(kstate);
					m_keys.Add(new MuTuple<int, MuTuple<string, float>>((int) key, kstate));
				}
			}
			
			foreach(var k in m_keys){
				Debug.WriteLine(k.Item2.Item1);
			}
			
			m_timer1 = new System.Windows.Forms.Timer();
			m_timer1.Interval = 1;
			m_timer1.Tick += timer1_Tick;
			m_timer1.Start();
		}
		
		public override void Dispose(){
			m_timer1.Stop();
			m_timer1.Dispose();
		}
				
		public override List<MuTuple<string, float>> getState(){
			return m_state;
		}
		
		private void timer1_Tick(object sender, EventArgs ea){
			bool stateChanged = false;
			
			for(int ii = 0; ii < m_keys.Count; ++ii){
				int state = ((GetAsyncKeyState(m_keys[ii].Item1) >> 15) == -1 ? 1 : 0);
				if(state != m_keys[ii].Item2.Item2){
					m_keys[ii].Item2.Item2 = state;
					stateChanged = true;
				}
			}
			
			if(stateChanged){
				SignalStateChange();
			}
		}
	}
}
