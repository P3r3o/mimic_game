using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

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
    public AudioSource deathCry;
    public AudioSource breathe;
    public AudioSource wake;
    public GameObject gameOverScreen;

    void Start() {
        wake.Play();
        chargeTimer = chargeTime;
        waitTimer = waitTime;
        isCharging = false;
        player = GameObject.Find("Player");
        playerScript = player.GetComponent<PlayerController>();
    }

    void OnEnable() {
        // destroy all furniture
        GameObject[] otherObjects = GameObject.FindGameObjectsWithTag("Furniture");

        foreach (GameObject obj in otherObjects) {
            Destroy(obj);
        }
        StartCoroutine(spookyDarkness());
    }
    IEnumerator spookyDarkness()
    {
        GameObject.Find("Canvas/darkness").GetComponent<Image>().color = new Color(0, 0, 0, 1 );

        yield return new WaitForSeconds(0.2f);

        GameObject.Find("Canvas/darkness").GetComponent<Image>().color = new Color(0, 0, 0, 0);
    }

    // Hunting
    public void Update() {
        if (!wake.isPlaying && !breathe.isPlaying) {
            breathe.Play();
        }

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
            StartCoroutine(DeathScreen(other.transform));
        }
    }

    private IEnumerator DeathScreen(Transform player)
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
        playerScript.canvasGameObject.SetActive(true);
        Cursor.visible = true;
    }

    void OnDestroy() {
        breathe.Stop();
    }
}
