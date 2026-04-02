using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.AI;


public class Enemy : lifeManager
{

    public enum Status { Idle, Trace, Attack, Die }
    public Transform target;

    public ParticleSystem hitEffect;

    private NavMeshAgent navMeshAgent;
    private Animator animator;

    public AudioSource audio;
    public AudioClip hitSound;
    public AudioClip deathSound;

    //public Collider collider;


    public float attackDistance = 1.5f;
    public float attackDelay = 1f;

    private float lastAttackTime;

    private float damage;
    public int score;

    public float sinkSpeed = 0.5f;

    private Status currentStatus;

    //public Renderer render;

    public Status CurrentStatus
    {
        get { return currentStatus; }
        set
        {
            var PrevStatus = currentStatus;
            currentStatus = value;
            Debug.Log($"{currentStatus}");

            switch (currentStatus)
            {
                case Status.Idle:
                    animator.SetBool("isChase", false);
                    navMeshAgent.isStopped = true;
                    break;
                case Status.Trace:
                    animator.SetBool("isChase", true);
                    navMeshAgent.isStopped = false;
                    break;
                case Status.Attack:
                    animator.SetBool("isChase", false);
                    navMeshAgent.isStopped = true;
                    break;
                case Status.Die:
                    Debug.Log($"[Enemy] Die: {name} 사망 처리 시작");
                    animator.SetBool("dead",true);
                    navMeshAgent.isStopped = true;
                    navMeshAgent.enabled = false;
                    foreach (var col in GetComponents<Collider>())
                    {
                        col.isTrigger = true;
                    }
                    audio.enabled = false;
                 
                    //gameObject.SetActive(false);
                    break;
            }
        }
    }

    private void Awake()
    {
        if (navMeshAgent == null)
            navMeshAgent = GetComponent<NavMeshAgent>();
        if (animator == null)
            animator = GetComponent<Animator>();
        if (audio == null)
            audio = GetComponent<AudioSource>();
    }

    public void SetUp(EnemyData data)
    {
        if (navMeshAgent == null) navMeshAgent = GetComponent<NavMeshAgent>();
        if (animator == null)    animator    = GetComponent<Animator>();
        if (audio == null)       audio       = GetComponent<AudioSource>();

        gameObject.SetActive(false);

        startingHealth = data.health;
        health = data.health;
        damage = data.damage;
        score = data.score;
        navMeshAgent.speed = data.speed;
        gameObject.SetActive(true);

    }
    protected override void OnEnable()
    {
        base.OnEnable();
        navMeshAgent.enabled = true;
        navMeshAgent.isStopped = false;
        navMeshAgent.ResetPath();


        //현재 위치에서 반경 10의 범위 내에서 NavMesh 상의 가장 가까운 위치를 찾음
        if (NavMesh.SamplePosition(transform.position, out NavMeshHit hit, 10f, NavMesh.AllAreas))
        {
            navMeshAgent.Warp(hit.position);
        }

        target = FindPlayer();
        CurrentStatus = target != null ? Status.Trace : Status.Idle;
    }
    private void OnDisable()
    {
        if (navMeshAgent != null && navMeshAgent.enabled)
            navMeshAgent.enabled = false;
    }

    private void Update()
    {
        switch (currentStatus)
        {
            case Status.Idle:
                UpdateIdle();
                break;
            case Status.Trace:
                UpdateTrace();
                break;
            case Status.Attack:
                UpdateAttack();
                break;
            case Status.Die:
                UpdateDie();
                break;
        }
    }


    private void UpdateDie()
    {
        // 사망 처리는 CurrentStatus = Status.Die 진입 시 1회 완료됨
    }

    private void UpdateAttack()
    {
        if (target == null)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        var distance = Vector3.Distance(target.position, transform.position);
        if (distance > attackDistance)
        {
            CurrentStatus = Status.Trace;
            return;
        }

        var lookAt = target.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);

