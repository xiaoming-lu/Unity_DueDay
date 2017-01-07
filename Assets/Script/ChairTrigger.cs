using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class ChairTrigger : MonoBehaviour {


  
   public  GameObject player;
    playerController script;
    Animator anim;
   
    void Awake()
    {

       anim = player.GetComponent<Animator>();
        script = player.GetComponent<playerController>();
    }


    void OnTriggerEnter(Collider other)
    {
        print("Enter");
        if (other.gameObject == player)
        {
            
            script.IsNearChair = true;
            anim.SetBool("IsNearChair", true);
        }
    }


    void OnTriggerExit(Collider other)
    {
        print("Exit");
        if (other.gameObject == player)
        {
       
            script.IsNearChair = false;
            anim.SetBool("IsNearChair", false);
        }
    }


   
}
