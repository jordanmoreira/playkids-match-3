using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tile : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    Board board;

    public void Init(int x, int y, Board b)
    {
        xIndex = x;
        yIndex = y;

        board = b;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnMouseDown()
    {
        if (board != null)
        {
            board.ClickTile(this);
        }
    }
    void OnMouseEnter()
    {
        if (board != null)
        {
            board.DragTile(this);
        }
    }
    void OnMouseUp()
    {
        if (board != null)
        {
            board.ReleaseTile();
        }
    }
}
