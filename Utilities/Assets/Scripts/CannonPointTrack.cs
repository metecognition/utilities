using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using OVR;

public class CannonPointTrack : MonoBehaviour
{
    [Header("Aiming Variables")]
    public GameObject leftController;
    public GameObject rightController;
    private Vector3 trackingPoint;
    public float verticalOffset;

    [Header("Grabbing Variables")]
    public GameObject leftHandle;
    public GameObject rightHandle;
    public float grabDistance = 0.1f;
    public float forceReleaseDistance = 0.5f;
    private bool canGrabLeft, canGrabRight, isGrabbedLeft, isGrabbedRight = false;
    public bool canFire = false;

    [Header("Hand Positioning Variables")]
    public GameObject leftHand;
    public GameObject rightHand;


    void Update(){

        canFire = false;

        //Check if can grab each handle
        //Checks to see if player can grab handles based on distance
        canGrabLeft = false;
        canGrabRight = false;
        if (Vector3.Distance(leftController.transform.position, leftHandle.transform.position) < grabDistance) {
            canGrabLeft = true;
            //Debug.Log("can grab left");
        }
        if (Vector3.Distance(rightController.transform.position, rightHandle.transform.position) < grabDistance) {
            canGrabRight = true;
            //Debug.Log("can grab right");
        }


        //Check grabbing
        //Checks if player is attemping to grab handles, and if they can based on their current distance to the handles
        if (OVRInput.Get(OVRInput.Axis1D.PrimaryHandTrigger) > 0.9f) {
            if (canGrabLeft & !isGrabbedLeft) {
                isGrabbedLeft = true;
            }
        }
        else {
            isGrabbedLeft = false;
        }
        if (OVRInput.Get(OVRInput.Axis1D.SecondaryHandTrigger) > 0.9f) {
            if (canGrabRight & !isGrabbedRight) {
                isGrabbedRight = true;
            }
        }
        else {
            isGrabbedRight = false;
        }

        //Debug.Log(isGrabbedLeft + " " + isGrabbedRight);

        //Check force let go by distance
        //If player has moved their hands too far from the handles, then they lose grip of handles
        if (Vector3.Distance(leftController.transform.position, leftHandle.transform.position) > forceReleaseDistance) {
            isGrabbedLeft = false;
        }
        if (Vector3.Distance(rightController.transform.position, rightHandle.transform.position) > forceReleaseDistance) {
            isGrabbedRight = false;
        }

        //Moves tracking point
        //If player is grabbing both handles, then the tracking point moves, rotating the cannon
        if (isGrabbedLeft && isGrabbedRight) {
            //Debug.Log("is grabbed");
            canFire = true;
            trackingPoint = (leftController.transform.position + rightController.transform.position) / 2;
            trackingPoint.y -= verticalOffset;

            transform.position = trackingPoint;
        }

        //Move hand objects to handles if the handle is grabbed
        if (isGrabbedRight) {
            rightHand.transform.position = rightHandle.transform.position;
            rightHand.transform.rotation = rightHandle.transform.rotation;
        }else {
            rightHand.transform.position = rightController.transform.position;
            rightHand.transform.rotation = rightController.transform.rotation;
        }
        if (isGrabbedLeft) {
            leftHand.transform.position = leftHandle.transform.position;
            leftHand.transform.rotation = leftHandle.transform.rotation;
        }else {
            leftHand.transform.position = leftController.transform.position;
            leftHand.transform.rotation = leftController.transform.rotation;
        }
    }
}
