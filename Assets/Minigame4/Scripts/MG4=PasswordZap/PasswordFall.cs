using UnityEngine;

public class PasswordFall : MonoBehaviour
{
    public float speed = 200f;

    void Update()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);
    }
}
