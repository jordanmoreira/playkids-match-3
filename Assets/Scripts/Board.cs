using System.Collections;
using System.Collections.Generic;
using System.Xml;
using UnityEngine;

public class Board : MonoBehaviour
{
    public int width;
    public int height;

    public int borderSize;

    public GameObject tilePrefab;
    public GameObject[] gemPrefabs;

    Tile[,] allTiles;
    Gem[,] allGems;

    Tile _clickedTile;
    Tile _targetTile;

    // Start is called before the first frame update
    void Start()
    {
        allTiles = new Tile[width, height];
        allGems = new Gem[width, height];

        SetupTiles();
        FillBoardWithRandom();
    }

    void SetupTiles()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject tile = Instantiate(tilePrefab, new Vector3(i, j, 0), Quaternion.identity);
                tile.name = "Tile (" + i + "," + j + ")";
                allTiles[i, j] = tile.GetComponent<Tile>();

                tile.transform.parent = transform;
                allTiles[i, j].Init(i, j, this);
            }
        }
    }

    GameObject GetRandomGem()
    {
        int randomIdX = Random.Range(0, gemPrefabs.Length);
        return gemPrefabs[randomIdX];
    }

    void PlaceGem(Gem gem, int x, int y)
    {
        gem.transform.position = new Vector3(x, y, 0);
        gem.transform.rotation = Quaternion.identity;
        gem.SetCoord(x, y);
    }

    void FillBoardWithRandom()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject randomGem = Instantiate(GetRandomGem(), Vector3.zero, Quaternion.identity) as GameObject;
                if (randomGem != null)
                {
                    PlaceGem(randomGem.GetComponent<Gem>(), i, j);
                }
            }
        }
    }

    public void ClickTile(Tile tile)
    {
        if (_clickedTile == null)
        {
            _clickedTile = tile;
            Debug.Log("clicked tile: " + tile.name);
        }
    }

    public void DragTile(Tile tile)
    {
        if (_clickedTile != null)
        {
            _targetTile = tile;
        }
    }

    public void ReleaseTile()
    {
        if (_clickedTile != null && _targetTile != null)
        {
            SwitchTiles(_clickedTile, _targetTile);
        }
    }

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        _clickedTile = null;
        _targetTile = null;
    }
}
