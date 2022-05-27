using UnityEngine;
using DG.Tweening;
using System.Collections;
using Cinemachine;

public class PlayerCombat : MonoBehaviour
{
    //public
    public InputManagergodofwar im;
    public PlayerMovement pm;
    public Animator anim;
    public GameObject axe, unEquipted, cam,trailEffect;
    public Rigidbody axeRb;
    public Transform curvePos, handPos, axeHandPos;
    public float throwPower;
    [HideInInspector]
    public Vector3 des;
    //private
    private Vector3 pullPos;
    private bool equip, visable, moreWeight, pulling, hasWeapon, attackCoolDown, dubbelclick, clickResetOn, isTrowing;
    private float returnTime, speed, runningSpeed;
    private int clickCount;

    private AxeController axeController;
    private void Start()//gets here everythink and sets everythink on false or true 
    {
        axeController = axe.GetComponent<AxeController>();
        hasWeapon = true;
        equip = true;
        visable = true;
        speed = pm.speed;
        runningSpeed = pm.runningSpeed;
        trailEffect.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        InputUpdate();
        AxeUpdate();
    }
    public void InputUpdate()///lots of input
    {
        if (im.leftClick)
        {
            clickCount++;
            Invoke("ResetClickCount", 1);
            if (!clickResetOn)
            {
                Invoke("ResetClickCount", 0.4f);
                clickResetOn = true;
            }
        }
        if (clickCount >= 2)
        {
            dubbelclick = true;
        }
        else
        {
            dubbelclick = false;
        }
        if (dubbelclick && !attackCoolDown && hasWeapon&&equip)
        {
            AttackSpin();
        }
        else if (im.leftClick&& !attackCoolDown&&!dubbelclick&&hasWeapon&&!im.rightClick && equip)
        {
            NormalAttack();
        }
        if (im.spacebar && !attackCoolDown)
        {
            Kick();
        }
        if (im.taunt)
        {
            Taunt();
        }
        if (im.equip&& hasWeapon&&!attackCoolDown)
        {
            anim.SetLayerWeight(1, 1);
            if (!equip)
            {
                Equip();
            }
            else if (equip)
            {
                Dequip();
            }
        }
        if (!hasWeapon && im.leftClick && !pulling)
        {
            if (equip)
            {
                anim.SetBool("IsRetrieving", true);
                Pull();
                isTrowing = false;
            }
        }
        if (im.rightClick)
        {
            if (equip)
            {
                ChargingAxe();
                if (!moreWeight)
                {
                    anim.SetLayerWeight(1, 0.75f);
                }
            }
        }
        else
        {
            anim.SetBool("IsCharging", false);
        }
    }


    //axe functions
    public void AxeUpdate()//is for pulling
    {
        if (pulling)
        {
            if (returnTime < 1)
            {
                axe.transform.position = GetQuadraticCurvePoint(returnTime, pullPos, curvePos.position, handPos.position);
                returnTime += Time.deltaTime * 1.5f;
            }
            else
            {
                TurnOffTrowingBool();
                hasWeapon = true;
                pulling = false;
                StartCoroutine("Gotcha");
            }
        }
    }
    IEnumerator Gotcha()//caught function
    {
        anim.SetBool("isCatching", true);
        returnTime = 0;
        axeController.activated = false;
        trailEffect.SetActive(false);
        axe.transform.parent = handPos;
        axe.transform.position = axeHandPos.position;
        axe.transform.rotation = axeHandPos.rotation;
        pulling = false;

        yield return new WaitForSeconds(0.1f);
        anim.SetBool("isCatching", false);
    }
    public void TurnOffTrowingBool()//reset axe animation bools
    {
        anim.SetBool("IsTrowing", false);
        anim.SetBool("IsCharging", false);
        anim.SetBool("IsRetrieving", false);
        anim.SetBool("isCatching", false);
    }
    public void TrowAxe()//axe trow function = called with a animation event
    {
        moreWeight = false;
        if (visable && equip)
        {
            hasWeapon = false;
            axeController.activated = true;
            trailEffect.SetActive(true);
            axeRb.isKinematic = false;
            axeRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            axe.transform.parent = null;
            axe.transform.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
            axe.transform.position += transform.right / 5;
            axeRb.AddForce(cam.transform.forward * throwPower + transform.up * 4, ForceMode.Impulse);
        }
    }
    public void Pull()//pulling function more in AxeUpdate
    {
        pullPos = axe.transform.position;
        axeRb.Sleep();
        axeRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        axeRb.isKinematic = true;
        axeController.activated = true;
        trailEffect.SetActive(true);
        pulling = true;      
    }
    public void ChargingAxe()//charge animation
    {
        isTrowing = true;
        anim.SetBool("IsCharging", true);
        if (im.rightClick && im.leftClick&&!attackCoolDown)
        {
            moreWeight = true;
            anim.SetLayerWeight(1, 1);
            RelasingAxe();
        }
    }
//attack functions
    public void Resset()//reset for speed afther attack
    {
        pm.speed = speed;
        pm.runningSpeed = runningSpeed;
    }
    public void AttackCooldown()//cooldown for attack
    {
        attackCoolDown = false;
    }
    public void ResetClickCount()//a reset for dubbelclick
    {
        clickCount = 0;
    }
    //curve
    public Vector3 GetQuadraticCurvePoint(float f, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float i = 1 - f;
        float ff = f * f;
        float ii = i * i;
        return (ii * p0) + (2 * i * f * p1) + (ff * p2);
    }
    #region animation functions
    public void RelasingAxe()
    {
        anim.SetBool("IsTrowing", true);
    }
    public void RetrievingAxe()
    {
        anim.SetBool("IsRetrieving", true);
    }
    public void AttackSpin()//dubbelclick attack (spinning attack)
    {
        AnimReset();
        attackCoolDown = true;
        anim.SetBool("Is360", true);
        Invoke("AnimReset", 2);
        Invoke("AttackCooldown", 2);
    }
    public void Kick()//kick
    {
        attackCoolDown = true;
        pm.speed = 0;
        pm.runningSpeed = 0;
        anim.SetBool("IsKicking", true);
        Invoke("AnimReset", 1);
        Invoke("Resset", 1);
        Invoke("AttackCooldown", 1);
    }
    public void NormalAttack()//normal attack 
    {
        attackCoolDown = true;
        anim.SetBool("IsAttacking", true);
        Invoke("AnimReset", 2.2f);
        Invoke("AttackCooldown", 2.2f);
    }

    public void Taunt()//taunt function
    {
        anim.SetBool("IsTaunting", true);
        Invoke("AnimReset", 2.8f);
    }
    public void AnimReset()//reset animations
    {
        anim.SetBool("IsTaunting", false);
        anim.SetBool("IsKicking", false);
        anim.SetBool("Is360", false);
        anim.SetBool("IsAttacking", false);
    }
    public void Equip()
    {
        if (hasWeapon && !isTrowing)
        {
            equip = true;
            anim.SetBool("isUniQuiping", false);
            anim.SetBool("IsEquipt", true);
        }
    }
    public void Dequip()
    {
        if (hasWeapon && !isTrowing)
        {
            equip = false;
            anim.SetBool("IsEquipt", false);
            anim.SetBool("isUniQuiping", true);
        }
    }
    public void EquipmentOn()
    {
        unEquipted.SetActive(false);
        axe.SetActive(true);
        anim.SetBool("IsEquipt", false);
        visable = true;
    }
    public void EquipmentOff()
    {
        axe.SetActive(false);
        unEquipted.SetActive(true);
        anim.SetBool("isUniQuiping", false);
        visable = false;
    }
    #endregion
}