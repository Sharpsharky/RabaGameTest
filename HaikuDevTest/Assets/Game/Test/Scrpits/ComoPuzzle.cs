using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComoPuzzle : PuzzleController {

    public List<Sprite> listOfSprites = new List<Sprite>();
    public GameObject emptyPrefab;
    public float xStart = -6.718f, yStart = 4.175f;

    private int x = 6; //puzzle x field dimensions
    private float halfDimensionOfATile = 2.755f;

    private List<Transform> switchingTiles = new List<Transform>();
    private Transform highLight;
    private Vector3 initialPositionOfHighlightPuzzle = new Vector3(-10, 10, -1.2f);
    //private int[] randomSetOfTiles = new int[24] { 2, 1, 23, 3, 8, 22, 7, 18, 9, 4, 21, 5, 10, 6, 0, 11, 16, 13, 17, 12, 15, 14, 19, 20 }; //I've decided to set the values myself to give all the players the same difficulty level
    private int[] randomSetOfTiles = new int[24] {0,1,2,3,4,5,6,7,8,9,10,11,12,13,14,15,16,17,18,19,20,21,23,22 }; //this is only for debugging

    private List<GameObject> winningTileSet = new List<GameObject>();
    private List<GameObject> currentTileSet = new List<GameObject>();

    public GameObject camoHotSpot;
    public GameObject backButton;
    public GameObject starAnimation;

    List<Vector3> positionsOfCorrectTiles;
    Vector3[] posOfIncorrectTiles;

    bool isFirstReset = true;

    public List<Transform> SwitchingTiles { get { return switchingTiles; } set { switchingTiles = value; } }

    void Start() {

        highLight = transform.Find("CamoTilePuzzile_Highlight");
        
        SpawnTiles();
    }

    void Update()
    {
        SwitchTiles();
    }

    void IsWinner()
    { int i = 0, count = 0;
        foreach (GameObject tile in currentTileSet)
        {
            if (tile.GetComponent<Tile>().TileNumber == i)
            {
                count++;
            }
            i++;
        }
        Debug.Log(i + " " + count);
        if (i == count)
        {
            Win(); //HERE IS WIN !!!!!!!!!!!!!!
        }
    }
    void SwitchTiles()
    {
        if (SwitchingTiles.Count == 1)
        {
            if (isFirstReset == true)
            {
               //Debug.Log("setting values");
                SetValues();
            }
            highLight.position = new Vector3(SwitchingTiles[0].position.x, SwitchingTiles[0].position.y, -1.2f);
        }

        if (SwitchingTiles.Count >= 2)
        {
            Vector3 bufor = SwitchingTiles[0].position;

            SwitchingTiles[0].position = SwitchingTiles[1].position;
            SwitchingTiles[1].position = bufor;

            //Debug.Log(SwitchingTiles[0].GetComponent<Tile>().TileNumber + " with " + SwitchingTiles[1].GetComponent<Tile>().TileNumber);
            highLight.position = initialPositionOfHighlightPuzzle;
            GameObject buforGo = currentTileSet[SwitchingTiles[0].GetComponent<Tile>().TileNumber];
            currentTileSet[SwitchingTiles[0].GetComponent<Tile>().TileNumber] = currentTileSet[SwitchingTiles[1].GetComponent<Tile>().TileNumber];
            currentTileSet[SwitchingTiles[1].GetComponent<Tile>().TileNumber] = buforGo;

            for(int i = 0; i < 24; i++)
            {
                Debug.Log(currentTileSet[i].GetComponent<Tile>().TileNumber);
            }


            SwitchingTiles = new List<Transform>();
            IsWinner();
        }
    }
    void SpawnTiles()
    {
        int tileCount = 0;
        foreach (Sprite sprite in listOfSprites) //At first we Spawn the tiles at the correct positions
        {
            GameObject thisTile = Instantiate(emptyPrefab, new Vector3(transform.position.x, transform.position.y, -1f), Quaternion.identity);
            thisTile.transform.parent = transform;
            thisTile.GetComponent<SpriteRenderer>().sprite = sprite;
            thisTile.transform.localScale = new Vector3(2, 2, 0);
            BoxCollider2D bc = thisTile.AddComponent(typeof(BoxCollider2D)) as BoxCollider2D;
            thisTile.transform.GetComponent<Tile>().TileNumber = tileCount;

            winningTileSet.Add(thisTile);   //We assign the tiles to a list that makes a win

            tileCount++;

        }
        SetTilesIncorrectly(true);
    }

    void SetTilesIncorrectly(bool isAtTheStart)
    {
        float xOffset = 0; float yOffset = 0;
        int currentRandomizedTile = 0;
        if (isAtTheStart == false) currentTileSet = new List<GameObject>();

        for (int i = 0; i <= 24; i++) //Here we put the tiles in inccorect positions
        {
            for (int j = 0; j <= 24; j++)
            {
                if (currentRandomizedTile < 24 && j == randomSetOfTiles[currentRandomizedTile]) //najpierw znajduje trzecie
                {
                    //Debug.Log("xpos:" + transform.position.x + " xst:" + xStart + " xOff:" + xOffset);
                    winningTileSet[j].transform.position = new Vector3(transform.position.x + xStart + xOffset, transform.position.y + yStart + yOffset, -1f);
                    currentTileSet.Add(winningTileSet[randomSetOfTiles[currentRandomizedTile]]);
                    if (xOffset <= x * 2)
                    {
                        xOffset += halfDimensionOfATile;
                    }
                    else
                    {
                        xOffset = 0;
                        yOffset -= halfDimensionOfATile;
                    }
                    currentRandomizedTile++;
                }
            }
        }
    
    }

    void SkipIt()
    {
        int i = 0;
        foreach (GameObject tile in currentTileSet)
        {
            tile.transform.position = positionsOfCorrectTiles[tile.GetComponent<Tile>().TileNumber];
            i++;
        }
    }

    public void SetValues()
    {
        posOfIncorrectTiles = new Vector3[24];

        foreach(GameObject tile in currentTileSet)
        {
            posOfIncorrectTiles[tile.GetComponent<Tile>().TileNumber] = tile.transform.position;
        }
        isFirstReset = false;


        positionsOfCorrectTiles = new List<Vector3>();
        foreach (GameObject tile in currentTileSet)
        {
            positionsOfCorrectTiles.Add(tile.transform.position);
        }
        isFirstReset = false;
    }
    public void Reset()
    {
        int i = 0;

        foreach (GameObject tile in winningTileSet)
        {
            tile.transform.position = posOfIncorrectTiles[i];
            i++;     
        }

        currentTileSet = new List<GameObject>();
        i = 0;
        foreach (GameObject tile in winningTileSet)
        {
            currentTileSet.Add(winningTileSet[randomSetOfTiles[i]]);
            i++;
        }
    }

    public override void ResetPuzzle()
    {
        Reset();
    }

    protected override void Win()
    {
        base.Win();
        camoHotSpot.SetActive(true);
        backButton.SetActive(true);
        starAnimation.SetActive(true);
        highLight.position = initialPositionOfHighlightPuzzle;
    }
    public override void Skip(){

        SkipIt();
        base.Skip();
    }
}
