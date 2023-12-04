using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public LayerMask enemyLayer;
    [SerializeField] private float rangeWidth;
    [SerializeField] private float rangeHeight;
    [SerializeField] private float offsetX;
    [SerializeField] private float offsetY;

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), new Vector3(rangeWidth, rangeHeight, 1f));
    }

    // Update is called once per frame
    void Update()
    {
        Collider2D[] hitEnemies = Physics2D.OverlapBoxAll(new Vector2(transform.position.x + offsetX, transform.position.y + offsetY), new Vector2(rangeWidth, rangeHeight), 0f, enemyLayer);

        foreach (Collider2D enemy in hitEnemies)
        {
            Debug.Log(enemy);
            if (enemy.tag == "Enemy")
            {
                enemy.GetComponent<EnemyInBattle>().TakeDamage(gameObject, 0.34f);
                Destroy(gameObject);
            }
        }
    }
}
