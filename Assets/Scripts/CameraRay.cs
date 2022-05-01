using System;
using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Quaternion = UnityEngine.Quaternion;
using Vector3 = UnityEngine.Vector3;

public class CameraRay : MonoBehaviour
{
	private bool movementLeft = true;
	
	[SerializeField] float navigationSpeed = 2.4f;
	[SerializeField] float shiftMultiplier = 2f;
	[SerializeField] float sensitivity = 1.0f;
 
	private Camera cam;
	public float ForceUp;
	private Vector3 anchorPoint;
	private Quaternion anchorRot;
	private Vector3 previousPos;
	private float startTime;
	private GameObject[] spheres;
	private GameObject text;
	private List<Color> colorList;
	private SliderController _sliderController;

	private void Awake () {
		cam = GetComponent<Camera>();
	}
    // Start is called before the first frame update
    void Start()
    {
	    previousPos = new Vector3();
	    spheres = GameObject.FindGameObjectsWithTag("Sphere");
	    text = GameObject.FindGameObjectWithTag("Text");
	    colorList = new List<Color>(){Color.magenta, Color.blue, Color.cyan, Color.black};
	    _sliderController = GameObject.FindObjectOfType<SliderController>();
    }

    private void Update()
    {
	    if (text != null)
	    {
		    //GetMouseButton(0) = Left Mouse Button
		    if (text.activeInHierarchy && Input.GetMouseButton(0))
		    {
			    text.SetActive(false);
		    }
	    }
	    //Zoom in and out with Mouse Wheel
	    this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.navigationSpeed, Space.Self);

	    float xAxis = Input.GetAxis("Horizontal");
	    float zAxis = Input.GetAxis("Vertical");

	    //as long as you hold the left mouse button, you can navigate through the scene
	    Vector3 move = Vector3.zero;
	    float speed = navigationSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime * 9.1f;
	    move += (Vector3.up * zAxis + Vector3.right * xAxis) * speed;
	    /*if(Input.GetKey(KeyCode.W))
		    move += Vector3.forward * speed;
	    if(Input.GetKey(KeyCode.S))
		    move -= Vector3.forward * speed;
	    if(Input.GetKey(KeyCode.D))
		    move += Vector3.right * speed;
	    if(Input.GetKey(KeyCode.A))
		    move -= Vector3.right * speed;
	    if(Input.GetKey(KeyCode.E))
		    move += Vector3.up * speed;
	    if(Input.GetKey(KeyCode.Q))
		    move -= Vector3.up * speed;*/
	    transform.Translate(move);

	    if(Input.GetMouseButtonDown(0)) {
		    anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
		    anchorRot = transform.rotation;
	    }
	    if(Input.GetMouseButton(0)) {
		    Quaternion rot = anchorRot;
 
		    Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
		    rot.eulerAngles += dif * sensitivity;
		    transform.rotation = rot;
	    }
	    //Debug.Log(spheres[0]);
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
				Vector3 dir = rb.transform.position - hit.point;
				Vector3 hit_dir = new Vector3(dir.x, 0f, dir.z).normalized;
				
				//Spheres
				if (rb.CompareTag("Sphere")) //&& !isMovingSphere(rb) <-- for checking if sphere is moving, does not work as I want to...
				{
					Vector3 force = hit_dir * _sliderController.getSlider().value;
					force += Vector3.up * ForceUp;
					Debug.DrawRay(rb.position, force, Color.black, 3f); //activate gizmo in game view to see ray
					rb.AddForce(force, ForceMode.Impulse); //needs a bit tinkering 
					//Debug.Log("Velocity vector: " + rb.velocity);
					Vector3 middle = new Vector3();
				}
				//Cubes
				else if (rb.CompareTag("Cubi") && !isMoving(rb))
				{
					Vector3 force = hit_dir * _sliderController.getSlider().value;
					force += Vector3.up * ForceUp;
					Debug.DrawRay(rb.position, force, Color.grey, 3f);
					rb.AddForce(force, ForceMode.Impulse);
				}
			}

			previousPos = hit.point; //mouse position in 3D
		}
    }

    private bool isMoving(Rigidbody rb)
    {
	    if (rb.velocity.Equals(Vector3.zero))
	    {
		    // object is not moving
		    return false;
	    }
	    //object is moving
	    return true;
    }

    private bool isMovingSphere(Rigidbody rb)
    {
	    //0.3f is middle force
	    if ((rb.velocity.z <= 0.4f && rb.velocity.z >= -0.4f) || (rb.velocity.x <= 1f && rb.velocity.x >= -1f))
	    {
		    return false;
	    }
	    return true;
    }
}
