using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI; 

public class EnemyFSM : MonoBehaviour
{
    

    //에너미 상태 상수

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    //에너미 상태 변수
    EnemyState m_State;

    //플레이어 발견범위
    public float findDistance = 8f;

    //공격 가능 범위
    public float attackDistance = 2f;

    //이동속도
    public float moveSpeed = 5f;

    //에너미 공격력
    public int attackPower = 3;

    //이동거리
    public float moveDistance = 20f;

    //에너미 체력
    public int hp = 15;

    CharacterController cc;

    //플레이어 트랜스폼
    Transform player;

    //누적시간
    float currentTime = 0;

    //공격 딜레이 시간
    float attackDelay = 2f;

    Vector3 originPos;
	Quaternion originRot;

    int maxHP=15;

	public Slider hpSlider;

	//애니메이터 변수
	Animator anim;

	//내비게이션 에이전트 변수
	NavMeshAgent smith;


    public void HitEnemy(int hitPower)
    {

        if (m_State ==EnemyState.Damaged || m_State == EnemyState.Die || m_State ==EnemyState.Return)
        {
            return;
        }

        //플레이어의 공격력만큼 에너미의 체력을 감소시킨다.
        hp -= hitPower;

		//내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
		smith.isStopped = true;
		smith.ResetPath();

        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("상태 전환: Any State -> Damaged");

			//피격 애니메이션을 플레이 한다.
			anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("상태 전환: Any State -> Die");
			//죽음 애니메이션을 플레이한다.
			anim.SetTrigger("Die");
            Die();
        }
    }
    void Idle()
    {


        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("상태 전환: Idle -> Move");

			anim.SetTrigger("IdleToMove");
        }

		
    }



    void Move()
    {
        //일정거리이상 멀어질시 다시 돌아오게하기
        if (Vector3.Distance(transform.position, player.position) > moveDistance)
        {

            m_State = EnemyState.Return;
            print("상태 전환: Move -> Return");
        }
        //플레이어와의 거리가 공격범위 밖이라면 플레이어를 향해 이동한다.
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {

			//이동방향 설정
			//Vector3 dir = (player.position - transform.position).normalized;

			//캐릭터 콘트롤러를 이용해 이동하기
			//cc.Move(dir * moveSpeed * Time.deltaTime);

			//플레이어를 향해 방향을 전환한다.
			//transform.forward = dir;


			//내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
			smith.isStopped = true;
			smith.ResetPath();


			//내비게이션으로 접근하는 최소 거리를 공격 가능 거리로 설정한다.
			smith.stoppingDistance = attackDistance;

			//내비게이션의 목적지를 플레어이의 위치로 설정한다.
			smith.destination = player.position;

        }
        else
        {


            m_State = EnemyState.Attack;
            print("상태 전환 : Move -> Attack");

			//누적 시간을 공격 딜레이 시간만큼 미리 진행시켜 놓는다.
			currentTime = attackDelay;

			//공격 대기 애니메이션 플레이
			anim.SetTrigger("MoveToAttackDelay");
        }


    }

    void Attack()
    {

        //플레이어가 공격범위에 있다면.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("공격");
                currentTime = 0;

				//공격 애니메이션 플레이
				anim.SetTrigger("StartAttack");
            }
        }
        else
        {

            m_State = EnemyState.Move;
            print("상태 전환: Attack -> Move");
            currentTime = attackDelay;

			anim.SetTrigger("AttackToMove");
        }
    }

	public void AttackAction()
	{
		player.GetComponent<PlayerMove>().DamageAction(attackPower);
	}

    void Return()
    {

        //초기 위치에서 거리가 0.1f 이상이라면 초기위치 쪽으로 이동
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
			//Vector3 dir = (originPos - transform.position).normalized;
			//cc.Move(dir * moveSpeed * Time.deltaTime);

			//transform.forward = dir;

			//내비게이션의 목적지를 초기 저장된 위치로 설정한다.
			smith.destination = originPos;

			//내비게이션으로 접근하는 최소 거리를 '0'으로 설정한다.
			smith.stoppingDistance = 0;
        }
        else
        {
			//내비게이션 에이전트의 이동을 멈추고 경로를 초기화한다.
			smith.isStopped = true;
			smith.ResetPath();

            transform.position = originPos;
			transform.rotation = originRot;
            hp = maxHP;
            m_State = EnemyState.Idle;
            print("상태 전환: Return -> Idle");

			//대기 애니메이션으로 전환하는 트랜지션을 호출한다.
			anim.SetTrigger("MoveToIdle");
        }


    }

    void Damaged()
    {

        StartCoroutine(DamageProcess());
    }

    void Die()
    {
        StopAllCoroutines();

        StartCoroutine(DieProcess());
    }


    // Start is called before the first frame update
    void Start()
    {
        m_State = EnemyState.Idle;

        player = GameObject.Find("Player").transform;

        //캐릭터 콘트롤러 컴포넌트 받아오기
        cc = GetComponent<CharacterController>();

        maxHP = hp;

		originPos = transform.position;
		originRot = transform.rotation;

		//애니메이터 변수 할당
		anim = transform.GetComponentInChildren<Animator>();

		//내비게이션 에이전트 컴포넌트 받아오기
		smith = GetComponent<NavMeshAgent>();
    }


 
    // Update is called once per frame
    void Update()
    {

        switch (m_State)
        {


            case EnemyState.Idle:
                Idle();
                break;
            case EnemyState.Move:
                Move(); 
                break;
            case EnemyState.Attack:
                Attack();
                break;
            case EnemyState.Return:
                Return();
                break;
            case EnemyState.Damaged:
                //Damaged();
                break;
            case EnemyState.Die:
                //Die();
                break;

        }

		hpSlider.value = (float)hp / (float)maxHP;

    }

    IEnumerator DamageProcess()
    {

        //피격 모션 시간만큼 기다리기
        yield return new WaitForSeconds(1.0f);

        //현재 상태를 이동 상태로 전환한다.
        m_State = EnemyState.Move;
        print("상태 전환: Damaged -> Move");


    }

    IEnumerator DieProcess()
    {
        //캐릭터 콘트롤러 컴포넌트를 비활성화 시킴
        cc.enabled = false;

        //2초 동안 기다린 후에 자기 자신을 제거한다.
        yield return new WaitForSeconds(2f);
        print("소멸!");
        Destroy(gameObject);

    }


}
