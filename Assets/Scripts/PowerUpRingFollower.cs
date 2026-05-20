using TMPro;
using UnityEngine;
using UnityEngine.UIElements;

public class PowerUpRingFollower : MonoBehaviour
{
    public Transform playerLocate;
    public GameObject powerupRing;

    private void Update()
    {
        Follow();
    }

    public void Follow()
    {
        transform.position = new Vector3(playerLocate.position.x, playerLocate.position.y, playerLocate.position.z);
    }

}
