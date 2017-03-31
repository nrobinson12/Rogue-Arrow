//Nicholas Robinson
//Code to make checkpoints work

using UnityEngine;
using System.Collections;

public class Checkpoint : MonoBehaviour
{

    public LevelManager levelManager;		//Reference LevelManager
    private Animator anim;		//Reference animator that is attached to this gameObject

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();		//Find animator

        levelManager = FindObjectOfType<LevelManager>();		//Find LevelManager
    }

    // Update is called once per frame
    void Update()
    {
    }

    //If anything enters checkpoint
    void OnTriggerEnter2D(Collider2D other)
    {

        if (other.name == "Player")		//If who entered was the player,
        {
            anim.SetBool("PlayerEntered", true);		//Set "PlayerEntered" boolean to true 
            //(When this boolean is true, it plays the animation of flag going up)

            levelManager.currentCheckpoint = gameObject;	//Set currentCheckpoint in LevelManager to this gameObject

            //Debug.Log("Activated Checkpoint " + transform.position);

        }
    }
}
