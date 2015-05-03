using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PlayerController : MonoBehaviour {

	public int indexSoldier;

	public ColorType colorType;
	public List<Material> materials;

	public float TimeToComeBackUp = 0.2f;
	public float CameraManiability = 75;

	public Transform backLeft;
	public Transform backRight;
	public Transform frontLeft;
	public Transform frontRight;
	
	public float speed = 10.0f;
	public float gravity = 1.0f;
	public float maxVelocityChange = 10.0f;
	public float jumpHeight = 2.0f;

	public GameObject mesh;
	
	public float timer;

	// Use this for initialization
	void Start () 
	{
		m_camera = GetComponentInChildren<Camera>();

		controller = ControllerInterface.GetController(indexSoldier);
		Debug.Log("Received "+controller.name+"game controller");
		m_anim = transform.Find("soldier").animation;

		switch (colorType) 
		{
			case ColorType.Bleu:
				mesh.renderer.material = materials[0];
				break;
			case ColorType.Rouge:
				mesh.renderer.material = materials[1];
				break;
			case ColorType.Vert:
				mesh.renderer.material = materials[2];
				break;
			case ColorType.Jaune:
				mesh.renderer.material = materials[3];
				break;
		}

		InitCamera();
	}

	void FixedUpdate () 
	{
		
		UpdateTargetVelocity();
		UpdateAnimation();
		UpdateCamera();

		if (timer > 0)
		{
			timer -= Time.deltaTime;
			rigidbody.angularVelocity = Vector3.zero;
			rigidbody.velocity = Vector3.zero;
			return;
		}

		rigidbody.angularVelocity = Vector3.zero;

		
		Vector3 velocity = rigidbody.velocity;
		//if (grounded) 
		{
			// Calculate how fast we should be moving
			targetVelocity *= speed;
			
			// Apply a force that attempts to reach our target velocity
			Vector3 velocityChange = (targetVelocity - velocity);
			velocityChange.x = Mathf.Clamp(velocityChange.x, -maxVelocityChange, maxVelocityChange);
			velocityChange.z = Mathf.Clamp(velocityChange.z, -maxVelocityChange, maxVelocityChange);
			velocityChange.y = 0f;
			rigidbody.AddForce(velocityChange, ForceMode.VelocityChange);
			//rigidbody.velocity = targetVelocity;

		}
		
		// Jump
		if (controller.GetKey("jump") || controller.GetKey("action") || controller.GetKey("validate")) 
		{
			rigidbody.velocity = new Vector3(velocity.x, CalculateJumpVerticalSpeed(), velocity.z);
		}
		else if (rigidbody.velocity.y > 0 && grounded)
		{
			Vector3 veloc = rigidbody.velocity;
			veloc.y = 0;
			rigidbody.AddForce(veloc - rigidbody.velocity, ForceMode.VelocityChange);
		}
		
		Debug.DrawRay(rr.point, rigidbody.velocity);
		//Debug.DrawRay(transform.position, rigidbody.velocity);
		
		// We apply gravity manually for more tuning control
		//rigidbody.AddForce(new Vector3 (0, -gravity * rigidbody.mass, 0));
		
		grounded = false;

		//UpdateNormal();

	}

	void OnCollisionStay () 
	{
		grounded = true;    
	}

	public void ResetPosition()
	{
		transform.localPosition = Vector3.zero;
		transform.localRotation = Quaternion.identity;
		//m_angleCameraX = 0;
		m_angleCameraY = 0;
	}

	private void UpdateAnimation()
	{
		if (m_animationStopped == false && targetVelocity == Vector3.zero)
		{
			m_animationStopped = true;
			StartCoroutine(StopAnimation());
		}
		else if (m_anim.isPlaying == false && targetVelocity != Vector3.zero)
		{
			m_anim.Play();
			m_animationStopped = false;
		}
	}

	private float CalculateJumpVerticalSpeed () 
	{
		// From the jump height and gravity we deduce the upwards speed 
		// for the character to reach at the apex.
		return Mathf.Sqrt(2 * jumpHeight * 10);
	}

	private void UpdateCamera() 
	{
		if (controller.GetKey("cameraRight"))
		{
			m_angleCameraY += 1.6f * Time.deltaTime * CameraManiability * controller.GetAxis("cameraRight");
		}
		if (controller.GetKey("cameraLeft"))
		{
			m_angleCameraY -= 1.6f * Time.deltaTime * CameraManiability * controller.GetAxis("cameraLeft");
		}
		if (controller.GetKey("cameraUp"))
		{
			m_angleCameraX += Time.deltaTime * CameraManiability * controller.GetAxis("cameraUp");
		}
		if (controller.GetKey("cameraDown"))
		{
			m_angleCameraX -= Time.deltaTime * CameraManiability * controller.GetAxis("cameraDown");
		}
		
		transform.localRotation = Quaternion.Euler(new Vector3(0,
		                                                       m_angleCameraY,
		                                                       0));
		transform.Find("camera").localRotation = Quaternion.Euler(new Vector3(m_angleCameraX,
		                                                       0,
		                                                       0));
	}

	private float m_angleCameraX;

	private void UpdateTargetVelocity()
	{
		targetVelocity = Vector3.zero;
		if (controller.GetKey("up"))
		{
			targetVelocity += transform.forward * controller.GetAxis("up");
		}
		if (controller.GetKey("down"))
		{
			targetVelocity -= transform.forward * controller.GetAxis("down");
		}
		if (controller.GetKey("right"))
		{
			targetVelocity += transform.right * controller.GetAxis("right");
		}
		if (controller.GetKey("left"))
		{
			targetVelocity -= transform.right * controller.GetAxis("left");
		}
	}

	private IEnumerator StopAnimation()
	{
		m_anim["Armature|ArmatureAction"].time = 0;
		yield return new WaitForEndOfFrame();
		m_anim.Stop();

	}

	private void UpdateNormal()
	{
		Physics.Raycast(backLeft.position + Vector3.up, Vector3.down, out lr);
		Physics.Raycast(backRight.position + Vector3.up, Vector3.down, out rr);
		Physics.Raycast(frontLeft.position + Vector3.up, Vector3.down, out lf);
		Physics.Raycast(frontRight.position + Vector3.up, Vector3.down, out rf);
		
		upDir = (Vector3.Cross(rr.point - Vector3.up, lr.point - Vector3.up) +
		         Vector3.Cross(lr.point - Vector3.up, lf.point - Vector3.up) +
		         Vector3.Cross(lf.point - Vector3.up, rf.point - Vector3.up) +
		         Vector3.Cross(rf.point - Vector3.up, rr.point - Vector3.up)
		         ).normalized;

		
		transform.up = upDir;//Vector3.Slerp(transform.up, upDir, Time.deltaTime / TimeToComeBackUp);
	}

	void InitCamera()
	{
		m_camera.rect = cameraMap[GameManager.TotalPlayers][indexSoldier];
	}

	public static Dictionary <int, List<Rect>> cameraMap = new Dictionary <int, List<Rect>>{
		{1, new List<Rect>(){new Rect(0, 0, 1, 1)}},
		{2, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 1, 0.49f)}},
		{3, new List<Rect>(){new Rect(0, 0.51f, 1, 0.49f), new Rect(0, 0, 0.495f, 0.49f), 
				new Rect(0.505f, 0, 0.495f, 0.49f)}},
		{4, new List<Rect>(){new Rect(0, 0.51f, 0.495f, 0.49f), new Rect(0, 0, 0.495f, 0.49f), 
				new Rect(0.505f, 0.51f, 0.495f, 0.49f), new Rect(0.505f, 0, 0.495f, 0.49f)}}
	};

	private Camera m_camera;
	
	private RaycastHit lr;
	private RaycastHit rr;
	private RaycastHit lf;
	private RaycastHit rf;
	private Vector3 upDir;
	
	private Animation m_anim;
	
	private float m_angleCameraY;
	
	private bool m_animationStopped = false;
	private bool grounded = false;
	
	private Vector3 targetVelocity;
	private ControllerBase controller;

}
