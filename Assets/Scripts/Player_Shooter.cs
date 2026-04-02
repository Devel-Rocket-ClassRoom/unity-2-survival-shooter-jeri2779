using TMPro.EditorUtilities;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShooter : MonoBehaviour
{

    public Gun gun;
    public Transform gunPivot;
    //public Transform leftHandMount;
    //public Transform rightHandMount;

    private Player_Input playerInput;
    private Animator animator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<Player_Input>();
        animator = GetComponent<Animator>();
        UpdateUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInput.fire)
        {
            gun.Fire();
            //UpdateUI();
        }
       
    }
    public void UpdateUI()
    {
        if (gun != null)
        {
            //UIManager.Instance.SetAmmoText(gun.magAmmo, gun.ammoRemain);
        }
    }

    private void OnEnable()
    {
        gun.gameObject.SetActive(true);
    }
    private void OnDisable()
    {
        gun.gameObject.SetActive(false);
    }



    //private void OnAnimatorIK(int layerIndex)
    //{
    //    if (gun == null) return;
    //    gunPivot.position = animator.GetIKHintPosition(AvatarIKHint.RightElbow);

    //    //오른손 IK 가중치 설정
    //    animator.SetIKPositionWeight(AvatarIKGoal.RightHand, 1f);
    //    animator.SetIKRotationWeight(AvatarIKGoal.RightHand, 1f);
 

    //    //오른손 위치와 회전을 gun의 오른손 마운트 위치와 회전으로 설정
    //    animator.SetIKPosition(AvatarIKGoal.RightHand, rightHandMount.position);
    //    animator.SetIKRotation(AvatarIKGoal.RightHand, rightHandMount.rotation);

    //}


}
