using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Vitality : MonoBehaviour
{
    public float maxHealth = 10f;
    private float currHealth;
    public float regenRate = 0f;

    public float maxSatiety = 10f;
    private float currSatiety;
    public float hungerRate = .1f;
    private bool hungerFrozen = false;

    private VitalityBar healthBar;
    private VitalityBar hungerBar;

    private PlayerController controller;

    // For flashing white when damaged
    SpriteRenderer sr;
    public Material matWhite;
    Material matDefault;
    private PlayerSound playerSound;

    public enum Stat
    {
        Health,
        Satiety
    }
    
    // Start is called before the first frame update
    void Awake()
    {
        currHealth = maxHealth;
        healthBar = GameObject.FindGameObjectWithTag("HealthBar").GetComponent<VitalityBar>();
        
        currSatiety = maxSatiety;
        hungerBar = GameObject.FindGameObjectWithTag("HungerBar").GetComponent<VitalityBar>();

        controller = GetComponent<PlayerController>();

        playerSound = GetComponent<PlayerSound>();
    }

    private void Start() {
        sr = GetComponent<SpriteRenderer>();
        matDefault = sr.material;
    }

    // Update is called once per frame
    void Update()
    {
        if (!hungerFrozen)
            ChangeVitality(Stat.Satiety, -(hungerRate * Time.deltaTime));
    }

    public void ChangeVitality(Stat targetStat, float change)
    {
        switch (targetStat)
        {
            // Check isActiveAndEnabled because an enabled controller indicates a current player (should be affecting the HUD)
            case Stat.Health:
                currHealth = CheckVitality(currHealth + change, maxHealth);
                if (controller.IsActive()) healthBar.SetVitality(currHealth);
                CheckVitality(currHealth, maxHealth);

                if (change < 0) {
                    // Damage Sound
                    playerSound.Hurt();

                    // Flash White
                    sr.material = matWhite;
                    Invoke("ResetMaterial", .1f);
                }
                break;
            case Stat.Satiety:
                currSatiety = CheckVitality(currSatiety + change, maxSatiety);
                if (controller.IsActive()) hungerBar.SetVitality(currSatiety);
                CheckVitality(currSatiety, maxSatiety);
                break;
        }
    }

    public float GetVitality(Stat targetStat) {
        switch (targetStat) {
            case Stat.Health:
                return currHealth;
            case Stat.Satiety:
                return currSatiety;
        }

        return -1;
    }

    public float GetMaxVitality(Stat targetStat) {
        switch (targetStat) {
            case Stat.Health:
                return maxHealth;
            case Stat.Satiety:
                return maxSatiety;
        }

        return -1;
    }

    private float CheckVitality(float curr, float max)
    {
        if (curr < 0)
        {
            Kill();
            return 0;
        }
        else if (curr > max)
            return max;
        else
            return curr;
    }

    private void Kill()
    {
        gameObject.GetComponent<PlayerController>().GameOver();
    }

    public void ResetVitality() {
        currHealth = maxHealth;
        currSatiety = maxSatiety;
        
        hungerFrozen = false;
    }

    private void ResetMaterial() {
        sr.material = matDefault;
    }

    public void FreezeHunger(bool frozen) {
        hungerFrozen = frozen;
    }
}
