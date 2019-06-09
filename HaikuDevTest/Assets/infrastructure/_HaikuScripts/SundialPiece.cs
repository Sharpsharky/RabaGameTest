using UnityEngine;
using System.Collections;

public class SundialPiece : MonoBehaviour {
	public GameObject[] sundialPositions;
	public int correctIndex;
	public SundialManager manager;
	public int startingIndex;

	private int index;
	public AudioClip sundialTapSound;

	// Use this for initialization
	void Start () {
		index = startingIndex;
		sundialPositions[index].SetActive(true);
	}

	public bool IsCorrect() {
		return (index == correctIndex);
	}

	void OnMouseDown() {
		Helper.PlayAudioIfSoundOn(sundialTapSound);

		sundialPositions[index].SetActive(false);
		if ((index + 1) >= sundialPositions.Length) {
			index = 0;
		} else {
			index++;
		}
		sundialPositions[index].SetActive(true);
		if (index == correctIndex) {
			manager.CheckIfWin();
		}
	}
}
