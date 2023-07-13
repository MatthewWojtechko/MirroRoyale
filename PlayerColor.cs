using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerColor : MonoBehaviour
{
    public Reflection Reflection;
    public SpriteRenderer outline;
    public string colorTag;
    public AudioSource[] colorSounds;
    public ParticleSystem[] colorParts;

    // Start is called before the first frame update
    void Start()
    {
        Reflection.Type = Reflection.ReflectionType.NONE;
        setColor();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void setColor()
    {
        outline.color = Reflection.getColor();
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.tag == colorTag)
        {
            Reflection.ReflectionType refl = col.gameObject.GetComponent<Reflection>().Type;
            if (refl != Reflection.Type)
            {
                Reflection.Type = refl;
                setColor();
                colorSounds[(int)Reflection.Type].Play();
                ParticleSystem temppart = colorParts[(int)Reflection.Type];
                temppart.Play();
            }
        }
    }
}
