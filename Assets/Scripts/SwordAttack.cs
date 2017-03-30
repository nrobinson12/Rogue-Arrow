using UnityEngine;
using System.Collections;

public class SwordAttack : MonoBehaviour {

    private Animator anim;

    public WeaponCharge weapon;

    public bool swordAttack;

	// Use this for initialization
	void Start () {

        anim = GetComponent<Animator>();

        weapon = FindObjectOfType<WeaponCharge>();
	}
	
	// Update is called once per frame
	void Update () {


        if (swordAttack == false)
        {
            weapon.enabled = true;
            weapon.GetComponent<Renderer>().enabled = true;
        }


        if (Input.GetButtonDown("Fire2"))
        {	//Player presses down RMB

            Debug.Log("Stabbing");

            swordAttack = true;

            weapon.enabled = false;
            weapon.GetComponent<Renderer>().enabled = false;

            //When player uses sword, play sword animation
            anim.SetBool("Sword", swordAttack);
        }

        

        if (Input.GetMouseButtonUp(1))
        {	//Player presses up RMB

            swordAttack = false;
            anim.SetBool("Sword", swordAttack);
        }

	}

}
