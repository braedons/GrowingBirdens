using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crafting : MonoBehaviour {
    private Dictionary<string, Dictionary<string, int>> recipes;
    
    void Start() {
        recipes = new Dictionary<string, Dictionary<string, int>>();
        recipes.Add("Nest", new Dictionary<string, int>());
        recipes["Nest"].Add("Stick", 5);
    }

    void Update() {
        
    }

    public bool BuildItem(Inventory inv, string item) {
        foreach (KeyValuePair<string, int> ingredient in recipes[item]) {
            if (inv.GetQuantity(ingredient.Key) < ingredient.Value) {
                return false;
            }
        }

        foreach (KeyValuePair<string, int> ingredient in recipes[item]) {
            inv.EditQuantity(ingredient.Key, -ingredient.Value);
        }
        return true;
    }
}
