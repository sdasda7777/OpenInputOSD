using System;
using System.Collections.Generic;

namespace OpenInputOSD {
	public delegate void InputSignal();
	
	public abstract class InputSource : IDisposable {
		
		public abstract void Dispose();
		public abstract List<MuTuple<string, float>> getState();
		
		public static List<MuTuple<string, string>> splitAndParseAliases(string trackString){
			List<MuTuple<string, string>> ret = new List<MuTuple<string, string>>();
			
			foreach (string aliased in trackString.Split(',')){
			    string keyEnumName = aliased.Trim();
				string keyDisplayName = keyEnumName;
				
				if(keyEnumName.Contains("[") && keyEnumName.Contains("]")){
					int indexLeft = keyEnumName.IndexOf('[');
					int indexRight = keyEnumName.IndexOf(']');
					
					if(indexRight > indexLeft + 1){
						keyDisplayName = keyEnumName.Substring(indexLeft + 1, indexRight - indexLeft - 1).Trim();
						keyEnumName = keyEnumName.Substring(0, keyEnumName.IndexOf('[')).Trim();
					}
				}
				
				ret.Add(new MuTuple<string, string>(keyEnumName, keyDisplayName));
			}
			
			return ret;
		}
		
		public event InputSignal OnInputStateChanged;
		public void SignalStateChange(){
			OnInputStateChanged();
		}
	}
}
