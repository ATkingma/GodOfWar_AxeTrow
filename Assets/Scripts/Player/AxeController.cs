using UnityEngine;
using System;
public class AxeController : MonoBehaviour
{
    //public
    public GameObject player;
    public float grabDistance=3.5f;
    [HideInInspector]
    public bool activated;
    //serialized privates
    [SerializeField]
    private float rotationSpeed= 180f;
    //privates
    private float rotationValue;
    private Rigidbody rb;
	private void Start()
	{
        rb = GetComponent<Rigidbody>();
    }
	void Update()
    {    
        if (activated)
        {            
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= grabDistance)//roate the axe to hand rotation so it wil be nicer when it snaps in the hand
            {
                Quaternion.Slerp(transform.rotation, Quaternion.Euler(90, 0, 0), 1);
            }
            else if (distance > grabDistance)//roatate the axe till its in range
            {
                rotationValue += rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(4, rotationValue, rotationValue);
            }
        }
        else
		{
            rb.Sleep();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
		if (collision.transform.gameObject != player)
        { 
            rb.Sleep();
            rb.collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            rb.isKinematic = true;
            activated = false;
		}
    }
}