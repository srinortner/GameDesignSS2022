using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;

public class CubiController : MonoBehaviour
{
    public GameObject audioController;
    private bool isHouse;
    private bool isGroundFloor;
    private bool isHighestFloor;
    private bool isMagnetic;

    private AudioManager _audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioController = GameObject.FindWithTag("AudioManager");
        GetComponent<TrailRenderer>().startColor = GetComponent<Renderer>().material.color;
        GetComponent<TrailRenderer>().endColor = GetComponent<Renderer>().material.color;
        _audioManager = audioController.GetComponent<AudioManager>();
        isHouse = false;
        isGroundFloor = false;
        isMagnetic = false;
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
           
            if (currentColor == collision.gameObject.GetComponent<Renderer>().material.color && checkDirection(currentColor, direction))
            {
                
                changeCubeToHouse(currentColor);
                PlatformController currentController = collision.gameObject.GetComponent<PlatformController>();
                currentController.increaseHouseCounter();
                currentController.addHouse(GetComponent<Transform>());
                isHouse = true;
                isGroundFloor = true;
                isHighestFloor = true;
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

        else
        {
            CubiController cubiController = collision.gameObject.GetComponent<CubiController>();
            if (collision.gameObject.CompareTag("Cubi") &&
                cubiController.isCubeHouse() && !isGroundFloor && cubiController.isCubeHighestFloor())
            {
                Color currentColor = GetComponent<Renderer>().material.color;
                if (currentColor == collision.gameObject.GetComponent<Renderer>().material.color)
                {
                    changeCubeToHouse(currentColor);
                    GetComponent<MeshRenderer>().enabled = false;
                    var collidingPos = collision.gameObject.transform.position;
                    var collidingScale = collision.gameObject.transform.localScale;
                    if (currentColor == Color.red || currentColor == Color.cyan)
                    {
                        transform.position = new Vector3(collidingPos.x-collidingScale.y,collidingPos.y, collidingPos.z);
                    }
                    else
                    {
                        transform.position = new Vector3(collidingPos.x,collidingPos.y + collidingScale.y, collidingPos.z);
                    }
                    
                    isHouse = true;
                    isGroundFloor = true;
                    isHighestFloor = true;
                    cubiController.SetIsHighestFloor(false);
                }
            
            
            }
        }

        if (collision.gameObject.CompareTag("Magnet"))
        {
            isMagnetic = true;
        }
    }

    public bool isInMagneticField()
    {
        return isMagnetic;
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

    public bool isCubeGroundFloor()
    {
        return isGroundFloor;
    }

    public void SetIsHighestFloor(bool value)
    {
        isHighestFloor = value;
    }

    public bool isCubeHighestFloor()
    {
        return isHighestFloor;
    }
}