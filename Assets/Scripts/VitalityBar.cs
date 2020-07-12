using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VitalityBar : MonoBehaviour {
    public Slider slider;
    
    public void SetVitality(float vitality) {
        slider.value = vitality;
    }

    public void SetMaxVitality(float maxVitality) {
        slider.maxValue = maxVitality;
        slider.value = maxVitality;
    }
}
