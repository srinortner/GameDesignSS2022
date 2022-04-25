using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    [SerializeField] private Slider forceSlider = null;
    [SerializeField] private Text forceText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void ForceSlider(float strength)
    {
        Debug.Log(strength);
        forceText.text = strength.ToString("0");
    }

    public Slider getSlider()
    {
        return forceSlider;
    }
}
