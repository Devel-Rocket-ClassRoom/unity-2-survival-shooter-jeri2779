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
        Transform cam = Camera.main.transform;
        Vector3 ws = new Vector3(cam.forward.x, 0f, cam.forward.z).normalized;
        Vector3 ad   = new Vector3(cam.right.x,   0f, cam.right.z).normalized;

        Vector3 moveDir = ws * playerInput.move + ad * playerInput.strafe;
        if (moveDir.sqrMagnitude > 0.001f)
            rb.MovePosition(rb.position + moveDir.normalized * moveSpeed * Time.fixedDeltaTime);
    }

    private void Rotate()//마우스 좌표를 향해 플레이어가 회전
    {
        Vector3 lookDir = playerInput.mouseWorldPosition - transform.position;
        lookDir.y = 0f;
        if (lookDir.sqrMagnitude > 0.001f)
        {
            rb.MoveRotation(Quaternion.LookRotation(lookDir));
        }
    }
}

