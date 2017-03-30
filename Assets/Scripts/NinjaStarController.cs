//Nicholas Robinson
//Code to make arrows work

using UnityEngine;
using System.Collections;

//ACTUALLY AN ARROW, NOT A NINJA STAR
public class NinjaStarController : MonoBehaviour
{
    public float timeToDie;		//Time that it has to live

    public PlayerController player;		//Reference to player

    public float projDamage = 10f;

    public GameObject enemyDeathEffect;		//Reference to death particle

    public GameObject impactEffect;		//Reference to impact particle

    public int pointsForKill;		//Number of points player gets per kill

    public Vector3 dir;		//Variable for arrow's velocity
    public float angle;		//Variable for angle in which the arrow should be facing

    public int damageToGive;

    // Use this for initialization
    void Start()
    {
        player = FindObjectOfType<PlayerController>();		//Find player
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)	//If arrow is moving
        {
            dir = GetComponent<Rigidbody2D>().velocity;		//Variable "dir" equals the velocity
            angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;		//Get angle arrow should be at
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);		//Rotate arrow so that it simulates arrow physics
        }
    }

    void OnTriggerEnter2D(Collider2D other)		//If anything collides with arrow
    {
        if (other.tag == "Enemy" || other.tag == "FlyingEnemy")		//If who collided with it was an enemy,
        {
            if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)	//If arrow is still moving (and not just on the ground),
            {
                //Instantiate(enemyDeathEffect, other.transform.position, other.transform.rotation);	//Play death particle on enemy
                //Destroy(other.gameObject);		//Destroy enemy
                //Destroy(gameObject);		//Destroy arrow	
                //ScoreManager.AddPoints(pointsForKill);		//Add points for kill

                other.GetComponent<EnemyHealthManager>().giveDamage(damageToGive);

                if (other.gameObject != null)
                {
                    GetComponent<Rigidbody2D>().isKinematic = true;

                    transform.parent = other.gameObject.transform;
                }
            }
        }
        else if (other.tag != "Player" && other.tag != "NinjaStar")		//If who collided with it was NOT the player or another arrow,
        {
            if (GetComponent<Rigidbody2D>().velocity != Vector2.zero)	//If arrow is still moving,
                Instantiate(impactEffect, transform.position, transform.rotation);		//Play impact particle (Because it hit a wall or ground)
            GetComponent<Rigidbody2D>().velocity = Vector2.zero;	//Change velocity to 0
            GetComponent<Rigidbody2D>().gravityScale = 0;		//Change gravity to 0 (This simulates being stuck in the wall or ground)
        }
    }
    void Awake()
    {
        Destroy(this.gameObject, timeToDie);	//Destroy arrow after timeToDie seconds
    }

}
