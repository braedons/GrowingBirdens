using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BirdNumber : MonoBehaviour {
    public TextMeshProUGUI text;

    void Start() {
        
    }

    void Update() {
        
    }

    public void SetBirdNumber(int num) {
        text.SetText("<b>Controlling:</b> Bird " + num);
    }
}
