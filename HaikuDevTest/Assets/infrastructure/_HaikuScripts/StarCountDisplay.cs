using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using HutongGames.PlayMaker;
using HutongGames.PlayMaker.Actions;
using TMPro;

public class StarCountDisplay : MonoBehaviour {

	[SerializeField]
	TextMeshProUGUI _starCountText;

	[SerializeField]
	Transform _flyToLocation;
	public Transform flyToLocation{
		get{
			return _flyToLocation;
		}
	}

    [SerializeField]
    GameObject _starSpawnerPrefab;

    [SerializeField]
    GameObject _starRemovePrefab;

    const int MAX_REMOVE_STARS = 10;

    const float DELAY_BETWEEN_STARS = 0.1f;

    bool _starsInitialized;

    void Start() {
        if(!_starsInitialized){
            ShowLoadingState();
        }
    }

	public void RemoveStars(int pRemoveCount,Vector3 pWorldLocation){
		float delay = 0;

        int numSpawns = Mathf.Min (pRemoveCount, MAX_REMOVE_STARS);

		for (int i = 0; i < numSpawns; ++i) {
            StartCoroutine (SpawnRemoveStar(delay,pWorldLocation));
            delay += DELAY_BETWEEN_STARS;
		}
	}


    public void SetStarCount(int pNumStars){
        _starsInitialized = true;
        _starCountText.text = pNumStars.ToString();
        _starCountText.gameObject.SetActive(true);
    }

    public void ShowLoadingState(){
        //TODO: Need a better visual indication that a transaction is in progress
        //_starCountText.gameObject.SetActive(false);
    }

    public void SpawnStars(IAPConstants.IAPProduct pProduct, Vector3 pWorldSpawnLocation) {
        GameObject spawn = GameObject.Instantiate(_starSpawnerPrefab, pWorldSpawnLocation, Quaternion.identity);

    }

    public void SpawnStars(string pStarCollectId, Vector3 pWorldSpawnLocation){


        string episodeId;



       
        

        GameObject spawn = GameObject.Instantiate(_starSpawnerPrefab, pWorldSpawnLocation, Quaternion.identity);

    }

    IEnumerator SpawnRemoveStar(float pDelay, Vector3 pFromPosition) {

        yield return new WaitForSeconds(pDelay);

        GameObject removeStar = GameObject.Instantiate(_starRemovePrefab, _flyToLocation.position, Quaternion.identity);
        PlayMakerFSM fsm = removeStar.GetComponent<PlayMakerFSM>();

        HutongGames.PlayMaker.Actions.SetEventProperties.properties = new Dictionary<string, object>{
            {"flyToPosition",pFromPosition}
        };

        fsm.SendEvent("flyToLocation");
    }
}
