using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f;
    public float rotationSpeed = 2f;
    private Rigidbody2D rb;
    private Vector2 movement;
    private Vector2 mousePos;
    public Camera cam;
    public int bulletsRemaining = 10; 
    public int bulletsInChamber = 2;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        mousePos = cam.ScreenToWorldPoint(Input.mousePosition);
    }

    void FixedUpdate()
    {
        rb.MovePosition(rb.position + movement * moveSpeed * Time.fixedDeltaTime);
        Vector2 lookDir = mousePos - rb.position;
        float finalAngle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f; 
        float lerpAngle = Mathf.LerpAngle(rb.rotation, finalAngle, rotationSpeed * Time.fixedDeltaTime);
        rb.rotation = lerpAngle;
    }
}
