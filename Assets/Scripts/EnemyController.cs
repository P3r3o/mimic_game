using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class EnemyController : MonoBehaviour
{
    private PlayerController playerScript;
    public GameObject player;
    public float rotationSpeed = 5f;
    public float chargeSpeed = 3f;
    public float chargeTime = 2f;
    public float waitTime = 3f;

    private float chargeTimer;
    private float waitTimer;
    private bool isCharging;

    public void RestartCurrentScene()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }

    void Start() {
        chargeTimer = chargeTime;
        waitTimer = waitTime;
        isCharging = false;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    // Hunting
    public void Update() {
        // Rotate towards the player
        Vector2 direction = player.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion targetRotation = Quaternion.AngleAxis(angle, Vector3.forward);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);

        if (isCharging)
        {
            // Charge towards the player
            transform.position += transform.right * chargeSpeed * Time.deltaTime;
            chargeTimer -= Time.deltaTime;

            if (chargeTimer <= 0)
            {
                // Stop charging and start waiting
                isCharging = false;
                waitTimer = waitTime;
            }
        }

        else
        {
            // Wait for the next charge
            waitTimer -= Time.deltaTime;

            if (waitTimer <= 0)
            {
                // Start charging
                isCharging = true;
                chargeTimer = chargeTime;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player")) {
            StartCoroutine(DeathAndRestart(other.transform));
        }
    }

    private IEnumerator DeathAndRestart(Transform player)
    {
        PlayerController playerScript = player.GetComponent<PlayerController>();
        playerScript.isAlive = false;

        Animator playerAnimator = player.GetComponent<Animator>();
        playerAnimator.SetTrigger("dead");
        
        SpriteRenderer playerRenderer = player.GetComponent<SpriteRenderer>();
        playerRenderer.sortingLayerName = "Default";

        Rigidbody2D rb = player.GetComponent<Rigidbody2D>();
        rb.velocity = Vector2.zero;
        rb.isKinematic = true; 

        yield return new WaitForSeconds(2);
        rb.isKinematic = false;
        RestartCurrentScene();
    }
}
