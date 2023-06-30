using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : CharacterController
{
    public ParticleSystem colliderWithKame;
    public void Start()
    {
        InitializeHealth(health);
        base.uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
        ParticleSystem.CollisionModule collisionModuleKame = colliderWithKame.collision;
        isLayer = LayerMask.LayerToName(gameObject.layer);
        if (isLayer == isPlayer)
        {
            collisionLayer = LayerMask.GetMask(isEnemy);
            base.uiManager.DisplayHealth(health, true);
            targetEnemy = GameObject.Find(isEnemy).transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isEnemy);
        }
        else if (isLayer == isEnemy)
        {
            collisionLayer = LayerMask.GetMask(isPlayer);
            base.uiManager.DisplayHealth(health, false);
            targetEnemy = GameObject.Find(isPlayer).transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isPlayer);
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
