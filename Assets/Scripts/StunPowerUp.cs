using System.Collections;
using UnityEngine;

public class StunPowerUp : MonoBehaviour
{
    public PlayerController player;

    void Update()
    {
        if (player.hasPowerUp)
        {
            Enemy[] allEnemies = FindObjectsByType<Enemy>(FindObjectsSortMode.None);
            foreach (Enemy enemy in allEnemies)
            {
                enemy.Stun(5f);
            }
        }
    }
}
