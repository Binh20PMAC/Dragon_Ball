using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goku : CharacterController
{
    public ParticleSystem colliderWith;
    public void Start()
    {
        GameObject spiritboom = Instantiate(skillThree);
        spiritboom.SetActive(false);
        spiritboom.name = "SpiritBoomOfGoku";
        skillThree = spiritboom;
        ParticleSystem.CollisionModule collisionModule = colliderWith.collision;
        uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
        InitializeHealth(health);
        isLayer = LayerMask.LayerToName(gameObject.layer);
        if (isLayer == isPlayer)
        {
            collisionLayer = LayerMask.GetMask(isEnemy);
            uiManager.DisplayHealth(health, true);
            targetEnemy = GameObject.Find(isEnemy).transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModule.collidesWith = LayerMask.GetMask(isEnemy);
        }
        else if (isLayer == isEnemy)
        {
            collisionLayer = LayerMask.GetMask(isPlayer);
            uiManager.DisplayHealth(health, false);
            targetEnemy = GameObject.Find(isPlayer).transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModule.collidesWith = LayerMask.GetMask(isPlayer);
            skillThree.layer = gameObject.layer;
            SetLayerRecursively(skillThree, skillThree.layer);
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
        if (stateInfo.IsName("kame"))
        {
            transform.position = new Vector3(transform.position.x, 2.2f, transform.position.z);
            kiFull.transform.position = new Vector3(kiFull.transform.position.x, -0.0001f, kiFull.transform.position.z);
        }
        else
        {
            kiFull.transform.position = transform.position;
        }
        if (skillThree.activeInHierarchy)
        {
            ParticleLifetimesAndMoveSpiritBoom(skillThree.transform);
        }
        else if (!skillThree.activeInHierarchy)
        {
            isSpiritRuning = false;
        }
    }
}
