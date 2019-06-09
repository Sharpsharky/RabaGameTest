using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace HutongGames.PlayMaker.Actions
{
    [ActionCategory("_Common")]
    [Tooltip("Zoom in on an object. Scale to a given scale.")]
    public class ZoomCameraOnObject : FsmStateAction
    {
        // It will be used for zoom out
        //private const float CONTAINER_X_AXIS_DEFAULT_SCALE = 0.0035f;
        private const float CONTAINER_X_AXIS_DEFAULT_SCALE = 0.0f;

        public FsmOwnerDefault objectToZoomInto;
        public FsmBool ResetZoom;
        [Tooltip("When none it will scale current room background/container to default value.")]
        public FsmGameObject ResetSpecificObject;
        public FsmVector3 position;
        public FsmBool useContainerObjectInsteadOfScene = true;
        public FsmGameObject UseThisObjectInsteadOfScene;
        public FsmBool localSpace;
        public FsmVector3 scaleUpBy;
        public FsmFloat animationTime;

        public FsmEvent finishedEvent;

        public iTween.EaseType easeType = iTween.EaseType.linear;
        public FsmBool useOriginalScaleAndPositionObject = false;
        public FsmGameObject originalScaleAndPositionObject;

        public override void Reset()
        {
            ResetSpecificObject = null;
            UseThisObjectInsteadOfScene = null;
            finishedEvent = null;
            objectToZoomInto = null;
            scaleUpBy = null;
            ResetZoom = false;
            position = new FsmVector3 { UseVariable = true };
            animationTime = 0;
            localSpace = false;
            useContainerObjectInsteadOfScene = true;
        }

        public override void OnEnter()
        {
            ZoomIn();
            StartCoroutine(CallDone());
            Finish();
        }

        IEnumerator CallDone()
        {
            yield return new WaitForSeconds(animationTime.Value);
            if (finishedEvent != null)
            {
                Fsm.Event(finishedEvent);
            }
        }

        void ZoomIn()
        {
            // Take background to scale
            GameObject background = null; // EscapeMainCamera.instance.currentRoom;

            // If it should reset zoom and there is a specific gameobject to scale/zoom out
            if (ResetZoom.Value && ResetSpecificObject != null && ResetSpecificObject.Value != null)
            {
                background = ResetSpecificObject.Value;
            }
            // If there is specifc object to zoom/scale
            if (UseThisObjectInsteadOfScene.Value != null)
            {
                background = UseThisObjectInsteadOfScene.Value;
            }
            // If there is no specific object, but it should take Container instead of Room
            else if (useContainerObjectInsteadOfScene.Value)
            {
                var containerTransform = background.transform.Find("Container");
                if (containerTransform)
                {
                    background = containerTransform.gameObject;
                }
                else
                {
                    Debug.LogError("Use Container instead of a scene but there is no Container gameobject!");
                }
            }

            if (background == null)
            {
                Debug.LogError("In OneApp you need to specify object to scale. It's good to set background, room or container.");
                return;
            }
            
            // Get space
            bool local = localSpace.Value;

            // If this is zoom reset/zoom out
            if (ResetZoom.Value)
            {
                scaleUpBy.Value = Vector3.one;
                // Container default scale is not 1,1,1 but something like 1.0035,0,0 so we need to add 0.0035
                if (useContainerObjectInsteadOfScene.Value)
                {
                    scaleUpBy.Value += new Vector3(CONTAINER_X_AXIS_DEFAULT_SCALE, 0, 0);
                }              
            }


            // correct x to meet device aspect ratio
            Vector3 scaleUpByValue = scaleUpBy.Value;
            if (useOriginalScaleAndPositionObject.Value)
            {
                var roomScale = originalScaleAndPositionObject.Value.transform.localScale;
                scaleUpByValue = new Vector3(scaleUpByValue.x * roomScale.x, scaleUpByValue.y * roomScale.y, scaleUpByValue.z * roomScale.z);
            }


            iTween.ScaleTo(background, iTween.Hash("scale", scaleUpByValue, "time", animationTime.Value, "easeType", easeType));


            if (ResetZoom.Value)
            {
                if (useOriginalScaleAndPositionObject.Value)
                {
                    iTween.MoveTo(background, iTween.Hash("position", originalScaleAndPositionObject.Value.transform.position, "time", animationTime.Value, "easeType", easeType, "islocal", false));
                }
                else
                {
                    local = true;
                    iTween.MoveTo(background, iTween.Hash("position", Vector3.zero, "time", animationTime.Value, "easeType", easeType, "islocal", local));
                }
                return;
            }

            Vector3 targetPosition = Vector3.zero;
            Vector3 distanceToMove;

            if (position != null && !position.IsNone)
            {
                targetPosition = position.Value;
            }
            else
            {
                targetPosition = Fsm.GetOwnerDefaultTarget(objectToZoomInto).transform.position;
            }

            distanceToMove = background.transform.position - targetPosition;


            distanceToMove.x *= scaleUpBy.Value.x;
            distanceToMove.y *= scaleUpBy.Value.y;
            distanceToMove.z *= scaleUpBy.Value.z;
            iTween.MoveBy(background, iTween.Hash("amount", distanceToMove, "time", animationTime.Value, "easeType", easeType, "islocal", local));


        }

    }
}