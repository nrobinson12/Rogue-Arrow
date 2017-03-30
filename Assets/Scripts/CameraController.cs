using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public PlayerController player;
	public bool isFollowing;
	Vector3 velocity = Vector3.zero;
	public float dampTime = 0.15f;

	public float xOffset;
	public float yOffset;

	// Use this for initialization
	void Start () {
	
		player = FindObjectOfType<PlayerController> (); // finds the player object

		isFollowing = true; // camera is following
	}
	
	// Update is called once per frame
	void Update () {
	
		if (isFollowing) {
			Vector3 playerPos = new Vector3 (player.transform.position.x + xOffset, player.transform.position.y + yOffset, transform.position.z);
			//Smooth camera movement
			transform.position = Vector3.SmoothDamp(transform.position, playerPos, ref velocity, dampTime);
		}
	}
}
