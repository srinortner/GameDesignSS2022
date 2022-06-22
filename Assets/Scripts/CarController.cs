using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarController : MonoBehaviour
{
    public Transform end;
    public Transform start;
    private Transform target;

    public float speed;
    // Start is called before the first frame update
    void Start()
    {
        target = end;
    }

    // Update is called once per frame
    void Update()
    {
        var step =  speed * Time.deltaTime; // calculate distance to move
        transform.position = Vector3.MoveTowards(transform.position, target.position, step);
        
        if (Vector3.Distance(transform.position, end.position) < 0.001f)
        {
            // Swap the position of the cylinder.
            transform.Rotate(0,180,0);
            target = start;
        }  
        else if (Vector3.Distance(transform.position, start.position) < 0.001f)
        {
            target = end;
            transform.Rotate(0,180,0);
        }

    }
}
