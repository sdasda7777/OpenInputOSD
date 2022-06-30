using System;
using System.Collections.Generic;

namespace OpenInputOSD {
	public delegate void InputSignal();
	
	public abstract class InputSource : IDisposable {
		
		public abstract void Dispose();
		public abstract List<MuTuple<string, float>> getState();
		
		public event InputSignal OnInputStateChanged;
		public void SignalStateChange(){
			OnInputStateChanged();
		}
	}
}
