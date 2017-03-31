using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartingCheckpoint : MonoBehaviour {

	public LevelManager levelManager;

	// Use this for initialization
	void Start () {
		levelManager = FindObjectOfType<LevelManager>();		//Find LevelManager
		levelManager.currentCheckpoint = gameObject;	//Set currentCheckpoint in LevelManager to this gameObject
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
