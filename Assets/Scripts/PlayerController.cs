using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerController : MonoBehaviour
{

    public TextMeshProUGUI bulletsRemainingText;
    public TextMeshProUGUI bulletsInChamberText;
    public bool movingLeft = true;
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;
    public Camera cam;
    public int bulletsRemaining = 10; 
    public int bulletsInChamber = 2;
    public AudioSource gunSound;

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        bulletsRemainingText.text = "Bullets Remaining: " + bulletsRemaining.ToString();
        bulletsInChamberText.text = "Bullets in Chamber: " + bulletsInChamber.ToString();
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
        if (Input.GetButtonDown("Fire1") && bulletsInChamber > 0)
        {
            shoot();
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit)) {
                if (hit.collider.gameObject.CompareTag("Monster"))
                {
                    Debug.Log("Clicked on object with tag 'Monster'");
                }
            }   
            bulletsInChamber--;
            
        }
        if (Input.GetKeyDown(KeyCode.R)) {
            reload();
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
            Debug.Log("baby get!");
        }    
    }

    private void shoot()
    {
        gunSound.Play();
        //add lighting effect
    }
    public void reload()
    {
        if (bulletsInChamber < 2 && bulletsRemaining > 0){
            bulletsInChamber = 2;
            bulletsRemaining -= 2;
        }
    }
}
