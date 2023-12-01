using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInBattle : MonoBehaviour
{
    // bool to check if each object is inited for battle
    private bool areChildrenInited = false;

    private bool inBattle = false;
    private Vector3 destinationCoords;
    private Vector3 originCoords;
    private bool shouldMoveNextTurn = false;
    private bool isMyTurn = false;
    private bool hasMoved = false;

    [SerializeField] private GameObject weapon;
    [SerializeField] private GameObject battleManager;

    public void InitBattle()
    {
        inBattle = true;
    }

    public void EndBattle()
    {
        inBattle = false;
    }

    // used by ArrowUI
    public void SetMovementCoords(Vector3 coords)
    {
        destinationCoords = coords;
        shouldMoveNextTurn = true;
        Debug.Log(destinationCoords);
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, destinationCoords, 2.0f * Time.deltaTime);
    }

    public void TakeTrun()
    {
        isMyTurn = true;
        originCoords = transform.position;
        StartCoroutine(WaitUntilMoved());
    }

    private IEnumerator WaitUntilMoved()
    {
        yield return new WaitUntil(() => shouldMoveNextTurn && hasMoved);
        hasMoved = false;
        isMyTurn = false;
        shouldMoveNextTurn = false;
        battleManager.GetComponent<BattleManager>().TurnOver();
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
                MoveToTarget();

                if (transform.position == destinationCoords)
                {
                    hasMoved = true;
                }
            }
        }
    }
}
