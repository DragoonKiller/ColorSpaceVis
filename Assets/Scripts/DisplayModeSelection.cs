using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class DisplayModeSelection : MonoBehaviour
{
    public Env env;
    public Toggle modeCylinderToggle;
    public Toggle modeConeToggle;
    public Toggle modeDoubleConeToggle;
    
    ToggleGroup toggles => this.GetComponent<ToggleGroup>();
    
    void Update()
    {
        var toggle = toggles.ActiveToggles().First();
        if(toggle == modeCylinderToggle) env.displayType = DisplayType.Cylinder;
        else if(toggle == modeConeToggle) env.displayType = DisplayType.Cone;
        else if(toggle == modeDoubleConeToggle) env.displayType = DisplayType.DoubleCone;
        else env.displayType = DisplayType.None;
    }
    
}
