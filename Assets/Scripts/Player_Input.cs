using UnityEngine;

public class Player_Input : MonoBehaviour
{

    public static readonly int HashMove = Animator.StringToHash("Move");
    public static readonly string moveAxis = "Vertical";
    public static readonly string strafeAxis = "Horizontal";
    public static readonly string fireButton = "Fire1";
    //public static readonly string reloadButton = "reload";

    public float move { get; private set; }
    public float strafe { get; private set; }       // A/D 횟이동
    public bool fire { get; private set; }
    //public bool reload { get; private set; }
    public Vector3 mouseWorldPosition { get; private set; } // 마우스 월드 좌표

    private void Update()
    {
       

        move = Input.GetAxisRaw(moveAxis);
        strafe = Input.GetAxisRaw(strafeAxis);
        fire = Input.GetButton(fireButton);
        //reload = Input.GetButton(reloadButton);

        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); //마우스 위치에서 카메라를 향해 Ray 생성
        if(Physics.Raycast(ray, out RaycastHit hitInfo))
        {
            mouseWorldPosition = hitInfo.point; // Ray가 충돌한 지점의 월드 좌표
        }   


    }
}

