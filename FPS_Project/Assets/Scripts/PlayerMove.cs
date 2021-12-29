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

	//피격 효과 
	public GameObject hitEffect;


	//애니메이터 변수
	Animator anim;

    public void DamageAction(int damage)
    { 
       hp -= damage;


		if(hp>0)
		{
			StartCoroutine(PlayHitEffect());
		}
        Debug.Log("적에게 공격받음 HP = " + hp);
        //print("적에게 공격받음 남은 HP: hp");
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

		//애니메이터 받아오기
		anim = GetComponentInChildren<Animator>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //키보드 입력(GetAxis)값을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

		//이동방향 설정
        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

		//이동 블랜딩 트리를 호출하고 벡터의 크기 값을 넘겨준다.
		anim.SetFloat("MoveMotion", dir.magnitude);

        dir = Camera.main.transform.TransformDirection(dir);


        //점프중일경우 2단점프를 막기 + 캐릭터가 오브젝트에 올라갔다가 내려올떄 중력값에의해 빠르게 떨어지는것을 막는코드
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }


        //InputSystem에서 Jump에 해당하는 값을 눌렀을경우 + 점프중이 아닐경우
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
