//Nicholas Robinson
//Script that controls checkpoints, score, and respawn

using UnityEngine;
using System.Collections;

public class LevelManager : MonoBehaviour
{

    public GameObject currentCheckpoint;	//Variable to put the current checkpoint the player has obtained

    public PlayerController player;	//Reference to player

    public WeaponCharge weapon;		//Reference to weapon

    public GameObject deathParticle;	//Reference to death particle
    public GameObject respawnParticle;	//Reference to respawn particle

    public int pointPenaltyOnDeath;		//Integer for how many points to be lost upon death

    public float respawnDelay;		//Delay between death and respawn

    private CameraController camera;	//Reference to camera

    public HealthManager healthManager;

    public SwordAttack sword;

    // Use this for initialization
    void Start()
    {

        //player = FindObjectOfType<PlayerController>();		//Find player

        camera = FindObjectOfType<CameraController>();		//Find camera

        weapon = FindObjectOfType<WeaponCharge>();		//Find weapon

        healthManager = FindObjectOfType<HealthManager>();

        sword = FindObjectOfType<SwordAttack>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void RespawnPlayer()
    {

        StartCoroutine("RespawnPlayerCo");		//Coroutine happens outside the normal sequence of events, 
    }											//so I can put a delay in it, and it doesn't affect all the other code

    public IEnumerator RespawnPlayerCo()
    {

        //KILL EVERYTHING -------------------------------------------

        //Death particle
        Instantiate(deathParticle, player.transform.position, player.transform.rotation);

        //Player
        player.GetComponent<Renderer>().enabled = false;
        player.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
        player.enabled = false;

        //Camera
        camera.isFollowing = false;

        //Bow
        weapon.enabled = false;
        weapon.GetComponent<Renderer>().enabled = false;

        //Sword
        sword.enabled = false;
        sword.GetComponent<Renderer>().enabled = false;

        //RESPAWN EVERYTHING -------------------------------------------

        //Subtract points
        ScoreManager.AddPoints(-pointPenaltyOnDeath);

        //Move player before delay
        player.transform.position = currentCheckpoint.transform.position;

        //Respawn Delay
        yield return new WaitForSeconds(respawnDelay);

        //Respawn particle
        Instantiate(respawnParticle, currentCheckpoint.transform.position, currentCheckpoint.transform.rotation);

        //Player
        player.GetComponent<Renderer>().enabled = true;
        player.enabled = true;

        //Health
        healthManager.FullHealth();
        healthManager.isDead = false;

        //Camera
        camera.isFollowing = true;

        //Bow
        weapon.enabled = true;
        weapon.GetComponent<Renderer>().enabled = true;
        weapon.GetComponent<Animator>().SetBool("PlayerShoot", false);
        weapon.GetComponent<Animator>().CrossFade("Bow_Idle", 0f);

        //Sword
        sword.enabled = true;

    }

}
