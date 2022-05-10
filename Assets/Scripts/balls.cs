using System;
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
    private int _count;
    private ParticleSystem.Particle[] _particles;
    public GameObject cubiPrefab;
    public Transform middle;
    private Vector3 startPosition;
    public GameObject audioController;
    private AudioManager _audioManager;
    
    
    // Start is called before the first frame update
    void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
        orginalPosition = this.transform.position;
        _particles = new ParticleSystem.Particle[1000]; //initialize variable because else NullReference
        _count = 0;
        startPosition = GetComponent<Transform>().position;
    }

    // Update is called once per frame
    void Update()
    {
        Transform tr = GetComponent<Transform>();
        Rigidbody rb = GetComponent<Rigidbody>();
        if (tr.position != startPosition && (tr.position.x > 5f || tr.position.x < -5f) && (tr.position.z > 5f || tr.position.z < -5f))
        {
            tr.LookAt(middle);
            float power = 0.3f;
            rb.AddRelativeForce(0, 0, power);  
        }
        
        
        
        if (this.transform.position.x > 25.0f || this.transform.position.x < -25.0f)
        {
            this.transform.position = orginalPosition;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        
        ParticleSystem.MainModule settings = GetComponent<ParticleSystem>().main;
        ParticleSystem.MinMaxGradient particleColor = settings.startColor;
        ParticleSystem.MinMaxGradient noColor = new ParticleSystem.MinMaxGradient(Color.white);
        if (other.collider.tag == "Wall" && !particleColor.color.Equals(Color.white))
        {
            float cubi = Random.Range(0.0f,1.0f);
       //     Debug.Log(cubi);
            Vector3 currentPosition = gameObject.transform.position;
   
            // cubiPrefab.GetComponent<Renderer>().sharedMaterial.color = c;
            cubiRain(particleColor.color, currentPosition);

            
            settings.startColor = particleColor;
            _particles = new ParticleSystem.Particle[settings.maxParticles]; // generate array of particles
            _amount = _particleSystem.GetParticles(_particles); // get int value of all particles

            this.GetComponent<Rigidbody>().AddForce(Vector3.right,ForceMode.Impulse);
          //  Debug.Log("color is set");
          _count++;
          if (_count >= 6)
          {
              //reset count variable
              _count = 0;
          }
        }

        if (other.collider.tag == "Platform")
        {
            Color c = other.gameObject.GetComponent<Renderer>().material.color;
            Vector3 currentPosition = gameObject.transform.position;
   
            // cubiPrefab.GetComponent<Renderer>().sharedMaterial.color = c;
            cubiRain(c, currentPosition);
            
            settings.startColor = new ParticleSystem.MinMaxGradient(c);
            _particles = new ParticleSystem.Particle[settings.maxParticles]; // generate array of particles
            _amount = _particleSystem.GetParticles(_particles); // get int value of all particles
            GetComponent<MeshRenderer>().material.color = c;

            this.GetComponent<Rigidbody>().AddForce(Vector3.right,ForceMode.Impulse);
            //  Debug.Log("color is set");
            //TODO
            _count++;
            if (_count >= 6)
            {
                //reset count variable
                _count = 0;
            }
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

        if (_count == 0)
        {
            //when generating cubes from particles weird side effect happening when spawning from "white", so ugly workaround
            //TODO @Viktoria-1 look into this issue
            GameObject cubi1 = Instantiate(cubiPrefab, new Vector3(currentPosition.x, currentPosition.y + 10f, currentPosition.z + 0), Quaternion.identity); 
            cubi1.GetComponent<Renderer>().material.color = c;
            cubi1.GetComponent<CubiController>().audioController = audioController;
       /*     GameObject cubi2 = Instantiate(cubiPrefab, new Vector3(currentPosition.x + 3, currentPosition.y + 10f, currentPosition.z - 3), Quaternion.identity);
            cubi2.GetComponent<Renderer>().material.color = c;
            cubi2.GetComponent<CubiController>().audioController = audioController;
            GameObject cubi3 = Instantiate(cubiPrefab, new Vector3(currentPosition.x + 1, currentPosition.y + 10f, currentPosition.z + 3), Quaternion.identity);
            cubi3.GetComponent<Renderer>().material.color = c;
            cubi3.GetComponent<Renderer>().material.color = c;
            GameObject cubi4 = Instantiate(cubiPrefab, new Vector3(currentPosition.x - 3, currentPosition.y + 10f, currentPosition.z + 2), Quaternion.identity);
            cubi4.GetComponent<Renderer>().material.color = c;
            cubi4.GetComponent<CubiController>().audioController = audioController;
            GameObject cubi5 = Instantiate(cubiPrefab, new Vector3(currentPosition.x, currentPosition.y + 10f, currentPosition.z + 3), Quaternion.identity);
            cubi5.GetComponent<Renderer>().material.color = c;
            cubi5.GetComponent<CubiController>().audioController = audioController;*/
        }
        else
        {
            GameObject cubi1 = Instantiate(cubiPrefab, new Vector3(list[0].x, list[0].y + 1, list[0].z + 0),
                Quaternion.identity);
            cubi1.GetComponent<Renderer>().material.color = c;
            cubi1.GetComponent<CubiController>().audioController = audioController;
         /*   GameObject cubi2 = Instantiate(cubiPrefab, new Vector3(list[1].x, list[1].y + 1, list[1].z + 0),
                Quaternion.identity);
            cubi2.GetComponent<Renderer>().material.color = c;
            cubi2.GetComponent<CubiController>().audioController = audioController;
            GameObject cubi3 = Instantiate(cubiPrefab, new Vector3(list[2].x, list[2].y + 1, list[2].z + 0),
                Quaternion.identity);
            cubi3.GetComponent<Renderer>().material.color = c;
            cubi3.GetComponent<CubiController>().audioController = audioController;
            GameObject cubi4 = Instantiate(cubiPrefab, new Vector3(list[3].x, list[3].y + 1, list[3].z + 0),
                Quaternion.identity);
            cubi4.GetComponent<Renderer>().material.color = c;
            cubi4.GetComponent<CubiController>().audioController = audioController;
            GameObject cubi5 = Instantiate(cubiPrefab, new Vector3(list[4].x, list[4].y + 1, list[4].z + 0),
                Quaternion.identity);
            cubi5.GetComponent<Renderer>().material.color = c;
            cubi5.GetComponent<CubiController>().audioController = audioController;*/
        }

    }
    
    
    private Color generateRandomColor(int count)
    {
        Color[] colors = {Color.blue, Color.cyan, Color.green, Color.magenta, Color.red, Color.yellow,};

        //   Debug.Log(colors[random].ToString());
        return colors[count];

    }
    
}
