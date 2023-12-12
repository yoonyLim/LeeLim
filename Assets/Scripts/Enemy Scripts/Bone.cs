using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bone : MonoBehaviour
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

        foreach (Collider2D collision in hitEnemies)
        {
            Debug.Log(collision);
            if (collision.tag == "Player")
            {
                collision.GetComponent<PlayerInBattle>().TakeDamage(gameObject);
                Destroy(gameObject);
            } else if (collision.tag == "Collision")
            {
                Destroy(gameObject);
            }
        }
    }
}
