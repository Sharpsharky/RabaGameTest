using UnityEngine;
using System.Collections;
using System.Linq;

public class ToggleWithVisualManager : MonoBehaviour {
	public ToggleWithVisualPiece[] column0;
	public ToggleWithVisualPiece[] column1;
	public ToggleWithVisualPiece[] column2;
	public ToggleWithVisualPiece[] column3;
	public ToggleWithVisualPiece[] column4;
	public ToggleWithVisualPiece[] column5;

	public SpriteRenderer column0sprite;
	public SpriteRenderer column1sprite;
	public SpriteRenderer column2sprite;
	public SpriteRenderer column3sprite;
	public SpriteRenderer column4sprite;
	public SpriteRenderer column5sprite;

	public Sprite on0000;
	public Sprite on1000;
	public Sprite on0100;
	public Sprite on0010;
	public Sprite on0001;
	public Sprite on1100;
	public Sprite on1010;
	public Sprite on1001;
	public Sprite on0110;
	public Sprite on0101;
	public Sprite on0011;
	public Sprite on1110;
	public Sprite on1101;
	public Sprite on1011;
	public Sprite on0111;
	public Sprite on1111;

	public PlayMakerFSM sendWonEvent;

	public AudioClip tapSound;

	// Use this for initialization
	void Start () {
	
	}


	public void PieceToggled(ToggleWithVisualPiece piece) {
		ToggleWithVisualPiece[] column = null;
		SpriteRenderer spriteToChange = null;
		if (column0.Contains(piece)) {
			column = column0;
			spriteToChange = column0sprite;
		} else if (column1.Contains(piece)) {
			column = column1;
			spriteToChange = column1sprite;
		} else if (column2.Contains(piece)) {
			column = column2;
			spriteToChange = column2sprite;
		} else if (column3.Contains(piece)) {
			column = column3;
			spriteToChange = column3sprite;
		} else if (column4.Contains(piece)) {
			column = column4;
			spriteToChange = column4sprite;
		} else if (column5.Contains(piece)) {
			column = column5;
			spriteToChange = column5sprite;
		}
		spriteToChange.sprite = RefreshWavesForColumn(column);
		Helper.PlayAudioIfSoundOn(tapSound);
		CheckIfWin();
	}

	void CheckIfWin() {
		ToggleWithVisualPiece[] allPieces = GetComponentsInChildren<ToggleWithVisualPiece>();
		foreach (ToggleWithVisualPiece piece in allPieces) {
			if (!piece.isCorrect()) {
				Debug.Log("Incorrect at " + piece.name);
				return;
			}
		}
		sendWonEvent.SendEvent("won");
	}

	Sprite RefreshWavesForColumn(ToggleWithVisualPiece[] column) {
		string waveString = (column[0].isPressed == true ? 1 : 0).ToString() + 
			(column[1].isPressed == true ? 1 : 0).ToString() +
			(column[2].isPressed == true ? 1 : 0).ToString() + 
				(column[3].isPressed == true ? 1 : 0).ToString();

		if (waveString.Equals("0000")) {
			return on0000;
		} else if (waveString.Equals("1000")) {
			return on1000;
		} else if (waveString.Equals("0100")) {
			return on0100;
		} else if (waveString.Equals("0010")) {
			return on0010;
		} else if (waveString.Equals("0001")) {
			return on0001;
		} else if (waveString.Equals("1100")) {
			return on1100;
		} else if (waveString.Equals("1010")) {
			return on1010;
		} else if (waveString.Equals("1001")) {
			return on1001;
		} else if (waveString.Equals("0110")) {
			return on0110;
		} else if (waveString.Equals("0101")) {
			return on0101;
		} else if (waveString.Equals("0011")) {
			return on0011;
		} else if (waveString.Equals("1110")) {
			return on1110;
		} else if (waveString.Equals("1101")) {
			return on1101;
		} else if (waveString.Equals("1011")) {
			return on1011;
		} else if (waveString.Equals("0111")) {
			return on0111;
		} else if (waveString.Equals("1111")) {
			return on1111;
		}
		return null;
	}
	// Update is called once per frame
	void Update () {
	
	}
}
