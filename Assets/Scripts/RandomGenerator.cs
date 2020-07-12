using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomGenerator : MonoBehaviour {
    public GameObject[] objects;
    public Vector3[] positions;
    public GameObject parent;
    public bool generateOnStart = false;

    private void Awake() {
        if (generateOnStart) Generate();
    }
    public void Generate() {
        if (positions.Length == 0) {
            positions = new Vector3[1];
            positions[0] = transform.position;
        }

        foreach (Vector3 pos in positions) {
            int rand = Random.Range(0, objects.Length);
            if (objects[rand] != null) {
                GameObject gm = Instantiate(objects[rand], pos, Quaternion.identity);

        
                if (parent == null)
                    parent = gameObject;
                gm.transform.SetParent(parent.transform);
            }
        }
    }
}
