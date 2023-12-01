using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public bool canMove = true;
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 2.0f;

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
            moveV = Input.GetAxisRaw("Vertical") * moveSpeed;
            rb.velocity = new Vector2(moveH, moveV);

            Vector2 direction = new Vector2(moveH, moveV);
            FindObjectOfType<PlayerAnimation>().setDirection(direction);
        }
        else
        {
            rb.velocity = Vector2.zero;
            FindObjectOfType<PlayerAnimation>().setDirection(Vector2.zero);
        }
    }
}
