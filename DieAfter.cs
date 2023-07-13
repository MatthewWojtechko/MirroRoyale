using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieAfter : MonoBehaviour
{
    public float dieTime;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(dieAfter());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator dieAfter()
    {
        yield return new WaitForSeconds(dieTime);
        GameObject.Destroy(this.gameObject);
    }
}
