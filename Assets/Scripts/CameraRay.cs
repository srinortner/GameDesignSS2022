using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraRay : MonoBehaviour
{
	[SerializeField] float navigationSpeed = 2.4f;
	[SerializeField] float shiftMultiplier = 2f;
	[SerializeField] float sensitivity = 1.0f;

	private Camera cam;
	
	public float ForceUp;
	public Transform platformYellow;
	public Transform platformRed;
	public Transform platformBlue;
	public Transform platformMagenta;
	public Transform platformCyan;
	public Transform platformGreen;
	
	private Vector3 anchorPoint;
	private Quaternion anchorRot;
	private Vector3 previousPos;
	private float startTime;
	private GameObject[] spheres;
	private GameObject text;
	private SliderController _sliderController;
	private bool outOfBounds;
	private GameObject _audioController;
	private AudioManager _audioManager;
	private bool musicOn = true;
	private string backgroundMusic = "BaseMusic";

	private void Awake()
	{
		cam = GetComponent<Camera>();
	}

	// Start is called before the first frame update
	void Start()
	{
		previousPos = new Vector3();
		spheres = GameObject.FindGameObjectsWithTag("Sphere");
		text = GameObject.FindGameObjectWithTag("Text");
		_sliderController = GameObject.FindObjectOfType<SliderController>();
		outOfBounds = false;
		_audioController = GameObject.FindWithTag("AudioManager");
		_audioManager = _audioController.GetComponent<AudioManager>();
		_audioManager.Play(backgroundMusic);
	}

	private void Update()
	{
		float xMin = -24.0f;
		float xMax = 24.0f;
		float yMin = 1.0f;
		float yMax = 17.0f;
		float zMin = -41f;
		float zMax = 23.7f;

		if (Input.GetKey(KeyCode.Q))
		{
			_audioManager.MusicToggle(backgroundMusic);
		}


		//Zoom in and out with Mouse Wheel
		this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.navigationSpeed, Space.Self);

		float xAxis = Input.GetAxis("Horizontal");
		float zAxis = Input.GetAxis("Vertical");

		//as long as you hold the left mouse button, you can navigate through the scene
		Vector3 move = Vector3.zero;
		float speed = navigationSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime *
		              9.1f;
		move += (Vector3.up * zAxis + Vector3.right * xAxis) * speed;
		transform.Translate(move);
		transform.position = new Vector3(
			Mathf.Clamp(transform.position.x, xMin, xMax),
			Mathf.Clamp(transform.position.y, yMin, yMax),
			Mathf.Clamp(transform.position.z, zMin, zMax));

		if (Input.GetMouseButtonDown(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				//if mouse is clicked but is currently over the slider, we do not want to move the camera :)
				return;
			}
			anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
			anchorRot = transform.rotation;
		}

		if (Input.GetMouseButton(0))
		{
			if (EventSystem.current.IsPointerOverGameObject())
			{
				//if mouse is clicked but is currently over the slider, we do not want to move the camera :)
				return;
			}
			Quaternion rot = anchorRot;

			Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
			rot.eulerAngles += dif * sensitivity;
			transform.rotation = rot;
		}
	}
	

	// Update is called once per frame
    void FixedUpdate()
    {
	    RaycastHit hit;
		
        cam = GetComponent<Camera>();
		//Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		var ray = cam.ScreenPointToRay(Input.mousePosition);

		if (Physics.Raycast(ray, out hit, 60f))
		{
			if (hit.rigidbody != null)
			{
				Rigidbody rb = hit.rigidbody;
				//norm direction = norm(destination - source)
				Transform closestPlatform = null;
				float closestDistance = 1000000f;
				foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Platform")) {
					float distance = Vector3.Distance(hit.point, obj.transform.position);
					if (closestDistance > distance)
                    {
						closestPlatform = obj.transform;
						closestDistance = distance;
                    }
                }
				Vector3 dir = rb.transform.position - hit.point;
				Vector3 toClosest = (hit.point - closestPlatform.position);
				Vector3 hit_dir = new Vector3(dir.x, 0f, dir.z * 5 + 2.5f * dir.y);
				hit_dir += toClosest.normalized * -10/ toClosest.magnitude; // increase -x value if you want the sphere to have higher magnetic value
				hit_dir = hit_dir.normalized;
				float forceForward = 0;
				
				//Spheres
				if (rb.CompareTag("Sphere") && rb.GetComponent<balls>().canJump) 
				{
					rb.GetComponent<balls>().canJump = false;
					Vector3 force = hit_dir * _sliderController.getSliderStrength().value;
					force += Vector3.up * ForceUp;
					force += Vector3.back * forceForward;
					//Debug.DrawRay(rb.position, force, Color.black, 3f); //activate gizmo in game view to see ray
					rb.AddForce(force, ForceMode.Impulse);
				}
				//Cubes
				else if (rb.CompareTag("Cubi") && rb.GetComponent<CubiController>().canJump)
				{
					var varForceUp = 4f;
					rb.GetComponent<CubiController>().canJump = false;
					Transform closestPlatformCube = null;
					float closestDistanceCube = 1000000f;
					foreach (GameObject obj in GameObject.FindGameObjectsWithTag("Platform")) {
						float distance = Vector3.Distance(hit.point, obj.transform.position);
						if ((closestDistanceCube > distance) && obj.GetComponent<Renderer>().material.color == rb.GetComponent<Renderer>().material.color)
						{
							if (rb.GetComponent<Renderer>().material.color == Color.red)
							{
								varForceUp = 7f;
							}
							else if (rb.GetComponent<Renderer>().material.color == Color.cyan)
							{
								varForceUp = 7f;
							}
							else
							{
								varForceUp = 4f;
							}
							closestPlatformCube = obj.transform;
							closestDistanceCube = distance;
						}
					}
					Vector3 _dir = rb.transform.position - hit.point;
					Vector3 _toClosest = (hit.point - closestPlatformCube.position);
					Vector3 _hit_dir = new Vector3(_dir.x, 0f, _dir.z * 5 + 2.5f * _dir.y);
					_hit_dir += _toClosest.normalized * -50/ _toClosest.magnitude;
					_hit_dir = _hit_dir.normalized;
					Vector3 force = (_hit_dir) * _sliderController.getSliderStrength().value; //
					force += Vector3.up * ((ForceUp - varForceUp) + _sliderController.getForceUp().value);
					force += Vector3.back * forceForward;
					//Debug.DrawRay(rb.position, force, Color.grey, 3f);
					rb.AddForce(force, ForceMode.Impulse);
				}
			}
			previousPos = hit.point; //mouse position in 3D
		}
    }

    private bool hitOnTop(RaycastHit MyRayHit)
    {
	    Vector3 MyNormal = MyRayHit.normal;
	    MyNormal = MyRayHit.transform.TransformDirection( MyNormal );
	    float diff = MyRayHit.transform.up.y - MyNormal.y;
	    if (diff <= 0.1)
	    {
		    print("HIT ON TOP");
		    return true;
	    }

	    return false;
    }
    
    private enum HitDirection { None, Top, Bottom, Forward, Back, Left, Right }
    private HitDirection ReturnDirection( GameObject Object, GameObject ObjectHit ){
         
	    HitDirection hitDirection = HitDirection.None;
	    RaycastHit MyRayHit;
	    Vector3 direction = ( Object.transform.position - ObjectHit.transform.position ).normalized;
	    Ray MyRay = new Ray( ObjectHit.transform.position, direction );
         
	    if ( Physics.Raycast( MyRay, out MyRayHit ) ){
                 
		    if ( MyRayHit.collider != null ){
                 
			    Vector3 MyNormal = MyRayHit.normal;
			    MyNormal = MyRayHit.transform.TransformDirection( MyNormal );
                 
			    if( MyNormal == MyRayHit.transform.up ){ hitDirection = HitDirection.Top; }
			    if( MyNormal == -MyRayHit.transform.up ){ hitDirection = HitDirection.Bottom; }
			    if( MyNormal == MyRayHit.transform.forward ){ hitDirection = HitDirection.Forward; }
			    if( MyNormal == -MyRayHit.transform.forward ){ hitDirection = HitDirection.Back; }
			    if( MyNormal == MyRayHit.transform.right ){ hitDirection = HitDirection.Right; }
			    if( MyNormal == -MyRayHit.transform.right ){ hitDirection = HitDirection.Left; }
		    }    
	    }
	    return hitDirection;
    
	}
    

}
