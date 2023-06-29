using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buu : CharacterController
{
    public void Start()
    {
        InitializeHealth(health);
        uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
        isLayer = LayerMask.LayerToName(gameObject.layer);
        if (isLayer == isPlayer)
        {
            collisionLayer = LayerMask.GetMask(isEnemy);
            uiManager.DisplayHealth(health, true);
            targetEnemy = GameObject.Find(isEnemy).transform;
        }
        else if (isLayer == isEnemy)
        {
            collisionLayer = LayerMask.GetMask(isPlayer);
            uiManager.DisplayHealth(health, false);
            targetEnemy = GameObject.Find(isPlayer).transform;
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        Rotation();
        BO3();
        ResetComboState();
        AttackPoint();
        MoveFireball();
        Ki();
    }

}
