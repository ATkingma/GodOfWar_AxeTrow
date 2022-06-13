                 using UnityEngine;
public class PlayerMovement : MonoBehaviour
{
    ///public
    public float speed=1f;
    public float runningSpeed = 2f;
    public float downForce;
    public Camera cam;
    public CharacterController controller;
	public InputManagergodofwar im;

    public PlayerAnimationController animationController;
    public float acceleration = 2f;
    public float decelaration = 2f;
    public float maxWalkVelocity = 0.5f;
    public float maxRunVelocity = 2.0f;
    //private
    private float velocitiyZ = 0.0f;
    private float velocitiyX = 0.0f;
    ///private
    private Vector3 moveDir;
    void Update()
    {
        Movement();
        UpdateAnimations();
    }
    private void Movement()
    {
        moveDir = im.moveDir;

        transform.forward = new Vector3(cam.transform.forward.x, transform.forward.y, cam.transform.forward.z);
        moveDir = transform.TransformDirection(moveDir);

        if (!im.runPressed)
        {
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            controller.Move(new Vector3(0, downForce, 0) * speed * Time.deltaTime);
        }
        if (im.rightPressed | im.leftPressed)
        {
            controller.Move(moveDir.normalized * speed * Time.deltaTime);
            controller.Move(new Vector3(0, downForce, 0) * speed * Time.deltaTime);
        }
        else if (im.runPressed&&!im.leftPressed&&!im.rightPressed)
        {
            controller.Move(moveDir.normalized * runningSpeed * Time.deltaTime);
            controller.Move(new Vector3(0, downForce, 0) * runningSpeed * Time.deltaTime);
        }      
    }
    private void UpdateAnimations()
	{

        float currentMaxVelocity = im.runPressed ? maxRunVelocity : maxWalkVelocity;

        if (im.forwardPressed && velocitiyZ < currentMaxVelocity)
        {
            velocitiyZ += Time.deltaTime * acceleration;
        }
        if (im.backwardsPressed && velocitiyZ > -currentMaxVelocity)
        {
            velocitiyZ -= Time.deltaTime * acceleration;
        }
        if (im.leftPressed && velocitiyX > -currentMaxVelocity)
        {
            velocitiyX -= Time.deltaTime * acceleration;
        }
        if (im.rightPressed && velocitiyX < currentMaxVelocity)
        {
            velocitiyX += Time.deltaTime * acceleration;
        }

        if (!im.forwardPressed && velocitiyZ > 0.0f)
        {
            velocitiyZ -= Time.deltaTime * decelaration;
        }

        if (!im.backwardsPressed && velocitiyZ < 0.0f)
        {
            velocitiyZ += Time.deltaTime * decelaration;
        }

        if (!im.leftPressed && velocitiyX < 0.0f)
        {
            velocitiyX += Time.deltaTime * decelaration;
        }

        if (!im.rightPressed && velocitiyX > 0.0f)
        {
            velocitiyX -= Time.deltaTime * decelaration;
        }

        if (!im.leftPressed && !im.rightPressed && velocitiyX != 0.0f && (velocitiyX > -0.5f && velocitiyX < 0.5f))
        {
            velocitiyX = 0;
        }

        if (im.forwardPressed && im.runPressed && velocitiyZ > currentMaxVelocity)
        {
            velocitiyZ = currentMaxVelocity;
        }
        else if (im.forwardPressed && velocitiyZ > currentMaxVelocity)
        {
            velocitiyZ -= Time.deltaTime * decelaration;
            if (velocitiyZ > currentMaxVelocity && velocitiyZ > (currentMaxVelocity - 0.05f))
            {
                velocitiyZ = currentMaxVelocity;
            }
        }
        else if (im.forwardPressed && velocitiyZ < currentMaxVelocity && velocitiyZ > (currentMaxVelocity - 0.05f))
        {
            velocitiyZ = currentMaxVelocity;
        }

        animationController.SetVelocity(velocitiyZ, velocitiyX);
    }
}