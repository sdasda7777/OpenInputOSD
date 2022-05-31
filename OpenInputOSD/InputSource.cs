using System;
using System.Collections.Generic;

namespace OpenInputOSD {
	public delegate void InputSignal();
	
	public class MuTuple<T1, T2>{
    	public MuTuple(T1 item1, T2 item2){
	        this.Item1 = item1;
	        this.Item2 = item2;
	    }

    	public T1 Item1 { get; set; }
	    public T2 Item2 { get; set; }
	}
	
	public abstract class InputSource : IDisposable {
		
		public abstract void Dispose();
		public abstract List<MuTuple<string, float>> getState();
		
		public event InputSignal OnInputStateChanged;
		public void SignalStateChange(){
			OnInputStateChanged();
		}
	}
}
