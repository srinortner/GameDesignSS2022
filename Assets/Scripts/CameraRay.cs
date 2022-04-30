using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
	private bool usabilityOn = false;
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
	    
	    //as long as you hold the left mouse button, you can navigate through the scene
	    if(Input.GetMouseButton(0)) {
		    Vector3 move = Vector3.zero;
		    float speed = navigationSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime * 9.1f;
		    if(Input.GetKey(KeyCode.W))
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
			    move -= Vector3.up * speed;
		    transform.Translate(move);
	    }
 
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
				if (rb.CompareTag("Sphere"))
				{
					Vector3 force = hit_dir * _sliderController.getSlider().value;
					force += Vector3.up * ForceUp;
					Debug.DrawRay(rb.position, force, Color.black, 3f); //activate gizmo in game view to see ray
					rb.AddForce(force, ForceMode.Impulse); //needs a bit tinkering 
					Vector3 middle = new Vector3();
					usabilityOn = true;
				}
				//Cubes
				else if (rb.CompareTag("Cubi"))
				{
					if (!isMoving(rb))
					{
						Vector3 force = hit_dir * _sliderController.getSlider().value;
						force += Vector3.up * ForceUp;
						Debug.DrawRay(rb.position, force, Color.grey, 3f);
						rb.AddForce(force, ForceMode.Impulse);
						usabilityOn = true;
					}
				}
			}

			previousPos = hit.point; //mouse position in 3D
			/* TODO implement timer that actually counts argh and change color with colorList over time
			if (usabilityOn == false)
			{
				foreach (var sphere in spheres)
				{
					sphere.GetComponent<Renderer>().material.SetColor("_Color", Color.black);
				}
			}*/
			usabilityOn = false;
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
    
}
