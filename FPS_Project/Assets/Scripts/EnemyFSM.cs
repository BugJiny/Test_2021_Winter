using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.AI; 

public class EnemyFSM : MonoBehaviour
{
    

    //���ʹ� ���� ���

    enum EnemyState
    {
        Idle,
        Move,
        Attack,
        Return,
        Damaged,
        Die
    }

    //���ʹ� ���� ����
    EnemyState m_State;

    //�÷��̾� �߰߹���
    public float findDistance = 8f;

    //���� ���� ����
    public float attackDistance = 2f;

    //�̵��ӵ�
    public float moveSpeed = 5f;

    //���ʹ� ���ݷ�
    public int attackPower = 3;

    //�̵��Ÿ�
    public float moveDistance = 20f;

    //���ʹ� ü��
    public int hp = 15;

    CharacterController cc;

    //�÷��̾� Ʈ������
    Transform player;

    //�����ð�
    float currentTime = 0;

    //���� ������ �ð�
    float attackDelay = 2f;

    Vector3 originPos;
	Quaternion originRot;

    int maxHP=15;

	public Slider hpSlider;

	//�ִϸ����� ����
	Animator anim;

	//������̼� ������Ʈ ����
	NavMeshAgent smith;


    public void HitEnemy(int hitPower)
    {

        if (m_State ==EnemyState.Damaged || m_State == EnemyState.Die || m_State ==EnemyState.Return)
        {
            return;
        }

        //�÷��̾��� ���ݷ¸�ŭ ���ʹ��� ü���� ���ҽ�Ų��.
        hp -= hitPower;

		//������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
		smith.isStopped = true;
		smith.ResetPath();

        if(hp > 0)
        {
            m_State = EnemyState.Damaged;
            print("���� ��ȯ: Any State -> Damaged");

			//�ǰ� �ִϸ��̼��� �÷��� �Ѵ�.
			anim.SetTrigger("Damaged");
            Damaged();
        }
        else
        {
            m_State = EnemyState.Die;
            print("���� ��ȯ: Any State -> Die");
			//���� �ִϸ��̼��� �÷����Ѵ�.
			anim.SetTrigger("Die");
            Die();
        }
    }
    void Idle()
    {


        if (Vector3.Distance(transform.position, player.position) < findDistance)
        {
            m_State = EnemyState.Move;
            print("���� ��ȯ: Idle -> Move");

			anim.SetTrigger("IdleToMove");
        }

		
    }



    void Move()
    {
        //�����Ÿ��̻� �־����� �ٽ� ���ƿ����ϱ�
        if (Vector3.Distance(transform.position, player.position) > moveDistance)
        {

            m_State = EnemyState.Return;
            print("���� ��ȯ: Move -> Return");
        }
        //�÷��̾���� �Ÿ��� ���ݹ��� ���̶�� �÷��̾ ���� �̵��Ѵ�.
        else if (Vector3.Distance(transform.position, player.position) > attackDistance)
        {

			//�̵����� ����
			//Vector3 dir = (player.position - transform.position).normalized;

			//ĳ���� ��Ʈ�ѷ��� �̿��� �̵��ϱ�
			//cc.Move(dir * moveSpeed * Time.deltaTime);

			//�÷��̾ ���� ������ ��ȯ�Ѵ�.
			//transform.forward = dir;


			//������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
			smith.isStopped = true;
			smith.ResetPath();


			//������̼����� �����ϴ� �ּ� �Ÿ��� ���� ���� �Ÿ��� �����Ѵ�.
			smith.stoppingDistance = attackDistance;

			//������̼��� �������� �÷������� ��ġ�� �����Ѵ�.
			smith.destination = player.position;

        }
        else
        {


            m_State = EnemyState.Attack;
            print("���� ��ȯ : Move -> Attack");

			//���� �ð��� ���� ������ �ð���ŭ �̸� ������� ���´�.
			currentTime = attackDelay;

			//���� ��� �ִϸ��̼� �÷���
			anim.SetTrigger("MoveToAttackDelay");
        }


    }

    void Attack()
    {

        //�÷��̾ ���ݹ����� �ִٸ�.
        if (Vector3.Distance(transform.position, player.position) < attackDistance)
        {
            currentTime += Time.deltaTime;
            if (currentTime > attackDelay)
            {
                //player.GetComponent<PlayerMove>().DamageAction(attackPower);
                print("����");
                currentTime = 0;

				//���� �ִϸ��̼� �÷���
				anim.SetTrigger("StartAttack");
            }
        }
        else
        {

            m_State = EnemyState.Move;
            print("���� ��ȯ: Attack -> Move");
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

        //�ʱ� ��ġ���� �Ÿ��� 0.1f �̻��̶�� �ʱ���ġ ������ �̵�
        if (Vector3.Distance(transform.position, originPos) > 0.1f)
        {
			//Vector3 dir = (originPos - transform.position).normalized;
			//cc.Move(dir * moveSpeed * Time.deltaTime);

			//transform.forward = dir;

			//������̼��� �������� �ʱ� ����� ��ġ�� �����Ѵ�.
			smith.destination = originPos;

			//������̼����� �����ϴ� �ּ� �Ÿ��� '0'���� �����Ѵ�.
			smith.stoppingDistance = 0;
        }
        else
        {
			//������̼� ������Ʈ�� �̵��� ���߰� ��θ� �ʱ�ȭ�Ѵ�.
			smith.isStopped = true;
			smith.ResetPath();

            transform.position = originPos;
			transform.rotation = originRot;
            hp = maxHP;
            m_State = EnemyState.Idle;
            print("���� ��ȯ: Return -> Idle");

			//��� �ִϸ��̼����� ��ȯ�ϴ� Ʈ�������� ȣ���Ѵ�.
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

        //ĳ���� ��Ʈ�ѷ� ������Ʈ �޾ƿ���
        cc = GetComponent<CharacterController>();

        maxHP = hp;

		originPos = transform.position;
		originRot = transform.rotation;

		//�ִϸ����� ���� �Ҵ�
		anim = transform.GetComponentInChildren<Animator>();

		//������̼� ������Ʈ ������Ʈ �޾ƿ���
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

        //�ǰ� ��� �ð���ŭ ��ٸ���
        yield return new WaitForSeconds(1.0f);

        //���� ���¸� �̵� ���·� ��ȯ�Ѵ�.
        m_State = EnemyState.Move;
        print("���� ��ȯ: Damaged -> Move");


    }

    IEnumerator DieProcess()
    {
        //ĳ���� ��Ʈ�ѷ� ������Ʈ�� ��Ȱ��ȭ ��Ŵ
        cc.enabled = false;

        //2�� ���� ��ٸ� �Ŀ� �ڱ� �ڽ��� �����Ѵ�.
        yield return new WaitForSeconds(2f);
        print("�Ҹ�!");
        Destroy(gameObject);

    }


}
