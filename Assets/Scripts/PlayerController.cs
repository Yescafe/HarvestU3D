using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject groundSurface;
    public Transform groundSurfaceParent;
    public float speed;

    private Rigidbody rigidbody;
    // Start is called before the first frame update

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        for (int i = -10; i <= 10; i++)
        {
            for (int j = -10; j <= 10; j++)
            {
                Instantiate(groundSurface, new Vector3(i, 0.3f, j), Quaternion.identity, groundSurfaceParent);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        rigidbody.AddForce(movement * speed);
    }
}
