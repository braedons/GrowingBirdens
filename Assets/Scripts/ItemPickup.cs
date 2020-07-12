using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public string actorTag;
    public Vitality.Stat vitalityStat = Vitality.Stat.Health;
    public float vitalityStatChange = 0f;
    public bool addToInventory = true;

    private bool pickedUp = false;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.gameObject.tag == actorTag && !pickedUp)
            Pickup(other.gameObject);
    }

    void Pickup(GameObject obj)
    {
        Animator animator = obj.GetComponent<Animator>();
        if (animator != null)
            animator.SetTrigger("Eat");
            
        pickedUp = true;

        if (addToInventory)
            obj.GetComponent<Inventory>().EditQuantity(gameObject.tag, 1);
        obj.GetComponent<Vitality>().ChangeVitality(vitalityStat, vitalityStatChange);

        Destroy(gameObject);
    }
}
