using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public TextMeshProUGUI bulletsRemainingText;
    public GameObject bloodSplatter;
    public Animator animator;
    public bool movingLeft = true;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    public Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;
    public Camera cam;
    public int bulletsRemaining = 8; 
    public int bulletsInChamber = 2;
    public bool isReloading = false; 
    public AudioSource gunSound;
    public AudioSource reloadSound;
    public AudioSource discardShellsSound;
    public AudioSource deathCry;
    public AudioSource babyGet;
    public AudioSource doorOpen;

    public bool killedMonsterInRoom = false;

    public bool justGotBaby = false;
    public bool isAlive = true;

    public GameObject canvasGameObject;

    public CameraShake cameraShake;
    
    void Update()
    {
        bulletsRemainingText.text = bulletsRemaining.ToString();

        if (isAlive) {
            if (bulletsInChamber <= 0 && bulletsRemaining <= 0) {
                isReloading = true;
            }

            if (bulletsRemaining < 0) {
                bulletsRemaining = 0;
            }

            animator.SetBool("isShooting", false);
            bool isMoving = Input.GetKey(KeyCode.W);
            animator.SetBool("isWalking", isMoving);

            if (isMoving)
            {
                movement = (mousePos - rb.position).normalized;
            }

            else
            {
                movement = Vector2.zero;
            }

            mousePos = cam.ScreenToWorldPoint(Input.mousePosition);

            if (Input.GetButtonDown("Fire1") && bulletsInChamber > 0 && !movingLeft)
            {   
                RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
                
                if (hit.collider != null && (hit.collider.gameObject.CompareTag("Monster") || hit.collider.gameObject.CompareTag("Furniture")))
                {
                    Shoot();
                    StartCoroutine(cameraShake.Shake(0.15f, 0.4f));

                    // If you shot a monster
                    if (hit.collider.gameObject.CompareTag("Monster")) {
                        Vector3 hitPosition = hit.collider.gameObject.transform.position;
                        Vector3 playerPosition = transform.position;
                        Vector3 fromPlayerToHit = (hitPosition - playerPosition).normalized;
                        Quaternion bloodSplatterRotation = Quaternion.FromToRotation(Vector3.up, fromPlayerToHit) * Quaternion.Euler(0, 0, 90);
                        GameObject blood = Instantiate(bloodSplatter, new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, -1), bloodSplatterRotation);
                        blood.transform.SetParent(hit.collider.gameObject.transform.parent, false);
                        Destroy(hit.collider.gameObject);
                        killedMonsterInRoom = true;
                        isReloading = false;
                        deathCry.Play();
                    }
                }
            }
        }
    }

    void FixedUpdate()
    {
        if (isAlive) {
            rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
            Vector2 lookDir = mousePos - rb.position;
            float finalAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; 
            float lerpAngle = Mathf.LerpAngle(rb.rotation, finalAngle, rotationSpeed * Time.fixedDeltaTime);
            rb.rotation = lerpAngle;
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Baby") && !justGotBaby)
        {
            babyGet.Play();
            Animator babyAnimator = other.gameObject.transform.GetComponent<Animator>();
            babyAnimator.SetBool("taken", true);
            movingLeft = false;
            animator.SetBool("hasBaby", true);
            justGotBaby = true;
        }    
    }

    private void Shoot()
    {
        animator.SetBool("hasBaby", false);
        animator.SetBool("isShooting", true);
        gunSound.Play();
        bulletsInChamber--;

        if (bulletsInChamber <= 0 && bulletsRemaining > 0) {
            discardShellsSound.Play();
            StartCoroutine(Reload());
        }
    }

    public IEnumerator Reload()
    {
        isReloading = true;
        bulletsRemaining -= 2;

        yield return new WaitForSeconds(5f);

        if (isAlive && bulletsInChamber == 0) {
            if (bulletsInChamber < 2 && bulletsRemaining > 0){
                bulletsInChamber = 2;
            }

            reloadSound.Play();
            isReloading = false;
        }
    }
}
