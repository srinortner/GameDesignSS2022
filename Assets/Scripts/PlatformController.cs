using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private int houseCounter;
    private List<Transform> children;

    private List<Transform> houses;
    // Start is called before the first frame update
    void Start()
    {
        houseCounter = 0;
        houses = new List<Transform>();
        children = GetAllChildren(GetComponent<Transform>());
    }

    // Update is called once per frame
    void Update()
    {
        
        if (houseCounter == 5)
        {
            print(children.ToString());
            foreach (var child in children)
            {
                if (child.CompareTag("Street"))
                {
                    print("found Child with tag street");
                    child.GetComponentInParent<MeshRenderer>().enabled = true;
                }
                    
            }
        }

        if (houseCounter == 8)
        {
            foreach (var child in children)
            {
                if (child.CompareTag("StreetLight"))
                {
                    print("found Child with tag streetLight");
                    child.GetComponentInParent<MeshRenderer>().enabled = true;
                }
                    
            }
        }
        
        if (houseCounter == 12)
        {
            foreach (var child in children)
            {
                if (child.CompareTag("Car"))
                {
                    print("found Child with tag car");
                    List<Transform> carparts = GetAllChildren(child.GetComponent<Transform>());
                    foreach (var part in carparts)
                    {
                        part.GetComponentInParent<MeshRenderer>().enabled = true;
                    }
                    
                }
                    
            }
        }
    }

    public void increaseHouseCounter()
    {
        houseCounter++;
        print(houseCounter);
    }

    public void addHouse(Transform house)
    {
        houses.Add(house);
    }

    public void activateHouses()
    {
        foreach (var house in houses)
        {
            house.GetComponent<MeshRenderer>().enabled = false;
        }
        
    }

    public int getHouseCounter()
    {
        return houseCounter;
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
