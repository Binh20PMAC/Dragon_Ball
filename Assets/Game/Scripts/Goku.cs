using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goku : CharacterController
{
    public ParticleSystem colliderWithKame;
    public void Start()
    {
        initialCameraPosition = cameraSkill.transform.localPosition;
        GameObject spiritboom = Instantiate(skillThree);
        spiritboom.SetActive(false);
        spiritboom.name = "SpiritBoomOfGoku";
        skillThree = spiritboom;
        ParticleSystem.CollisionModule collisionModuleKame = colliderWithKame.collision;
        uiManager = GameObject.Find("UICode").GetComponent<UIManager>();
        InitializeHealth(health);
        isLayer = LayerMask.LayerToName(gameObject.layer);
        if (isLayer == isPlayer)
        {
            collisionLayer = LayerMask.GetMask(isEnemy);
            uiManager.DisplayHealth(health, true);
            uiManager.DisplayRage(rage, true);
            targetEnemy = GameObject.Find(isEnemy).transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isEnemy);
            skillThree.layer = gameObject.layer;
            SetLayerRecursively(skillThree, skillThree.layer);
        }
        else if (isLayer == isEnemy)
        {
            collisionLayer = LayerMask.GetMask(isPlayer);
            uiManager.DisplayHealth(health, false);
            uiManager.DisplayRage(rage, false);
            targetEnemy = GameObject.Find(isPlayer).transform;
            skillTwo.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isPlayer);
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
        IncreaseRage();
        if (skillThree.activeInHierarchy)
        {
            ParticleLifetimesAndMoveSpiritBoom(skillThree.transform);
            CameraSmooth(cameraSkill, Vector3.up);
            cameraSkill.enabled = true;
            CheckCameraCoreGoku(skillThree.transform);
        }
        else if (!skillThree.activeInHierarchy)
        {
            isSpiritRuning = false;
            if (Input.GetKeyDown(KeyCode.Alpha3) && isLayer == isPlayer && uiManager.isCountdownFinished)
            {
                if (80f > energy) return;
                ApplyReduceEnergy(80f);
                skillThree.SetActive(true);
                playerAnim.SetTrigger("spiritboomfirst");
                playerAnim.SetTrigger("spiritboommiddle");
            }
            else if (Input.GetKeyDown(KeyCode.L) && isLayer == isEnemy && uiManager.isCountdownFinished)
            {
                if (80f > energy) return;
                ApplyReduceEnergy(80f);
                skillThree.SetActive(true);
                playerAnim.SetTrigger("spiritboomfirst");
                playerAnim.SetTrigger("spiritboommiddle");
            }
        }
    }
}
