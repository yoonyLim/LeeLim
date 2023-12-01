using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInBattle : MonoBehaviour
{
    private bool inBattle = false;
    private Vector3 coordsToMoveTo;

    [SerializeField] private GameObject battleManager;
    [SerializeField] private GameObject weapon;

    public void InitBattle()
    {
        inBattle = true;
    }

    public void EndBattle()
    {
        inBattle = false;
    }

    public void SetMovementCoords(Vector3 coords)
    {
        coordsToMoveTo = coords;
        Debug.Log(coordsToMoveTo);
    }

    void Update()
    {
        if (inBattle)
        {
            gameObject.GetComponent<PlayerMovement>().canMove = false;
            battleManager.GetComponent<BattleManager>().InitBattle();
            weapon.GetComponent<WieldWeapon>().InitBattle();
        }
    }
}
