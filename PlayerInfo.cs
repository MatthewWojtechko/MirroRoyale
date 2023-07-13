using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerInfo : MonoBehaviour
{
    public int num; // player 1, player 2, etc...
    public int numKills;
    public int numKilled;

    public int maxHealth;
    public int currentHealth;


    public Text scoreText;

    public GameObject scoreEffect;

    public SceneManager Manager;

    public PlayerMovement3 Movement;

    public Reflection Reflection;

    public Transform spawnPoint;
    public Transform hidePoint;

    public GameObject deathEffect;
    public GameObject spawnEffect;

    public bool isAlive = true;

    public float respawnTime;

    public bool respawnTimeUp = false;

    public Quaternion spawnRotation;

    public Color goodScoreColor, badScoreColor;

    private int showScoreCounter = 0;

    public float displayScoreTime = 2;

    private bool isScoreDisplayed = false;

    public SpriteRenderer rend;
    private Material currentMat;
    public Texture[] cracks;  // [0] being most broken, [last] being least broken


    void Start()
    {
        currentMat = rend.material;
    }


    void Update()
    {
        if (showScoreCounter == 0 && isScoreDisplayed)
        {
            scoreText.text = "";
            isScoreDisplayed = false;
        }
    }




    public int getScore()
    {
        return numKills - numKilled;
    }



    public void increaseScore(int n)
    {
        numKills += n;
        GameObject.Instantiate(scoreEffect, this.transform.position, this.transform.rotation);
        showScore(goodScoreColor);
    }


    public void hit(int attacker, int strength)
    {
        currentHealth -= strength;
        if (currentHealth <= 0) // I've been killed!
        {
            currentHealth = 0;

            if (attacker == num)
                attacker = 0;

            Manager.registerKill(attacker, this);
        }
        determineCracks();    
    }

    public void die()
    {
        numKilled++;
        Movement.reset();
        Movement.enabled = false;
        GameObject.Instantiate(deathEffect, this.transform.position, this.transform.rotation);
        this.transform.position = hidePoint.position;
        StartCoroutine(respawnWait());
    }

    IEnumerator respawnWait()
    {
        //isAlive = false;
        //respawnTimeUp = false;
        yield return new WaitForSeconds(respawnTime);
        //respawnTimeUp = true;
        respawn();
    }

    void respawn()
    {
        this.reset();
        this.transform.position = spawnPoint.position;
        this.transform.rotation = spawnRotation;
        GameObject.Instantiate(spawnEffect, this.transform.position, this.transform.rotation);
        Movement.enabled = true;
        showScore(badScoreColor);
    }

    void showScore(Color c)
    {
        scoreText.text = numKills - numKilled + "";
        scoreText.color = c;
        showScoreCounter++;
        isScoreDisplayed = true;
        StartCoroutine(waitDisplayScore());
    }

    IEnumerator waitDisplayScore()
    {
        yield return new WaitForSeconds(displayScoreTime);
        showScoreCounter--;
    }

    void reset()
    {
        currentHealth = maxHealth;
        currentMat.SetTexture("_OcclusionMap", null);
    }

    void determineCracks()
    {
        int numLevels = cracks.Length;
        float level = maxHealth / numLevels;

        if (currentHealth != maxHealth)
        {
            float tempLevel = maxHealth - level;
            for (int i = 1; i <= numLevels; i++)
            {
                if (currentHealth < level * i)
                {
                    currentMat.SetTexture("_OcclusionMap", cracks[i - 1]);
                    break;
                }
            }
        }
    }


}
