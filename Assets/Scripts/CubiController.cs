using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CubiController : MonoBehaviour
{
    public GameObject audioController;
    private bool isHouse;

    private AudioManager _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.FindWithTag("AudioManager");
        GetComponent<TrailRenderer>().startColor = GetComponent<Renderer>().material.color;
        GetComponent<TrailRenderer>().endColor = GetComponent<Renderer>().material.color;
        _audioManager = audioController.GetComponent<AudioManager>();
        isHouse = false;
    }

    // Update is called once per frame
    void Update()
    {
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Platform") && !isHouse) {
            /*   If( direction.x == 1 ) print(“right”);
            If( direction.x == -1 ) print(“left”);
            If( direction.y == 1 ) print(“up”);
            If( direction.y == -1 ) print(“down”); */
            Vector2 direction = collision.GetContact(0).normal;
            Color currentColor = GetComponent<Renderer>().material.color;
           // print("Collision Color: " + collision.gameObject.GetComponent<Renderer>().material.color + " Current Color: " + currentColor + " direction: " + direction);
           
            if (currentColor == collision.gameObject.GetComponent<Renderer>().material.color && checkDirection(currentColor, direction))
            {
                
                changeCubeToHouse(currentColor);
                PlatformController currentController = collision.gameObject.GetComponent<PlatformController>();
                currentController.increaseHouseCounter();
                currentController.addHouse(GetComponent<Transform>());
                isHouse = true;
                print("HERE");
                Light light = GetComponentInChildren<Light>();          
                if (currentController.getHouseCounter() == 3) {
                    currentController.activateHouses();
                    light.intensity = 10;
                } else if (currentController.getHouseCounter() >= 3) {
                    GetComponent<MeshRenderer>().enabled = false;
                    light.intensity = 10;
                }

            }
        }

        else if (collision.gameObject.CompareTag("Cubi") &&
            collision.gameObject.GetComponent<CubiController>().isCubeHouse())
        {
            Color currentColor = GetComponent<Renderer>().material.color;
            if (currentColor == collision.gameObject.GetComponent<Renderer>().material.color)
            {
                //TODO: only on top of eachother
                changeCubeToHouse(currentColor);
                print("HERE");
                isHouse = true;
                GetComponent<MeshRenderer>().enabled = false;
            }
            
            
        }
    }

    private void changeCubeToHouse(Color currentColor)
    {
        GetComponent<Rigidbody>().useGravity = true;
        GetComponent<Rigidbody>().isKinematic = true;
                
        GetComponent<Transform>().rotation = Quaternion.identity;
        List <Transform> children = GetAllChildren(GetComponent<Transform>());
       
        _audioManager.Play("addCube");
        foreach (var child in children)
        {
            if (child.CompareTag("House"))
            {
                float change = 0.2f;
                float r = currentColor.r >= 0.5f ? currentColor.r - change : currentColor.r + change;
                float g = currentColor.g >= 0.5f ? currentColor.g - change : currentColor.g + change;
                float b = currentColor.b >= 0.5f ? currentColor.b - change : currentColor.b + change;
                Color darker = new Color(r, g, b);
                //    print(currentColor);
                //    print(darker);
                child.GetComponent<Renderer>().material.color = darker;
                child.transform.position =
                    new Vector3(child.position.x, child.position.y - 0.01f, child.position.z);

            }
        }


    }
    
    private bool checkDirection(Color color, Vector2 direction)
    {
        if (color == Color.cyan || color == Color.red)
        {
            if (direction.x == -1f)
            {
                return true;
            }
        }
        else
        {
         //   print("ELSE");
       //     print(direction);
            if (direction.y == 1f)
            {
         //      print("TRUE");
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

    public bool isCubeHouse()
    {
        return isHouse;
    }
}