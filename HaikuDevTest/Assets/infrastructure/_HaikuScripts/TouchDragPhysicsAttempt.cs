using UnityEngine;
using System.Collections;

public class TouchDrag : MonoBehaviour {
	private bool dragObject;

	// Use this for initialization
	void Start () {
		dragObject = false;
	}
	
	// Update is called once per frame
	private float speed = 0.1f;
	public float maxSpeed = 0.2f;
	public bool onlyMoveY = false;
	public bool onlyMoveX = false;

	void OnCollisionEnter2D(Collision2D collision) {
		dragObject = false;
		}

	void Update() {
				if (Input.touchCount == 1) {
						if (Input.GetTouch (0).phase == TouchPhase.Began) {
								Vector3 wp = Camera.main.ScreenToWorldPoint (Input.GetTouch (0).position);
								Vector2 touchPos = new Vector2 (wp.x, wp.y);
								if (GetComponent<Collider2D>() == Physics2D.OverlapPoint (touchPos)) {
										dragObject = true;
								} else {
										dragObject = false;
								}
						}
				}

				if (dragObject && Input.touchCount > 0 && Input.GetTouch (0).phase == TouchPhase.Moved) {
						Vector2 touchDeltaPosition = Input.GetTouch (0).deltaPosition;
			
						float deltaY = Mathf.Clamp (touchDeltaPosition.y * speed, -maxSpeed, maxSpeed);
						float deltaX = Mathf.Clamp (touchDeltaPosition.x * speed, -maxSpeed, maxSpeed);
						if (onlyMoveX) {
								deltaY = 0;
						} else if (onlyMoveY) {
								deltaX = 0;
						}
						transform.Translate (deltaX, deltaY, 0);
				}
		}
}
