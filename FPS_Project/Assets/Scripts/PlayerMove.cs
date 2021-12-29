using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 7f;
    public float jumpPower = 2f;
    public bool isJumping = false;

    public int hp=20;

	int maxHp = 20;

	public Slider hpSlider;

    CharacterController cc;

    float gravity = -8f;

    float yVelocity = 0;

	//�ǰ� ȿ�� 
	public GameObject hitEffect;


	//�ִϸ����� ����
	Animator anim;

    public void DamageAction(int damage)
    { 
       hp -= damage;


		if(hp>0)
		{
			StartCoroutine(PlayHitEffect());
		}
        Debug.Log("������ ���ݹ��� HP = " + hp);
        //print("������ ���ݹ��� ���� HP: hp");
    }

	IEnumerator PlayHitEffect()
	{

		hitEffect.SetActive(true);

		yield return new WaitForSeconds(0.3f);

		hitEffect.SetActive(false);
	}

    // Start is called before the first frame update
    void Start()
    {

        cc = GetComponent<CharacterController>();

		//�ִϸ����� �޾ƿ���
		anim = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //Ű���� �Է�(GetAxis)���� �޴´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

		//�̵����� ����
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

		//�̵� ���� Ʈ���� ȣ���ϰ� ������ ũ�� ���� �Ѱ��ش�.
		anim.SetFloat("MoveMotion", dir.magnitude);

        dir = Camera.main.transform.TransformDirection(dir);


        //�������ϰ�� 2�������� ���� + ĳ���Ͱ� ������Ʈ�� �ö󰬴ٰ� �����Ë� �߷°������� ������ �������°��� �����ڵ�
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }


        //InputSystem���� Jump�� �ش��ϴ� ���� ��������� + �������� �ƴҰ��
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }


        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);

		hpSlider.value = (float)hp / (float)maxHp;

		if (GameManager.gm.gState != GameManager.GameState.Run)
		{
			return;
		}

	}
}
