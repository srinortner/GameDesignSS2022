﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRay : MonoBehaviour
{
	private bool movementLeft = true;
	
	[SerializeField] float navigationSpeed = 2.4f;
	[SerializeField] float shiftMultiplier = 2f;
	[SerializeField] float sensitivity = 1.0f;
 
	private Camera cam;
	private Vector3 anchorPoint;
	private Quaternion anchorRot;
	private Vector3 previousPos;
	private bool usabilityOn = false;
	private float startTime;
	private GameObject[] spheres;

	private void Awake () {
		cam = GetComponent<Camera>();
	}
    // Start is called before the first frame update
    void Start()
    {
	    previousPos = new Vector3();
	    spheres = GameObject.FindGameObjectsWithTag("Sphere");
    }

    private void Update()
    {
	    //Zoom in and out with Mouse Wheel
	    this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.navigationSpeed, Space.Self);
	    //Debug.Log(spheres[0]);
    }

    // Update is called once per frame
    void FixedUpdate()
    {
	    RaycastHit hit;
		
        cam = GetComponent<Camera>();
		//Vector3 mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
		var ray = cam.ScreenPointToRay(Input.mousePosition);

		if(Physics.Raycast(ray, out hit, 60f))
		{
			if (hit.rigidbody != null)
			{
				Rigidbody rb = hit.rigidbody;
				//norm direction = norm(destination - source)
				//Spheres
				if (rb.CompareTag("Sphere"))
				{
					Vector3 force = (rb.transform.position - previousPos).normalized * 3f;
					force += Vector3.up * 1.5f;
					rb.AddForce(force,ForceMode.Impulse); //needs a bit tinkering 
					Vector3 middle = new Vector3();
				}
				//Cubes
				else
				{
					Vector3 force = (rb.transform.position - previousPos).normalized * 3f;
					force += Vector3.up * 1.5f;
					rb.AddForce(force, ForceMode.Impulse);
				}
				
			}
			previousPos = hit.point; //mouse position in 3D
			foreach (var sphere in spheres)
			{
				sphere.GetComponent<Renderer>().material.SetColor("_Color", Color.blue);
			}
		}
		
		//can be commented out/removed
		//as long as you hold the right mouse button, you can navigate through the scene
		/*if(Input.GetMouseButton(1)) {
			Vector3 move = Vector3.zero;
			float speed = navigationSpeed * (Input.GetKey(KeyCode.LeftShift) ? shiftMultiplier : 1f) * Time.deltaTime * 9.1f;
			if(Input.GetKey(KeyCode.W))
				move += Vector3.forward * speed;
			if(Input.GetKey(KeyCode.S))
				move -= Vector3.forward * speed;
			if(Input.GetKey(KeyCode.F))
				move += Vector3.right * speed;
			if(Input.GetKey(KeyCode.A))
				move -= Vector3.right * speed;
			if(Input.GetKey(KeyCode.E))
				move += Vector3.up * speed;
			if(Input.GetKey(KeyCode.Q))
				move -= Vector3.up * speed;
			transform.Translate(move);
		}
 
		if(Input.GetMouseButtonDown(1)) {
			anchorPoint = new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
			anchorRot = transform.rotation;
		}
		if(Input.GetMouseButton(1)) {
			Quaternion rot = anchorRot;
 
			Vector3 dif = anchorPoint - new Vector3(Input.mousePosition.y, -Input.mousePosition.x);
			rot.eulerAngles += dif * sensitivity;
			transform.rotation = rot;
		}*/
		
		
    }
}
