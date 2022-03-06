using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CameraRay : MonoBehaviour
{
	private bool movementLeft = true;
	
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
        
    }
}
