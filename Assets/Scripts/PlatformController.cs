using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformController : MonoBehaviour
{
    private int houseCounter;

    private List<Transform> houses;
    // Start is called before the first frame update
    void Start()
    {
        houseCounter = 0;
        houses = new List<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        if (houseCounter == 5)
        {
            List<Transform> children = GetAllChildren(GetComponent<Transform>());
            foreach (var child in children)
            {
                if (child.CompareTag("Street"))
                {
                    child.GetComponentInParent<GameObject>().SetActive(true);
                }
                    
            }
        }
    }

    public void increaseHouseCounter()
    {
        houseCounter++;
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
    
    static List<Transform> GetAllChildren(Transform parent)
    {
        List<Transform> transformList = new List<Transform>();
          
        foreach (Transform child in parent) {
            transformList.Add(child);
            GetAllChildren(child);
        }
        return transformList;
    }
    
}
