using System.Collections;
using System.Collections.Generic;
using UnityEngine;



//-----------------------------------------------------------------------------------------------
//This class creates a laser pointer line, or can be used to create a 3D line between two objects
//-----------------------------------------------------------------------------------------------

public class Line3D : MonoBehaviour
{
    //line should be a cylinder 1 unit tall, and 1 unity in radius
    [Header("Line Settings")]
    public Transform pointingObject;  //assign to hand, or whatever is shining the laser
    public Transform pointingAt; //assign to raycast hit through code
    public float radius; //recommend 0.02 for radius

    //Note, pointEnd must be assigned regardless of whether it is used
    [Header("Point Settings")]
    public bool hasPoint = false;
    public Transform pointEnd;
    public float pointScale;  //if using default cube, I recommend something around 0.05;


    

    // Update is called once per frame
    void LateUpdate()
    {
        //finds position between the hand and raycast hit
        Vector3 newPos = Vector3.Lerp(pointingAt.position, pointingObject.position, 0.5f);
        
        //finds the required length to draw the lien
        float length = Vector3.Distance(pointingObject.position, pointingAt.position);
        Vector3 newScale = new Vector3(radius, radius, length) ;

        //rotate to point at the raycast hit
        transform.LookAt(pointingAt.position);

        //assign transform variables
        transform.position = newPos;
        transform.localScale = newScale;

        //if hasPoint is true, then move pointer to the raycast hit, and rotate it to face the hand
        if (hasPoint)
        {
            pointEnd.position = pointingAt.position;
            pointEnd.localScale = new Vector3(pointScale, pointScale, pointScale);
            pointEnd.LookAt(pointingObject);
        }
    }
}
