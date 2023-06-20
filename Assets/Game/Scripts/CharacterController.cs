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
    public Transform playerTrans;
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

    void Awake()
    {
        
        if (LayerMask.LayerToName(gameObject.layer) == "Player")
        {
            health_UI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Enemy");
        }
        else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            enemyHealthUI = GameObject.Find("UICode").GetComponent<UIManager>();
            collisionLayer = LayerMask.GetMask("Player");
        }
        hit_FX = Resources.Load("HitEffect", typeof(GameObject)) as GameObject;
    }

    // Start is called before the first frame update
    public void Start()
    {
        //playerRb = GetComponent<Rigidbody>();
        //current_combo_timer = default_combo_timer;
        //current_combo_state = ComboState.none;
        //Debug.Log("start");
    }


    // Update is called once per frame
    //void FixedUpdate()
    //{
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        playerRb.velocity = transform.forward * w_speed * Time.deltaTime;

    //    }
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        playerRb.velocity = -transform.forward * wb_speed * Time.deltaTime;
    //    }
    //}
    void Update()
    {
        //xoay nhan vat
        if (targetEnemy != null)
        {
            Vector3 targetDirection = targetEnemy.position - playerTrans.transform.position;
            float angle = Mathf.Atan2(targetDirection.y, targetDirection.x) * Mathf.Rad2Deg;

            // Xoay "CharacterContainer"
            if (angle > 90 || angle < -90)
            {
                playerTrans.transform.rotation = Quaternion.Euler(0f, 270f, 0f);
                isFlipped = true;
            }
            else
            {
                playerTrans.transform.rotation = Quaternion.Euler(0f, 90f, 0f);
                isFlipped = false;
            }
        }
    }
    void Movement()
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
            StartCoroutine(WaitForSecondReadyJump());
        }
    }
    protected void Ki()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            playerAnim.SetTrigger("chargeki");
            playerAnim.SetTrigger("chargekimidle");
        }
        if (Input.GetKeyUp(KeyCode.R))
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
        if (!ki.activeInHierarchy)
        {
            Movement();
            ComboAttacks();
        }
        else if (ki.activeInHierarchy)
        {
            ApplyEnergy(5f);
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

    //protected GameObject GetHitEffect()
    //{
    //    return Resources.Load("HitEffect", typeof(GameObject)) as GameObject;
    //}
    void ComboAttacks()
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
            StartCoroutine(WaitForSecondTouchGround());
        }
    }

    void DetectCollisionRightArm()
    {
        Collider[] hit = Physics.OverlapSphere(RightArmAttackPoint.transform.position, radius, collisionLayer);
        if (hit.Length > 0)
        {
            print("Hit the" + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            RightArmAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionLeftArm()
    {
        Collider[] hit = Physics.OverlapSphere(LeftArmAttackPoint.transform.position, radius, collisionLayer);
        if (hit.Length > 0)
        {
            //print("Hit the" + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            LeftArmAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionRightLeg()
    {
        Collider[] hit = Physics.OverlapSphere(RightLegAttackPoint.transform.position, radius, collisionLayer);
        if (hit.Length > 0)
        {
            //print("Hit the" + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            RightLegAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionLeftLeg()
    {
        Collider[] hit = Physics.OverlapSphere(LeftLegAttackPoint.transform.position, radius, collisionLayer);
        if (hit.Length > 0)
        {
            //print("Hit the" + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            LeftLegAttackPoint.gameObject.SetActive(false);
        }
    }

    void DetectCollisionRightForeArm()
    {
        Collider[] hit = Physics.OverlapSphere(RightForeArmAttackPoint.transform.position, radius, collisionLayer);
        if (hit.Length > 0)
        {
            //print("Hit the" + hit[0].gameObject.name);
            //cho player
            if (LayerMask.LayerToName(gameObject.layer) == "Player")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
            }
            //cho enemy
            else if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
            {
                Vector3 hitFX_Pos = hit[0].transform.position;
                hitFX_Pos.y += 1.3f;
                if (hit[0].transform.forward.x > 0)
                {
                    hitFX_Pos.x += 0.3f;
                }
                else if (hit[0].transform.forward.x < 0)
                {
                    hitFX_Pos.x -= 0.3f;
                }
                Instantiate(hit_FX, hitFX_Pos, Quaternion.identity);
                hit[0].GetComponent<CharacterController>().ApplyDamage(damage, false);
                //if (gameObject.tag == "RightArm" || gameObject.tag == "RightLeg")
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, true);
                //}
                //else
                //{
                //    hit[0].GetComponent<HealthScript>().ApplyDamage(damage, false);
                //}
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
    //!!!!!!////////
    void TagRightArm()
    {
        RightArmAttackPoint.tag = "RightArm";
    }
    void UnTagRightArm()
    {
        RightArmAttackPoint.tag = "Untagged";
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
        if (LayerMask.LayerToName(gameObject.layer) == "Enemy")
        {
            if (knockDown)
            {
                if (Random.Range(0, 2) > 0)
                {
                    playerAnim.SetTrigger("died");
                }
            }
            else
            {
                if (Random.Range(0, 3) >= 1)
                {
                    playerAnim.SetTrigger("hit");
                }
            }
        }
    }
    public void ApplyEnergy(float charge)
    {
        energy += charge * Time.deltaTime;

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
    IEnumerator WaitForSecondTouchGround()
    {
        yield return new WaitForSeconds(0f);
        playerAnim.SetTrigger("idle");
        playerAnim.ResetTrigger("jump");
        isOnGround = true;
        isJump = false;
    }
    IEnumerator WaitForSecondReadyJump()
    {
        playerAnim.SetTrigger("jump");
        playerAnim.ResetTrigger("idle");
        isOnGround = false;
        isJump = true;
        yield return new WaitForSeconds(0.5f);
        //playerRb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse); 
        playerRb.AddForce(transform.up * jumpForce);
    }
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
