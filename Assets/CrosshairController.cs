using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CrosshairController : MonoBehaviour
{
    public Sprite inactiveCrosshair;
    public Sprite activeCrosshair;
    private SpriteRenderer spriteRenderer;

    void Start() 
    {
        Cursor.visible = false;
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        Vector3 mouseWorldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0; 
        transform.position = mouseWorldPosition;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Furniture") || other.gameObject.CompareTag("Monster")) {
            spriteRenderer.sprite = activeCrosshair;
        }
    }

    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Furniture") || other.gameObject.CompareTag("Monster")) {
            spriteRenderer.sprite = inactiveCrosshair;
        }
    }
}
