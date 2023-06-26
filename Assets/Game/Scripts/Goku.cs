using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goku : CharacterController
{
    public ParticleSystem colliderWith;
    public void Start()
    {
        ParticleSystem.CollisionModule collisionModule = colliderWith.collision;

        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Enemy");
            health_UI.DisplayHealth(health, true);
            targetEnemy = GameObject.Find("Enemy").transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModule.collidesWith = LayerMask.GetMask("Enemy");
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Player");
            enemyHealthUI.DisplayHealth(health, false);
            targetEnemy = GameObject.Find("Player").transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModule.collidesWith = LayerMask.GetMask("Player");
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
        if (skillTwo.activeInHierarchy)
        {
            transform.position = new Vector3(transform.position.x, 2.2f, transform.position.z);
        }
    }
}
