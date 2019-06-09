using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Canvas)),RequireComponent(typeof(CanvasScaler))]
public class ChapterUIManager : MonoBehaviour {

	[SerializeField]
	PlayMakerFSM _topBarText;

	[SerializeField]
	RectTransform _roomAreaRect;

	[SerializeField]
	GUIContainer _bottomUI;

	[SerializeField]
	GUIContainer _rightUI;

	[SerializeField]
	RectTransform _uiContainer;

	[SerializeField]
	float _maxRoomXScale = 1.3f;

	[SerializeField]
	RectTransform _blackBarLeft;

	[SerializeField]
	RectTransform _blackBarRight;

	[SerializeField]
    GameObject _itemAnimationPrefab;

    [SerializeField]
    GameObject _tapBlocker;



	[SerializeField]
	PlayMakerFSM _hintBuyFSM;
	public PlayMakerFSM hintBuyFSM{
		get{
			return _hintBuyFSM;
		}
	}



	public InventoryIconContainer inventoryIconContainer{
		get{
			return _guiContainer.inventoryContainer;
		}
	}

	Vector3 _roomCenter;
	Vector3 _roomScale;

	bool _initialized = false;

	GUIContainer _guiContainer;

    [SerializeField]
    AudioClip _getItemSoundEffect;

	public StarCountDisplay starCountDisplay{
		get{
            return _guiContainer.starCountDisplay;
		}
	}

    public Image hintNotifier{
        get{
            return _guiContainer.hintNotifier;
        }
    }

	static ChapterUIManager s_instance;
	public static ChapterUIManager instance{
		get{

			if (s_instance == null) {
				//Awake not called yet
				s_instance = GameObject.FindObjectOfType<ChapterUIManager>();
			}

			return s_instance;
		}
	}

	void Awake(){
		s_instance = this;
        if(_tapBlocker != null){
            _tapBlocker.SetActive(false);
        }
        if (Helper.IsRightUI()) {
            _guiContainer = _rightUI;
        } else {
            _guiContainer = _bottomUI;
        }
	}
		
	void Start(){




	}

    void OnPlayerSaveDataReady(){

    }

	void InitializeUI(){
		CanvasScaler canvasScaler = GetComponent<CanvasScaler> ();

		bool isRightUI = Helper.IsRightUI ();

		float top = 0;
		float left = 0;
		float right;
		float bottom;

        if (isRightUI) {
            RectTransform rightRect = _rightUI.GetComponent<RectTransform>();
            right = rightRect.rect.width;
			bottom = 0;
			_guiContainer = _rightUI;
		} else {
            RectTransform bottomRect = _bottomUI.GetComponent<RectTransform>();
            bottom = bottomRect.rect.height;
			right = 0;
			_guiContainer = _bottomUI;
		}

        //Debug.Log("right " + right + " bottom  " + bottom);

		_roomAreaRect.offsetMin = new Vector2 (left, bottom);
		_roomAreaRect.offsetMax = new Vector2 (-right, -top);
		_rightUI.gameObject.SetActive (isRightUI);
		_bottomUI.gameObject.SetActive (!isRightUI);

		Vector3[] worldCorners = new Vector3[4];
		_roomAreaRect.GetWorldCorners (worldCorners);

		float roomAreaWidth = worldCorners [2].x - worldCorners [1].x;
		float roomAreaHeight = worldCorners [2].y - worldCorners [0].y;

		float screenHeight = Camera.main.orthographicSize * 2f;

		float aspect = canvasScaler.referenceResolution.x / canvasScaler.referenceResolution.y;

		float referenceWorldHeight = screenHeight;
		float referenceWorldWidth = screenHeight * aspect;

		_roomCenter = Vector3.Lerp (worldCorners [0], worldCorners [2], 0.5f);
		_roomCenter.z = 0f;

		_roomScale = new Vector3 (roomAreaWidth / referenceWorldWidth,roomAreaHeight / referenceWorldHeight,1f);

		if (_roomScale.x > _maxRoomXScale) {
			float relativeScale = _maxRoomXScale / _roomScale.x;
			_roomScale.x = _maxRoomXScale;

			RectTransform canvasRect = GetComponent<RectTransform> ();
			float canvasWidth = canvasRect.rect.width;
			float scaledCanvasWidth = canvasWidth * relativeScale;
			float blackBarWidth = (canvasWidth - scaledCanvasWidth) / 2f;
			//Debug.Log (canvasWidth + " " + relativeScale + " " + blackBarWidth);
			_uiContainer.offsetMin = new Vector2 (blackBarWidth, 0);
			_uiContainer.offsetMax = new Vector2 (-blackBarWidth, 0);

			Vector2 barSize = _blackBarLeft.sizeDelta;
			barSize.x = blackBarWidth;
			_blackBarLeft.sizeDelta = barSize;
			barSize = _blackBarRight.sizeDelta;
			barSize.x = blackBarWidth;
			_blackBarRight.sizeDelta = barSize;

			_blackBarLeft.gameObject.SetActive (true);
			_blackBarRight.gameObject.SetActive (true);
		} else {
			_blackBarLeft.gameObject.SetActive (false);
			_blackBarRight.gameObject.SetActive (false);
		}
		_initialized = true;
	}

