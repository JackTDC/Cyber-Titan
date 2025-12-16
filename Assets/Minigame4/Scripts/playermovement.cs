using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    public float speed = 6f;
    Rigidbody rb;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        float move = Input.GetAxis("Horizontal");
        Vector3 movement = new Vector3(move * speed, rb.linearVelocity.y, 0);
        rb.linearVelocity = movement;
    }

    void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Enemy"))
        {
            Debug.Log("GAME OVER");
            Time.timeScale = 0f;
        }
    }
}
