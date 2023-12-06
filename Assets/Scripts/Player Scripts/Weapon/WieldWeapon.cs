using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.EventSystems.EventTrigger;

public class WieldWeapon : MonoBehaviour
{
    public LayerMask enemyLayer;
    public Transform attackPoint;
    private bool isInBattle = false;
    private bool shouldAttack = false;
    private int curWeaponIndex = 0;
    private Camera mainCam;
    [SerializeField] private float arrowSpeed;

    [SerializeField] private float attackRange;
    [SerializeField] private GameObject[] weapons;
    [SerializeField] private GameObject attackArea;
    [SerializeField] private GameObject battleUI;
    [SerializeField] private GameObject arrow;

    public void InitBattle()
    {
        weapons[curWeaponIndex].GetComponent<SpriteRenderer>().enabled = true;
        isInBattle = true;
    }
    public void BattleOver()
    {
        weapons[curWeaponIndex].GetComponent<SpriteRenderer>().enabled = false;
    }

    private void ChangeWeapon(int selectedWeapon)
    {
        curWeaponIndex = selectedWeapon;
        weapons[curWeaponIndex].GetComponent<SpriteRenderer>().enabled = true;

        for (int i = 0; i < weapons.Length; i++)
        {
            if (i != curWeaponIndex)
            {
                weapons[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }

        battleUI.GetComponent<PlayerBattleUI>().changeWeapon(curWeaponIndex);
    }
    public void Attack(Vector3 aimCoords)
    {
        shouldAttack = true;

        transform.rotation = Quaternion.Euler(0, 0, aimCoords.z);
        // play animation
        weapons[curWeaponIndex].GetComponent<Animator>().SetTrigger("Attack");

        if (curWeaponIndex == 0)
        {
            // create hitbox overlay
            Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, enemyLayer);

            foreach (Collider2D enemy in hitEnemies)
            {
                if (enemy.tag == "Enemy")
                {
                    enemy.GetComponent<EnemyInBattle>().TakeDamage(gameObject, 0.5f);
                }
            }
        }
        else if (curWeaponIndex == 1)
        {
            GameObject arrowInstance = Instantiate(arrow, transform.position, Quaternion.identity);

            Vector3 rotation = aimCoords - transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            arrowInstance.transform.rotation = Quaternion.Euler(0, 0, rotZ);

            Vector2 direction = new Vector2(rotation.x, rotation.y).normalized;

            arrowInstance.GetComponent<Rigidbody2D>().velocity = direction * arrowSpeed;
        }
    }
    private void Start()
    {
        foreach (GameObject weapon in weapons)
        {
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (isInBattle)
        {
            if (!shouldAttack)
            {
                Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                Vector3 rotation = mousePos - gameObject.transform.position;
                float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                transform.rotation = Quaternion.Euler(0, 0, rotZ);
            } 
            else
            {
                if (weapons[curWeaponIndex].GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Idle"))
                {
                    shouldAttack = false;
                }
            }

            if (Input.GetKey(KeyCode.Alpha1))
            {
                ChangeWeapon(0);
            }
            else if (Input.GetKey(KeyCode.Alpha2))
            {
                ChangeWeapon(1);
            }
        }
    }
}
