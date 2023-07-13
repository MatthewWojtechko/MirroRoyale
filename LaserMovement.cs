using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserMovement : MonoBehaviour
{
    public float startSpeed = 5;
    public float currentSpeed;
    public LayerMask collisionMask = 0;

    public float reflectScoreFactor = 2.5f;

    public float reflectWait = 0.3f; // minimum seconds laser must wait before reflecting off the object again

    public float circleRadius;
    
    private Vector2 projection; // where the next position will be (if no reflection)
    private RaycastHit2D[] hits;
    private RaycastHit2D hit;

    public AudioSource audioSource;
    public AudioClip reflectSFX;
    public AudioClip hurtSFX;

    public GameObject hurtEffect;
    public GameObject playerReflectEffect;
    public GameObject regReflectEffect;
    public GameObject laserDieEffect;


    public string hurtBox = "HurtBox";

    private bool isDying = false;

    public float reflectScore = 0; // from 0 to 1, how close the player reflection was at center

    public List<GameObject> cantReflect = new List<GameObject>();

    public int lastPlayer; // what player was last to reflect the laser

    public Reflection Reflection;

    public ParticleSystem laserVisual;
    public Light pointLight;
    public Light spotLight;

    // Start is called before the first frame update
    void Start()
    {
        currentSpeed = startSpeed;
        setColor(Reflection.getColor());
    }

    // Update is called once per frame
    void Update()
    {
    }

    void LateUpdate()
    {
    }

    void FixedUpdate()
    {
        move();
    }

    void move()
    {
        bool goContinue = true;
        projection = this.transform.position + (this.transform.up * currentSpeed * Time.deltaTime);  // next forward step

        // Determine any hit using a strict linecast
        hit = Physics2D.Linecast(this.transform.position, projection, collisionMask);
        if (!hit || hit.point == new Vector2(this.transform.position.x, this.transform.position.y)) // if no hit, keep going
        {
            // If there was no hit, check if there was a hit if we generously check for a player reflection
            hits = Physics2D.CircleCastAll(this.transform.position + (this.transform.up * circleRadius), circleRadius, this.transform.up, Vector2.Distance(this.transform.position, projection), collisionMask);
            foreach (RaycastHit2D h in hits)
            {
                Reflection refl = h.collider.gameObject.GetComponent<Reflection>();
                if (refl != null)
                {
                    if (h.collider.tag == "Player" && !cantReflect.Contains(h.collider.gameObject))
                    {
                        playerReflect(h);
                        goContinue = false;
                        break;
                    }
                    
                }
            }
            if (goContinue) // there was no player reflection, so just keep moving forward
                this.transform.position = projection;
        }
        else // else, hit
        {
            Reflection refl = hit.collider.gameObject.GetComponent<Reflection>();
            if (hit.collider.tag == hurtBox) // if hurt box
            {
                if (!isDying)
                    playerHit(hit.collider);
            }
            else  // else, reflect  
            {
                if (hit.collider.tag == "Player")
                    playerReflect(hit);
                else if (refl.Type != Reflection.ReflectionType.NONE && (refl.Type == this.Reflection.Type || this.Reflection.Type == Reflection.ReflectionType.WHITE))
                {
                    regReflect(hit);
                }
                else
                {
                    explode(laserDieEffect);
                }
            }
        }
    }

    void playerReflect(RaycastHit2D hit)
    {
        PlayerInfo player = hit.collider.gameObject.GetComponent<PlayerInfo>();
        lastPlayer = player.num;
        if (player.Reflection.Type != Reflection.ReflectionType.NONE)
        {
            this.Reflection.Type = player.Reflection.Type;
            setColor(player.Reflection.getColor());
        }
        setReflectScore(hit);
        currentSpeed += reflectScore;
        GameObject.Instantiate(playerReflectEffect, this.transform.position, this.transform.rotation);// Quaternion.Inverse(this.transform.rotation));
        reflect(hit);
    }

    void regReflect(RaycastHit2D hit)
    {
        GameObject.Instantiate(regReflectEffect, this.transform.position, this.transform.rotation);//Quaternion.Inverse(this.transform.rotation));
        reflect(hit);
    }

    void reflect(RaycastHit2D hit)
    {
        if (!cantReflect.Contains(hit.collider.gameObject))
        {
            this.transform.position = hit.point;
            this.transform.up = Vector2.Reflect(this.transform.up, hit.normal);
            StartCoroutine(addToNoReflectList(hit.collider.gameObject));
        }
    }

    void playerHit(Collider2D collider)
    {
        isDying = true;
        // decrement player health, inform GM

        // will do this with GM
        collider.gameObject.transform.parent.gameObject.GetComponent<PlayerMovement3>().addForce(this.transform.up * currentSpeed * 0.02f, 0.8f);
        collider.gameObject.transform.parent.gameObject.GetComponent<PlayerInfo>().hit(lastPlayer, (int)(currentSpeed / startSpeed));

        explode(hurtEffect);
    }

    IEnumerator addToNoReflectList(GameObject g)
    {
        cantReflect.Add(g);
        yield return new WaitForSeconds(reflectWait);
        cantReflect.Remove(g);
    }

    void setReflectScore(RaycastHit2D hit)
    {
        //float maxDist = hit.collider.bounds.size.y / 2;
        //float center = hit.collider.bounds.center.y;
        //Vector2 pointLocal = hit.collider.gameObject.transform.InverseTransformPoint(hit.point);

        //float dist = pointLocal.y - center;

        //Debug.Log(dist + " " + maxDist);//

        reflectScore = Vector2.Distance(hit.collider.bounds.center, hit.point) * reflectScoreFactor;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.tag == hurtBox)
        {
            if (!cantReflect.Contains(col.transform.parent.gameObject))
                playerHit(col);
        }
    }

    void explode(GameObject effect)
    {
        GameObject g = GameObject.Instantiate(effect, this.transform.position, hurtEffect.transform.rotation);
        g.transform.localScale *= currentSpeed;

        if (effect == laserDieEffect)
            g.GetComponent<ParticleSystem>().startColor = Reflection.getColor();

        GameObject.Destroy(this.gameObject);

        GameObject.Destroy(this.gameObject);
    }

    void setColor(Color c)
    {
        laserVisual.startColor = c;
        spotLight.color = c;
        pointLight.color = c;
    }


}
