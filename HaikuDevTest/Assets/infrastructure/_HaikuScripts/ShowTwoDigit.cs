using UnityEngine;
using System.Collections;

public class ShowTwoDigit : MonoBehaviour {
	public Sprite[] digits;

	public SpriteRenderer onesDigit;
	public SpriteRenderer tensDigit;

	public void ShowNumber(string number) {
		int tens = 0;
		int ones = 0;
		int.TryParse(number[0].ToString(), out tens);
		int.TryParse(number[1].ToString(), out ones);

		onesDigit.sprite = digits[ones];
		tensDigit.sprite = digits[tens];
	}

}
