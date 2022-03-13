using System;
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
	
	private void Awake () {
		cam = GetComponent<Camera>();
	}
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		RaycastHit hit;
		var ray = GetComponent<Camera>().ScreenPointToRay(Input.mousePosition);

		if (Input.GetKey(KeyCode.D))
		{
			movementLeft = !movementLeft;
			Debug.Log("Switch movement direction");
		}
		if(Physics.Raycast(ray, out hit, 50f) && hit.rigidbody != null){
			hit.rigidbody.AddForce(Vector3.up, ForceMode.Impulse);
			if (movementLeft)
			{
				hit.rigidbody.AddForce(Vector3.left, ForceMode.Impulse);
			}
			else
			{
				hit.rigidbody.AddForce(Vector3.right, ForceMode.Impulse);
			}
			
		}
        
		//can be commented out/removed
		//as long as you hold the right mouse button, you can navigate through the scene
		if(Input.GetMouseButton(1)) {
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
		}
		
		//Zoom in and out with Mouse Wheel
		this.transform.Translate(0, 0, Input.GetAxis("Mouse ScrollWheel") * this.navigationSpeed, Space.Self);
    }
}
