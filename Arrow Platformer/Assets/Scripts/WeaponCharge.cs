//Name: Jeffrey Langballe
//Desc: Script for weapons that "charge up"
//		Hold down LMB for increased speed
//		Projectile has does damage equal to its velocity
using UnityEngine;
using System.Collections;

public class WeaponCharge : MonoBehaviour
{

    bool charging;

    public float timeToMaxCharge = 2f;		//In seconds
    public float timeBetweenAdd = 0.5f; 	//How much time between each charge "tier"

    public float maxVelocity = 20f;
	public float minVelocity = 0f;
    float velocityAddPerTime;
    float velocityToAdd;

    float timeOld = 0.0f;
    //public float fireRate;

    public GameObject projectile;

    private Animator anim;

    public Transform firePoint;
    public GameObject ninjaStar;
	PlayerController player;

    void Start()
    {
		//Calculate amount of velocity to add per "charge tick"
		velocityAddPerTime = ((maxVelocity - minVelocity) / timeToMaxCharge) * timeBetweenAdd;

		player = FindObjectOfType<PlayerController> ();
        anim = GetComponent<Animator>();

    }

    void Update()
    {
		if (player.isDead) {
			if (charging)	//Player dies while charging
				charging = false;	//Stop charging
			return;		//Prevent player from charging more arrows while dead
		}

        if (Input.GetButtonDown("Fire1"))
        {	//Player presses down LMB
            charging = true;
            velocityToAdd = minVelocity;
            Debug.Log("Charging");

            //When player shoots arrow, play Bow_PullBack animation (plays when "PlayerShoot" is true)
            anim.SetBool("PlayerShoot", charging);
        }

        if (charging)
        {	//Player is holding LMB
            if (velocityToAdd < maxVelocity)
            {
                if (Time.time - timeOld >= timeBetweenAdd)
                {
                    timeOld = Time.time;
                    velocityToAdd += velocityAddPerTime;
                }
            }
        }

        if (Input.GetMouseButtonUp(0))
        {	//Player releases LMB (Fire the arrow!)
            GameObject clone = (GameObject)Instantiate(ninjaStar, transform.position + 1.0f * transform.forward, transform.rotation);

            float rotationInRad = transform.rotation.eulerAngles.z * Mathf.Deg2Rad;
            Vector2 dir = new Vector2(velocityToAdd * Mathf.Cos(rotationInRad), velocityToAdd * Mathf.Sin(rotationInRad));
            Debug.Log("pew");
            clone.GetComponent<Rigidbody2D>().velocity += dir;	//Add chargevelocity to the arrow
            clone.GetComponent<NinjaStarController>().projDamage = velocityToAdd;
            charging = false;

            //When player fires arrow, change animation back to rest (without arrow)
            anim.SetBool("PlayerShoot", charging);
			anim.CrossFade("Bow_Back", 0f);
        }
    }
}
