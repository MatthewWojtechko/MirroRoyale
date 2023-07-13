using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PerfectMovementCollider : MonoBehaviour
{
    public PolygonCollider2D collider;
    public Transform[] horizontalPoints1 = new Transform[2];
    public Transform[] horizontalPoints2 = new Transform[2];
    public Transform[] verticalPoints1 = new Transform[2];
    public Transform[] verticalPoints2 = new Transform[2];

    private Vector2[] oldHPoints1 = new Vector2[2];
    private Vector2[] oldVPoints1 = new Vector2[2];
    private Vector2[] oldHPoints2 = new Vector2[2];
    private Vector2[] oldVPoints2 = new Vector2[2];

    private Vector2[] newHPoints1 = new Vector2[2];
    private Vector2[] newVPoints1 = new Vector2[2];
    private Vector2[] newHPoints2 = new Vector2[2];
    private Vector2[] newVPoints2 = new Vector2[2];

    // Start is called before the first frame update
    void Start()
    {
        newHPoints1[0].x = horizontalPoints1[0].position.x;
        newHPoints1[1].y = horizontalPoints1[1].position.y;
        newHPoints2[0].x = horizontalPoints2[0].position.x;
        newHPoints2[1].y = horizontalPoints2[1].position.y;

        newVPoints1[0].x = verticalPoints1[0].position.x;
        newVPoints1[1].y = verticalPoints1[1].position.y;
        newVPoints2[0].x = verticalPoints2[0].position.x;
        newVPoints2[1].y = verticalPoints2[1].position.y;
    }

    // Update is called once per frame
    void Update()
    {
        drawCollider();
    }

    void updateOldLocations()
    {
        for (int i = 0; i < horizontalPoints1.Length; i++)
        {
            Debug.Log(i);///
            oldHPoints1[i].x = newHPoints1[i].x;
            oldHPoints1[i].y = newHPoints1[i].y;

            oldVPoints1[i].x = newVPoints1[i].x;
            oldVPoints1[i].y = newVPoints1[i].y;

            oldHPoints2[i].x = newHPoints2[i].x;
            oldHPoints2[i].y = newHPoints2[i].y;

            oldVPoints2[i].x = newVPoints2[i].x;
            oldVPoints2[i].y = newVPoints2[i].y;
        }
    }

    void updateNewLocations()
    {
        for (int i = 0; i < horizontalPoints1.Length; i++)
        {
            newHPoints1[i].x = horizontalPoints1[i].position.x;
            newHPoints1[i].y = horizontalPoints1[i].position.y;

            newVPoints1[i].x = verticalPoints1[i].position.x;
            newVPoints1[i].y = verticalPoints1[i].position.y;

            newHPoints2[i].x = horizontalPoints2[i].position.x;
            newHPoints2[i].y = horizontalPoints2[i].position.y;

            newVPoints2[i].x = verticalPoints2[i].position.x;
            newVPoints2[i].y = verticalPoints2[i].position.y;
        }
    }

    void updateCollider()
    {
        //int pointIndex = 0;
        //for (int i = 0; i < oldLocations.Length; i++)
        //{
        //    //collider.points[pointIndex].x = oldLocations[i].x;
        //    //collider.points[pointIndex].y = oldLocations[i].y;
        //    //pointIndex++;
        //    //collider.points[pointIndex].x = newLocations[i].x;
        //    //collider.points[pointIndex].y = newLocations[i].y;
        //    //pointIndex++;

        //    Debug.Log(oldLocations[i] + ", " + newLocations[i]);//
        //}

        Vector2[] path0 = { toWorldSpace(oldHPoints1[0]), toWorldSpace(newHPoints1[0]), toWorldSpace(newHPoints1[1]), toWorldSpace(oldHPoints1[1]) };
        Vector2[] path1 = { toWorldSpace(oldVPoints1[0]), toWorldSpace(newVPoints1[0]), toWorldSpace(newVPoints1[1]), toWorldSpace(newVPoints1[1]) };
        Vector2[] path2 = { toWorldSpace(oldHPoints2[0]), toWorldSpace(newHPoints2[0]), toWorldSpace(newHPoints2[1]), toWorldSpace(oldHPoints2[1]) };
        Vector2[] path3 = { toWorldSpace(oldVPoints2[0]), toWorldSpace(newVPoints2[0]), toWorldSpace(newVPoints2[1]), toWorldSpace(newVPoints2[1]) };

        collider.SetPath(0, path0);
        collider.SetPath(1, path1);
        collider.SetPath(2, path2);
        collider.SetPath(3, path3);

    }

    public void drawCollider()
    {
        updateOldLocations();
        updateNewLocations();
        updateCollider();
    }

    Vector2 toWorldSpace(Vector2 v)
    {
        return this.transform.InverseTransformPoint(v);
    }
}
