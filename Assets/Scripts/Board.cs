using Unity.Mathematics;
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
        int randomIdX = UnityEngine.Random.Range(0, gemPrefabs.Length);
        return gemPrefabs[randomIdX];
    }

    public void PlaceGem(Gem gem, int x, int y)
    {
        gem.transform.position = new Vector3(x, y, 0);
        gem.transform.rotation = Quaternion.identity;
        // if (IsWithinBounds(x, y))
        //{
        allGems[x, y] = gem;
        //}
        gem.SetCoord(x, y);
    }

    bool IsWithinBounds(int x, int y)
    {
        return (x >= 0 && x < width && y >= 0 && y < height);
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
                    randomGem.GetComponent<Gem>().Init(this);
                    PlaceGem(randomGem.GetComponent<Gem>(), i, j);
                    randomGem.transform.parent = transform;
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
        if (_clickedTile != null && IsNextTo(tile, _clickedTile))
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

        _clickedTile = null;
        _targetTile = null;
    }

    void SwitchTiles(Tile clickedTile, Tile targetTile)
    {
        Gem clickedGem = allGems[clickedTile.xIndex, clickedTile.yIndex];
        Gem targetGem = allGems[targetTile.xIndex, targetTile.yIndex];
        clickedGem.Move(targetTile.xIndex, targetTile.yIndex, 0.3f);
        targetGem.Move(clickedTile.xIndex, clickedTile.yIndex, 0.3f);
    }

    bool IsNextTo(Tile clickedTile, Tile targetTile)
    {
        if (math.abs(clickedTile.xIndex - targetTile.xIndex) == 1 && clickedTile.yIndex == targetTile.yIndex)
        {
            return true;
        }

        if (math.abs(clickedTile.yIndex - targetTile.yIndex) == 1 && clickedTile.xIndex == targetTile.xIndex)
        {
            return true;
        }

        return false;
    }
}
