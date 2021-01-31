using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AxeController : MonoBehaviour
{
    //public
    public bool activated;
    public float rotationSpeed= 180f;
    public float rotationValue;
    public GameObject player;
    void Update()
    {
        if (activated)
        {
            float distance = Vector3.Distance(transform.position, player.transform.position);
            if (distance <= 3.5f)
            {
                transform.rotation = Quaternion.Euler(90, 0, 0);
            }
            else if (distance > 3.5f)
            {
                rotationValue += rotationSpeed * Time.deltaTime;
                transform.rotation = Quaternion.Euler(4, rotationValue, rotationValue);
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer ==0| collision.gameObject.layer == 9)
        {
            GetComponent<Rigidbody>().Sleep();
            GetComponent<Rigidbody>().collisionDetectionMode = CollisionDetectionMode.ContinuousSpeculative;
            GetComponent<Rigidbody>().isKinematic = true;
            activated = false;
        }
    }
}