	public void PlayAddInventoryItemAnimation(InventoryItemData pItemData, int pNewCount){
        GameObject itemAnimationGameObject = GameObject.Instantiate(_itemAnimationPrefab, _roomAreaRect);
        GetItemAnimation getItemAnimation = itemAnimationGameObject.GetComponent<GetItemAnimation>();
        getItemAnimation.Activate(pItemData, pNewCount,
                                  _guiContainer.inventoryContainer.GetItemImageRectTransform(pItemData.slotNumber),_getItemSoundEffect);
	}

	public void UpdateInventoryItemCount(InventoryItemData pItemData, int pNewCount, bool pPlaySoundEffect = false){
        if(pPlaySoundEffect){
            if(_getItemSoundEffect != null){
                Helper.PlayAudioIfSoundOn(_getItemSoundEffect);
            }
        }
		_guiContainer.inventoryContainer.UpdateItemCount (pItemData, pNewCount);
        PlayMakerFSM.BroadcastEvent("SAVE SCENE");
	}

    public void ClearSelectedItem() {
        _guiContainer.inventoryContainer.ClearSelected();
    }

	public void ForceSelectItemId(int pItemId){
		_guiContainer.inventoryContainer.ForceSelectItemId (pItemId);
	}

	public int GetSelectedItemId(){
		return _guiContainer.inventoryContainer.selectedItemId;
	}
		
	public Bounds GetRoomAreaBounds(){
		Bounds bounds = new Bounds ();
		Vector3[] worldCorners = new Vector3[4];
		_roomAreaRect.GetWorldCorners (worldCorners);
		foreach (Vector3 corner in worldCorners) {
			bounds.Encapsulate (corner);
		}
		return bounds;
	}

	public void AlignRoomToRoomAreaRect(RoomHelper pRoom){
		if (!_initialized) {
			InitializeUI ();
		}

		pRoom.transform.position = _roomCenter;
		pRoom.transform.localScale = _roomScale;
	}

    public void ShowTopBarText(string pText, bool pDoubleHeight = false, bool pDisableTapBlocker=false){
		HutongGames.PlayMaker.Actions.SetEventProperties.properties = new Dictionary<string, object>{
			{"topBarText", pText},
			{"isDoubleHeight",pDoubleHeight},
            {"disableTapBlocker",pDisableTapBlocker}
		};
		_topBarText.SendEvent("Generic Text");
	}



    public void EnableTapBlocker(bool pEnable){
        if (_tapBlocker != null) {
            _tapBlocker.SetActive(pEnable);
        }
    }

    public void ShowPurchasePanel(){
        /*if (OneAppUIManager.instance != null) {
            OneAppUIManager.instance.ShowPurchasePanel();
        } else {
            Debug.LogError("Purchasing only works when the starting scene is InitialLoadScene");
        }*/

        if(_hintBuyFSM != null){
            _hintBuyFSM.SendEvent("activate");
        }
    }

    public void QuitChapter(){
       
    }

    public void LoadNextScene(){

    }



}
