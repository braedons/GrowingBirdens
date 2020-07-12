using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIMovement : MonoBehaviour
{
    public Transform startPoint;
    public Transform groundCheck;
    public Transform gapCheck;
    public Transform wallCheck;
    public Transform wallCheckShort;

    public Transform descentCheck;
    public Transform jumpCheckStart;
    public Transform jumpCheckEnd;
    public float jumpForce = 40f;
    public LayerMask layer;
    private bool isGrounded;

    public float speed = 30f;
    private Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        rb.velocity = new Vector2(transform.localScale.x * speed, rb.velocity.y);

        isGrounded = (Physics2D.Linecast(startPoint.position, groundCheck.position, layer));

        // Avoid gaps
        if (!Physics2D.Linecast(startPoint.position, gapCheck.position, layer))
        {
            if (Physics2D.Linecast(startPoint.position, wallCheck.position, layer) && !Physics2D.Linecast(jumpCheckStart.position, jumpCheckEnd.position, layer) && isGrounded) {
                // Jump
                rb.velocity = new Vector2(rb.velocity.x*4, jumpForce);
                isGrounded = false;
            }
            else if (Physics2D.Linecast(startPoint.position, descentCheck.position, layer) && isGrounded) {
                // Descend
                rb.velocity = new Vector2(rb.velocity.x, jumpForce * .75f);
            }
            else if (isGrounded) {
                // Turn around
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }

        // Avoid walls
        if (Physics2D.Linecast(startPoint.position, wallCheckShort.position, layer))
        {
            if (!Physics2D.Linecast(jumpCheckStart.position, jumpCheckEnd.position, layer) && isGrounded) {
                // Jump
                rb.velocity = new Vector2(rb.velocity.x*4, jumpForce*.75f);
                isGrounded = false;
            }
            else if (isGrounded) {
                // Turn around
                transform.localScale = new Vector3(transform.localScale.x * -1, transform.localScale.y, transform.localScale.z);
            }
        }
    }
}
