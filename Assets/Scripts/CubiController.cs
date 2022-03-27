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
            Vector2 direction = collision.GetContact(0).normal;
         /*   If( direction.x == 1 ) print(“right”);
            If( direction.x == -1 ) print(“left”);
            If( direction.y == 1 ) print(“up”);
            If( direction.y == -1 ) print(“down”); */
            
            Color currentColor = GetComponent<Renderer>().material.color;
            if (currentColor == collision.gameObject.GetComponent<Renderer>().material.color && checkDirection(currentColor, direction))
            {
                GetComponent<Rigidbody>().useGravity = true;
                GetComponent<Rigidbody>().isKinematic = true;
                GetComponent<Transform>().rotation = Quaternion.identity;
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

                PlatformController currentController = collision.gameObject.GetComponent<PlatformController>();
                currentController.increaseHouseCounter();
                currentController.addHouse(GetComponent<Transform>());
                
                if (currentController.getHouseCounter() == 3)
                {
                    //GetComponent<MeshRenderer>().enabled = false;
                    currentController.activateHouses();
                    light.intensity = 10;
                    var lightTransform = light.transform;
                    var lightPosition = lightTransform.position;
                    lightPosition = new Vector3(lightPosition.x, lightPosition.y - 0.5f, lightPosition.z);
                    lightTransform.position = lightPosition; 
                } else if (currentController.getHouseCounter() >= 3)
                {
                    GetComponent<MeshRenderer>().enabled = false;
                    light.intensity = 10;
                    var lightTransform = light.transform;
                    var lightPosition = lightTransform.position;
                    lightPosition = new Vector3(lightPosition.x, lightPosition.y - 0.5f, lightPosition.z);
                    lightTransform.position = lightPosition; 
                }
            }
        }
    }
    
    private bool checkDirection(Color color, Vector2 direction)
    {
        if (color == Color.cyan || color == Color.red)
        {
            if (direction.x == -1)
            {
                return true;
            }
        }
        else
        {
            if (direction.y == 1)
            {
                return true;
            }
        }

        return false;
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