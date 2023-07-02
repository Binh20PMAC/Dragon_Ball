using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Gohan : CharacterController
{
    public ParticleSystem colliderWithKame;
    public ParticleSystem colliderWithSuperKame;
    private Vector3 betweenHandsKame;
    private bool isSuperSaiyan = false;
    private GameObject gohanSuperSaiyanPrefab;

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
            uiManager.DisplayRage(rage, true);
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
            uiManager.DisplayRage(rage, false);
            targetEnemy = GameObject.Find(isPlayer).transform;
            skillTwo.layer = gameObject.layer;
            skillThree.layer = gameObject.layer;
            SetLayerRecursively(skillTwo, skillTwo.layer);
            SetLayerRecursively(skillThree, skillThree.layer);
            collisionModuleKame.collidesWith = LayerMask.GetMask(isPlayer);
            collisionModuleSuperKame.collidesWith = LayerMask.GetMask(isPlayer);
        }
        gohanSuperSaiyanPrefab = Resources.Load<GameObject>("GohanSS2");
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
        IncreaseRage();
        if (skillThree.activeInHierarchy)
        {
            betweenHandsKame = (RightArmAttackPoint.transform.position + LeftArmAttackPoint.transform.position) / 2;
            CheckChildParticleLifetimesKame(skillThree.transform);
            skillThree.transform.position = betweenHandsKame;
            if (isLayer == isPlayer)
                CameraSmooth(cameraSkill, Vector3.left);
            else CameraSmooth(cameraSkill, Vector3.right);
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
        if (Input.GetKeyDown(KeyCode.Alpha4) && isLayer == isPlayer && uiManager.isCountdownFinished)
        {
            if (!isSuperSaiyan)
            {
                TransformToSuperSaiyan();
            }
        }
        else if(Input.GetKeyDown(KeyCode.Alpha7) && isLayer == isEnemy && uiManager.isCountdownFinished)
        {
            if (!isSuperSaiyan)
            {
                TransformToSuperSaiyan();
            }
        }
    }
    private void TransformToSuperSaiyan()
    {
        isSuperSaiyan = true;
        health *= 2;
        damage *= 2;
        GameObject gohanSuperSaiyan = Instantiate(gohanSuperSaiyanPrefab, transform.position, Quaternion.identity);
        Gohan gohanSuperSaiyanScript = gohanSuperSaiyan.GetComponent<Gohan>();
        gohanSuperSaiyanScript.health = health;
        gohanSuperSaiyanScript.damage = damage;
        gohanSuperSaiyan.layer = gameObject.layer;
        Destroy(gameObject);
    }
}
