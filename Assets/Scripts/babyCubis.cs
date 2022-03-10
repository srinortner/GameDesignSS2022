using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class babyCubis : MonoBehaviour
{
    public GameObject cubiPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Wall")
        {
            if (transform.localScale.x != 0.2f)
            {
                Vector3 currentPosition = transform.position;
                Vector3 scale = new Vector3(0.2f, 0.2f, 0.2f);
                GameObject cubi1 = Instantiate(cubiPrefab, new Vector3(currentPosition.x + 1, 3, currentPosition.z + 1),
                    Quaternion.identity);
                cubi1.transform.localScale = scale;
            }
        }
        
        
    }
}
