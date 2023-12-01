using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowUIInBattle : MonoBehaviour
{
    private bool isInBattle = false;
    private bool isResizable = false;
    private float maxDistance;
    private float curDistance;
    private SpriteRenderer arrowRenderer;
    private Camera mainCam;

    [SerializeField] private GameObject playerInBattle;
    [SerializeField] private GameObject arrowTip;

    public void InitBattle()
    {
        arrowRenderer.enabled = true;
        isInBattle = true;
        isResizable = true;
    }

    public void EndBattle()
    {
        arrowRenderer.enabled = false;
        isInBattle = false;
    }

    public void WaitTurn()
    {
        arrowRenderer.enabled = false;
        isResizable = false;
    }

    public void TurnOver()
    {
        arrowRenderer.enabled = true;
        isResizable = true;
    }

    void Start()
    {
        arrowRenderer = gameObject.transform.GetComponentInChildren<SpriteRenderer>();
        arrowRenderer.enabled = false;
        isInBattle = false;
        maxDistance = Vector2.Distance(arrowTip.transform.position, gameObject.transform.position);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isInBattle)
        {
            Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
            Vector3 rotation = mousePos - gameObject.transform.position;
            float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0, 0, rotZ);

            if (isResizable)
            {
                curDistance = Vector2.Distance(mousePos, gameObject.transform.position);

                if (curDistance > maxDistance)
                {
                    transform.localScale = new Vector3(1.0f, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
                else
                {
                    transform.localScale = new Vector3(curDistance / maxDistance, gameObject.transform.localScale.y, gameObject.transform.localScale.z);
                }
            }

            if (Input.GetMouseButtonDown(0))
            {
                playerInBattle.GetComponent<PlayerInBattle>().SetMovementCoords(arrowTip.transform.position);
            }
        }
    }
}
