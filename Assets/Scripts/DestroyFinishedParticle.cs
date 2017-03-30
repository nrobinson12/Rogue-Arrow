//Nicholas Robinson
//Whoever has this code, will destroy the particle it played appropriately

using UnityEngine;
using System.Collections;

public class DestroyFinishedParticle : MonoBehaviour
{

    private ParticleSystem thisParticleSystem;

    // Use this for initialization
    void Start()
    {

        thisParticleSystem = GetComponent<ParticleSystem>();	//Reference to particle
    }

    // Update is called once per frame
    void Update()
    {

        if (thisParticleSystem.isPlaying)	//If particle is playing, return to top of Update function
            return;

        Destroy(gameObject);	//Destroy particle
    }

    void OnBecameInvisible()
    {		//If camera cannot see particle anymore

        Destroy(gameObject);		//Destroy particle
    }

}
