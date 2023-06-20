using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buu : CharacterController
{

    GameObject enemy;
    GameObject player;
    public void Start()
    {
        //enemy = GameObject.Find("");
        //player = GameObject.Find("");
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        //if (LayerMask.LayerToName(gameObject.layer) == "PLayer")
        //{
        //    playerTrans = GetComponent<Transform>();
        //    targetEnemy = enemy.GetComponent<Transform>();
        //}
        //else
        //{
        //    playerTrans = player.GetComponent<Transform>();
        //    targetEnemy = GetComponent<Transform>();
        //}
        Ki();
        ResetComboState();
        AttackPoint();
    }

}
