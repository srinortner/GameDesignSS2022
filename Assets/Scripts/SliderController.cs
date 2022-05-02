using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SliderController : MonoBehaviour
{

    [SerializeField] private Slider forceSlider = null;
    [SerializeField] private Text forceText = null;
    [SerializeField] private Slider forceUpSlider = null;
    [SerializeField] private Text forceUpText = null;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }
    
    public void ForceSlider(float strength)
    {
    //    Debug.Log(strength);
        forceText.text = strength.ToString("0");
    }

    public void ForceUpSlider(float value)
    {
        forceUpText.text = value.ToString("0");
    }

    public Slider getSliderStrength()
    {
        return forceSlider;
    }

    public Slider getForceUp()
    {
        return forceUpSlider;
    }
}
