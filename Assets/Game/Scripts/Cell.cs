using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : CharacterController
{
    private UIManager uiManager;
    public void Start()
    {
        uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
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
        InitializeHealth(health);
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        Rotation(); 
        MoveFireball();
        skillOne.SetActive(false);
        if (!uiManager.isCountdownFinished)
            return;

        if (UIManager.playerWins == 1 || UIManager.enemyWins == 1)
        {
            if (!uiManager.shouldRestartCountdown)
            {
                uiManager.shouldRestartCountdown = true;
                uiManager.RestartCountdown();
            }
        }
        Ki();
        ResetComboState();
        AttackPoint();
       
    }

}
