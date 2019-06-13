using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour {

    private int tileNumber;

    public int TileNumber
    {
        get
        {
            return tileNumber;
        }

        set
        {
            tileNumber = value;
        }
    }

    void OnMouseDown()
    {
        gameObject.transform.parent.transform.GetComponent<ComoPuzzle>().SwitchingTiles.Add(transform);
    }
}
