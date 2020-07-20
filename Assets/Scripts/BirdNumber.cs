using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class BirdNumber : MonoBehaviour {
    public TextMeshProUGUI birdNumber;
    public TextMeshProUGUI nestsLeft;

    void Start() {
        
    }

    void Update() {
        
    }

    public void SetBirdNumber(int num) {
        birdNumber.SetText("<b>Controlling:</b> Bird " + num);
    }

    public void SetNestsLeft(int num) {
        nestsLeft.SetText("<b>Nests Left:</b>  " + num);
    }
}
