using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class StatisticModeSelection : MonoBehaviour
{
    public Env env;
    public Toggle toggleOriginMode;
    public Toggle toggleDensityMode;
    ToggleGroup toggleGroup => this.GetComponent<ToggleGroup>();
    
    void Update()
    {
        var curSelection = toggleGroup.ActiveToggles().First();
        if(curSelection == toggleOriginMode) env.statisticType = StatisticType.Origin;
        else if(curSelection == toggleDensityMode) env.statisticType = StatisticType.Density;
        else env.statisticType = StatisticType.None;
    }
}
