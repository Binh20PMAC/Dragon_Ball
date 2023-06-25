using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buu : CharacterController
{

    GameObject enemy;
    GameObject player;
    public void Start()
    {
        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Enemy");
            health_UI.DisplayHealth(health, true);
            targetEnemy = GameObject.Find("Enemy").transform;
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Player");
            enemyHealthUI.DisplayHealth(health, false);
            targetEnemy = GameObject.Find("Player").transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        Rotation();
        Ki();
        ResetComboState();
        AttackPoint();
        MoveFireball();
    }

}
