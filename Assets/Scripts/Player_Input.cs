using UnityEngine;

public class Player_Input : MonoBehaviour
{

    public static readonly int HashMove = Animator.StringToHash("Move");
    public static readonly string moveAxis = "Vertical";
    public static readonly string strafeAxis = "Horizontal";
    public static readonly string fireButton = "Fire1";
    //public static readonly string reloadButton = "reload";
    [SerializeField] private Vector3 _aimDebug;
    public float move { get; private set; }
    public float strafe { get; private set; }       // A/D 횟이동
    public bool fire { get; private set; }

    private int floorMask;

    public Vector3 aimDirection { get; private set; }
    //public bool reload { get; private set; }
    //public Vector3 mouseWorldPosition { get; private set; } // 마우스 월드 좌표
    
    private void Awake()
    {
        floorMask = LayerMask.GetMask("Ground");
    }

    private void Update()
    {
       
        move = Input.GetAxisRaw(moveAxis);
        strafe = Input.GetAxisRaw(strafeAxis);
        fire = Input.GetButton(fireButton);




        //Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 위치에서 카메라를 향해 Ray 생성
        //if(Physics.Raycast(ray, out RaycastHit hitInfo))
        //{
        //    mouseWorldPosition = hitInfo.point; // Ray가 충돌한 지점의 월드 좌표
        //}


        //// 화면상 플레이어 위치
        //Vector3 playerScreen = Camera.main.WorldToScreenPoint(transform.position);



        //Vector3 dir = Input.mousePosition - playerScreen;
        //dir.z = 0f;

        //if (dir.sqrMagnitude > 0.001f)
        //{

        //    Transform cam = Camera.main.transform;
        //    // TransformDirection 제거 — 화면 x,y를 월드 x,z로 직접 매핑
        //    Vector3 worldDir = new Vector3(dir.normalized.x, 0f, dir.normalized.y);
        //    aimDirection = worldDir.normalized;
        //    _aimDebug = aimDirection; // 디버그용
        //}

        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out RaycastHit hit, 100f, floorMask))
        {
            Debug.DrawRay(ray.origin, ray.direction * 100f, Color.blue);
            Vector3 playerToMouse = hit.point - transform.position;
            playerToMouse.y = 0f;
            aimDirection = playerToMouse.normalized;
        }


    }
}

