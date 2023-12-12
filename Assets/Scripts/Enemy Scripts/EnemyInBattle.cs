using Cinemachine;
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
    static public bool isZombieDead = false;
    [SerializeField] float speed;

    [SerializeField] private GameObject healthBar;
    [SerializeField] private GameObject enemySprite;
    [SerializeField] private GameObject enemyBone;
    [SerializeField] private float boneSpeed;
    private GameObject player;
    private GameObject battleManager;
    private CinemachineVirtualCamera virtualCamera;

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
        virtualCamera.m_Lens.OrthographicSize = 2.5f;
        StartCoroutine(SlowDownDeath());
    }

    private IEnumerator SlowDownDeath()
    {
        spriteAnim.Play("Die");
        Time.timeScale = 0.5f;
        yield return new WaitForSeconds(0.5f);
        virtualCamera.m_Lens.OrthographicSize = 3.0f;
        Time.timeScale = 1.0f;
        if (!isZombieDead)
        {
            isZombieDead = true;
            battleManager.GetComponent<BattleManager>().ZombieDied();
        }
        else
        {
            isZombieDead = false;
            battleManager.GetComponent<BattleManager>().BattleOver();
        }
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
            spriteAnim.Play("Run Left");
        }
        else
        {
            spriteAnim.Play("Run Right");
        }
        transform.position = Vector3.MoveTowards(transform.position, destinationCoords, speed * Time.deltaTime);
    }

    private void ThrowBone()
    {
        Vector3 rotation = destinationCoords - transform.position;
        float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;

        GameObject boneInstance = Instantiate(enemyBone, transform.position, Quaternion.identity);
        boneInstance.transform.rotation = Quaternion.Euler(0, 0, rotZ);
        Vector2 direction = new Vector2(rotation.x, rotation.y).normalized;
        boneInstance.GetComponent<Rigidbody2D>().velocity = direction * boneSpeed;
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
        virtualCamera = GameObject.FindGameObjectWithTag("VirtualCamera").GetComponent<CinemachineVirtualCamera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMyTurn)
        {
            if (!isZombieDead)
            {
                Dash();
            }
            else
            {
                ThrowBone();
                EndTurn();
            }
            
            if (transform.position == destinationCoords)
            {
                spriteAnim.Play("Idle");
                EndTurn();
            }
        }
    }
}
