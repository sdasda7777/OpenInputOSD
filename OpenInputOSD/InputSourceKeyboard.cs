using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace OpenInputOSD {
	public class InputSourceKeyboard : InputSource {
		[DllImport("User32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		private System.Windows.Forms.Timer m_timer;
		private List<MuTuple<int, MuTuple<string, float>>> m_trackedKeys;
		private List<MuTuple<string, float>> m_trackedKeysStates;
		
		public InputSourceKeyboard(List<string> trackedKeyNames) {
						
			// Validate and store keys that should be tracked
			m_trackedKeys = new List<MuTuple<int, MuTuple<string, float>>>();
			m_trackedKeysStates = new List<MuTuple<string, float>>();
			foreach(string kn in trackedKeyNames){
				string keyEnumName = kn.Trim();
				string keyDisplayName = keyEnumName;
				
				if(keyEnumName.Contains("[") && keyEnumName.Contains("]")){
					int indexLeft = keyEnumName.IndexOf('[');
					int indexRight = keyEnumName.IndexOf(']');
					
					if(indexRight > indexLeft + 1){
						keyDisplayName = keyEnumName.Substring(indexLeft + 1, indexRight - indexLeft - 1).Trim();
						keyEnumName = keyEnumName.Substring(0, keyEnumName.IndexOf('[')).Trim();
					}
				}
				
				System.Windows.Forms.Keys key;
				if(Enum.TryParse<System.Windows.Forms.Keys>(keyEnumName, out key)){
					MuTuple<string, float> kstate = new MuTuple<string, float>(keyDisplayName, 0);
					
					m_trackedKeysStates.Add(kstate);
					m_trackedKeys.Add(new MuTuple<int, MuTuple<string, float>>((int) key, kstate));
				}
			}
			
			/*
			foreach(var k in m_keys){
				Console.WriteLine(k.Item2.Item1);
			}
			*/
			
			m_timer = new System.Windows.Forms.Timer();
			m_timer.Interval = 1;
			m_timer.Tick += timer_Tick;
			m_timer.Start();
		}
		
		public override void Dispose(){
			m_timer.Stop();
			m_timer.Dispose();
		}
				
		public override List<MuTuple<string, float>> getState(){
			return m_trackedKeysStates;
		}
		
		private void timer_Tick(object sender, EventArgs ea){
			bool stateChanged = false;
			
			for(int ii = 0; ii < m_trackedKeys.Count; ++ii){
				int state = ((GetAsyncKeyState(m_trackedKeys[ii].Item1) >> 15) == -1 ? 1 : 0);
				if(state != m_trackedKeys[ii].Item2.Item2){
					m_trackedKeys[ii].Item2.Item2 = state;
					stateChanged = true;
				}
			}
			
			if(stateChanged){
				SignalStateChange();
			}
		}
	}
}
