using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour {

	static int players;

	public float TimeToComeBackUp = 0.2f;
	public float VelocityCoef = 20;
	public float CameraManiability = 1;

	public Transform camera;

	private float m_angleCamera;

	Vector3 velocity;
	ControllerBase controller;

	// Use this for initialization
	void Start () {
		controller = ControllerInterface.GetController(players);
		players++;
		camera = transform;//.Find("camera");
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
		velocity.y = 0;
		//rigidbody.velocity = velocity;
		rigidbody.AddForce(velocity);

		
		if (controller.GetKey("cameraRight"))
		{
			m_angleCamera += Time.deltaTime * CameraManiability;
		}
		if (controller.GetKey("cameraLeft"))
		{
			m_angleCamera -= Time.deltaTime * CameraManiability;
		}

		camera.localRotation = Quaternion.Euler(new Vector3(camera.localRotation.eulerAngles.x,
		                                                    m_angleCamera,
		                                                    camera.localRotation.eulerAngles.z));


	}

	void Update() {
		transform.up = Vector3.up;
		
		camera.localRotation = Quaternion.Euler(new Vector3(camera.localRotation.eulerAngles.x,
		                                                    m_angleCamera,
		                                                    camera.localRotation.eulerAngles.z));
		return;
		RaycastHit hit;
		if (Physics.Raycast(transform.position + 5 * transform.up, -20 * transform.up, out hit)){
			transform.up = Vector3.Slerp(transform.up, hit.normal, Time.deltaTime / TimeToComeBackUp);
		}
		else
		{
			transform.up = Vector3.Slerp(transform.up, Vector3.up, Time.deltaTime / TimeToComeBackUp);
		}
	}

}
