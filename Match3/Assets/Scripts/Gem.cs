using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    [HideInInspector]
    public Vector2Int position;
    [HideInInspector]
    public Board board;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetupGem(int x, int y, Board b)
    {
        position = new Vector2Int(x, y);
        board = b;
    }
}
