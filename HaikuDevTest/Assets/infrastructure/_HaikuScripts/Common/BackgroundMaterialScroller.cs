using UnityEngine;
using System.Collections;

public class BackgroundMaterialScroller : MonoBehaviour
{

	public enum ScrollDirection
	{
		Left,
		Right,
		Up,
		Down
	}

	[SerializeField, Header("Must be between 0 and 1")]
	private float _scrollSpeed = 0.1f;
	public float scrollSpeed{
		get{
			return _scrollSpeed;
		}set{
			_scrollSpeed = value;
			OnPropertyChange ();
		}
	}

	[SerializeField]
	private ScrollDirection _direction;
	public ScrollDirection direction{
		get{
			return _direction;
		}set{
			_direction = value;
			OnPropertyChange ();
		}
	}

	private Material _material;
	public Material cachedMaterial{
		get{ return _material; }
	}

	private float _offset = 0f;

	private bool _horizontal;

	private int _multiplier;

	void Awake(){
		_material = GetComponent<Renderer> ().material;
		OnPropertyChange ();
	}

	private void OnPropertyChange(){
		if (_direction == ScrollDirection.Left || _direction == ScrollDirection.Right) {
			//_material.mainTextureScale = new Vector2 (2f, 1f);
			_horizontal = true;
		} else {
			//_material.mainTextureScale = new Vector2 (1f, 2f);
			_horizontal = false;
		}

		if (_direction == ScrollDirection.Up || _direction == ScrollDirection.Right) {
			_multiplier = -1;
		} else {
			_multiplier = 1;
		}

		_scrollSpeed = Mathf.Clamp (_scrollSpeed, 0, 1);
	}

	void OnDestroy(){
		_material = null;
	}

	void Update (){
		_offset = Mathf.Repeat(_offset + (_multiplier * Time.deltaTime * _scrollSpeed),1f);

		if (_horizontal) {
			_material.mainTextureOffset = new Vector2 (_offset, 0f);
		} else {
			_material.mainTextureOffset = new Vector2 (0f, _offset);
		}

	}
}