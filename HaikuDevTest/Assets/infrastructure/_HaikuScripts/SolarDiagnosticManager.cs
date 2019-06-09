using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class SolarDiagnosticManager : PuzzleController {
	private SolarDiagnosticPiece[] allPieces;
	public int rows;
	public int columns;
	public SolarDiagnosticPiece[,] matrix;
	public GameObject lastPieceHalo;
	private SolarDiagnosticPiece originalPiece;
	private SolarDiagnosticPiece lastPiece;

	// Connections is which of the SolarDiagnosticPieces are activated
	private List<SolarDiagnosticPiece> connections = new List<SolarDiagnosticPiece>();

	public AudioClip pieceAddedSound;
	public SolarDiagnosticBar diagnosticBar;

    [SerializeField]
    private bool dontUseDiagnosticBar;

    [SerializeField]
    private bool dontUseLastPieceHalo;

    [SerializeField]
    private bool useSound;

    [SerializeField, Tooltip("Mark TRUE if should spawn connecting object between two pieces.")]
    private bool useConnectingObjects;

    [SerializeField]
    private GameObject connectingObject;

    private List<GameObject> visibleConnectingObjects = new List<GameObject>();

    [SerializeField, Tooltip("Attach gameobject to be activated showing puzzle solution.")]
    private GameObject skipSolution;

    [SerializeField]
    private GameObject playablePuzzlesToDeactivate;

    [SerializeField]
    private string obstacleTxtKey;
    public string ObstacleTxtKey { get { return obstacleTxtKey; } }

    [SerializeField]
    private string goalTxtKey;
    public string GoalTxtKey { get { return goalTxtKey; } }


    [SerializeField]
    private string reachedGoalAndFailedTxtKey; // Player reaches goal chip but have not attached all chips

    [SerializeField]
    private bool useObstacleKey;

    [SerializeField, Tooltip("Allow player to draw a line backwards to remove last connection (erasing line)")]
    private bool allowDrawingBackwards;

    [SerializeField]
    private bool useDebugLogs;

    // Use this for initialization
    void Start () {
		allPieces = GetComponentsInChildren<SolarDiagnosticPiece>();
		matrix = new SolarDiagnosticPiece[rows, columns];

		SetupSolarPieces();

        if(!dontUseDiagnosticBar)
		    SetupDiagnosticBar();
	}

	private void SetupDiagnosticBar() {
		int numberOfBars = 0;
		foreach (SolarDiagnosticPiece piece in allPieces) {
			if (!piece.isObstacle) {
				numberOfBars++;
			}
		}
		diagnosticBar.InitializeWithUnits(numberOfBars);
	}

	// Returns true if you can continue the line
	public bool StartLineFromPieceTap(SolarDiagnosticPiece piece) {

        if (piece.isSource)
        {
            originalPiece = piece;
            lastPiece = piece;

            // Clear previous line
            RemoveConnectionsFromHereTillEnd(0);

            if (!dontUseLastPieceHalo)
                lastPieceHalo.transform.position = piece.transform.position;

            if (connections.Count < 1)
            {
                // If you tap on a source piece multiple times, don't add it
                // Also if you toggle between the two source pieces, don't add it.
                connections.Add(piece);
            }

            return true;

        }
        else
        {
            if (connections != null)
            {

                if (connections[connections.Count - 1].Equals(piece))
                {

                    if (useDebugLogs) Debug.Log("Continue the line");
                    originalPiece = piece;
                    lastPiece = piece;

                    return true;
                }
            }
        }
            return false; 
	}
	
	private void RemoveConnectionsFromHereTillEnd(int startRemove) {

        if (useDebugLogs) Debug.Log("Start remove : " + startRemove);
        
		SolarDiagnosticPiece[] connectionsCopy = new SolarDiagnosticPiece[connections.Count];
		connections.CopyTo(connectionsCopy);
        int startIndex = (connectionsCopy.Length - 1);

        if (startIndex < 0) return;

        for (int i = startIndex; i >= startRemove; i--) {

			SolarDiagnosticPiece piece = connections[i];

            if (useDebugLogs) Debug.Log("Remove piece:" + piece.name + " at i " + i + " length: " + connectionsCopy.Length);

			if (!piece.isSource) piece.activated = false;

			connections.Remove(piece);
		}

        if (useConnectingObjects)
        {
            foreach (GameObject connectingObject in visibleConnectingObjects)
            {
                Destroy(connectingObject);
            }

            visibleConnectingObjects.Clear();
        }

        if (!dontUseDiagnosticBar)
            diagnosticBar.ShowBars(connections.Count);
	}

	public void SiblingCollided(SolarDiagnosticPiece piece) {

		if (lastPiece.neighbors.Contains(piece)) {
            if (piece.isSource)
            {
                //  if (piece == originalPiece) 
                if (piece == lastPiece.PreviousPiece)
                {
                    // End touch if you hit yourself
                    //originalPiece.EndTouch();
                    
                    lastPiece.EndTouch();
                    if(allowDrawingBackwards) removeLastConnection(lastPiece);

                }
                else
                {
                    // You have hit the other source
                    AddPieceToSelectedPieceConnection(piece);
                    CheckIfWin();
                }
            }
            else
            {
                if (piece.isObstacle)
                {
                    if (useDebugLogs) Debug.Log("Obstacle at: " + piece.name);
                    if (useObstacleKey) Helper.LocalizeKeyToTopBar(obstacleTxtKey);
                    piece.activated = true; // Activate the obstacle
                }
                else
                {
                    if (!piece.activated)
                    {
                        AddPieceToSelectedPieceConnection(piece);
                        if (useDebugLogs) DebugLogConnections();
                    }
                    else
                    {
                        
                        // remove piece if getting backwards
                        if (piece == lastPiece.PreviousPiece &&  allowDrawingBackwards)
                        {
                            removeLastConnection(piece);
                        }
                        else if (useDebugLogs) Debug.Log("Piece already activated");
                    }
                }
            }

            CheckIfWin();
        }
	}

	private void DebugLogConnections() {
		string connectionsSoFar = "";
		for (int i = 0; i < connections.Count; i++) {
			connectionsSoFar = connectionsSoFar + " " + connections[i].name;
		}

		Debug.Log("All Connex: " + connectionsSoFar);
	}
	
	private void CheckIfWin() {
        bool failed = false;
        bool finalPieceConnected = false;
        foreach (SolarDiagnosticPiece piece in allPieces)
        {
            if (piece.activated && piece.IsGoal)
                finalPieceConnected = true;
            // A piece must be activated, or not be a source
            if (!piece.activated && !piece.isSource && !piece.isObstacle)
            {
                failed = true;
            }
        }

        if (failed)
        {
            if (finalPieceConnected) Helper.LocalizeKeyToTopBar(reachedGoalAndFailedTxtKey);
            return;
        }


		ShowAllObstacles();
		_wonEventFsm.SendEvent("won");
	}

	// Turn on the obstacle pieces, in the player did not bump into them yet still completed the puzzle
	private void ShowAllObstacles() {
		foreach (SolarDiagnosticPiece piece in allPieces) {
			if (piece.isObstacle) {
				piece.activated = true;
			}
		}
	}
	
	void AddPieceToSelectedPieceConnection(SolarDiagnosticPiece piece) {

        if(useSound)  Helper.PlayAudioIfSoundOn(pieceAddedSound);

        if (useConnectingObjects)
        {
            Vector3 connectionPosition = (piece.transform.position + lastPiece.transform.position) / 2;
            GameObject currentConnectingObject = Instantiate(connectingObject, connectionPosition, new Quaternion(0, 0, 0, 0), piece.transform);

            visibleConnectingObjects.Add(currentConnectingObject);


            if (piece.row == lastPiece.row) currentConnectingObject.transform.eulerAngles = new Vector3(0, 0, 90);
            else if (piece.column == lastPiece.column) currentConnectingObject.transform.eulerAngles = new Vector3(0, 0, 0);
        }

        piece.activated = true;

		connections.Add(piece);

        if(!dontUseDiagnosticBar)
		    diagnosticBar.ShowBars(connections.Count);

        piece.PreviousPiece = lastPiece;
        lastPiece = piece;

        if (!dontUseLastPieceHalo)
            lastPieceHalo.transform.position = lastPiece.transform.position;
	}

    private void removeLastConnection(SolarDiagnosticPiece piece)
    {
        if (useConnectingObjects)
        {
            // destroy connecting object
            GameObject lastConnectingObject = visibleConnectingObjects[visibleConnectingObjects.Count - 1];
            visibleConnectingObjects.Remove(lastConnectingObject);
            Destroy(lastConnectingObject);
        }

        // turn off piece
        lastPiece.activated = false;

        SolarDiagnosticPiece tempPiece = lastPiece;
        // set new piece order
        lastPiece = lastPiece.PreviousPiece;
        tempPiece.PreviousPiece = null;

        connections.Remove(tempPiece);

        if (useDebugLogs) Debug.Log(piece.name + ": piece deleted");
    }

    public override void Activate()
    {
        base.Activate();
    }

    public override void ResetPuzzle()
    {
        lastPiece = null;
        RemoveConnectionsFromHereTillEnd(0);
        base.ResetPuzzle();
    }

    public override void Skip()
    {
        RemoveConnectionsFromHereTillEnd(0);
        ShowAllObstacles();

        foreach (SolarDiagnosticPiece piece in allPieces)
        {
            if (!piece.isObstacle && !piece.isSource) piece.activated = true;
        }

        skipSolution.SetActive(true);
        base.Skip();
    }

    void SetupSolarPieces() {

		foreach (SolarDiagnosticPiece piece in allPieces) {
            if (useDebugLogs) Debug.Log("Piece " + piece.name);
			matrix[piece.row, piece.column] = piece;
			piece.siblings = new List<SolarDiagnosticPiece>(allPieces);
		}
		
		foreach (SolarDiagnosticPiece piece in matrix) {
			// Add four neighbors. We have to be be 2 less than rows for it to be valid.  For example, if rows is 6, the row index (on PulleyPiece) only goes up to 5
			if (piece.row < rows - 1) {
				piece.neighbors.Add(matrix[piece.row + 1, piece.column]);
			}
			
			if (piece.column > 0) {
				piece.neighbors.Add(matrix[piece.row, piece.column - 1]);
			}
			
			if (piece.column < columns - 1) {
				piece.neighbors.Add(matrix[piece.row, piece.column + 1]);
			}
			
			if (piece.row > 0) {
				piece.neighbors.Add(matrix[piece.row - 1, piece.column]);
			}
		}
	}
}
