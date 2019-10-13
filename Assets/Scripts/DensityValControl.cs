using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DensityValControl : MonoBehaviour
{
    public Env env;
    public float maxDensity;
    public float minDensity;
    public Text label;
    
    Slider slider => this.GetComponent<Slider>();
    
    void Update()
    {
        env.densityPointCount = Mathf.RoundToInt(Mathf.Lerp(minDensity, maxDensity, slider.value));
        label.text = "Density: " + env.densityPointCount.ToString();
    }
}
