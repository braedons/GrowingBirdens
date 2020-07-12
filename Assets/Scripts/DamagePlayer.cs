using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayer : MonoBehaviour {
    public string targetTag;
    public float damageAmount = .4f;
    public float damageRate = 1f;
    private float nextDamageTime = 0f;
    
    void Start() {
        
    }

    void Update() {
        
    }

    private void OnCollisionStay2D(Collision2D other) {
        if (other.gameObject.tag == targetTag && Time.time > nextDamageTime) {
            other.gameObject.GetComponent<Vitality>().ChangeVitality(Vitality.Stat.Health, -damageAmount);
            nextDamageTime = Time.time + 1f / damageRate;
        }
    }
}
