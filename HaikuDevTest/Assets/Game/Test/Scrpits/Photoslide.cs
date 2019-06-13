using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Photoslide : MonoBehaviour {

    private float timeStartedLerping;   //time that starts when we start lerping
    public Transform targetPosition;
    bool isMoving = false;

    public void clickThePhoto()
    {
        StartLerping();
        isMoving = true;
    }

    private void Update()
    {
        if(isMoving == true)
        transform.position = LerpV3(transform.position, targetPosition.position, timeStartedLerping, 1);
    }


    private void StartLerping() //Function for taking the starting time of lerping
    {
        timeStartedLerping = Time.time;
    }

    private Vector3 LerpV3(Vector3 startPosition, Vector3 endPosition, float timeStartedLerping, float lerpTime = 1) //Easier usage of 3D lerp
    {
        float timeSinceStarted = Time.time - timeStartedLerping;
        float percentageComplete = timeSinceStarted / lerpTime;
        Vector3 result = Vector3.Lerp(startPosition, endPosition, percentageComplete);
        return result;
    }
}
