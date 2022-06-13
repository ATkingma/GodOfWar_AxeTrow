using UnityEngine;
using System.Collections;

public class PlayerCombat : MonoBehaviour
{
    //public
    public InputManagergodofwar im;
    public PlayerMovement pm;
    public PlayerAnimationController animController;
    public GameObject axe, unEquipted, cam,trailEffect;
    public Rigidbody axeRb;
    public Transform curvePos, handPos, axeHandPos;
    public float throwPower;
    [HideInInspector]
    public Vector3 des;
    //private
    private Vector3 pullPos;
    private bool  moreWeight, pulling, hasWeapon, attackCoolDown, dubbelclick, clickResetOn;
    private float returnTime, speed, runningSpeed;
    private int clickCount;

    private AxeController axeController;
    private void Start()
    {
        axeController = axe.GetComponent<AxeController>();
        hasWeapon = true;
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
    public void InputUpdate()
    {
        if (im.leftClick)
        {
            clickCount++;
            Invoke("ResetClickCount", 1.5f);
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


        if (dubbelclick && !attackCoolDown && hasWeapon)
        {
            AttackSpin();
        }
        else if (im.leftClick&& !attackCoolDown&&!dubbelclick&&hasWeapon&&!im.rightClick)
        {
            NormalAttack();
        }


        if (im.spacebar && !attackCoolDown)
        {
            Kick();
        }

        if (im.taunt)
        {
            animController.Taunt();
        }

        if (hasWeapon&&!attackCoolDown)
        {
            animController.SetAnimLayerWeight(1, 1);
        }

        if (!hasWeapon && im.leftClick && !pulling)
        {
            animController.RetrievingAxe();
            Pull();
        }

        if (im.rightClick)
        {
            ChargingAxe();
            if (!moreWeight)
            {
                animController.SetAnimLayerWeight(1, 0.75f);
            }
        }
        else
        {
            animController.StopCharging();
        }
    }

    //axe functions
    public void AxeUpdate()
    {
        if (pulling)
        {
            if (returnTime < 1)
            {
                axe.transform.position = Quat.GetQuadraticCurvePoint(returnTime, pullPos, curvePos.position, handPos.position);
                returnTime += Time.deltaTime * 1.5f;
            }
            else
            {
                animController.TurnOffTrowingBool();
                hasWeapon = true;
                pulling = false;
                StartCoroutine(Catch());
            }
        }
    }
    IEnumerator Catch()
    {
        animController.ToggleCatching();
        returnTime = 0;
        axeController.activated = false;
        trailEffect.SetActive(false);
        axe.transform.parent = handPos;
        axe.transform.position = axeHandPos.position;
        axe.transform.rotation = axeHandPos.rotation;
        pulling = false;

        yield return new WaitForSeconds(0.1f);
        animController.ToggleCatching();
    }
    public void TrowAxe()
    {
        moreWeight = false;
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
    public void Pull()
    {
        pullPos = axe.transform.position;
        axeRb.Sleep();
        axeRb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
        axeRb.isKinematic = true;
        axeController.activated = true;
        trailEffect.SetActive(true);
        pulling = true;      
    }
    public void ChargingAxe()
    {

        animController.startCharging();

        if (im.rightClick && im.leftClick&&!attackCoolDown)
        {
            moreWeight = true;
            animController.SetAnimLayerWeight(1, 1);
            animController.RelasingAxe();
        }
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

    #region animation functions

    public void AttackSpin()
    {
        attackCoolDown = true;
        animController.AttackSpin();
        Invoke("Resset", 0.5f);
        Invoke("AttackCooldown", 0.5f);
    }

    public void Kick()
    {
        animController.Kick();
        attackCoolDown = true;
        pm.speed = 0;
        pm.runningSpeed = 0;
        Invoke("Resset", 1);
        Invoke("AttackCooldown", 1);
    }

    public void NormalAttack() 
    {
        attackCoolDown = true;
        animController.NormalAttack();
        Invoke("AttackCooldown", 2.2f);
    }
    #endregion
}