using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CubiController : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform"))
        {
            Color currentColor = GetComponent<Renderer>().material.color;
            if (currentColor == collision.gameObject.GetComponent<Renderer>().material.color)
            {
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = true;
                List <Transform> children = GetAllChildren(GetComponent<Transform>());
                Light light = GetComponentInChildren<Light>();
                foreach (var child in children)
                {
                    if (child.CompareTag("House"))
                    {
                        child.GetComponent<Renderer>().material.color = currentColor;
                        child.transform.position = new Vector3(child.position.x, child.position.y - 0.01f, child.position.z);
                    }
                    
                }
                GetComponent<MeshRenderer>().enabled = false;
                light.intensity = 10;
                var lightTransform = light.transform;
                var lightPosition = lightTransform.position;
                lightPosition = new Vector3(lightPosition.x, lightPosition.y - 0.2f, lightPosition.z);
                lightTransform.position = lightPosition;
            }
        }
    }
    
    static List<Transform> GetAllChildren(Transform parent, List<Transform> transformList = null)
    {
        if (transformList == null) transformList = new List<Transform>();
          
        foreach (Transform child in parent) {
            transformList.Add(child);
            GetAllChildren(child, transformList);
        }
        return transformList;
    }
}