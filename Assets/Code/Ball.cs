using UnityEngine;
using System.Collections;

public class Ball : MonoBehaviour {

	public float maxVelocityChange;

	public float speed = 25;

	public float timer;

	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (timer > 0)
		{
			timer -= Time.deltaTime;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.velocity = Vector3.zero;
			return;
		}

		if(rigidbody.velocity.magnitude > maxVelocityChange)
		{
			rigidbody.velocity = rigidbody.velocity.normalized * maxVelocityChange;
		}
	
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name == "Goal")
			transform.GetChild(0).renderer.enabled = false;
		if (collision.gameObject.name == "mesh")
			return;
		rigidbody.AddForce(collision.contacts[0].normal * speed, ForceMode.VelocityChange);
	}

	void OnCollisionExit(Collision collisionInfo) {
		if (collisionInfo.gameObject.name == "Goal")
			transform.GetChild(0).renderer.enabled = true;
	}
}
