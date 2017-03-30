//Nicholas Robinson
//Picking up coins and gaining score

using UnityEngine;
using System.Collections;

public class CoinPickup : MonoBehaviour
{

    public int pointsToAdd;		//How many points you get for collecting a coin
    public GameObject currentCoin;		//Reference to current coin obtained
    public GameObject coinParticle;		//Reference to the coinParticle

    void OnTriggerEnter2D(Collider2D other)
    {		//If anything enters coin

        if (other.GetComponent<PlayerController>() == null)	//If not the player, go back to top of function
            return;

        Instantiate(coinParticle, gameObject.transform.position, gameObject.transform.rotation);	//Play the coinParticle

        ScoreManager.AddPoints(pointsToAdd);		//Add the points to your score

        Destroy(gameObject);		//Destroy coin
    }

}
