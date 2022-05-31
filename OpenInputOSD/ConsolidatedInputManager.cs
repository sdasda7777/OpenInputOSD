using System;
using System.Collections.Generic;

namespace OpenInputOSD {
	public delegate void ManagerSignal();
	
	public class ConsolidatedInputManager : IDisposable {
		List<InputSource> m_inputSources;
		
		public ConsolidatedInputManager(List<InputSource> inputSources){
			m_inputSources = inputSources;
			
			foreach(InputSource inputSource in m_inputSources){
				inputSource.OnInputStateChanged += ManagerStateChanged;
			}
		}
		
		public void Dispose(){
			foreach(InputSource inputSource in m_inputSources)
				inputSource.Dispose();
		}
		
		public List<InputSource> GetInputSources(){
			return m_inputSources;
		}
		
		private void ManagerStateChanged(){
			OnManagerStateChanged();
		}
		
		public event ManagerSignal OnManagerStateChanged;
	}
}
