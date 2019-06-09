using UnityEngine;
using System.Collections;
using System.Collections.Generic;

/// <summary>
/// Puzzles sprite manager.
/// A generic sprite manager that can be extended to handle sprites for puzzles.
/// Extend or use this class if you want add many sprite references in a puzzle.
/// </summary>
public class PuzzlesSpriteManager : MonoBehaviour
{
	public SpriteData [] _sprites;

	private Dictionary<string, Sprite> _spriteDict;

	[System.Serializable]
	public class SpriteData {
		public string _name;
		public Sprite _sprite;
	}

	protected virtual void Awake () {
		_spriteDict = new Dictionary<string, Sprite> ();
		for (int i = 0; i < _sprites.Length; i++) {
			_spriteDict.Add(_sprites[i]._name, _sprites[i]._sprite);
		}
	}

	#region To Be Called From Editor
	public Sprite GetSpriteEditor (string spriteName) {
		for (int i = 0; i < _sprites.Length; i++) {
			if (_sprites [i]._name == spriteName) {
				return _sprites [i]._sprite;
			}
		}

		return null;
	}
	#endregion

	/// <summary>
	/// Gets the sprite.
	/// To be used at runtime.
	/// </summary>
	/// <returns>The sprite.</returns>
	/// <param name="spriteName">Sprite name.</param>
	public Sprite GetSprite (string spriteName) {
		Sprite outSprite = null;
		if (_spriteDict.TryGetValue(spriteName, out outSprite)) {
			return outSprite;
		}
		return null;
	}


}

