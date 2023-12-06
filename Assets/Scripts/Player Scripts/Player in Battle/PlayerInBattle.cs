using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInBattle : MonoBehaviour
{
    // bool to check if each object is inited for battle
    private bool areChildrenInited = false;
    private bool inBattle = false;
    private Vector3 moveDestinationCoords;
    private Vector3 attackTargetCoords;
    private bool shouldMoveNextTurn = false;
    private bool shouldAttackNextTurn = false;
    private bool isMyTurn = false;
    private float strength = 4, delay = 0.2f;

    private Rigidbody2D rb;
    private Animator anim;

    [SerializeField] private int health = 100;
    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject playerSprite;
    [SerializeField] private GameObject battleManager;
    [SerializeField] private GameObject healthBar;
    private Slider healthBarSlider;
    [SerializeField] private TextMeshProUGUI healthBarText;
    [SerializeField] private TextMeshProUGUI healthPotionNum;
    [SerializeField] private GameObject healthPotionMenu;
    [SerializeField] private Sprite healthPotionEmpty;

    public void InitBattle()
    {
        inBattle = true;
    }

    public void BattleOver()
    {
        inBattle = false;
        gameObject.GetComponent<PlayerMovement>().BattleOver();
        weapon.GetComponent<WieldWeapon>().BattleOver();
    }

    // accessed by ArrowUI
    public void SetMovementCoords(Vector3 coords)
    {
        moveDestinationCoords = coords;
        shouldMoveNextTurn = true;
    }

    public void SetWeaponDirCoords(Vector3 aimCoords)
    {
        attackTargetCoords = aimCoords;
        shouldAttackNextTurn = true;
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, moveDestinationCoords, 5.0f * Time.deltaTime);
    }

    // accessed by Battle Manager
    public void PlayTurn()
    {
        isMyTurn = true;
        if (shouldMoveNextTurn)
        {
            playerSprite.GetComponent<PlayerAnimation>().setDirection(new Vector2(moveDestinationCoords.x - transform.position.x, moveDestinationCoords.y - transform.position.y));
        }
    }
    public void TakeDamage(GameObject sender)
    {
        health -= 20;
        healthBarSlider.value = health;
        healthBarText.text = "HP: " + health.ToString() + "%";
        
        if (health <= 0)
        {
            Debug.Log("player dead");
        }

        anim.Play("Player Got Hit");
        Vector2 direction = (transform.position - sender.transform.position).normalized;
        rb.AddForce(direction * strength, ForceMode2D.Impulse);
        StartCoroutine(KnockBacked());
        EndTurn();
    }

    private IEnumerator KnockBacked()
    {
        yield return new WaitForSeconds(delay);
        rb.velocity = Vector3.zero;
        anim.Play("Static");
    }

    private void EndTurn()
    {
        isMyTurn = false;
        shouldMoveNextTurn = false;
        shouldAttackNextTurn = false;
        battleManager.GetComponent<BattleManager>().PlayerTurnOver();
    }

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = playerSprite.GetComponent<Animator>();
        healthBarSlider = healthBar.GetComponent<Slider>();
    }

    void Update()
    {
        if (inBattle)
        {
            if (!areChildrenInited)
            {
                gameObject.GetComponent<PlayerMovement>().InitBattle();
                weapon.GetComponent<WieldWeapon>().InitBattle();
                areChildrenInited = true;
            }

            if (isMyTurn)
            {
                if (shouldMoveNextTurn)
                {
                    MoveToTarget();

                    if (transform.position == moveDestinationCoords)
                    {
                        if (shouldAttackNextTurn)
                        {
                            weapon.GetComponent<WieldWeapon>().Attack(attackTargetCoords);
                        }
                        playerSprite.GetComponent<PlayerAnimation>().setDirection(Vector2.zero);
                        EndTurn();
                    }
                }
                else if (shouldAttackNextTurn)
                {
                    weapon.GetComponent<WieldWeapon>().Attack(attackTargetCoords);
                    EndTurn();
                }
                else
                {
                    EndTurn();
                }
            }

            if (Input.GetKeyDown(KeyCode.E))
            {
                if (int.Parse(healthPotionNum.text) != 0 && health != 100)
                {
                    health = 100;
                    healthPotionNum.text = (int.Parse(healthPotionNum.text) - 1).ToString();
                    healthBarSlider.value = health;
                    healthBarText.text = "HP: " + health.ToString() + "%";

                    if (healthPotionNum.text == "0")
                    {
                        healthPotionMenu.GetComponent<Image>().sprite = healthPotionEmpty;
                    }
                }
            }
        }
    }
}
