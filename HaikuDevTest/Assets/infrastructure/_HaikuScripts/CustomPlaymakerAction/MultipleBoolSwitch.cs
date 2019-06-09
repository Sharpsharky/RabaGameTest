namespace HutongGames.PlayMaker.Actions{
    
	[ActionCategory("_Common")]
	[Tooltip("Takes an array of bool variables and sends different events depending on which ones are true or false")]
	public class MultipleBoolSwitch : FsmStateAction{

		[UIHintAttribute(UIHint.Variable)]
		public FsmBool[] bools;

		public int[] intValues;
		public FsmEvent[] optionEvents;

		public FsmEvent noMatchEvent;

		public override void OnEnter(){
			int value = GetIntFromFsmBoolArray ();
			for (int i = 0; i < intValues.Length; ++i) {
				if (intValues [i] == value) {
					Fsm.Event (optionEvents [i]);
					Finish ();
					return;
				}
			}

			Fsm.Event (noMatchEvent);
			Finish ();
		}

		int GetIntFromFsmBoolArray(){
			int value = 0;
			for (int i = 0; i < bools.Length; ++i) {
				if (bools[i].Value) {
					value += (1 << i);
				}
			}
			return value;
		}

		public bool[] GetBoolArrayFromInt(int pInt){
			bool[] result = new bool[bools.Length];
			for (int i = 0; i < result.Length; ++i) {
				result [i] = (pInt & (1 << i)) != 0;
			}
			return result;
		}

		public int GetIntFromBoolArray(bool[] pBools){
			int value = 0;
			for (int i = 0; i < pBools.Length; ++i) {
				if (pBools [i]) {
					value += (1 << i);
				}
			}
			return value;
		}
	}
}