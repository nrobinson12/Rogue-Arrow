//Name: Jeffrey Langballe
using UnityEngine;
using System.Collections;
[RequireComponent (typeof (BoxCollider2D))]
public class Controller2D : MonoBehaviour {

	public float skinWidth = 0.05f;

	public float maxClimbAngle = 75;
	public float maxDescendAngle = 75;
	public float climbSpeedMultiplier = 1;
	public float descendSpeedMultiplier = 1;

	public LayerMask whatIsGround;			//Layer that is "ground"

	[HideInInspector]
	public bool doubleJumped; 	
	
	//Stuff for raycasting collision
	[HideInInspector]
	public int horizontalRayCount = 4;
	[HideInInspector]
	public int verticalRayCount = 4;
	float horizontalRaySpacing;
	float verticalRaySpacing;
	
	BoxCollider2D collider;
	RaycastOrigins raycastOrigins;
	
	public CollisionInfo collisions;
	public bool climbingLadder;

	public virtual void Start () {
		collider = GetComponent<BoxCollider2D> ();
		CalculateRaySpacing ();
		collisions.Reset();
	}

	public void Move (Vector3 velocity) {
		UpdateRaycastOrigins ();
		collisions.Reset();
		collisions.velocityOld = velocity;

		if (!climbingLadder){	
			if (velocity.y < 0){
				DescendSlope (ref velocity);
			}
			if (velocity.x !=0){
				HorizontalCollisions (ref velocity);
			}
			if (velocity.y !=0){
				VerticalCollisions(ref velocity);
			}
		}
	
		transform.Translate(velocity);
	}

