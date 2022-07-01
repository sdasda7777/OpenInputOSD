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
		
		public InputSourceKeyboard(string inputTrackingString) {
						
			// Validate and store keys that should be tracked
			m_trackedKeys = new List<MuTuple<int, MuTuple<string, float>>>();
			m_trackedKeysStates = new List<MuTuple<string, float>>();
			
			List<MuTuple<string, string>> splitAndParsed = InputSource.splitAndParseAliases(inputTrackingString);
			foreach(MuTuple<string, string> sap in splitAndParsed){				
				System.Windows.Forms.Keys key;
				if(Enum.TryParse<System.Windows.Forms.Keys>(sap.Item1, out key)){
					MuTuple<string, float> kstate = new MuTuple<string, float>(sap.Item2, 0);
					
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
