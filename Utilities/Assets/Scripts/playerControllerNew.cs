using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerControllerNew : MonoBehaviour {

    public Transform bottomLeft;
    public Transform bottomRight;
    public Transform middleLeft;
    public Transform middleRight;
    public Transform topLeft;
    public Transform topRight;
    public Transform bottomCenter;

    public float moveSpeed;
    public float maxFallSpeed;
    public float gravity;
    private float fallSpeed = 0;

    private bool canMoveLeft;
    private bool canMoveRight;

    public LayerMask whatIsGround;

    private Vector2 restartLocation;

    public float checkGroundRadius = 0.1f;

    private bool canClimb = false;
    public float climbSpeed;
    private bool climbing = false;

    public GameObject respawnEffect;

    private void Start() {
        restartLocation = transform.position;
    }

    // Update is called once per frame
    void Update()
    {

        //----------------------------
        //Check what parts of the player are touching ground
        //----------------------------
        bool bottomLeftGrounded = Physics2D.OverlapCircle(bottomLeft.position, checkGroundRadius, whatIsGround);
        bool bottomRightGrounded = Physics2D.OverlapCircle(bottomRight.position, checkGroundRadius, whatIsGround);
        bool middleLeftGrounded = Physics2D.OverlapCircle(middleLeft.position, checkGroundRadius, whatIsGround);
        bool middleRightGrounded = Physics2D.OverlapCircle(middleRight.position, checkGroundRadius, whatIsGround);
        bool topLeftGrounded = Physics2D.OverlapCircle(topLeft.position, checkGroundRadius, whatIsGround);
        bool topRightGrounded = Physics2D.OverlapCircle(topRight.position, checkGroundRadius, whatIsGround);
        bool bottomCenterGrounded = Physics2D.OverlapCircle(bottomCenter.position, checkGroundRadius, whatIsGround);

        //----------------------------
        //Horizontal player movement
        //----------------------------

        if (bottomLeftGrounded && middleLeftGrounded) {
            canMoveLeft = false;
        } else {
            canMoveLeft = true;
        }

        if (bottomRightGrounded && middleRightGrounded) {
            canMoveRight = false;
        }
        else {
            canMoveRight = true;
        }

        Vector2 goingTo = transform.position;

        if (Input.GetAxis("Horizontal") < 0 && canMoveLeft){
            goingTo.x += Input.GetAxis("Horizontal") * moveSpeed;
        } else if (Input.GetAxis("Horizontal") > 0 && canMoveRight) {
            goingTo.x += Input.GetAxis("Horizontal") * moveSpeed;
        }

        //----------------------------
        //Check if the player is on the ground
        //----------------------------
        if (bottomLeftGrounded && bottomRightGrounded && bottomCenterGrounded) {
            //print("On ground");
            fallSpeed = 0;
        } else if (bottomLeftGrounded || bottomRightGrounded){
            if (bottomCenterGrounded) {
                //print("Can choose to fall");
            }else if (!topRightGrounded && !topLeftGrounded && !climbing) {
                if (bottomLeftGrounded) {
                    goingTo.x += gravity;
                }
                else if (bottomRightGrounded) {
                    goingTo.x -= gravity;
                }
            }
        }
        else {
            //print("Falling");
            fallSpeed += gravity;
            if (fallSpeed > maxFallSpeed) {
                fallSpeed = maxFallSpeed;
            }
            goingTo.y -= fallSpeed;
            climbing = false;
        }

        //----------------------------
        //Check if the player can climb
        //----------------------------
        

        if (bottomLeftGrounded || bottomRightGrounded) {
            if (topLeftGrounded || topRightGrounded) {
                canClimb = true;
            } else if (climbing){
                canClimb = true;
            }
        }
        else {
            canClimb = false;
        }

        /*if (topRightGrounded || topLeftGrounded) {
            //print("Can choose to climb");
            if (bottomLeftGrounded || bottomRightGrounded) {
                canClimb = true;
            }
        } else if(climbing) {
            canClimb = true;
        } else {
            canClimb = false;
        }*/
        
        if (canClimb == true) {
            if (Input.GetAxis("Vertical") != 0) {
                if (bottomCenterGrounded || Input.GetAxis("Vertical") > 0) {
                    goingTo.y += climbSpeed * Input.GetAxis("Vertical");
                }
                climbing = true;
            } else {
                climbing = false;
            }
        }

        //----------------------------
        //Respawn player if falls off stage
        //----------------------------

        if (transform.position.y < -10) {
            goingTo = restartLocation;
            GameObject i = Instantiate(respawnEffect);
            i.transform.position = restartLocation;
        }

        transform.position = goingTo;

        
    }
}