        if (Time.time > lastAttackTime + attackDelay)
        {

            lastAttackTime = Time.time;
            var Enemy = target.GetComponentInParent<lifeManager>();
            if (Enemy != null)
            {
                if (!Enemy.isDead)
                {
                    Enemy.OnDamage(10f, transform.position, -transform.forward);
                    //CurrentStatus = Status.Idle;
                    //return;
                }
            }
        }
    }


    private void UpdateTrace()
    {
        if (target == null)
        {
            CurrentStatus = Status.Idle;
            return;
        }

        var lifeComp = target.GetComponentInParent<lifeManager>();
        if (lifeComp != null && lifeComp.isDead)
        {
            target = null;
            CurrentStatus = Status.Idle;
            return;
        }

        float distance = Vector3.Distance(target.position, transform.position);
        if (distance <= attackDistance)
        {
            CurrentStatus = Status.Attack;
            return;
        }

        navMeshAgent.SetDestination(target.position);
    }

    private void UpdateIdle()
    {
        target = FindPlayer();
        if (target != null)
            CurrentStatus = Status.Trace;
    }

    private Transform FindPlayer()
    {
        var player = FindObjectOfType<Player_Health>();
        return (player != null && !player.isDead) ? player.transform : null;
    }

    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
    {
        if (!isDead && hitEffect != null)
        {
            hitEffect.transform.position = hitPoint;
            hitEffect.transform.forward = hitNormal;
            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
            hitEffect.Play();
        }
        base.OnDamage(damage, hitPoint, hitNormal);
    }
    public override void Die()
    {
        base.Die(); // isDead = true, OnDead 이벤트 발생
        CurrentStatus = Status.Die; // 사망 애니메이션 및 정리
    }

    public void StartSinking()
    {
        Debug.Log($"[Enemy] StartSinking 이벤트 수신! 가라앉기를 시작합니다.");
        // 바닥으로 가라앉는 코루틴 실행
        StartCoroutine(SinkCoroutine());
    }

    private IEnumerator SinkCoroutine()
    {
        foreach (var col in GetComponents<Collider>())
        {
            col.enabled = false;
        }
        float sinkDuration = 2.5f;
        float timer = 0f;

        while (timer < sinkDuration)
        {
            transform.position += Vector3.down * (sinkSpeed * Time.deltaTime);

            //transform.Translate(Vector3.down * sinkSpeed * Time.deltaTime);
            timer += Time.deltaTime;
            yield return null;  
        }

      
        Debug.Log($"[Enemy] 가라앉기 완료. 오브젝트를 비활성화합니다.");
        gameObject.SetActive(false);
    }
}

//좀비의 행동과 관련된 스크립트입니다. NavMeshAgent를 사용하여 플레이어를 추적하고, 공격하는 기능을 구현합니다.
//public class Zombie : LivingEntity
//{

//    public LayerMask whatIsTarget;
//    private LivingEntity targetEntity;
//    private NavMeshAgent navMeshAgent;

//    public ParticleSystem hitEffect;
//    public AudioClip hitSound;
//    public AudioClip deathSound;

//    private Animator zombieAnimator;
//    private AudioSource zombieAudio;

//    private Renderer zombieRenderer;

//    public float damage = 20f;

//    private float lastAttackTime;


//    private bool hasTarget 
//    { get 
//        { if (targetEntity != null && !targetEntity.dead) 
//            { return true; }return false;  
//        }  
//    }

//    private void Awake()
//    {
//        navMeshAgent = GetComponent<NavMeshAgent>();
//        zombieAnimator = GetComponent<Animator>();
//        zombieAudio = GetComponent<AudioSource>();
//        zombieRenderer = GetComponentInChildren<Renderer>();
//    }
//    public void SetUp(zombieData zombieData)
//    {
//        startingHealth = zombieData.health;//최대 체력 설정
//        health = zombieData.health;//체력 설정
//        damage = zombieData.damage;//공격력 설정
//        navMeshAgent.speed = zombieData.speed;
//        zombieRenderer.material.color = zombieData.skinColor;
//    }

