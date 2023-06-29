using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : CharacterController
{
    public void Start()
    {
        InitializeHealth(health);
        base.uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
        isLayer = LayerMask.LayerToName(gameObject.layer);
        if (isLayer == isPlayer)
        {
            collisionLayer = LayerMask.GetMask(isEnemy);
            base.uiManager.DisplayHealth(health, true);
            targetEnemy = GameObject.Find(isEnemy).transform;
        }
        else if (isLayer == isEnemy)
        {
            collisionLayer = LayerMask.GetMask(isPlayer);
            base.uiManager.DisplayHealth(health, false);
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
