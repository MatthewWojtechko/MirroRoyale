using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareSpawner : MonoBehaviour
{
    public GameObject[] squares;
    public float[] probabilities;

    public Transform[] spawnPoints;
    public bool[] spawnsOpen;
    public float beat;

    public Transform hidePoint;

    public bool isWaiting;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(wait());
    }

    // Update is called once per frame
    void Update()
    {
        if (!isWaiting)
        {
            openSpawns();
            for (int i = 0; i < squares.Length; i++)
            {
                if (Random.Range(0, 100) <= probabilities[i])
                {
                    spawn(squares[i]);
                }
                else
                {
                    hide(squares[i]);
                }
            }
            StartCoroutine(wait());
        }
    }

    void spawn(GameObject g)
    {
        int index = nextAvailable(Random.Range(0, spawnPoints.Length));
        g.transform.position = spawnPoints[index].position;
        spawnsOpen[index] = false;
    }

    int nextAvailable(int n)
    {
        int result = n;
        bool atEndOnce = false;
        while (result == spawnPoints.Length-1 || !spawnsOpen[result])
        {
            if (result == spawnPoints.Length-1)
            {
                if (atEndOnce) // avoid infinite loop from improperly set up parallel arrays
                    break;
                result = 0;
                atEndOnce = true;
            }
            else
                result++;
        }
        return result;
    }

    void hide(GameObject g)
    {
        g.transform.position = hidePoint.position;
    }

    IEnumerator wait()
    {
        isWaiting = true;
        yield return new WaitForSeconds(beat);
        isWaiting = false;
    }

    void openSpawns()
    {
        for (int i = 0; i < spawnsOpen.Length; i++)
        {
            spawnsOpen[i] = true;
        }
    }
}
