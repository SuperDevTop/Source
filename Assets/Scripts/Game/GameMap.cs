using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMap : MonoBehaviour
{
    [Header("Camera View:")]
    public Vector2 bounds;
    public Vector2 boundOffset;
    public bool isScrollRequired = true;

    [Header("Spawn Positions:")]
    public Transform[] spawnPositions;
    public int[] prevLocations;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireCube(new Vector3(boundOffset.x, boundOffset.y), new Vector3(bounds.x * 2, bounds.y * 2, 0));
    }
}
