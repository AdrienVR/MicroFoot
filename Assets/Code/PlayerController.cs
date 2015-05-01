using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	static int players;

	public float TimeToComeBackUp = 0.2f;
	public float VelocityCoef = 20;
	public float CameraManiability = 1;

	private Animation anim;

	private float m_angleCamera;

	private bool flying = true;

	Vector3 velocity;
	ControllerBase controller;

	// Use this for initialization
	void Start () {
		controller = ControllerInterface.GetController(players);
		players++;
		anim = transform.Find("soldier").animation;
	}

	void OnCollisionEnter(Collision collision) {
		if (collision.gameObject.name == "mesh")
			flying = false;
	}
	
	void OnCollisionExit(Collision collision) {
		if (collision.gameObject.name == "mesh")
			flying = true;
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
		velocity = velocity.normalized * VelocityCoef;
		if (flying == true)
		{
			velocity.y = -10;
		}
		else
		{
			velocity.y = 0;
		}
		//rigidbody.velocity = velocity;
		rigidbody.velocity = (velocity);

		if (velocity == Vector3.zero)
		{
			anim["Armature|ArmatureAction"].time = 0;
			anim.Stop();
		}
		else if (anim.isPlaying == false)
		{
			anim.Play();
		}

		
		if (controller.GetKey("cameraRight"))
		{
			m_angleCamera += Time.deltaTime * CameraManiability;
		}
		if (controller.GetKey("cameraLeft"))
		{
			m_angleCamera -= Time.deltaTime * CameraManiability;
		}



	}

	void Update() {

		Vector3 up;
		RaycastHit hit;
		if (Physics.Raycast(transform.position + 5 * transform.up, -20 * transform.up, out hit)){
			up = Vector3.Slerp(transform.up, hit.normal, Time.deltaTime / TimeToComeBackUp);
		}
		else
		{
			up = Vector3.Slerp(transform.up, Vector3.up, Time.deltaTime / TimeToComeBackUp);
		}
		transform.localRotation = Quaternion.LookRotation(Vector3.forward, up);
		transform.localRotation = Quaternion.Euler(new Vector3(transform.localRotation.eulerAngles.x,
		                                                       m_angleCamera,
		                                                       transform.localRotation.eulerAngles.z));
	}

}
