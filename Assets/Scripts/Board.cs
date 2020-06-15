using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
        if (IsWithinBounds(x, y))
        {
            allGems[x, y] = gem;
        }
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
        StartCoroutine(SwitchTilesRoutine(clickedTile, targetTile));
    }

    IEnumerator SwitchTilesRoutine(Tile clickedTile, Tile targetTile)
    {
        Gem clickedGem = allGems[clickedTile.xIndex, clickedTile.yIndex];
        Gem targetGem = allGems[targetTile.xIndex, targetTile.yIndex];

        if (clickedGem != null && targetGem != null)
        {
            clickedGem.Move(targetTile.xIndex, targetTile.yIndex, 0.3f);
            targetGem.Move(clickedTile.xIndex, clickedTile.yIndex, 0.3f);

            yield return new WaitForSeconds(0.3f);

            List<Gem> clickedGemsMatches = FindMatchesAt(clickedTile.xIndex, clickedTile.yIndex);
            List<Gem> targetGemsMatches = FindMatchesAt(targetTile.xIndex, targetTile.yIndex);

            if (clickedGemsMatches.Count == 0 && targetGemsMatches.Count == 0)
            {
                clickedGem.Move(clickedTile.xIndex, clickedTile.yIndex, 0.3f);
                targetGem.Move(targetTile.xIndex, targetTile.yIndex, 0.3f);
            }
            else
            {
                yield return new WaitForSeconds(0.3f);

                ClearGemAt(clickedGemsMatches);
                ClearGemAt(targetGemsMatches);
            }
        }

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

    List<Gem> FindMatches(int startX, int startY, Vector2 searchDirection, int minLength = 3)
    {
        List<Gem> matches = new List<Gem>();
        Gem startGem = null;

        if (IsWithinBounds(startX, startY))
        {
            startGem = allGems[startX, startY];
        }
        if (startGem != null)
        {
            matches.Add(startGem);
        }
        else
        {
            return null;
        }

        int nextX;
        int nextY;

        int maxValue = (width > height) ? width : height;

        for (int i = 1; i < maxValue - 1; i++)
        {
            nextX = startX + (int)Mathf.Clamp(searchDirection.x, -1, 1) * i;
            nextY = startY + (int)Mathf.Clamp(searchDirection.y, -1, 1) * i;

            if (!IsWithinBounds(nextX, nextY))
            {
                break;
            }

            Gem nextGem = allGems[nextX, nextY];

            if (nextGem == null)
            {
                break;
            }
            else
            {
                if (nextGem.matchValue == startGem.matchValue && !matches.Contains(nextGem))
                {
                    matches.Add(nextGem);
                    //Debug.Log("match value deu igual");
                }
                else
                {
                    break;
                }
            }
        }

        if (matches.Count >= minLength)
        {
            return matches;
        }
        return null;
    }

    List<Gem> FindMatchesAt(int x, int y, int minLength = 3)
    {
        List<Gem> horizontalMatches = FindHorizontalMatches(x, y, minLength);
        List<Gem> verticalMatches = FindVerticalMatches(x, y, minLength);

        if (horizontalMatches == null)
        {
            horizontalMatches = new List<Gem>();
        }
        if (verticalMatches == null)
        {
            verticalMatches = new List<Gem>();
        }

        var combinedMatches = horizontalMatches.Union(verticalMatches).ToList();
        return combinedMatches;
    }

    List<Gem> FindHorizontalMatches(int startX, int startY, int minLength = 3)
    {
        List<Gem> rightSideMatches = FindMatches(startX, startY, new Vector2(1, 0), 2);
        List<Gem> leftSideMatches = FindMatches(startX, startY, new Vector2(-1, 0), 2);

        if (rightSideMatches == null)
        {
            rightSideMatches = new List<Gem>();
        }
        if (leftSideMatches == null)
        {
            leftSideMatches = new List<Gem>();
        }

        var combinedMatches = rightSideMatches.Union(leftSideMatches).ToList();
        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    List<Gem> FindVerticalMatches(int startX, int startY, int minLength = 3)
    {
        List<Gem> upwardMatches = FindMatches(startX, startY, new Vector2(0, 1), 2);
        List<Gem> downwardMatches = FindMatches(startX, startY, new Vector2(0, -1), 2);

        if (upwardMatches == null)
        {
            upwardMatches = new List<Gem>();
        }
        if (downwardMatches == null)
        {
            downwardMatches = new List<Gem>();
        }

        var combinedMatches = upwardMatches.Union(downwardMatches).ToList();
        return (combinedMatches.Count >= minLength) ? combinedMatches : null;
    }

    void ClearGemAt(int x, int y)
    {
        Gem gemToClear = allGems[x, y];

        if (gemToClear != null)
        {
            allGems[x, y] = null;
            Destroy(gemToClear.gameObject);
        }
    }

    void ClearGemAt(List<Gem> gems)
    {
        foreach (Gem gem in gems)
        {
            ClearGemAt(gem.xIndex, gem.yIndex);
        }
    }

    void ClearBoard()
    {
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                ClearGemAt(i, j);
            }
        }
    }
}
