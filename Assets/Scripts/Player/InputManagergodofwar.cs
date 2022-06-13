using UnityEngine;
public class InputManagergodofwar : MonoBehaviour
{
    //publi
    [HideInInspector]
    public Vector3 moveDir;
    [HideInInspector]
    public bool forwardPressed, backwardsPressed, leftPressed, rightPressed, runPressed, leftClick, rightClick,taunt,spacebar;
    void Update()
    {
        //input 
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        forwardPressed = Input.GetButton("w");
        backwardsPressed = Input.GetButton("s");
        leftPressed = Input.GetButton("a");
        rightPressed = Input.GetButton("d");
        spacebar = Input.GetButton("space");
        runPressed = Input.GetButton("left shift");
        taunt = Input.GetButton("taunt");
        leftClick = Input.GetButtonDown("Fire1");
        rightClick = Input.GetButton("Fire2");
    }
}