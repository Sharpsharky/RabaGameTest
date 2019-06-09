using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayAudioOnCollision : MonoBehaviour {

    [SerializeField]
    AudioClip _soundToPlay;

    [SerializeField, Tooltip("set this to prevent the sound from playing more than once when there are multiple OnCollisionEnters")]
    float _minumumTimeBetweenSoundPlays = 1f;

    Collider2D _lastSoundPlayCollider;
    float _lastSoundPlayTime;

	void OnCollisionEnter2D(Collision2D collision) {
        if (_soundToPlay == null) {
            return;
        }


        if(_lastSoundPlayCollider != null){
            if(collision.collider == _lastSoundPlayCollider){
                float timeSinceLastSoundPlay = Time.time - _lastSoundPlayTime;
                if(timeSinceLastSoundPlay < _minumumTimeBetweenSoundPlays){
                    return;
                }
            }
        }

        _lastSoundPlayTime = Time.time;
        _lastSoundPlayCollider = collision.collider;
        Helper.PlayAudioIfSoundOn(_soundToPlay);



	}

}
