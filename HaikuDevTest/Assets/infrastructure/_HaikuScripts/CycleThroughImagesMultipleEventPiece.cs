using UnityEngine;
using System.Collections;

public class CycleThroughImagesMultipleEventPiece : MonoBehaviour {
	public Sprite[] allSprites;
	public int currentSprite;
	public CycleThroughImagesMultipleEvent manager;
	
	public AudioClip cycleSound;
	public bool wheelSprites; // Wheel if you go 1234321 instead of the default - 12341234
	private bool countUp;
	
	private Sprite originalSprite;
	private int originalCurrentSpriteIndex;
	
	void Start() {
		for (int i = 0; i < allSprites.Length; i++) {
			Sprite s = allSprites[i];
			if (s == null) {
				if (GetComponent<SpriteRenderer>().sprite == null) {
					currentSprite = i;
				}
			} else if (s.Equals(GetComponent<SpriteRenderer>().sprite)) {
				currentSprite = i;
				break;
			}
		}
		originalCurrentSpriteIndex = currentSprite;
		originalSprite = GetComponent<SpriteRenderer>().sprite;
	}
	
	public void ResetPiece() {
		GetComponent<SpriteRenderer>().sprite = originalSprite;
		currentSprite = originalCurrentSpriteIndex;
	}
	
	void OnMouseDown() {
		if (wheelSprites) {
			if (currentSprite == (allSprites.Length - 1)) {
				currentSprite--;
				countUp = false;
			} else if (currentSprite == 0) {
				currentSprite++;
				countUp = true;
			} else {
				if (countUp) {
					currentSprite++;
				} else {
					currentSprite--;
				}
			}
		} else { // Count up and reset to 0 when you hit the max
			if (currentSprite == (allSprites.Length - 1)) {
				currentSprite = 0;
			} else {
				currentSprite++;
			}
		}
		Helper.PlayAudioIfSoundOn(cycleSound);

		GetComponent<SpriteRenderer>().sprite = allSprites[currentSprite];
		manager.CheckIfEvent();
	}
}
