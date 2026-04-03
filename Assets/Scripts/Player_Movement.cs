using UnityEngine;

public class Player_Movement : MonoBehaviour
{
     

    public float moveSpeed = 5f;
    private Player_Input playerInput;

    private Rigidbody rb;
    private Animator animator;

    void Start()
    {
        playerInput = GetComponent<Player_Input>();
        rb = GetComponent<Rigidbody>();
        animator = GetComponent<Animator>();

        //Cursor.lockState = CursorLockMode.Locked;
    }

    private void FixedUpdate()
    {
        Rotate();
        Move();
    }

    private void Update()
    {
        Vector2 inputVec = new Vector2(playerInput.move, playerInput.strafe);
        bool isMoving = inputVec.sqrMagnitude > 0f;
        animator.SetBool("isMove", isMoving);
    }

    private void Move()
    {
        Vector3 moveDir = new Vector3(playerInput.strafe, 0f, playerInput.move);

        if (moveDir.sqrMagnitude > 0.001f)
            rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

 

    // PlayerMovement.cs Rotate()
    private void Rotate()
    {
        //Debug.Log($"aimDirection: {playerInput.aimDirection}");
        if (playerInput.aimDirection.sqrMagnitude > 0.001f)
        {
            rb.MoveRotation(Quaternion.LookRotation(playerInput.aimDirection));
        }
    }
}

