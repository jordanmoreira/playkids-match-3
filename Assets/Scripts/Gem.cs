using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Gem : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    bool isMoving = false;

    public enum MatchValue
    {
        Apple,
        Bread,
        Broccoli,
        Coconut,
        Milk,
        Orange,
        Star,
        None
    }
    public MatchValue matchValue;

    Board board;

    public int scoreValue = 10;

    public void Init(Board b)
    {
        board = b;
    }
    public void SetCoord(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }
    public void Move(int targetX, int targetY, float time)
    {
        if (!isMoving)
        {
            StartCoroutine(MoveRoutine(new Vector3(targetX, targetY, 0), time));
        }
    }
    IEnumerator MoveRoutine(Vector3 destination, float time)
    {
        Vector3 startPosition = transform.position;
        bool reachedDestination = false;
        float elapsedTime = 0f;

        isMoving = true;
        while (!reachedDestination)
        {

            // if we are close enough to destination
            if (Vector3.Distance(transform.position, destination) < 0.01f)
            {
                reachedDestination = true;
                if (board != null)
                {
                    board.PlaceGem(this, (int)destination.x, (int)destination.y);
                }
                break;
            }
            // track the total running time
            elapsedTime += Time.deltaTime;

            // calculate the lerp value
            float t = elapsedTime / time;

            // move gem
            transform.position = Vector3.Lerp(startPosition, destination, t);

            // wait until next frame
            yield return null;
        }
        isMoving = false;
    }
    public void ScorePoints()
    {
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(scoreValue);
        }
    }
}
