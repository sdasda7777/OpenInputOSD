using System;
using System.Collections.Generic;

namespace OpenInputOSD {
	public class InputSourceMouse : InputSource {
		public InputSourceMouse() {
			
		}
		
		public override void Dispose(){}
		
		public override List<MuTuple<string, float>> getState(){
			return new List<MuTuple<string, float>>();
		}
	}
}
