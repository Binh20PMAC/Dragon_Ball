using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterController : MonoBehaviour
{
    // Move
    public GameObject ki;
    public GameObject kiFull;
    public List<ParticleSystem> kiGround;
    public Animator playerAnim;
    public Rigidbody playerRb;
    public Transform targetEnemy;
    private bool activateTimerToReset;
    private float default_combo_timer = 2f;
    private float current_combo_timer;
    private ComboState current_combo_state;
    public AnimatorStateInfo stateInfo;

    public float w_speed, wb_speed, rn_speed;
    public bool isWalking;
    public bool isWalkingBack = false;
    private bool isRunning = false;

    public float jumpForce = 5f;
    public bool isOnGround = true;
    private bool isJump = false;
    public bool isFlipped = false;
    public bool isKnockDown = false;

    // Collider
    public LayerMask collisionLayer;
    public float radius = 1f;
    public float damage = 10f;
    public GameObject hit_FX;

    // Attack Point
    public GameObject RightArmAttackPoint;
    public GameObject LeftArmAttackPoint;
    public GameObject RightLegAttackPoint;
    public GameObject LeftLegAttackPoint;
    public GameObject RightForeArmAttackPoint;

    // Apply Dame
    public float health = 100f;
    public float energy = 15f;
    private bool characterDied;
    public UIManager health_UI;
    public UIManager enemyHealthUI;

    //Skill
    public GameObject skillOne;
    public GameObject skillTwo;
    public GameObject skillThree;
    private bool isPooling = true;
    public List<GameObject> listSkillOne = new List<GameObject>();
    public int speedFireball = 15;
    private Vector3 betweenHands;


    protected void Rotation()
    {
        if (targetEnemy != null)
        {
            Vector3 targetDirection = targetEnemy.position - transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // Xoay "CharacterContainer"
            if (angle > 90 || angle < -90)
            {
                transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                isFlipped = true;
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                isFlipped = false;
            }
        }
    }
    void Movement()
    {
        if (LayerMask.LayerToName(gameObject.layer) == "Player" && !isKnockDown)
        {
            if (Input.GetKey(KeyCode.D) && !isJump)
            {
                transform.Translate(w_speed * Time.deltaTime, 0, 0, 0);

                if (isFlipped)
                {
                    playerAnim.SetTrigger("walkback");
                    isWalking = false;
                }
                else
                {
                    playerAnim.SetTrigger("walk");
                    isWalking = true;
                }
                playerAnim.ResetTrigger("idle");
            }
            else if (Input.GetKey(KeyCode.A) && !isJump)
            {
                transform.Translate(-w_speed * Time.deltaTime, 0, 0, 0);

                if (isFlipped)
                {
                    playerAnim.SetTrigger("walk");
                }
                else
                {
                    playerAnim.SetTrigger("walkback");
                }
                playerAnim.ResetTrigger("idle");
            }
            else if (!Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A) && !isJump)
            {
                if (isFlipped)
                {
                    playerAnim.ResetTrigger("walkback");
                    isWalking = true;
                }
                else
                {
                    playerAnim.ResetTrigger("walk");
                    isWalking = false;
                }
                playerAnim.SetTrigger("idle");
            }

            if (Input.GetKey(KeyCode.LeftShift))
            {
                if (Input.GetKey(KeyCode.D) && !isJump)
                {
                    isRunning = true;
                    w_speed = w_speed + rn_speed;
                    playerAnim.SetTrigger("run");
                    playerAnim.ResetTrigger("walk");
                }
                else
                {
                    isRunning = false;
                }
            }
            else
            {
                isRunning = false;
            }

            if (!isRunning)
            {
                w_speed = 3f;
            }

            if (isJump)
            {
                isWalking = false;
            }

            if (Input.GetKeyDown(KeyCode.W) && isOnGround)
            {
                playerAnim.SetTrigger("jumping");
                playerAnim.SetTrigger("falling");
                playerAnim.ResetTrigger("idle");
                isOnGround = false;
                isJump = true;
                //playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
                playerRb.AddForce(transform.up * jumpForce);
            }
        }
        if (LayerMask.LayerToName(gameObject.layer) == "Enemy" && !isKnockDown)
        {
            if (Input.GetKey(KeyCode.RightArrow) && !isJump)
            {
                transform.Translate(w_speed * Time.deltaTime, 0, 0, 0);

                if (isFlipped)
                {
                    playerAnim.SetTrigger("walkback");
                    isWalking = false;
                }
                else
                {
                    playerAnim.SetTrigger("walk");
                    isWalking = true;
                }
                playerAnim.ResetTrigger("idle");
            }
            else if (Input.GetKey(KeyCode.LeftArrow) && !isJump)
            {
                transform.Translate(-w_speed * Time.deltaTime, 0, 0, 0);

                if (isFlipped)
                {
                    playerAnim.SetTrigger("walk");
                }
                else
                {
                    playerAnim.SetTrigger("walkback");
                }
                playerAnim.ResetTrigger("idle");
            }
            else if (!Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.LeftArrow) && !isJump)
            {
                if (isFlipped)
                {
                    playerAnim.ResetTrigger("walkback");
                    isWalking = true;
                }
                else
                {
                    playerAnim.ResetTrigger("walk");
                    isWalking = false;
                }
                playerAnim.SetTrigger("idle");
            }

            if (Input.GetKey(KeyCode.RightShift))
            {
                if (Input.GetKey(KeyCode.RightArrow) && !isJump)
                {
                    isRunning = true;
                    w_speed = w_speed + rn_speed;
                    playerAnim.SetTrigger("run");
                    playerAnim.ResetTrigger("walk");
                }
                else
                {
                    isRunning = false;
                }
            }
            else
            {
                isRunning = false;
            }

            if (!isRunning)
            {
                w_speed = 3f;
            }

            if (isJump)
            {
                isWalking = false;
            }

            if (Input.GetKeyDown(KeyCode.UpArrow) && isOnGround)
            {
                playerAnim.SetTrigger("jumping");
                playerAnim.SetTrigger("falling");
                playerAnim.ResetTrigger("idle");
                isOnGround = false;
                isJump = true;
                //playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
                playerRb.AddForce(transform.up * jumpForce);
            }
        }
    }

    protected void Ki()
    {
        if (LayerMask.LayerToName(gameObject.layer) == "Player" && !isKnockDown)
        {
            if (stateInfo.IsName("chargekilast") || stateInfo.IsName("chargekimidle") || stateInfo.IsName("chargeki"))
            {
                ki.SetActive(true);
            }
            if (!stateInfo.IsName("chargekilast") && !stateInfo.IsName("chargekimidle") && !stateInfo.IsName("chargeki"))
            {
                ki.SetActive(false);
                playerAnim.ResetTrigger("chargekilast");
            }
            if (!ki.activeInHierarchy && !skillTwo.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.R))
                {
                    playerAnim.SetTrigger("chargeki");
                    playerAnim.SetTrigger("chargekimidle");
                }
                Movement();
                ComboAttacks();
                Skill();
            }
            if (Input.GetKeyUp(KeyCode.R))
            {
                playerAnim.SetTrigger("chargekilast");
            }
            if (ki.activeInHierarchy)
            {
                ApplyIncreaseEnergy(10f);
            }
            if (skillTwo.activeInHierarchy)
            {
                betweenHands = (RightArmAttackPoint.transform.position + LeftArmAttackPoint.transform.position) / 2;
                CheckChildParticleLifetimesKame(skillTwo.transform);
                skillTwo.transform.position = betweenHands;
            }

            if (kiFull.activeInHierarchy || ki.activeInHierarchy)
            {
                if (GetComponent<Rigidbody>().velocity.y > 0)
                {
                    foreach (ParticleSystem ki in kiGround)
                    {
                        ki.gameObject.SetActive(false);
                    }
                }
                else if (!isJump)
                {
                    foreach (ParticleSystem ki in kiGround)
                    {
                        ki.gameObject.SetActive(true);
                    }
                }
                // Ki opacity up and down
                ParticleSystem[] kiStartColor = kiFull.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem kiColor in kiStartColor)
                {
                    Color StartColor = kiColor.startColor;
                    float h, s, v;
                    Color.RGBToHSV(StartColor, out h, out s, out v);
                    if (v > (energy / 100f))
                    {
                        v -= 0.1f * Time.deltaTime;
                        if (v < 0.15)
                        {
                            kiFull.SetActive(false);
                        }
                    }
                    else if (v < (energy / 100f) && v < 0.7f)
                    {
                        v += 0.1f * Time.deltaTime;
                    }
                    Color newColor = Color.HSVToRGB(h, s, v);
                    kiColor.startColor = newColor;
                }
            }
        }
        if (LayerMask.LayerToName(gameObject.layer) == "Enemy" && !isKnockDown)
        {
            if (Input.GetKeyDown(KeyCode.P))
            {
                playerAnim.SetTrigger("chargeki");
                playerAnim.SetTrigger("chargekimidle");
            }
            if (Input.GetKeyUp(KeyCode.P))
            {
                playerAnim.SetTrigger("chargekilast");
            }
            if (stateInfo.IsName("chargekilast") || stateInfo.IsName("chargekimidle") || stateInfo.IsName("chargeki"))
            {
                ki.SetActive(true);
            }
            if (!stateInfo.IsName("chargekilast") && !stateInfo.IsName("chargekimidle") && !stateInfo.IsName("chargeki"))
            {
                ki.SetActive(false);
                playerAnim.ResetTrigger("chargekilast");
            }
            if (!ki.activeInHierarchy && !skillTwo.activeInHierarchy)
            {
                if (Input.GetKeyDown(KeyCode.P))
                {
                    playerAnim.SetTrigger("chargeki");
                    playerAnim.SetTrigger("chargekimidle");
                }
                Movement();
                ComboAttacks();
                Skill();
            }
            if (Input.GetKeyUp(KeyCode.P))
            {
                playerAnim.SetTrigger("chargekilast");
            }
            if (ki.activeInHierarchy)
            {
                ApplyIncreaseEnergy(10f);
            }
            if (skillTwo.activeInHierarchy)
            {
                betweenHands = (RightArmAttackPoint.transform.position + LeftArmAttackPoint.transform.position) / 2;
                CheckChildParticleLifetimesKame(skillTwo.transform);
                skillTwo.transform.position = betweenHands;
            }
            if (kiFull.activeInHierarchy || ki.activeInHierarchy)
            {
                if (GetComponent<Rigidbody>().velocity.y > 0)
                {
                    foreach (ParticleSystem ki in kiGround)
                    {
                        ki.gameObject.SetActive(false);
                    }
                }
                else if (!isJump)
                {
                    foreach (ParticleSystem ki in kiGround)
                    {
                        ki.gameObject.SetActive(true);
                    }
                }
                // Ki opacity up and down
                ParticleSystem[] kiStartColor = kiFull.GetComponentsInChildren<ParticleSystem>();
                foreach (ParticleSystem kiColor in kiStartColor)
                {
                    Color StartColor = kiColor.startColor;
                    float h, s, v;
                    Color.RGBToHSV(StartColor, out h, out s, out v);
                    if (v > (energy / 100f))
                    {
                        v -= 0.1f * Time.deltaTime;
                        if (v < 0.15)
                        {
                            kiFull.SetActive(false);
                        }
                    }
                    else if (v < (energy / 100f) && v < 0.7f)
                    {
                        v += 0.1f * Time.deltaTime;
                    }
                    Color newColor = Color.HSVToRGB(h, s, v);
                    kiColor.startColor = newColor;
                }
            }
        }
    }

    protected void Skill()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1) && LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            if (isPooling)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject fireball = Instantiate(skillOne, transform.position, Quaternion.identity);
                    fireball.layer = LayerMask.NameToLayer(LayerMask.LayerToName(gameObject.layer));
                    SetLayerRecursively(fireball, fireball.layer);
                    fireball.SetActive(false);
                    listSkillOne.Add(fireball);
                }
                isPooling = false;
            }
            ActivateFireball(10f);
        }
        else if (Input.GetKeyDown(KeyCode.J) && LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            if (isPooling)
            {
                for (int i = 0; i < 5; i++)
                {
                    GameObject fireball = Instantiate(skillOne, transform.position, Quaternion.identity);
                    fireball.layer = LayerMask.NameToLayer(LayerMask.LayerToName(gameObject.layer));
                    SetLayerRecursively(fireball, fireball.layer);
                    fireball.SetActive(false);
                    listSkillOne.Add(fireball);
                }
                isPooling = false;
            }
            ActivateFireball(10f);
        }

        if (Input.GetKeyDown(KeyCode.Alpha2) && LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            if (50f > energy) return;
            ApplyReduceEnergy(50f);
            skillTwo.SetActive(true);
            playerAnim.SetTrigger("kame");
        }
        else if (Input.GetKeyDown(KeyCode.K) && LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            if (50f > energy) return;
            ApplyReduceEnergy(50f);
            skillTwo.SetActive(true);
            playerAnim.SetTrigger("kame");
        }
        if (Input.GetKeyDown(KeyCode.L) && LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            if (80f > energy) return;
            ApplyReduceEnergy(80f);
            skillThree.transform.position = new Vector3(skillThree.transform.position.x, transform.position.y + 8f, skillThree.transform.position.z);
            skillThree.SetActive(true);
            playerAnim.SetTrigger("spiritboomfirst");
            playerAnim.SetTrigger("spiritboommiddle");
        }
    }

    void CheckChildParticleLifetimesKame(Transform parent)
    {
        foreach (Transform child in parent)
        {
            ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
            if (particleSystem != null)
            {
                ParticleSystem.MainModule mainModule = particleSystem.main;
                float remainingLifetime = mainModule.duration - particleSystem.time;
                if (remainingLifetime == 0 && child.name == "KameCore")
                {
                    skillTwo.SetActive(false);
                }
                //Debug.Log("Remaining Lifetime of Particle System: " + remainingLifetime + " seconds");
            }

            CheckChildParticleLifetimesKame(child);
        }
    }
    public void ParticleLifetimesAndMoveSpiritBoom(Transform parent)
    {
        foreach (Transform child in parent)
        {
            if (child.name == "SpiritBoom")
            {
                ParticleSystem particleSystem = child.GetComponent<ParticleSystem>();
                ParticleSystem.MainModule mainModule = particleSystem.main;
                float remainingLifetime = mainModule.duration - particleSystem.time;
                if (remainingLifetime == 0)
                {
                    float directionMultiplier = 1f;
                    if (transform.rotation.eulerAngles.y == 90f)
                    {
                        directionMultiplier = 1f; 
                    }
                    else if (transform.rotation.eulerAngles.y == 270f)
                    {
                        directionMultiplier = -1f; 
                    }
                    skillThree.transform.position += Vector3.right * directionMultiplier * 4f * Time.deltaTime;
                    skillThree.transform.position += (Vector3.down / 2f) * Time.deltaTime;
                    child.gameObject.SetActive(false);
                    playerAnim.SetTrigger("spiritboomlast");
                    CheckOffscreen(skillThree);
                }
            }
        }
    }
    public void SetLayerRecursively(GameObject obj, int layer)
    {
        obj.layer = layer;
        foreach (Transform child in obj.transform)
        {
            SetLayerRecursively(child.gameObject, layer);
        }
    }
    private void ActivateFireball(float fireballEnergy)
    {
        if (fireballEnergy > energy) return;
        ApplyReduceEnergy(fireballEnergy);
        foreach (GameObject fireball in listSkillOne)
        {
            if (!fireball.activeSelf)
            {
                fireball.transform.position = RightArmAttackPoint.transform.position;
                if (transform.rotation.eulerAngles.y > 90f)
                {
                    fireball.transform.rotation = new Quaternion(0, 180, 0, 0);
                }
                else
                {
                    fireball.transform.rotation = new Quaternion(0, 0, 0, 0);
                }
                fireball.SetActive(true);
                break;
            }
        }
    }
    public void MoveFireball()
    {
        if (!isPooling)
        {
            foreach (GameObject fireball in listSkillOne)
            {
                if (fireball.activeInHierarchy)
                {
                    fireball.transform.Translate(Vector3.right * speedFireball * Time.deltaTime);
                    fireball.transform.position = new Vector3(fireball.transform.position.x, fireball.transform.position.y, -10f);
                    CheckOffscreen(fireball);
                }
            }
        }
    }
    private void CheckOffscreen(GameObject skill)
    {
        Vector3 screenPos = Camera.main.WorldToScreenPoint(skill.transform.position);
        float screenWidth = Screen.width;
        float expandWidth = screenWidth * 0.2f;
        if (screenPos.x < -expandWidth || screenPos.x > screenWidth + expandWidth)
        {
            skill.SetActive(false);
        }
    }

    //protected GameObject GetHitEffect()
    //{
    //    return Resources.Load("HitEffect", typeof(GameObject)) as GameObject;
    //}
    void ComboAttacks()
    {
        if (LayerMask.LayerToName(gameObject.layer) == "Player" && !isKnockDown)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                if (current_combo_state == ComboState.attack3 ||
                    current_combo_state == ComboState.attack4 ||
                    current_combo_state == ComboState.attack5)
                    return;
                current_combo_state++;
                activateTimerToReset = true;
                current_combo_timer = default_combo_timer;

                if (current_combo_state == ComboState.attack1)
                {
                    playerAnim.SetTrigger("attack1");
                }
                if (current_combo_state == ComboState.attack2)
                {
                    playerAnim.SetTrigger("attack2");
                }
                if (current_combo_state == ComboState.attack3)
                {
                    playerAnim.SetTrigger("attack3");
                }
            }
            if (Input.GetKeyDown(KeyCode.Q))
            {
                if (current_combo_state == ComboState.attack5 ||
                    current_combo_state == ComboState.attack3)
                    return;
                if (current_combo_state == ComboState.none ||
                    current_combo_state == ComboState.attack1 ||
                    current_combo_state == ComboState.attack2)
                {
                    current_combo_state = ComboState.attack4;
                }
                else if (current_combo_state == ComboState.attack4)
                {
                    current_combo_state++;
                }
                activateTimerToReset = true;
                current_combo_timer = default_combo_timer;
                if (current_combo_state == ComboState.attack4)
                {
                    playerAnim.SetTrigger("attack4");
                }
                if (current_combo_state == ComboState.attack5)
                {
                    playerAnim.SetTrigger("attack5");
                }
            }
        }
        if (LayerMask.LayerToName(gameObject.layer) == "Enemy" && !isKnockDown)
        {
            if (Input.GetKeyDown(KeyCode.O))
            {
                if (current_combo_state == ComboState.attack3 ||
                    current_combo_state == ComboState.attack4 ||
                    current_combo_state == ComboState.attack5)
                    return;
                current_combo_state++;
                activateTimerToReset = true;
                current_combo_timer = default_combo_timer;

                if (current_combo_state == ComboState.attack1)
                {
                    playerAnim.SetTrigger("attack1");
                }
                if (current_combo_state == ComboState.attack2)
                {
                    playerAnim.SetTrigger("attack2");
                }
                if (current_combo_state == ComboState.attack3)
                {
                    playerAnim.SetTrigger("attack3");
                }
            }
            if (Input.GetKeyDown(KeyCode.I))
            {
                if (current_combo_state == ComboState.attack5 ||
                    current_combo_state == ComboState.attack3)
                    return;
                if (current_combo_state == ComboState.none ||
                    current_combo_state == ComboState.attack1 ||
                    current_combo_state == ComboState.attack2)
                {
                    current_combo_state = ComboState.attack4;
                }
                else if (current_combo_state == ComboState.attack4)
                {
                    current_combo_state++;
                }
                activateTimerToReset = true;
                current_combo_timer = default_combo_timer;
                if (current_combo_state == ComboState.attack4)
                {
                    playerAnim.SetTrigger("attack4");
                }
                if (current_combo_state == ComboState.attack5)
                {
                    playerAnim.SetTrigger("attack5");
                }
            }
        }
    }
    protected void ResetComboState()
    {
        if (activateTimerToReset)
        {
            current_combo_timer -= Time.deltaTime;
            if (current_combo_timer <= 0f)
            {
                current_combo_state = ComboState.none;
                activateTimerToReset = false;
                current_combo_timer = default_combo_timer;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground") && isJump)
        {
            playerAnim.SetTrigger("grouding");
            playerAnim.SetTrigger("idle");
            playerAnim.ResetTrigger("jumping");
            playerAnim.ResetTrigger("falling");
            isOnGround = true;
            isJump = false;
        }
    }

    private void OnParticleCollision(GameObject collision)
    {
        if (collision.gameObject.layer != gameObject.layer && collision.gameObject.CompareTag("FireBall"))
        {
            DeactivateTopmostParent(collision);
            ApplyDamage(10f, false);
        }
        if (collision.gameObject.CompareTag("Kame"))
        {
            ApplyDamage(6f, false);
        }

    }

    void DeactivateTopmostParent(GameObject childObject)
    {
        Transform parent = childObject.transform.parent;

        while (parent != null)
        {
            childObject = parent.gameObject;
            parent = childObject.transform.parent;
        }

        childObject.SetActive(false);
    }
    void DetectCollisionRightArm()
    {
        Collider[] hit = Physics.OverlapSphere(RightArmAttackPoint.transform.position, radius, collisionLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            //print("Hit the " + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                Instantiate(hit_FX, RightArmAttackPoint.transform.position, Quaternion.identity);

                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, RightArmAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            RightArmAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionLeftArm()
    {
        Collider[] hit = Physics.OverlapSphere(LeftArmAttackPoint.transform.position, radius, collisionLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            //print("Hit the " + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, LeftArmAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, LeftArmAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            LeftArmAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionRightLeg()
    {
        Collider[] hit = Physics.OverlapSphere(RightLegAttackPoint.transform.position, radius, collisionLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            //print("Hit the " + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, RightLegAttackPoint.transform.position, Quaternion.identity);
                if (RightLegAttackPoint.gameObject.tag == "RightArm" || RightLegAttackPoint.gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, RightLegAttackPoint.transform.position, Quaternion.identity);
                if (RightLegAttackPoint.gameObject.tag == "RightArm" || RightLegAttackPoint.gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            RightLegAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionLeftLeg()
    {
        Collider[] hit = Physics.OverlapSphere(LeftLegAttackPoint.transform.position, radius, collisionLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            //print("Hit the " + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, LeftLegAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, LeftLegAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            LeftLegAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionRightForeArm()
    {
        Collider[] hit = Physics.OverlapSphere(RightForeArmAttackPoint.transform.position, radius, collisionLayer);
        for (int i = 0; i < hit.Length; i++)
        {
            //print("Hit the " + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, RightForeArmAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                //Vector3 hitFX_Pos = hit[0].transform.position;
                //hitFX_Pos.y += 1.3f;
                //if (hit[0].transform.forward.x > 0)
                //{
                //    hitFX_Pos.x += 0.3f;
                //}
                //else if (hit[0].transform.forward.x < 0)
                //{
                //    hitFX_Pos.x -= 0.3f;
                //}
                Instantiate(hit_FX, RightForeArmAttackPoint.transform.position, Quaternion.identity);
                if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, true);
                }
                else
                {
                    hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                }
            }
            RightForeArmAttackPoint.gameObject.SetActive(false);
        }
    }
    void RightArmAttackOn()
    {
        RightArmAttackPoint.SetActive(true);
    }
    void RightArmAttackOff()
    {
        if (RightArmAttackPoint.activeInHierarchy)
        {
            RightArmAttackPoint.SetActive(false);
        }
    }
    void LeftTArmAttackOn()
    {
        LeftArmAttackPoint.SetActive(true);
    }
    void LeftArmAttackOff()
    {
        if (LeftArmAttackPoint.activeInHierarchy)
        {
            LeftArmAttackPoint.SetActive(false);
        }
    }
    void LeftTLegAttackOn()
    {
        LeftLegAttackPoint.SetActive(true);
    }
    void LeftLegAttackOff()
    {
        if (LeftLegAttackPoint.activeInHierarchy)
        {
            LeftLegAttackPoint.SetActive(false);
        }
    }
    void RightLegAttackOn()
    {
        RightLegAttackPoint.SetActive(true);
    }
    void RightLegAttackOff()
    {
        if (RightLegAttackPoint.activeInHierarchy)
        {
            RightLegAttackPoint.SetActive(false);
        }
    }
    void RightForeArmAttackOn()
    {
        RightForeArmAttackPoint.SetActive(true);
    }
    void RightForeArmAttackOff()
    {
        if (RightForeArmAttackPoint.activeInHierarchy)
        {
            RightForeArmAttackPoint.SetActive(false);
        }
    }
    void TagRightArm()
    {
        RightArmAttackPoint.tag = "RightArm";
    }
    void UnTagRightArm()
    {
        RightArmAttackPoint.tag = "Untagged";
    }
    void TagRightLeg()
    {
        RightLegAttackPoint.tag = "RightLeg";
    }
    void UnTagRightLeg()
    {
        RightLegAttackPoint.tag = "Untagged";
    }
    void StandUp()
    {
        isKnockDown = false;
        StartCoroutine(StandUpAfterTime());

    }
    IEnumerator StandUpAfterTime()
    {
        playerAnim.SetTrigger("standup");
        yield return new WaitForSeconds(2f);
    }

    public void ApplyDamage(float damage, bool knockDown)
    {
        if (characterDied)
            return;

        health -= damage;

        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI.DisplayHealth(health, true);
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI.DisplayHealth(health, false);
        }

        if (health <= 0f)
        {
            playerAnim.SetTrigger("died");
            characterDied = true;
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {

            }
            return;
        }
        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            if (knockDown)
            {
                if (Random.Range(0, 2) > 0)
                {
                    playerAnim.SetTrigger("knockdown");
                    isKnockDown = true;
                }
            }
            else
            {
                if (Random.Range(0, 3) > 1)
                {
                    playerAnim.SetTrigger("hit");
                }
            }
        }
        if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            if (knockDown)
            {
                if (Random.Range(0, 2) > 0)
                {
                    playerAnim.SetTrigger("knockdown");
                    isKnockDown = true;
                }
            }
            else
            {
                if (Random.Range(0, 3) > 1)
                {
                    playerAnim.SetTrigger("hit");
                }
            }
        }
    }
    public void ApplyIncreaseEnergy(float speed)
    {
        energy += speed * Time.deltaTime;

        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI.DisplayEnergy(energy, true);
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI.DisplayEnergy(energy, false);
        }

        if (energy > 100f)
        {
            energy = 100f;
            kiFull.SetActive(true);
        }
    }

    public void ApplyReduceEnergy(float charge)
    {
        energy -= charge;
        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI.DisplayEnergy(energy, true);
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI.DisplayEnergy(energy, false);
        }
    }

    //IEnumerator WaitForSecondTouchGround()
    //{
    //    yield return new WaitForSeconds(0f);

    //}
    //IEnumerator WaitForSecondReadyJump()
    //{
    //    //playerAnim.SetTrigger("jumping");
    //    //playerAnim.SetTrigger("falling");
    //    //playerAnim.ResetTrigger("idle");
    //    //isOnGround = false;
    //    //isJump = true;
    //    yield return new WaitForSeconds(0.5f);
    //    ////playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
    //    //playerRb.AddForce(transform.up * jumpForce);
    //}
    protected void AttackPoint()
    {
        if (RightArmAttackPoint.activeInHierarchy)
        {
            DetectCollisionRightArm();
        }
        if (LeftArmAttackPoint.activeInHierarchy)
        {
            DetectCollisionLeftArm();
        }
        if (RightForeArmAttackPoint.activeInHierarchy)
        {
            DetectCollisionRightForeArm();
        }
        if (LeftLegAttackPoint.activeInHierarchy)
        {
            DetectCollisionLeftLeg();
        }
        if (RightLegAttackPoint.activeInHierarchy)
        {
            DetectCollisionRightLeg();
        }


    }

}
