//Name: Jeffrey Langballe
//Date: June 8, 2015
//Desc: A script that attaches to a ladder that tells if the player is touching it
using UnityEngine;
using System.Collections;

public class ClimbLadder : MonoBehaviour
{

    public bool touchingLadder = false;
    Controller2D controller;

    void Start()
    {
        controller = FindObjectOfType<Controller2D>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.GetComponent<Controller2D>() == null)
            return;
        //Only reaches this point if the player is in the ladder area
        touchingLadder = true;
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.GetComponent<Controller2D>() == null)
            return;
        //Only reaches this point if the player leaves the ladder area
        touchingLadder = false;
        controller.doubleJumped = false;
        controller.climbingLadder = false;
    }
}
