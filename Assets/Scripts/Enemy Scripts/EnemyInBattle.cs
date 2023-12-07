using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class EnemyInBattle : MonoBehaviour
{
    private Animator spriteAnim;
    private Slider healthBarSlider;
    private Rigidbody2D rb;
    private float strength = 4, delay = 0.2f;
    public bool isMyTurn = false;
    private Vector3 destinationCoords;
    [SerializeField] float speed;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject enemySprite;
    private GameObject player;
    private GameObject battleManager;

    public void TakeDamage(GameObject sender, float damage)
    {
        healthBarSlider.value -= damage;

        if (healthBarSlider.value <= 0f)
        {
            Die();
        }
        else
        {
            spriteAnim.Play("Got Hit");
            Vector2 direction = (transform.position - sender.transform.position).normalized;
            rb.AddForce(direction * strength, ForceMode2D.Impulse);
            StartCoroutine(KnockBacked());
            EndTurn();
        }
    }

    private IEnumerator KnockBacked()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        spriteAnim.Play("Idle");
    }

    private void Die()
    {
        isMyTurn = false;
        healthBar.SetActive(false);
        StartCoroutine(SlowDownDeath());
    }

    private IEnumerator SlowDownDeath()
    {
        spriteAnim.Play("Zombie Dies");
        yield return new WaitForSeconds(1);
        battleManager.GetComponent<BattleManager>().BattleOver();
        Destroy(gameObject);
    }

    public void PlayTurn()
    {
        isMyTurn = true;
        destinationCoords = player.transform.position;
    }

    private void EndTurn()
    {
        isMyTurn = false;
        battleManager.GetComponent<BattleManager>().EnemyTurnOver();
    }

    private void Dash()
    {
        if (destinationCoords.x - transform.position.x < 0f)
        {
            spriteAnim.Play("Zombie Run Left");
        }
        else
        {
            spriteAnim.Play("Zombie Run Right");
        }
        transform.position = Vector3.MoveTowards(transform.position, destinationCoords, speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.TryGetComponent<PlayerInBattle>(out PlayerInBattle player))
        {
            player.TakeDamage(gameObject);
            spriteAnim.Play("Idle");
            EndTurn();
        }
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        healthBarSlider = healthBar.GetComponent<Slider>();
        spriteAnim = enemySprite.GetComponent<Animator>();
        battleManager = GameObject.FindGameObjectWithTag("BattleManager");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn)
        {
            Dash();
            
            if (transform.position == destinationCoords)
            {
                spriteAnim.Play("Idle");
                EndTurn();
            }
        }
    }
}
