using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gohan : CharacterController
{
    GameObject enemy;
    GameObject player;
    public void Start()
    {
        //enemy = GameObject.Find("");
        //player = GameObject.Find("");
        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Enemy");
            health_UI.DisplayHealth(health, true);
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Player");
            enemyHealthUI.DisplayHealth(health, false);
        }
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
        MoveFireball();
    }

}
