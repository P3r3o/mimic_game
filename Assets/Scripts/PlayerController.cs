using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public TextMeshProUGUI bulletsRemainingText;
    public TextMeshProUGUI bulletsInChamberText;
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

    public bool killedMonsterInRoom = false;

    public bool justGotBaby = false;
    
    void Update()
    {
        animator.SetBool("isShooting", false);
        bool isMoving = Input.GetKey(KeyCode.W);
        animator.SetBool("isWalking", isMoving);


        bulletsRemainingText.text = "Bullets Remaining: " + bulletsRemaining.ToString();
        bulletsInChamberText.text = "Bullets in Chamber: " + bulletsInChamber.ToString();

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

                // If you shot a monster
                if (hit.collider.gameObject.CompareTag("Monster")) {
                    Vector3 hitPosition = hit.collider.gameObject.transform.position;
                    Vector3 playerPosition = transform.position;
                    Vector3 fromPlayerToHit = (hitPosition - playerPosition).normalized;
                    Quaternion bloodSplatterRotation = Quaternion.FromToRotation(Vector3.up, fromPlayerToHit) * Quaternion.Euler(0, 0, 90);
                    GameObject blood = Instantiate(bloodSplatter, new Vector3(hit.collider.gameObject.transform.position.x, hit.collider.gameObject.transform.position.y, 0), bloodSplatterRotation);
                    blood.transform.SetParent(hit.collider.gameObject.transform.parent, false);
                    Destroy(hit.collider.gameObject);
                    killedMonsterInRoom = true;
                }
            }
        }
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float finalAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; 
        float lerpAngle = Mathf.LerpAngle(rb.rotation, finalAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.rotation = lerpAngle;
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Baby"))
        {
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

        if (bulletsInChamber <= 0) {
            StartCoroutine(Reload());
        }

        //add lighting effect
    }
    public IEnumerator Reload()
    {
        isReloading = true;

        yield return new WaitForSeconds(5f);

        if (bulletsInChamber < 2 && bulletsRemaining > 0){
            bulletsInChamber = 2;
            bulletsRemaining -= 2;
        }

        isReloading = false;
    }
}
