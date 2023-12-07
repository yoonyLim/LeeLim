using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class PlayerBattleUI : MonoBehaviour
{
    private bool isInBattle = false;
    private bool isResizable = false;
    private float maxDistance;
    private float curDistance;
    private SpriteRenderer moveArrowRenderer;
    private SpriteRenderer weaponUIRenderer;
    private Camera mainCam;
    private int curWeaponIndex = 0;

    [SerializeField] private GameObject playerInBattle;
    [SerializeField] private GameObject movePath;
    [SerializeField] private GameObject arrowTip;
    [SerializeField] private GameObject[] weaponUI;

    public void InitBattle()
    {
        isInBattle = true;
        isResizable = true;
    }

    public void BattleOver()
    {
        moveArrowRenderer.enabled = false;
        weaponUIRenderer.enabled = false;
        isInBattle = false;
    }

    public void WaitTurn()
    {
        moveArrowRenderer.enabled = false;
        weaponUIRenderer.enabled = false;
        isResizable = false;
    }

    public void TurnOver()
    {
        movePath.transform.localScale = new Vector3(0, movePath.transform.localScale.y, movePath.transform.localScale.z);
        isResizable = true;
    }

    public void changeWeapon(int index)
    {
        curWeaponIndex = index;
        weaponUIRenderer = weaponUI[curWeaponIndex].GetComponent<SpriteRenderer>();

        for (int i = 0; i < weaponUI.Length; i++)
        {
            if (i != curWeaponIndex)
            {
                weaponUI[i].GetComponent<SpriteRenderer>().enabled = false;
            }
        }
    }

    void Start()
    {
        moveArrowRenderer = movePath.transform.GetComponentInChildren<SpriteRenderer>();
        foreach (GameObject weapon in weaponUI)
        {
            weapon.GetComponent<SpriteRenderer>().enabled = false;
        }
        weaponUIRenderer = weaponUI[curWeaponIndex].GetComponent<SpriteRenderer>();
        moveArrowRenderer.enabled = false;
        maxDistance = Vector2.Distance(arrowTip.transform.position, movePath.transform.position);
        mainCam = GameObject.FindGameObjectWithTag("MainCamera").GetComponent<Camera>();
        movePath.transform.localScale = new Vector3(0, movePath.transform.localScale.y, movePath.transform.localScale.z);
    }

    // Update is called once per frame
    void Update()
    {
        if (isInBattle)
        {
            if (isResizable)
            {
                // left mouse click for movement
                if (Input.GetMouseButtonDown(0))
                {
                    moveArrowRenderer.enabled = true;

                    Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 rotation = mousePos - movePath.transform.position;
                    float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                    movePath.transform.rotation = Quaternion.Euler(0, 0, rotZ);

                    curDistance = Vector2.Distance(mousePos, movePath.transform.position);
                                        
                    if (curDistance > maxDistance)
                    {
                        movePath.transform.localScale = new Vector3(7.0f, movePath.transform.localScale.y, movePath.transform.localScale.z);
                    }
                    else
                    {
                        movePath.transform.localScale = new Vector3(7.0f * curDistance / maxDistance, movePath.transform.localScale.y, movePath.transform.localScale.z);
                    }

                    weaponUI[curWeaponIndex].transform.position = arrowTip.transform.position;

                    playerInBattle.GetComponent<PlayerInBattle>().SetMovementCoords(arrowTip.transform.position);
                }

                // right mouse click for attack
                if (Input.GetMouseButton(1))
                {
                    weaponUIRenderer.enabled = true;

                    weaponUI[curWeaponIndex].transform.position = arrowTip.transform.position;
                    if (curWeaponIndex == 0)
                    {
                        weaponUI[curWeaponIndex].transform.localScale = new Vector3(1.5f, 2.0f, 0);
                    }
                    else
                    {
                        weaponUI[curWeaponIndex].transform.localScale = new Vector3(2.0f, 2.0f, 0);
                    }

                    Vector3 mousePos = mainCam.ScreenToWorldPoint(Input.mousePosition);
                    Vector3 rotation = mousePos - weaponUI[curWeaponIndex].transform.position;
                    float rotZ = Mathf.Atan2(rotation.y, rotation.x) * Mathf.Rad2Deg;
                    weaponUI[curWeaponIndex].transform.rotation = Quaternion.Euler(0, 0, rotZ);

                    playerInBattle.GetComponent<PlayerInBattle>().SetWeaponDirCoords(mousePos);
                }
            }
        }
    }
}
