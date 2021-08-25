using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyScript : MonoBehaviour
{
    public float timeToDestroy;
    void Start()
    {
        Invoke("DestroyGameObject", timeToDestroy);
    }

    public void DestroyGameObject()
	{
        Destroy(gameObject);
	}
}