using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

[RequireComponent(typeof(SpriteRenderer))]
public class CycleThroughImages : MonoBehaviour, IPointerDownHandler{
	public Sprite[] allSprites;
	protected int currentSprite;
	public int correctSprite;

	[SerializeField,Tooltip("For when multiple complete images can be formed. Make sure this array is the same length for each " +
		"CycleThroughImages and that each index is for the same solution.")]
	protected int[] alternativeSolutionSprites;

	public CycleThroughImagesManager manager;

	public AudioClip cycleSound;
	public bool wheelSprites; // Wheel if you go 1234321 instead of the default - 12341234
	protected bool countUp;

	protected Sprite originalSprite;
	protected int originalCurrentSpriteIndex;

	public bool resetColliderOnSpriteSwitch;

	public GameObject fxWhenTapped;

	protected SpriteRenderer spriteRenderer;


	void OnEnable(){
		Collider2D collider = GetComponent<Collider2D> ();
		if (collider != null) {
			collider.enabled = true;
		}
	}

	void OnDisable(){
		Collider2D collider = GetComponent<Collider2D> ();
		if (collider != null) {
			collider.enabled = false;
		}
	}

	void Awake(){
		spriteRenderer = GetComponent<SpriteRenderer>();
	}

	void Start() {
		for (int i = 0; i < allSprites.Length; i++) {
			Sprite s = allSprites[i];
			if (s == null) {
				if (spriteRenderer.sprite == null) {
					currentSprite = i;
				}
			} else if (s.Equals(spriteRenderer.sprite)) {
				currentSprite = i;
				break;
			}
		}
		originalCurrentSpriteIndex = currentSprite;
		originalSprite = spriteRenderer.sprite;
	}

	public void ResetPiece() {
		spriteRenderer.sprite = originalSprite;
		currentSprite = originalCurrentSpriteIndex;
	}

    public virtual void OnPointerDown(PointerEventData pPointerEventData){
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

		manager.UpdateNumberCorrect();

		spriteRenderer.sprite = allSprites[currentSprite];
        if (resetColliderOnSpriteSwitch) {
			StartCoroutine(ResetCollider());
		}

		int alternativeImageIndex;
		if (currentSprite == correctSprite) {
			manager.CheckIfWin();
		}else if(isShowingAlternativeImageGetIndex(out alternativeImageIndex)){
			manager.CheckIfAlternativeImage (alternativeImageIndex);
		}

		if (fxWhenTapped != null) {
			Instantiate(fxWhenTapped, transform, false);
		}
	}

	IEnumerator ResetCollider()
	{
		Destroy(this.gameObject.GetComponent<PolygonCollider2D>());
		yield return new WaitForEndOfFrame();
		this.gameObject.AddComponent<PolygonCollider2D>();
	}

	public void SetCorrect(){
		currentSprite = correctSprite;
		spriteRenderer.sprite = allSprites[currentSprite];

		if (resetColliderOnSpriteSwitch) {
			StartCoroutine(ResetCollider());
		}
	}

	public bool isShowingAlternativeImage(int pAlternativeImageIndex){
		if (pAlternativeImageIndex < 0 || pAlternativeImageIndex >= alternativeSolutionSprites.Length) {
			return false;
		}

		return currentSprite == alternativeSolutionSprites [pAlternativeImageIndex];
	}


	protected bool isShowingAlternativeImageGetIndex(out int pAlternativeImageIndex){
		for (int i = 0; i < alternativeSolutionSprites.Length; ++i) {
			if (currentSprite == alternativeSolutionSprites [i]) {
				pAlternativeImageIndex = i;
				return true;
			}
		}

		pAlternativeImageIndex = -1;
		return false;
	}

	public bool isCorrect() {
		return (currentSprite == correctSprite);
	}
}