	void ClimbSlope(ref Vector3 velocity, float slopeAngle){
		/*Pre: Player is moving up a slope
		 * Post: proper x and y values for velocity are calculated using the slope angle and trig */
		float moveDistance = Mathf.Abs (velocity.x);
		//Calculate the x and y components of velocity using the hypotenuse, angle, and trig
		float climbVeloctiyY = moveDistance * Mathf.Sin (slopeAngle * Mathf.Deg2Rad);
		if (velocity.y <= climbVeloctiyY){
			//Player is NOT jumping on the slope
			velocity.y = climbVeloctiyY;
			velocity.x = moveDistance * Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
			collisions.below = true;
			collisions.climbingSlope = true;
			collisions.slopeAngle = slopeAngle;
		}
	}
	void DescendSlope (ref Vector3 velocity){
		/*Pre: Player is moving downwards
		 * Post: if player is on slope proper x and y values for velocity are calculated using the slope angle and trig */
		float directionX = Mathf.Sign (velocity.x);
		//Set rayorigin depending on direction
		Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomRight:raycastOrigins.bottomLeft;
		RaycastHit2D hit = Physics2D.Raycast (rayOrigin, -Vector2.up, Mathf.Infinity, whatIsGround);
		
		if (hit) {	//If the ray hits the ground layer
			float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
			if (slopeAngle != 0 && slopeAngle <= maxDescendAngle){
				if (Mathf.Sign (hit.normal.x) == directionX){
					if (hit.distance - skinWidth <= Mathf.Tan (slopeAngle * Mathf.Deg2Rad) * Mathf.Abs (velocity.x)){
						float moveDistance = Mathf.Abs (velocity.x);
						float descendVeloctiyY = moveDistance * Mathf.Sin (slopeAngle * Mathf.Deg2Rad);
						velocity.x = moveDistance * Mathf.Cos (slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
						velocity.y -= descendVeloctiyY;
						collisions.slopeAngle = slopeAngle;
						collisions.descendingSlope = true;
						collisions.below = true;
					}
				}
			}
		}
		
	}
	
	void HorizontalCollisions (ref Vector3 velocity){
		/*Pre: Player is moving horizontally
		 * Post: Check if player will move through object on update
		 * 			reduce velocity.x so player does not pass through object */
		float directionX = Mathf.Sign (velocity.x);
		float rayLength = Mathf.Abs (velocity.x) + skinWidth;

		for (int i = 0; i < horizontalRayCount; i++) {
			Vector2 rayOrigin = (directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight;
			rayOrigin += Vector2.up * (horizontalRaySpacing * i);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.right * directionX, rayLength, whatIsGround);

			Debug.DrawRay(rayOrigin, Vector2.right * directionX * rayLength, Color.red); 
			if (hit) {	//If the ray hits ground layer
				float slopeAngle = Vector2.Angle(hit.normal, Vector2.up);
				
				if (i==0 && slopeAngle <= maxClimbAngle){	//bottommost ray
					if (collisions.descendingSlope){
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float distanceToSlopeStart = 0;
					if (slopeAngle != collisions.slopeAngleOld){//Climbing a new slope
						distanceToSlopeStart = hit.distance - skinWidth;
						velocity.x -= distanceToSlopeStart * directionX;
					}
					ClimbSlope(ref velocity, slopeAngle);
					velocity.x += distanceToSlopeStart * directionX;
				}
				
				if (!collisions.climbingSlope || slopeAngle > maxClimbAngle){
					velocity.x = (hit.distance - skinWidth) * directionX;
					rayLength = hit.distance;
					
					if (collisions.climbingSlope){
						velocity.y = Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Abs(velocity.x);  
					}
					collisions.left = directionX == -1;
					collisions.right = directionX == 1;
				}
			}
		}
	}
	
	void VerticalCollisions (ref Vector3 velocity){
		/*Pre: Player is moving vertially
		 * Post: Check if player will move through object on update
		 * 			reduce velocity.y so player does not pass through object */
		float directionY = Mathf.Sign (velocity.y);
		float rayLength = Mathf.Abs (velocity.y) + skinWidth;
		
		for (int i = 0; i < verticalRayCount; i++) {
			Vector2 rayOrigin = (directionY == -1)?raycastOrigins.bottomLeft:raycastOrigins.topLeft;
			rayOrigin += Vector2.right * (verticalRaySpacing * i + velocity.x);
			RaycastHit2D hit = Physics2D.Raycast(rayOrigin, Vector2.up * directionY, rayLength, whatIsGround);

			Debug.DrawRay(rayOrigin, Vector2.up * directionY * rayLength, Color.red); 
			if (hit) {	//If the ray hits
				velocity.y = (hit.distance - skinWidth) * directionY;
				rayLength = hit.distance;
				
				if (collisions.climbingSlope){
					velocity.x = velocity.y / Mathf.Tan (collisions.slopeAngle * Mathf.Deg2Rad) * Mathf.Sign (velocity.x);
				}

				collisions.below = directionY == -1;
				collisions.above = directionY == 1;
			}
		}
		
		if (collisions.climbingSlope){
			float directionX = Mathf.Sign (velocity.x);
			rayLength = Mathf.Abs (velocity.x) + skinWidth;
			Vector2 rayOrigin = ((directionX == -1)?raycastOrigins.bottomLeft:raycastOrigins.bottomRight) + Vector2.up * velocity.y;
			RaycastHit2D hit = Physics2D.Raycast (rayOrigin, Vector2.right * directionX, rayLength, whatIsGround);
			
			if (hit){
				float slopeAngle = Vector2.Angle (hit.normal, Vector2.up);
				if (slopeAngle != collisions.slopeAngle){
					velocity.x = (hit.distance - skinWidth) * directionX;
					collisions.slopeAngle = slopeAngle;
				}
			}
		}
	}
	
	void UpdateRaycastOrigins () {
		Bounds bounds = collider.bounds;
		//Inset the raycast origins from the perimiter into the skinwidth of the collider
		bounds.Expand (skinWidth * -2);

		raycastOrigins.bottomLeft = new Vector2 (bounds.min.x, bounds.min.y);
		raycastOrigins.bottomRight = new Vector2 (bounds.max.x, bounds.min.y);
		raycastOrigins.topLeft = new Vector2 (bounds.min.x, bounds.max.y);
		raycastOrigins.topRight = new Vector2 (bounds.max.x, bounds.max.y);
	}
	
	void CalculateRaySpacing () {
		Bounds bounds = collider.bounds;
		//Inset the raycast origins from the perimiter into the skinwidth of the collider
		bounds.Expand (skinWidth * -2);
		
		//Ensure that the number of horizontal and vertical rays are at least 2
		horizontalRayCount = Mathf.Clamp (horizontalRayCount, 2, int.MaxValue);
		verticalRayCount = Mathf.Clamp (verticalRayCount, 2, int.MaxValue);

		horizontalRaySpacing = bounds.size.y / (horizontalRayCount - 1);
		verticalRaySpacing = bounds.size.x / (verticalRayCount - 1);
	}
	
	struct RaycastOrigins {		//Where to send raycasts from
		public Vector2 topLeft, topRight;
		public Vector2 bottomLeft, bottomRight;	
	}
	
	public struct CollisionInfo{
		public bool above, below;
		public bool left, right;

		public bool climbingSlope;
		public bool descendingSlope;

		public float slopeAngle, slopeAngleOld;
		public Vector3 velocityOld;
		
		public void Reset(){
			descendingSlope = false;
			climbingSlope = false;
			above = below = false;
			left = right = false;
			
			slopeAngleOld = slopeAngle;
			slopeAngle = 0;
		}
	}
}
