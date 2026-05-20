using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    public float speed = 5f;
    public Transform focalPoint;
    public bool hasPowerUp = false;
    public GameObject ringPower;
 
    private Rigidbody rb;
    public Animator ringAnim;
 
    private InputAction moveAction;
    private InputAction smashAction;
    private InputAction breakAction;

    private Coroutine powerUpRoutine;
 
 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        ringAnim = ringPower.GetComponent<Animator>();
        moveAction = InputSystem.actions.FindAction("Move");
        smashAction = InputSystem.actions.FindAction("Smash");
        breakAction = InputSystem.actions.FindAction("Break");
    }
 
    // Update is called once per frame
    void Update()
    {
        var move = moveAction.ReadValue<Vector2>();
        rb.AddForce(move.y * speed * focalPoint.forward);
 
        if (breakAction.IsPressed())
        {
            rb.linearVelocity = Vector3.zero;
            //rb.linearVeclocity = new Vector3(0, 0, 0);
        }
        RingShow();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("PowerUp"))
        {
            hasPowerUp = true;
            Destroy(other.gameObject);
            if (powerUpRoutine != null)
            {
                StopCoroutine(powerUpRoutine);
            }
            StartCoroutine(PowerUpCooldown());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            if (hasPowerUp)
            {
                var enemyRb = collision.gameObject.GetComponent<Rigidbody>();
                //var v = enemyRb.linearVelocity;
                var dir = enemyRb.transform.position - transform.position;
                dir.Normalize();
                enemyRb.AddForce(Vector3.up * 5, ForceMode.Impulse);
            }
        }
    }
    IEnumerator PowerUpCooldown()
    {
        yield return new WaitForSeconds(10);
        hasPowerUp = false;
    }
    public void RingShow()
    {
        if (hasPowerUp == true)
        {
            ringPower.SetActive(true);
            ringAnim.SetBool("RingSpin", true);
        }
        else 
        { 
            ringPower.SetActive(false);
            ringAnim.SetBool("RingSpin", false);
        }
    }
}
