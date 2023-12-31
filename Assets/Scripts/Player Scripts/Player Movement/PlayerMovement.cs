using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private bool canMove = true;
    private Rigidbody2D rb;
    private float moveH, moveV;
    [SerializeField] private float moveSpeed = 2.0f;
    [SerializeField] private GameObject arenaGate;

    public void InitBattle()
    {
        canMove = false;
    }

    public void BattleOver()
    {
        canMove = true;
    }

    private void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void FixedUpdate()
    {
        if (canMove)
        {
            arenaGate.SetActive(false);
            moveH = Input.GetAxisRaw("Horizontal") * moveSpeed;
            moveV = Input.GetAxisRaw("Vertical") * moveSpeed;
            rb.velocity = new Vector2(moveH, moveV);

            Vector2 direction = new Vector2(moveH, moveV);
            FindObjectOfType<PlayerAnimation>().setDirection(direction);
        }
        else if (rb.velocity.magnitude != 0)
        {
            if (rb.velocity.magnitude < 0.1f)
            {
                rb.velocity = Vector2.zero;
                FindObjectOfType<PlayerAnimation>().setDirection(Vector2.zero);
            }
            else
            {
                rb.velocity /= 1.05f;
            }
        }
        else if (rb.velocity.magnitude == 0)
        {
            arenaGate.SetActive(true);
        }
    }
}
