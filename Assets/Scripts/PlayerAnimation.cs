using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class PlayerAnimation : MonoBehaviour
{
    private Animator anim;

    public string[] staticDirections = { "Static", "Static" };
    public string[] runDirections = { "Run Left", "Run Right" }; // should be in order of counter clock wise starting from the up(90 degrees)

    int lastDirection;

    private void Awake()
    {
        anim = GetComponent<Animator>();
    }

    public void setDirection(Vector2 _direction)
    {
        string[] directionArray = null;

        if (_direction.magnitude < 0.01)
        {
            directionArray = staticDirections;
        } 
        else 
        {
            directionArray = runDirections;

            lastDirection = dirctionToIndex(_direction);
        }
        
        if (directionArray != null)
        {
            anim.Play(directionArray[lastDirection]);
        }
    }

    private int dirctionToIndex(Vector2 _direction)
    {
        Vector2 norDir = _direction.normalized;

        float directionAngle = Vector2.SignedAngle(Vector2.up, norDir); // returns angle with +, - signs between up and norDir

        if (directionAngle <= 0)
        {
            return 1;
        }
        else
        {
            return 0;
        }
    }
}
