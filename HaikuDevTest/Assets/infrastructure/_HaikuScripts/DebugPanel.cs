using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class DebugPanel : MonoBehaviour
{
    [Header("Won events buttons")]
    public bool ShowWonEventsButtons;
    public RectTransform WonButtonsContainer;
    public GameObject WonButtonToClone;

    [Header("Inventory buttons")]
    public bool ShowInventoryButtons;
    public RectTransform InventoryButtonsContainer;
    public GameObject InventoryButtonToClone;

    [Header("Focus rooms buttons")]
    public bool ShowRoomsButtons;
    public RectTransform RoomsButtonsContainer;
    public GameObject RoomsButtonToClone;

    private GameObject ChapterManager;

    void Start()
    {
        if (HaikuBuildSettings.isDebug == false)
        {
            Destroy(gameObject);
            return;
        }

        // find chapter manager object that contains all rooms if it is not set yet
        if (ChapterManager == null)
        {
            ChapterManager = GameObject.Find("ChapterManager");
        }

        // exit if not found
        if (ChapterManager == null)
        {
            return;
        }

        // populate won buttons list
        if (ShowWonEventsButtons)
        {
            InitializeWonButtons();
        }

        // populate inventory buttons list
        if (ShowInventoryButtons)
        {
            InitializeInventoryButtons();
        }

        // populate rooms buttons list
        if (ShowRoomsButtons)
        {
            InitializeRoomsButtons();
        }

        // remove prototype buttons
        if (WonButtonToClone != null) Destroy(WonButtonToClone);
        if (InventoryButtonToClone != null) Destroy(InventoryButtonToClone);
        if (RoomsButtonToClone != null) Destroy(RoomsButtonToClone);
    }

    private void InitializeRoomsButtons()
    {
        if (RoomsButtonsContainer == null) return;

        if (ChapterSceneManager.instance == null) return;

        // create button for every room item
        for (int i = 0; i < ChapterManager.transform.childCount; i++)
        {
            // get room
            Transform room = ChapterManager.transform.GetChild(i);

            // get room number
            string roomNumberAsText = room.name.Split(new string[] { "_" }, System.StringSplitOptions.None)[0];
            int roomNumber = -1;
            if (int.TryParse(roomNumberAsText, out roomNumber))
            {
                if (roomNumber >= 0)
                {
                    // create button
                    var newButton = Instantiate(RoomsButtonToClone, RoomsButtonsContainer);
                    newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { ChapterSceneManager.instance.FocusRoom(roomNumber); });
                    newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "" +
                        room.name;
                }
            }
        }
    }

    private void InitializeInventoryButtons()
    {
        if (InventoryButtonsContainer == null) return;

        if (InventoryManager.instance == null) return;

        // create button for every inventory item
        for (int i = 0; i < 50 ; i++)
        {
            if (InventoryManager.instance.GetItemData(i) != null)
            {
                // create button
                var newButton = Instantiate(InventoryButtonToClone, InventoryButtonsContainer);
                int itemId = i;
                var localizationArray = InventoryManager.instance.GetItemData(i).localizationKey.Split(new string[] { "_" }, System.StringSplitOptions.None);
                string itemName = InventoryManager.instance.GetItemData(i).localizationKey;
                if (localizationArray.Length > 2)
                {
                    itemName = "";
                    for (int j = 2; j < localizationArray.Length; j++)
                    {
                        itemName += localizationArray[j] + " ";
                    }
                }
                newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { InventoryManager.instance.AddItem(itemId); });
                newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "" +
                    itemName;
            }
        }
    }

    public void RemoveAllItemsFromInventory()
    {
        if (InventoryManager.instance == null) return;

        for (int i = 0; i < 50; i++)
        {
            if (InventoryManager.instance.GetItemData(i) != null)
            {
                while (InventoryManager.instance.HasItem(i))
                {
                    InventoryManager.instance.RemoveItem(i);
                }
            }
        }
    }

    private void InitializeWonButtons()
    {
        if (WonButtonsContainer == null) return;

        // find all puzzle controllers in the scene
        var puzzleControllers = ChapterManager.transform.GetComponentsInChildren<PuzzleController>(true);

        // make cheat win button for each puzzle
        foreach (var controller in puzzleControllers)
        {
            // find won fsm candidate
            PlayMakerFSM possibleWonFSM = null;
            FieldInfo wonEventFsmFieldInfo = controller.GetType().GetField("_wonEventFsm", BindingFlags.NonPublic | BindingFlags.Instance);
            if (wonEventFsmFieldInfo != null && wonEventFsmFieldInfo.GetValue(controller) != null)
            {
                possibleWonFSM = (PlayMakerFSM)wonEventFsmFieldInfo.GetValue(controller);
            }

            // create button (if there is no skip button for the puzzle)
            if (possibleWonFSM != null)
            {
                // do not create if skip button is present for the puzzle
                bool hasSkipButton = false;
                FieldInfo fieldInfo = controller.GetType().GetField("_puzzleUI", BindingFlags.NonPublic | BindingFlags.Instance);
                if (fieldInfo != null && fieldInfo.GetValue(controller) != null)
                {
                    PuzzleUI controllerPuzzleUI = (PuzzleUI)fieldInfo.GetValue(controller);
                    FieldInfo fieldInfoOfPuzzleUI = controllerPuzzleUI.GetType().GetField("_skipButton", BindingFlags.NonPublic | BindingFlags.Instance);
                    if (fieldInfoOfPuzzleUI != null && fieldInfoOfPuzzleUI.GetValue(controllerPuzzleUI) != null)
                    {
                        hasSkipButton = true;
                    }
                }

                // create button
                if (!hasSkipButton)
                {
                    var newButton = Instantiate(WonButtonToClone, WonButtonsContainer);
                    newButton.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(() => { possibleWonFSM.SendEvent("won"); });
                    newButton.GetComponentInChildren<UnityEngine.UI.Text>().text = "" +
                        GetNameOfParentRoom(controller.transform) + " > " +
                        controller.gameObject.name;
                }
            }
        }
    }

    private string GetNameOfParentRoom(Transform objectTransform)
    {
        string roomName = objectTransform.gameObject.name;
        Transform nextParent = objectTransform.parent;

        while (nextParent != null)
        {
            // if only ChapterManager is higher then we have our room name
            if (nextParent.gameObject.name.Equals("ChapterManager"))
            {
                break;
            }

            // get name
            roomName = nextParent.gameObject.name;

            // go higher
            nextParent = nextParent.parent;
        }

        return roomName;
    }
}
