//Nicholas Robinson
//Making the walking zombie enemy move left and right

using UnityEngine;
using System.Collections;

public class EnemyPatrol : MonoBehaviour
{

    public float moveSpeed;		//How fast zombie walks
    public bool moveRight;		//True if zombie is moving right

    public Transform wallCheck;		//Game object checks if zombie is hitting wall
    public float wallCheckRadius;	//A float that is the radius of the game object "WallCheck"
    public LayerMask whatIsWall;	//Determines what layer is wall (Ground)
    private bool hittingWall;		//True if game object "WallCheck" is hitting a wall (Ground)

    private bool notAtEdge;			//True if zombie is not at edge, just walking
    public Transform edgeCheck;		//Game object that checks if zombie is on the edge of a cliff 

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

        //If ground overlaps this wallCheck circle, it returns true
        hittingWall = Physics2D.OverlapCircle(wallCheck.position, wallCheckRadius, whatIsWall);

        //If NO ground overlaps this notAtEdge circle, it returns false
        notAtEdge = Physics2D.OverlapCircle(edgeCheck.position, wallCheckRadius, whatIsWall);

        if (hittingWall || !notAtEdge)	//If zombie is either hitting a wall or at an edge,
            moveRight = !moveRight;		//change direction of zombie

        if (moveRight)
        {								//If zombie is moving right, flip zombie to face direction, and 
            transform.localScale = new Vector3(-1f, 1f, 1f);		//change velocity to right direction
            GetComponent<Rigidbody2D>().velocity = new Vector2(-moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
        else
        {										//If zombie is moving left, flip zombie to face direction, and
            transform.localScale = new Vector3(1f, 1f, 1f);		//change velocity to left direction
            GetComponent<Rigidbody2D>().velocity = new Vector2(moveSpeed, GetComponent<Rigidbody2D>().velocity.y);
        }
    }
}
