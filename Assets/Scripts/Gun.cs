
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions.Must;

public class Gun : MonoBehaviour
{
    public enum State
    {
        Ready,
        Empty,
        
    }

    public State state { get; private set; }

    public Transform fireTransform;         //총알이 발사되는 위치

    public ParticleSystem muzzleFlashEffect;
    //public ParticleSystem shellEjectEffect;

    private LineRenderer bulletLineRenderer;
    private AudioSource gunAudioPlayer;

    public GunData gunData;//총의 데이터

    public LayerMask LayerMask;
 

    private float lastFireTime;             //마지막으로 총을 발사한 시간

    private Coroutine coShot;               //총알 궤적 효과 재생을 위한 Coroutine 참조

    private void Awake()
    {
        gunAudioPlayer = GetComponent<AudioSource>();

        bulletLineRenderer = GetComponent<LineRenderer>();

        bulletLineRenderer.positionCount = 2;
        bulletLineRenderer.enabled = false;
    }


 

    public void Fire()
    {
        if (state == State.Ready && Time.time >= lastFireTime + gunData.shotInterval)
        {
            lastFireTime = Time.time;
            Shot();

        }
    }

    public void Shot()
    {
        Ray ray = new Ray(fireTransform.position, fireTransform.forward);//총알이 발사되는 위치와 방향을 나타내는 Ray 생성
        RaycastHit hit;//총알이 맞은 물체에 대한 정보 저장
        Vector3 hitPosition = Vector3.zero;//총알이 맞은 위치
        //Ray가 물체와 충돌했는지 확인
        //if (Physics.Raycast(fireTransform.position, fireTransform.forward, out hit, fireDistance))
        if (Physics.Raycast(ray, out hit))
        {

            //충돌한 물체가 IDamageable 인터페이스를 구현하는지 확인
            //IDamageable target = hit.collider.GetComponent<IDamageable>();
            //if (target != null)
            //{
            //    target.OnDamage(gunData.damage, hit.point, hit.normal);
            //}
            hitPosition = hit.point;

            var tgt = hit.collider.GetComponentInParent<IDamageable>();
            if (tgt != null)
            {
                tgt.OnDamage(gunData.damage, hit.point, hit.normal);//총알이 맞은 위치와 총알이 날아온 방향을 이용하여 데미지 처리
            }


        }
        else
        {
            hitPosition = fireTransform.position + fireTransform.forward;
        }

        if (coShot != null)
        {
            StopCoroutine(coShot);
            coShot = null;
        }

        coShot = StartCoroutine(ShotEffect(hitPosition));//총알 궤적 효과 재생

       


    }


    private IEnumerator ShotEffect(Vector3 hitPosition)
    {

        muzzleFlashEffect.Play();                   //총구 화염 효과 재생

        //shellEjectEffect.Play();                    //탄피 배출 효과 재생

        gunAudioPlayer.PlayOneShot(gunData.shotClip);//총소리 재생

        bulletLineRenderer.SetPosition(0, fireTransform.position);//총알 궤적의 시작점 설정

        bulletLineRenderer.SetPosition(1, hitPosition);//총알 궤적의 끝점 설정

        bulletLineRenderer.enabled = true;              //총알 궤적 효과 활성화

        yield return new WaitForSeconds(0.03f);

        bulletLineRenderer.enabled = false;
    }

    

    private void OnEnable()
    {
       

        state = State.Ready;

        lastFireTime = 0;
    }
}
