using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	static int players;

	public float timeToComeBackUp = 0.2f;
	public float velocityCoef = 20;

	Vector3 velocity;
	ControllerBase controller;

	// Use this for initialization
	void Start () {
		controller = ControllerInterface.GetController(players);
		players++;
	}
	
	// Update is called once per frame
	void FixedUpdate () {
		velocity = Vector3.zero;
		if (controller.GetKey("up"))
		{
			velocity += transform.forward;
		}
		if (controller.GetKey("down"))
		{
			velocity -= transform.forward;
		}
		if (controller.GetKey("right"))
		{
			velocity += transform.right;
		}
		if (controller.GetKey("left"))
		{
			velocity -= transform.right;
		}
		velocity = velocity.normalized * velocityCoef;
		//velocity.y = -9;
		//rigidbody.velocity = velocity;
		rigidbody.AddForce(velocity);
	}

	void Update() {
		RaycastHit hit;
		if (Physics.Raycast(transform.position + 5 * transform.up, -20 * transform.up, out hit)){
			transform.up = Vector3.Slerp(transform.up, hit.normal, Time.deltaTime / timeToComeBackUp);
		}
		else
		{
			transform.up = Vector3.Slerp(transform.up, Vector3.up, Time.deltaTime / timeToComeBackUp);
		}
	}

}