//    public override void OnDamage(float damage, Vector3 hitPoint, Vector3 hitNormal)
//    {
//        if (!dead)
//        {
//            hitEffect.transform.position = hitPoint;
//            hitEffect.transform.rotation = Quaternion.LookRotation(hitNormal);
//            hitEffect.Play();
//            zombieAudio.PlayOneShot(hitSound);
//        }
//        base.OnDamage(damage, hitPoint, hitNormal);
//    }

//    public override void Die()
//    {
//        base.Die();
//        Collider[] zombieColliders = GetComponents<Collider>();
//        for(int i = 0; i < zombieColliders.Length; i++)
//        {
//            zombieColliders[i].enabled = false;
//        }

//        if (navMeshAgent.isOnNavMesh)
//            navMeshAgent.isStopped = true;
//        else
//            Debug.LogWarning($"[Zombie] Die: {name} isOnNavMesh=false, isStopped 스킵");

//        navMeshAgent.enabled = false;

//        zombieAnimator.SetTrigger("Die");
//        zombieAudio.PlayOneShot(deathSound);
//        Debug.Log($"[Zombie] {name} 사망 처리 완료");
//    }

//    void Start()
//    {
//        Debug.Log($"[Zombie] {name} Start: UpdatePath 코루틴 시작 / isOnNavMesh={navMeshAgent.isOnNavMesh}");
//        StartCoroutine(UpdatePath());
//    }

//    private IEnumerator UpdatePath()
//    {
//        while (!dead)
//        {
//            if(hasTarget)
//            {
//                if (navMeshAgent.isOnNavMesh)
//                {
//                    navMeshAgent.isStopped = false;
//                    navMeshAgent.SetDestination(targetEntity.transform.position);
//                }
//                else
//                {
//                    Debug.LogWarning($"[Zombie] {name} UpdatePath: 타겟 추적 중 isOnNavMesh=false");
//                }
//            }
//            else
//            {
//                if (navMeshAgent.isOnNavMesh)
//                    navMeshAgent.isStopped = true;//목적지 설정 중지
//                else
//                    Debug.LogWarning($"[Zombie] {name} UpdatePath: 대기 중 isOnNavMesh=false");

//                Collider[] colliders = Physics.OverlapSphere(transform.position, 20f, whatIsTarget);
//                //반경 20의 구체 영역에서 whatIsTarget 레이어에 해당하는 콜라이더를 모두 가져옴

//                for (int i = 0; i < colliders.Length; i++)//가져온 콜라이더들을 순회하면서
//                {
//                    LivingEntity livingEntity = colliders[i].GetComponent<LivingEntity>();//콜라이더에서 LivingEntity 컴포넌트를 가져옴
//                    if (livingEntity != null && !livingEntity.dead)//LivingEntity 컴포넌트가 존재하고, 해당 LivingEntity가 죽지 않았다면
//                    {
//                        targetEntity = livingEntity;//해당 LivingEntity를 타겟으로 설정
//                        break;
//                    }
//                }
//            }
//            yield return new WaitForSeconds(0.25f);

//        }

//    }

//    private void OnTriggerStay(Collider other)
//    {
//        if(!dead && Time.time >= lastAttackTime)
//        {
//            LivingEntity attackTarget = other.GetComponent<LivingEntity>();
//            if(attackTarget != null && attackTarget == targetEntity)
//            {
//                lastAttackTime = Time.time;
//                Vector3 hitPoint = other.ClosestPoint(transform.position);
//                Vector3 hitNormal = transform.position - other.transform.position;
//                attackTarget.OnDamage(damage, hitPoint, hitNormal);
//            }
//        }
//    }

//    // Update is called once per frame
//    void Update()
//    {
//        zombieAnimator.SetBool("HasTarget", hasTarget);
//    }
//}
