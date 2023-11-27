using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInBattle : MonoBehaviour
{
    public bool inBattle = false;
    private GameObject battleManager;
    private GameObject weapon;
    private void Start()
    {
        battleManager = GameObject.Find("BattleManager");
        foreach (Transform child in gameObject.transform)
        {
            if (child.tag == "Weapon")
            {
                weapon = child.gameObject;
                break;
            }
        }
    }
    void Update()
    {
        if (inBattle)
        {
            gameObject.GetComponent<PlayerMovement>().canMove = false;
            battleManager.GetComponent<ManageBattle>().StartBattle();
            weapon.GetComponent<WieldWeapon>().DrawWeapon();
        }
    }
}
