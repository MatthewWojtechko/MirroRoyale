using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SceneManager : MonoBehaviour
{
    public PlayerInfo[] players;

    public int killPoints = 10;

    public void registerKill(int attacker, PlayerInfo killed)
    {
        if (attacker != 0)
            players[attacker - 1].increaseScore(killPoints);

        killed.die();
    }
}
