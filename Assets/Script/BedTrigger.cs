using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BedTrigger : MonoBehaviour {


    public GameObject player;

    Animator anim;
    playerController script;

    void Awake()
    {

        anim = player.GetComponent<Animator>();
        script = player.GetComponent<playerController>();
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.gameObject == player)
        {
            script.IsNearBed = true;
            anim.SetBool("IsNearBed", true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        if (other.gameObject == player)
        {
            script.IsNearBed = false;
            anim.SetBool("IsNearBed", false);
        }
    }



}
