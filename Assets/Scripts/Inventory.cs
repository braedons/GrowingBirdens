using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    private TextMeshProUGUI countText;
    private PlayerSound playerSound;

    // Start is called before the first frame update
    void Awake()
    {
        countText = GameObject.FindGameObjectWithTag("Inventory").GetComponent<TextMeshProUGUI>();
        playerSound = GetComponent<PlayerSound>();
    }

    public void EditQuantity(string item, int change)
    {
        if (change > 0)
            playerSound.Pickup();

        if (!items.ContainsKey(item))
        {
            items.Add(item, change);
        }
        else
        {
            items[item] += change;
        }

        if (item == "Stick") {
            int count;
            items.TryGetValue(item, out count);
            countText.SetText(count.ToString());
        }
    }

    public int GetQuantity(string item) {
        int count;
        items.TryGetValue(item, out count);
        return count;
    }

    public void ResetInventory() {
        items = new Dictionary<string, int>();
    }
}
