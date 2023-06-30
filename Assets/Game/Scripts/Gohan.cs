using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gohan : CharacterController
{
    public ParticleSystem colliderWithKame;
    public ParticleSystem colliderWithSuperKame;
    private Vector3 betweenHandsKame;
    public void Start()
    {
        initialCameraPosition = cameraSkill.transform.localPosition;
        uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
        InitializeHealth(health);
        ParticleSystem.CollisionModule collisionModuleKame = colliderWithKame.collision;
        ParticleSystem.CollisionModule collisionModuleSuperKame = colliderWithSuperKame.collision;
        isLayer = LayerMask.LayerToName(gameObject.layer);
        if (isLayer == isPlayer)
        {
            collisionLayer = LayerMask.GetMask(isEnemy);
            uiManager.DisplayHealth(health, true);
            targetEnemy = GameObject.Find(isEnemy).transform;
            skillTwo.layer = gameObject.layer;
            skillThree.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            SetLayerRecursively(skillThree, skillThree.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isEnemy);
            collisionModuleSuperKame.collidesWith = LayerMask.GetMask(isEnemy);
        }
        else if (isLayer == isEnemy)
        {
            collisionLayer = LayerMask.GetMask(isPlayer);
            uiManager.DisplayHealth(health, false);
            targetEnemy = GameObject.Find(isPlayer).transform;
            skillTwo.layer = gameObject.layer;
            skillThree.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            SetLayerRecursively(skillThree, skillThree.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isPlayer);
            collisionModuleSuperKame.collidesWith = LayerMask.GetMask(isPlayer);
        }
    }

    // Update is called once per frame
    void Update()
    {
        stateInfo = playerAnim.GetCurrentAnimatorStateInfo(0);
        Rotation();
        BO3();
        ResetComboState();
        MoveFireball();
        AttackPoint();
        Ki();
        
        if (skillThree.activeInHierarchy)
        {
            betweenHandsKame = (RightArmAttackPoint.transform.position + LeftArmAttackPoint.transform.position) / 2;
            CheckChildParticleLifetimesKame(skillThree.transform);
            skillThree.transform.position = betweenHandsKame;
            CameraSmooth(cameraSkill, Vector3.left);
            cameraSkill.enabled = true;
            CheckCameraCoreGohan(skillThree.transform);
        }
        if (!skillThree.activeInHierarchy)
        {
            if (Input.GetKeyDown(KeyCode.Alpha3) && isLayer == isPlayer && uiManager.isCountdownFinished)
            {
                if (80f > energy) return;
                ApplyReduceEnergy(80f);
                skillThree.SetActive(true);
                playerAnim.SetTrigger("superkame");
            }
            else if (Input.GetKeyDown(KeyCode.L) && isLayer == isEnemy && uiManager.isCountdownFinished)
            {
                if (80f > energy) return;
                ApplyReduceEnergy(80f);
                skillThree.SetActive(true);
                playerAnim.SetTrigger("superkame");
            }
        }
    }
}
