using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goku : CharacterController
{
    public ParticleSystem colliderWithKame;
    private bool isSuperSaiyan = false;
    private GameObject gokuSuperSaiyanPrefab;

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
        gokuSuperSaiyanPrefab = Resources.Load<GameObject>("gokuSS1");

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
        if (Input.GetKeyDown(KeyCode.Alpha4) && isLayer == isPlayer && uiManager.isCountdownFinished)
        {
            if (!isSuperSaiyan)
            {
                TransformToSuperSaiyan();
            }
        }
        else if (Input.GetKeyDown(KeyCode.Alpha7) && isLayer == isEnemy && uiManager.isCountdownFinished)
        {
            if (!isSuperSaiyan)
            {
                TransformToSuperSaiyan();
            }
        }
        if(targetEnemy == null)
        {
            if (isLayer == isEnemy)
            {
                targetEnemy = GameObject.Find(isPlayer).transform;
            }
            else
            {
                targetEnemy = GameObject.Find(isEnemy).transform;
            }
        }
    }
    private void TransformToSuperSaiyan()
    {
        isSuperSaiyan = true;
        health *= 2;
        damage *= 2;
        GameObject gokuSuperSaiyan = Instantiate(gokuSuperSaiyanPrefab, transform.position, Quaternion.identity);
        Goku gokuSuperSaiyanScript = gokuSuperSaiyan.GetComponent<Goku>();
        gokuSuperSaiyanScript.health = health;
        gokuSuperSaiyanScript.damage = damage;
        gokuSuperSaiyan.layer = gameObject.layer;
        Destroy(gameObject);
    }
}
