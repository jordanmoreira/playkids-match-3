using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class BoardDeadlock : Singleton<BoardDeadlock>
{
    List<Gem> GetRowOrColumnList(Gem[,] allGems, int x, int y, int listLength = 3, bool checkRow = true)
    {
        int width = allGems.GetLength(0);
        int height = allGems.GetLength(1);

        List<Gem> gemsList = new List<Gem>();

        for (int i = 0; i < listLength; i++)
        {
            if (checkRow)
            {
                if (x + i < width && y < height)
                {
                    gemsList.Add(allGems[x + i, y]);
                }
            }
            else
            {
                if (x < width && y + i < height)
                {
                    gemsList.Add(allGems[x, y + i]);
                }
            }
        }
        return gemsList;
    }
    List<Gem> GetMinimumMatches(List<Gem> gems, int minForMatch = 2)
    {
        List<Gem> matches = new List<Gem>();

        var groups = gems.GroupBy(n => n.matchValue);

        foreach (var grp in groups)
        {
            if (grp.Count() >= minForMatch && grp.Key != Gem.MatchValue.None)
            {
                matches = grp.ToList();
            }
        }
        return matches;
    }
    List<Gem> GetNeighbors(Gem[,] allGems, int x, int y)
    {
        int width = allGems.GetLength(0);
        int height = allGems.GetLength(1);

        List<Gem> neighbors = new List<Gem>();

        Vector2[] searchDirections = new Vector2[4]
        {
            new Vector2(-1f, 0f),
            new Vector2(1f, 0f),
            new Vector2(0f, 1f),
            new Vector2(0f, -1f)
        };

        foreach (Vector2 dir in searchDirections)
        {
            if (x + (int)dir.x >= 0 && x + (int)dir.x < width && y + (int)dir.y >= 0 && y + (int)dir.y < height)
            {
                if (allGems[x + (int)dir.x, y + (int)dir.y] != null)
                {
                    if (!neighbors.Contains(allGems[x + (int)dir.x, y + (int)dir.y]))
                    {
                        neighbors.Add(allGems[x + (int)dir.x, y + (int)dir.y]);
                    }
                }
            }
        }
        return neighbors;
    }
    bool HasMoveAt(Gem[,] allGems, int x, int y, int listLength = 3, bool checkRow = true)
    {
        List<Gem> gems = GetRowOrColumnList(allGems, x, y, listLength, checkRow);
        List<Gem> matches = GetMinimumMatches(gems, listLength - 1);

        Gem unmatchedGem = null;
        if (gems != null && matches != null)
        {
            if (gems.Count == listLength && matches.Count == listLength - 1)
            {
                unmatchedGem = gems.Except(matches).FirstOrDefault();
            }

            if (unmatchedGem != null)
            {
                List<Gem> neighbors = GetNeighbors(allGems, unmatchedGem.xIndex, unmatchedGem.yIndex);
                neighbors = neighbors.Except(matches).ToList();
                neighbors = neighbors.FindAll(n => n.matchValue == matches[0].matchValue);
                matches = matches.Union(neighbors).ToList();
            }

            if (matches.Count >= listLength)
            {
                return true;
            }
        }
        return false;
    }
    public bool IsDeadlocked(Gem[,] allgems, int listLength = 3)
    {
        int width = allgems.GetLength(0);
        int height = allgems.GetLength(1);

        bool isDeadlocked = true;

        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                if (HasMoveAt(allgems, i, j, listLength, true) || HasMoveAt(allgems, i, j, listLength, false))
                {
                    isDeadlocked = false;

                }
            }
        }
        if (isDeadlocked)
        {
            Debug.Log(" ===========  BOARD DEADLOCKED ================= ");
        }
        return isDeadlocked;
    }
}
