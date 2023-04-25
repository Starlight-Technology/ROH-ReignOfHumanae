using UnityEngine;

public class PlayerMoviments : MonoBehaviour
{
    private Rigidbody body;
    private float velocity;
    private bool isGrounded;

    public GameObject Camera;

    // Start is called before the first frame update
    private void Start()
    {
        body = gameObject.GetComponent<Rigidbody>();
        body.freezeRotation = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (isGrounded)
        {
            velocity = Input.GetKey(KeyCode.LeftShift) ? 7.0f : 3.2f;

            if (Input.GetKey(KeyCode.W))
            {
                MoveForward();
            }

            if (Input.GetKey(KeyCode.S))
            {
                MoveBackward();
            }

            if (Input.GetKey(KeyCode.D))
            {
                MoveRight();
            }

            if (Input.GetKey(KeyCode.A))
            {
                MoveLeft();
            }

            if (Input.GetKey(KeyCode.Space))
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        body.AddRelativeForce(new Vector3(0, velocity * 2, 0), ForceMode.VelocityChange);
    }

    private void MoveForward()
    {
        body.AddRelativeForce(new Vector3(0, 0, velocity), ForceMode.VelocityChange);
        SetFront();
    }

    private void MoveBackward()
    {
        body.AddRelativeForce(new Vector3(0, 0, -velocity), ForceMode.VelocityChange);
        SetFront();
    }

    private void MoveRight()
    {
        body.AddRelativeForce(new Vector3(velocity, 0, 0), ForceMode.VelocityChange);
        SetFront();
    }

    private void MoveLeft()
    {
        body.AddRelativeForce(new Vector3(-velocity, 0, 0), ForceMode.VelocityChange);
        SetFront();
    }

    private void SetFront()
    {
        gameObject.transform.rotation = Quaternion.Euler(new Vector3(0, Camera.transform.eulerAngles.y, 0));
    }

    private void OnCollisionEnter(Collision theCollision)
    {
        if (theCollision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision theCollision)
    {
        if (theCollision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}
