using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Exit : MonoBehaviour {


    

    public GameObject redArrow;
    public GameObject blueArrow;
    public GameObject greenArrow;
    public GameObject doorManager;
    public GameObject exit;
    bool isExitActivated = false;

 

    void Update()
    {
   


        if (isExitActivated == false)
        {


            if (redArrow.activeSelf == true && blueArrow.activeSelf == true && greenArrow.activeSelf == true)
            {
                Debug.Log("Exit");
                doorManager.SetActive(true);
                exit.SetActive(true);
                isExitActivated = true;
            }
        }
    }

}
