using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goku : CharacterController
{
    private UIManager uiManager;
    public ParticleSystem colliderWith;
    public void Start()
    {
        GameObject spiritboom = Instantiate(skillThree);
        spiritboom.SetActive(false);
        spiritboom.name = "SpiritBoomOfGoku";
        skillThree = spiritboom;
        ParticleSystem.CollisionModule collisionModule = colliderWith.collision;
        uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
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
            skillThree.layer = gameObject.layer;
            SetLayerRecursively(skillThree, skillThree.layer);
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
       
        if (skillTwo.activeInHierarchy)
        {
            transform.position = new Vector3(transform.position.x, 2.2f, transform.position.z);
            kiFull.transform.position = new Vector3(kiFull.transform.position.x, -0.0001f, kiFull.transform.position.z);
        }
        else if (!skillTwo.activeInHierarchy)
        {
            kiFull.transform.position = transform.position;
        }
        if(skillThree.activeInHierarchy)
        {
            ParticleLifetimesAndMoveSpiritBoom(skillThree.transform);
        }
    }
}
