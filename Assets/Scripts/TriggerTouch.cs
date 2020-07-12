using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerTouch : MonoBehaviour {
    public string targetTag;
    private int targetsTouched = 0;
    void Start() {
        
    }

    void Update() {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == targetTag) {
            targetsTouched++;
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.gameObject.tag == targetTag) {
            targetsTouched--;
        }
    }

    public int GetTargetsTouched() {
        return targetsTouched;
    }
}
