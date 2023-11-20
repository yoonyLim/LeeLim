using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class CollisionController : MonoBehaviour
{
    private TilemapRenderer tilemapRenderer;

    public bool showCollision = false;
    void Start()
    {
        tilemapRenderer = GetComponent<TilemapRenderer>();
        tilemapRenderer.enabled = showCollision;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
