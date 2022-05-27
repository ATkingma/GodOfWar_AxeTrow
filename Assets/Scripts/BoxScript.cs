using System.Collections;
using UnityEngine;

public class BoxScript : MonoBehaviour
{
	public float timeBeforeResetting;

	private Vector3 _startingPosition;

    void Start()
    {
        _startingPosition = transform.position;
    }
	private void OnCollisionEnter(Collision collision)
	{
		if (collision.transform.CompareTag("Axe"))
		{
			StartCoroutine(ResetPosition());
		}
	}
	private IEnumerator ResetPosition()
	{
		yield return new WaitForSeconds(timeBeforeResetting);
		transform.rotation = Quaternion.identity;
		transform.position = _startingPosition;
	}
}