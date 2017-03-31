using UnityEngine;
using System.Collections;

public class HurtPlayerOnContact : MonoBehaviour {

    public int damageToGive;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

    void OnTriggerEnter2D(Collider2D other)
    {	//If anything collides this object

        if (other.name == "Player")
        {	//If who collided was the player,

            HealthManager.HurtPlayer(damageToGive);

            other.GetComponent<AudioSource>().Play();

            //Knockback doesn't work

            /*var player = other.GetComponent<PlayerController>();
            player.knockbackCount = player.knockbackLength;

            if (other.transform.position.x < transform.position.x)
                player.knockFromRight = true;
            else
                player.knockFromRight = false;*/
        }
    }

}
