using UnityEngine;

public class Bug : MonoBehaviour
{
    private float canvasHeight = 451.2f;
    public float fallSpeed ; 

    void Update()
    {
        if (transform.position.y < -canvasHeight)
        {
            Destroy(gameObject);
        }

        // Make the bug fall faster by modifying its velocity
        GetComponent<Rigidbody>().velocity = new Vector3(0, -fallSpeed, 0);
    }
}
