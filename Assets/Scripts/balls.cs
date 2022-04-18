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
    private int _amount;
    private ParticleSystem.Particle[] _particles;
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
        _particles = new ParticleSystem.Particle[1000];
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
            _particles = new ParticleSystem.Particle[settings.maxParticles];
            _amount = _particleSystem.GetParticles(_particles);

            this.GetComponent<Rigidbody>().AddForce(Vector3.right,ForceMode.Impulse);
          //  Debug.Log("color is set");
        }

        
    }

    private void cubiRain(Color c, Vector3 currentPosition)
    {

        Vector3[] list = new [] {new Vector3(0f,0f,0f), 
                                new Vector3(0f,0f,0f), 
                                new Vector3(0f,0f,0f), 
                                new Vector3(0f,0f,0f),
                                new Vector3(0f,0f,0f)};
        for (int i = 0; i < 5; i++)
        {
            int random = Random.Range(0, _amount);
            //Debug.Log(_particles);
            list[i] = _particles[random].position;
        }
        //Debug.Log("List values are: " + list);
        
        GameObject cubi1 = Instantiate(cubiPrefab, new Vector3( list[0].x, list[0].y + 1, list[0].z + 0), Quaternion.identity); 
        cubi1.GetComponent<Renderer>().material.color = c;
        GameObject cubi2 = Instantiate(cubiPrefab, new Vector3( list[1].x, list[1].y + 1, list[1].z + 0), Quaternion.identity);
        cubi2.GetComponent<Renderer>().material.color = c;
        GameObject cubi3 = Instantiate(cubiPrefab, new Vector3( list[2].x, list[2].y + 1, list[2].z + 0), Quaternion.identity);
        cubi3.GetComponent<Renderer>().material.color = c;
        GameObject cubi4 = Instantiate(cubiPrefab, new Vector3( list[3].x, list[3].y + 1, list[3].z + 0), Quaternion.identity);
        cubi4.GetComponent<Renderer>().material.color = c;
        GameObject cubi5 = Instantiate(cubiPrefab, new Vector3( list[4].x, list[4].y + 1, list[4].z + 0), Quaternion.identity);
        cubi5.GetComponent<Renderer>().material.color = c;

    }
    
    
    private Color generateRandomColor()
    {
        Color[] colors = {Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow,};

        int random = Random.Range(0,6);
        
     //   Debug.Log(colors[random].ToString());
        return colors[random];

    }
}
