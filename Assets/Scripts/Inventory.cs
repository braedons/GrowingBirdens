using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Inventory : MonoBehaviour
{
    private Dictionary<string, int> items = new Dictionary<string, int>();

    private TextMeshProUGUI countText;

    // Start is called before the first frame update
    void Awake()
    {
        countText = GameObject.FindGameObjectWithTag("Inventory").GetComponent<TextMeshProUGUI>();
    }

    public void EditQuantity(string item, int change)
    {
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
        Debug.Log(items.Count);
    }
}
