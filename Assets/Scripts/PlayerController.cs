//Name: Jeffrey Langballe
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
[RequireComponent (typeof (Controller2D))]
public class PlayerController : MonoBehaviour {

	public Vector2 input;
	public Vector3 velocity;
	public float jumpHeight = 4;			//How high the player can jump
	public float timeToJumpApex = 0.4f;
	public float moveSpeed;					//Move speed of the player
	public float ladderSpeed;
	float accelerationTimeAirborne = .2f;
	float accelerationTimeGrounded = .1f;
	float gravity;							//Depends on jump variables
	float jumpVelocity;						//Depends on jump variables
	float velocityXSmoothing;

	public bool isDead = false;

	[HideInInspector]
	public bool facingRight = true;

	BoxCollider2D collider;
	ClimbLadder[] ladder;
	Controller2D controller;
	Animator anim;

    public float knockback;
    public float knockbackLength;
    public float knockbackCount;
    public bool knockFromRight;

	public virtual void Start () {
		collider = GetComponent<BoxCollider2D> ();
		controller = GetComponent<Controller2D>();
		anim = GetComponent<Animator> ();
		ladder = FindObjectsOfType<ClimbLadder> ();
		//health.ResetHealth(maxHealth);
		//curHealth = health.curHealth;
		gravity = -(2 * jumpHeight) / Mathf.Pow (timeToJumpApex, 2);
		jumpVelocity = Mathf.Abs (gravity) * timeToJumpApex;
		//print ("Gravity: " + gravity + " Jump Velocity: " + jumpVelocity);
	}
	
	void Update () {
        if (isDead)	//If the player is dead, this prevents the user from moving the player around
        {
            return;
        }


		//Save the raw input of the user as a vector2(Up, down, left, right)
		input = new Vector2 (Input.GetAxisRaw ("Horizontal"), Input.GetAxisRaw ("Vertical"));

		if (controller.collisions.above || controller.collisions.below){	//Reset velocity if touching something from above or below
			velocity.y = 0;
		}

		if (controller.collisions.below) {	//If the player is on the ground, it is safe to assume they have not double jumped
			controller.doubleJumped = false;
		}

		anim.SetBool ("Grounded", controller.collisions.below);

		//Set health slider value			
		//healthSlider.value =  health.curHealth;

		//Working jump, doublejump, and jump from ladder
		if (Input.GetButtonDown("Jump")){
			if (controller.collisions.below){	//Jumping on ground
				velocity.y = jumpVelocity;
			}
			else if (!controller.doubleJumped && !controller.climbingLadder){	//Double jumping
				velocity.y = jumpVelocity;
				controller.doubleJumped = true;
			}
			if (controller.climbingLadder){	//Jumping off of ladder
				velocity.y = jumpVelocity;
				controller.climbingLadder = false;
			}
		}

		//Smooth left and right movement
		float targetVelocityX = input.x * moveSpeed;
		velocity.x = Mathf.SmoothDamp (velocity.x, targetVelocityX, ref velocityXSmoothing, (controller.collisions.below)?accelerationTimeGrounded:accelerationTimeAirborne);
		//Apply gravity
		velocity.y += gravity * Time.deltaTime;	

		//Climb ladder by touching it and pressing up or down to lock on
		for (int i = 0; i < ladder.Length; i++){
			if (ladder[i].touchingLadder) {
				if (input.y != 0)	//Lock onto ladder by pressing up or down
					controller.climbingLadder = true;			
				if (controller.climbingLadder){	//True if player is climbing ladder
					velocity.y = input.y * ladderSpeed;
					velocity.x = 0;
					transform.position = new Vector3(ladder[i].transform.position.x, transform.position.y, transform.position.z);
				}
				//Check if player is inside the ground
				Bounds bounds = collider.bounds;
				bounds.Expand (controller.skinWidth * -2);
				if (Physics2D.OverlapArea(new Vector2(bounds.min.x, bounds.min.y), new Vector2(bounds.max.x, bounds.max.y), controller.whatIsGround)){
				//Returns true if player is touching a ladder and is "inside" the ground
					velocity.x = 0;	//Prevent player from drifting side to side. (Player will autmatically move up or down to a valid region)
				}
			}
		}
	
		//Move player by calling collisions controller
		controller.Move (velocity * Time.deltaTime);

        //Set animation when player moves
        anim.SetFloat("Speed", Mathf.Abs(velocity.x));
		}

	}
