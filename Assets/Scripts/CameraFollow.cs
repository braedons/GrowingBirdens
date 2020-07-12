using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    public GameObject target;
    public Vector2 deadZone = Vector2.zero;
    public float smoothSpeed = 0.3f;
    private Vector3 vel = Vector3.zero;

    // Update is called once per frame
    void FixedUpdate()
    {
        if (target != null && outOfDeadZone()) {
            Vector3 desiredPosition = new Vector3(target.transform.position.x, target.transform.position.y, transform.position.z);
            Vector3 smoothedPosition = Vector3.SmoothDamp(transform.position, desiredPosition, ref vel, smoothSpeed);
            transform.position = smoothedPosition;
        }
    }

    private bool outOfDeadZone() {
        Vector3 tPos = target.transform.position;
        return (Mathf.Abs(tPos.x - transform.position.x) > deadZone.x || Mathf.Abs(tPos.y - transform.position.y) > deadZone.y);
    }

    public void UpdateTarget(GameObject newTarget) {
        target = newTarget;
    }
}
