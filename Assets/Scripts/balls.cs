﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
using Random = UnityEngine.Random;

public class balls : MonoBehaviour
{
    private ParticleSystem _particleSystem;
    private Vector3 orginalPosition;
    public GameObject cubiPrefab;
    public Transform platformYellow;
    public Transform platformRed;
    public Transform platformBlue;
    public Transform platformMagenta;
    public Transform platformCyan;
    public Transform platformGreen;
    
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        orginalPosition = this.transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.transform.position.x > 25.0f || this.transform.position.x < -25.0f)
        {
            this.transform.position = orginalPosition;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        if (other.collider.tag == "Wall")
        {
            float cubi = Random.Range(0.0f,1.0f);
            Debug.Log(cubi);
            Color c= generateRandomColor();
            Vector3 currentPosition = gameObject.transform.position;

                
               // cubiPrefab.GetComponent<Renderer>().sharedMaterial.color = c;
                cubiRain(c, currentPosition);

            ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;
            settings.startColor = new ParticleSystem.MinMaxGradient(c);
            this.GetComponent<Rigidbody>().AddForce(Vector3.right,ForceMode.Impulse);
          //  Debug.Log("color is set");
        }

        
    }

    private void cubiRain(Color c, Vector3 currentPosition)
    {
        Vector3 wantedPosition = new Vector3(0, 0, 0);
        if (c.Equals(Color.blue))
        {
            wantedPosition = new Vector3(platformBlue.position.x, 0,
                platformBlue.position.z);
        } else if (c.Equals(Color.green))
        {
            wantedPosition = new Vector3(platformGreen.position.x, 0,
                platformGreen.position.z);
        } else if (c.Equals(Color.red))
        {
            wantedPosition = new Vector3(platformRed.position.x, 0,
                platformRed.position.z);
        } else if (c.Equals(Color.magenta))
        {
            wantedPosition = new Vector3(platformMagenta.position.x, 0,
                platformMagenta.position.z);
        }
        else if (c.Equals(Color.cyan))
        {
            wantedPosition = new Vector3(platformCyan.position.x, 0,
                platformCyan.position.z);
        }
        else
        {
            //Color.yellow
            wantedPosition = new Vector3(platformYellow.position.x, 0,
                platformYellow.position.z);
        }

        GameObject cubi1 = Instantiate(cubiPrefab, new Vector3( 0, 3, wantedPosition.z + 0), Quaternion.identity);
        cubi1.GetComponent<Renderer>().material.color = c;
        GameObject cubi2 = Instantiate(cubiPrefab, new Vector3(3, 3, wantedPosition.z + 0), Quaternion.identity);
        cubi2.GetComponent<Renderer>().material.color = c;
        GameObject cubi3 = Instantiate(cubiPrefab, new Vector3(-3, 3, wantedPosition.z + 0), Quaternion.identity);
        cubi3.GetComponent<Renderer>().material.color = c;
        GameObject cubi4 = Instantiate(cubiPrefab, new Vector3(0, 3, wantedPosition.z - 3), Quaternion.identity);
        cubi4.GetComponent<Renderer>().material.color = c;
        GameObject cubi5 = Instantiate(cubiPrefab, new Vector3(0, 3, wantedPosition.z + 3), Quaternion.identity);
        cubi5.GetComponent<Renderer>().material.color = c;

    }
    
    
    private Color generateRandomColor()
    {
        Color[] colors = {Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow,};

        int random = Random.Range(0,5);
        
     //   Debug.Log(colors[random].ToString());
        return colors[random];

    }
}
