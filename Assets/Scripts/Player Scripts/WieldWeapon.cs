using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WieldWeapon : MonoBehaviour
{
    public float attachRange = 0.5f;
    public LayerMask enemyLayer;
    private SpriteRenderer weaponRenderer;
    private GameObject weapon;
    public Transform attackPoint;
    private bool isInBattle = false;
    private Camera mainCam;
    public void DrawWeapon()
    {
        weaponRenderer.enabled = true;
        isInBattle = true;
    }
    public void PutWeapon()
    {
        weaponRenderer.enabled = false;
    }
    private void Attack()
    {
        // play animation
        weapon.GetComponent<Animator>().SetTrigger("Attack");

        // create hitbox overlay
        Collider2D[] hitEnemies = Physics2D.OverlapCircleAll(attackPoint.position, attachRange, enemyLayer);

        foreach(Collider2D enemy in hitEnemies)
        {
            Debug.Log("Hit enemy: " + enemy.name);
        }
    }
    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(attackPoint.position, attachRange);
    }
    private void Start()
    {
        weaponRenderer = gameObject.transform.GetComponentInChildren<SpriteRenderer>();
        weapon = gameObject.transform.GetChild(0).gameObject;
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }
    private void Update()
    {
        if (isInBattle)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - gameObject.transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (Input.GetKeyDown(KeyCode.Space))
            {
                Attack();
            }
        }
    }
}
