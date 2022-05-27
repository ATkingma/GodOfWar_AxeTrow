using UnityEngine;
public class InputManagergodofwar : MonoBehaviour
{
    //publi
    [HideInInspector]
    public Vector3 moveDir;
    [HideInInspector]
    public bool forwardPressed, backwardsPressed, leftPressed, rightPressed, runPressed, leftClick, rightClick,taunt,equip,spacebar;
    void Update()
    {
        //input 
        moveDir = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical"));
        forwardPressed = Input.GetKey("w");
        backwardsPressed = Input.GetKey("s");
        leftPressed = Input.GetKey("a");
        rightPressed = Input.GetKey("d");
        spacebar = Input.GetKey("space");
        runPressed = Input.GetKey("left shift");
        equip = Input.GetKeyDown("f");
        taunt = Input.GetKeyDown("t");
        leftClick = Input.GetKeyDown(KeyCode.Mouse0);
        rightClick = Input.GetKey(KeyCode.Mouse1);
    }
}