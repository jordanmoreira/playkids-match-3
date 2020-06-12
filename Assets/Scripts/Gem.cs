using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gem : MonoBehaviour
{
    public int xIndex;
    public int yIndex;

    bool isMoving = false;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            Move((int)transform.position.x + 1, (int)transform.position.y, 0.5f);
        }
        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            Move((int)transform.position.x - 1, (int)transform.position.y, 0.5f);
        }
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
                transform.position = destination;
                SetCoord((int)destination.x, (int)destination.y);
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
}
