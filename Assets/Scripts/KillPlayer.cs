//Nicholas Robinson
//Whoever has this code, will kill the player if collided

using UnityEngine;
using System.Collections;

public class KillPlayer : MonoBehaviour
{

    public LevelManager levelManager;		//Reference LevelManager

    // Use this for initialization
    void Start()
    {

        levelManager = FindObjectOfType<LevelManager>();	//Find LevelManager
    }

    // Update is called once per frame
    void Update()
    {

    }

    void OnTriggerEnter2D(Collider2D other)
    {	//If anything collides this object

        if (other.name == "Player")
        {	//If who collided was the player,

            levelManager.RespawnPlayer();	//Call respawn method in LevelManager script
        }
    }

}
