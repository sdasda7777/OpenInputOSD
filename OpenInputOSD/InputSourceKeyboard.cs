using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

//using System.Diagnostics;

namespace OpenInputOSD {
	public class InputSourceKeyboard : InputSource {
		[DllImport("User32.dll")]
		private static extern short GetAsyncKeyState(int vKey);

		private System.Windows.Forms.Timer m_timer1;
		private bool m_addNewKeysAutomatically;
		private List<MuTuple<int, MuTuple<string, float>>> m_trackedKeys;
		private List<MuTuple<string, float>> m_trackedKeysStates;
		private HashSet<string> m_ignored;
		
		public InputSourceKeyboard(List<string> trackedKeyNames, 
		                           bool addNewKeysAutomatically,
		                           List<string> trackingExceptionsKeyNames) {
			
			m_addNewKeysAutomatically = addNewKeysAutomatically;
			
			// Validate and store keys that should be ignored
			m_ignored = new HashSet<string>();
			foreach(string kn in trackingExceptionsKeyNames){
				System.Windows.Forms.Keys key;
				if(Enum.TryParse<System.Windows.Forms.Keys>(kn, out key)){
					m_ignored.Add(kn);
				}
			}
			
			// Validate and store keys that should be tracked
			m_trackedKeys = new List<MuTuple<int, MuTuple<string, float>>>();
			m_trackedKeysStates = new List<MuTuple<string, float>>();
			foreach(string kn in trackedKeyNames){
				System.Windows.Forms.Keys key;
				if(Enum.TryParse<System.Windows.Forms.Keys>(kn, out key) && !m_ignored.Contains(kn)){
					MuTuple<string, float> kstate = new MuTuple<string, float>(kn, 0);
					
					m_trackedKeysStates.Add(kstate);
					m_trackedKeys.Add(new MuTuple<int, MuTuple<string, float>>((int) key, kstate));
				}
			}
			
			/*
			foreach(var k in m_keys){
				Debug.WriteLine(k.Item2.Item1);
			}
			*/
			
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
			return m_trackedKeysStates;
		}
		
		private void timer1_Tick(object sender, EventArgs ea){
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
