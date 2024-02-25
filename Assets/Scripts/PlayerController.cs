using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public TextMeshProUGUI bulletsRemainingText;
    public TextMeshProUGUI bulletsInChamberText;
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

        if (Input.GetButtonDown("Fire1") && bulletsInChamber > 0)
        {   
            RaycastHit2D hit = Physics2D.Raycast(mousePos, Vector2.zero);
            
            if (hit.collider != null && (hit.collider.gameObject.CompareTag("Monster") || hit.collider.gameObject.CompareTag("Furniture")))
            {
                Shoot();

                // If you shot a monster
                if (hit.collider.gameObject.CompareTag("Monster")) {
                    Destroy(hit.collider.gameObject);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Baby"))
        {
            Destroy(other.gameObject);
            movingLeft = false;
            animator.SetBool("hasBaby", true);
            Debug.Log("baby get!");
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
