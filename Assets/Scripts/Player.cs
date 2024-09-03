using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public float moveSpeed;
    Rigidbody rb;
    private bool isCollidingWithWall = false;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous; // Set collision detection to continuous
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 screenCenter = new Vector2(Screen.width / 2, Screen.height / 2);
            Vector2 mousePosition = Input.mousePosition;

            // Allow movement if not colliding with a wall or if the click is away from the wall
            if (!isCollidingWithWall || (isCollidingWithWall && mousePosition.x < screenCenter.x && rb.velocity.x >= 0) || (isCollidingWithWall && mousePosition.x > screenCenter.x && rb.velocity.x <= 0))
            {
                if (mousePosition.x < screenCenter.x)
                {
                    rb.AddForce(Vector3.left * moveSpeed, ForceMode.Impulse);
                }
                else
                {
                    rb.AddForce(Vector3.right * moveSpeed, ForceMode.Impulse);
                }
            }
        }
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isCollidingWithWall = true;
            rb.velocity = Vector3.zero; // Stop the player's movement immediately
        }
        else if (collision.gameObject.tag == "Bug")
        {
            SceneManager.LoadScene(0);
        }
    }

    void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.tag == "Wall")
        {
            isCollidingWithWall = false; // Allow movement again when not colliding
        }
    }

  
}
