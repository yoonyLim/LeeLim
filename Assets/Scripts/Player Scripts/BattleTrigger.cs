using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleTrigger : MonoBehaviour
{
    private bool inBattle = false;
    private GameObject battleManager;
    private GameObject weapon;
    private float CalcDistanceFromPlayer()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Enemy");
        Vector3 distance = player.transform.position - transform.position;
        return distance.sqrMagnitude; // squared magnitude for fater calc rather than square rooted distance.magnitude
    }

    private IEnumerator stopForTurn()
    {
        yield return new WaitUntil(() => inBattle == false);
    }
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
        if (CalcDistanceFromPlayer() <= 20 && !inBattle)
        {
            inBattle = true;
            battleManager.GetComponent<ManageBattle>().StartBattle();
            weapon.GetComponent<WieldWeapon>().DrawWeapon();
            StartCoroutine(stopForTurn());
        }
    }
}
