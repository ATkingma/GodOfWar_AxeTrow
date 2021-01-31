using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using Cinemachine;
public class CharachterController : MonoBehaviour
{
    public InputManagergodofwar im;
    public PlayerMovement pm;
    public Animator anim;
    public GameObject axe, unEquipted, cam,gotchaEffect,trailEffect;
    public Rigidbody axeRb;
    public Vector3 des;
    public Transform curvePos, handPos, axeHandPos;
    public float throwPower;
    private Vector3 pullPos;
    private bool equip, visable, moreWeight, pulling, hasWeapon, attackCoolDown, dubbelclick, clickResetOn, isTrowing;
    private float returnTime, speed, runningSpeed;
    private int clickCount;
    private void Start()
    {
        axe.GetComponent<ParticleSystem>().Stop();
        gotchaEffect.GetComponent<ParticleSystem>().Stop();
        hasWeapon = true;
        equip = true;
        visable = true;
        des = new Vector3(0, 0, 0);
        speed = pm.speed;
        runningSpeed = pm.runningSpeed;
        trailEffect.SetActive(false);
    }
    private void Update()
    {
        InputUpdate();
        AxeUpdate();
    }
    public void InputUpdate()
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
        else if (clickCount < 2)
        {
            dubbelclick = false;
        }
        if (dubbelclick && !attackCoolDown && hasWeapon&&equip)
        {
            AttackSpin();
        }
        if (im.leftClick&& !attackCoolDown&&!dubbelclick&&hasWeapon&&!im.rightClick && equip)
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
                TrowingAxe();
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
    public void AxeUpdate()
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
                Gotcha();
                Invoke("TurnOffParticle",0.5f);
            }
        }
    }
    public void Gotcha()
    {
        gotchaEffect.GetComponent<ParticleSystem>().Play();
        returnTime = 0;
        axe.GetComponent<AxeController>().activated = false;
        trailEffect.SetActive(false);
        axe.transform.parent = handPos;
        axe.transform.position = axeHandPos.position;
        axe.transform.rotation = axeHandPos.rotation;
        pulling = false;
    }
    public void TurnOffParticle()
    {
        gotchaEffect.GetComponent<ParticleSystem>().Stop();
    }
    public void TurnOffTrowingBool()
    {
        anim.SetBool("IsTrowing", false);
        anim.SetBool("IsCharging", false);
        anim.SetBool("IsRetrieving", false);
    }
    public void TrowAxe()
    {
        moreWeight = false;
        if (visable&& equip)
        {
            hasWeapon = false;
            axe.GetComponent<AxeController>().activated = true;
            trailEffect.SetActive(true);
            axeRb.isKinematic = false;
            axeRb.collisionDetectionMode = CollisionDetectionMode.Continuous;
            axe.transform.parent = null;
            axe.transform.eulerAngles = new Vector3(0, -90 + transform.eulerAngles.y, 0);
            axe.transform.position += transform.right / 5;
            axeRb.AddForce(cam.transform.forward * throwPower + transform.up * 4, ForceMode.Impulse);
        }
    }
    public void Pull()
    {
        pullPos = axe.transform.position;
        axeRb.Sleep();
        axeRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        axeRb.isKinematic = true;
        axe.transform.DORotate(new Vector3(-90, -90, 0), .2f).SetEase(Ease.InOutSine);
        axe.transform.DOBlendableLocalRotateBy(Vector3.right * 90, .5f);
        axe.GetComponent<AxeController>().activated = true;
        trailEffect.SetActive(true);
        pulling = true;      
    }
    public void TrowingAxe()
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
    public void RelasingAxe()
    {
        anim.SetBool("IsTrowing", true);
    }
    public void RetrievingAxe()
    {
        anim.SetBool("IsRetrieving", true);
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
        if (hasWeapon&&!isTrowing)
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
    public void AttackSpin()
    {
        AnimReset();
        attackCoolDown = true;
        anim.SetBool("Is360", true);
        Invoke("AnimReset", 2);
        Invoke("AttackCooldown", 2);
    }
    public void Kick()
    {
        attackCoolDown = true;
        pm.speed=0;
        pm.runningSpeed=0;
        anim.SetBool("IsKicking", true);
        Invoke("AnimReset", 1);
        Invoke("Resset", 1);
        Invoke("AttackCooldown", 1);
    }
    public void NormalAttack()
    {
        attackCoolDown = true;
        anim.SetBool("IsAttacking", true);
        Invoke("AnimReset", 2.2f);
        Invoke("AttackCooldown", 2.2f);
    }
    public void Taunt()
    {
     anim.SetBool("IsTaunting", true);
     Invoke("AnimReset", 2.8f);
    }
    public void AnimReset()
    {
        anim.SetBool("IsTaunting", false);
        anim.SetBool("IsKicking", false);
        anim.SetBool("Is360", false);
        anim.SetBool("IsAttacking", false);
    }
    public void Resset()
    {
        pm.speed = speed;
        pm.runningSpeed = runningSpeed;
    }
    public void AttackCooldown()
    {
        attackCoolDown = false;
    }
    public void ResetClickCount()
    {
        clickCount = 0;
    }
    public Vector3 GetQuadraticCurvePoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        return (uu * p0) + (2 * u * t * p1) + (tt * p2);
    }
}