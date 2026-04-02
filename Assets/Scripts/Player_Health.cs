using UnityEngine;
using UnityEngine.UI;

public class Player_Health : lifeManager
{
    //public Image uiHealth; //피격 시 화면에 표시할 이미지


    public Slider healthSlider;     //체력 슬라이더

    public AudioClip deathClip;     //사망시 재생할 오디오 클립
    public AudioClip hitClip;       //피격시 재생할 오디오 클립
     

    private AudioSource audioPlayer;
    private Animator animator;

    private Player_Movement playerMovement;
    private PlayerShooter playerShooter;




    private void Awake()
    {
        audioPlayer = GetComponent<AudioSource>();
        animator = GetComponent<Animator>();
        playerMovement = GetComponent<Player_Movement>();
        playerShooter = GetComponent<PlayerShooter>();
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        //uiHealth.gameObject.SetActive(true);
        //uiHealth.fillAmount = 1f;


        healthSlider.gameObject.SetActive(true);
        healthSlider.maxValue = startingHealth;

        healthSlider.value = health;

        playerMovement.enabled = true;
        playerShooter.enabled = true;


    }

  

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitDirection)
    {
        if (!isDead && audioPlayer != null && hitClip != null)
        {
            audioPlayer.PlayOneShot(hitClip);
        }
        Debug.Log("Player took damage: " + damage);

        base.OnDamage(damage, hitPoint, hitDirection);//기본적인 데미지 처리
        healthSlider.value = health;//체력 슬라이더 갱신

        //uiHealth.fillAmount = health / startingHealth;//화면에 표시되는 체력 이미지 갱신
    }

    public override void Die()
    {
        base.Die();

        //uiHealth.gameObject.SetActive(false);
        UIManager.Instance.SetActiveGameOverUi(true);

        healthSlider.gameObject.SetActive(false);

        audioPlayer.PlayOneShot(deathClip);//사망 사운드 재생
        animator.SetTrigger("Die");//사망 애니메이션 재생

        playerMovement.enabled = false; //플레이어 이동 스크립트 비활성화
        playerShooter.enabled = false; //플레이어 슈터 스크립트 비활성화




    }

   
}

