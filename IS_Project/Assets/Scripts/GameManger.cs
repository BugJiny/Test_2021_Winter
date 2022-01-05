using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManger : MonoBehaviour
{

	public GameObject[] Stairs;
	public Slider LifeTime;
	public Text Count;


	private GameObject player;
	private Animator anim;

	private bool reverse = false;

	private float hp_minus=0.1f;
	private int climbcount=0;

	


	//무한계단 만들기 + 계단과 캐릭터 속성추가(Right,left)해서 같은 속성일때 움직여야만 생명시간과 계단 카운트가 올라감..다른속성으로 움직이면 Die

	//일정 좌표 이상 내려간 계단(플레이어의 눈에 안보이게 된 계단)을 제일 위쪽에 배치시킴(제일 높은 위치의 계단 값을 알고 있어야함)





	private void Awake()
	{
		player = GameObject.Find("Player");

		anim = player.GetComponent<Animator>();

		Count.text = climbcount.ToString();

	}


	// Start is called before the first frame update
	void Start()
    {

		

    }



    // Update is called once per frame
    void Update()
    {
		//시간에 따른 생명감소
		LifeTime.value -= Time.deltaTime * hp_minus;


		Count.text = climbcount.ToString();

		//생명시간이 0이 되었을때 처리
		if (LifeTime.value <0)
		{
			//GameOver
			Application.Quit();
		}


		if(Input.GetKeyDown(KeyCode.Z))
		{
			//애니메이션 시작
			anim.SetTrigger("MoveAction");


			//계단 오르기 성공시 생명시간 회복
			LifeTime.value += 0.05f;


			//계단 오른 카운트를 증가
			climbcount++;

			//분기에 따른 이동
			if (reverse)
			{
				for (int i = 0; i < 20; i++)
				{
					Vector2 vec = Stairs[i].transform.position;
					vec.x += 0.5f;
					vec.y += -0.4f;
					Stairs[i].transform.position = vec;

				}
			}
			else
			{

				

				for (int i = 0; i < 20; i++)
				{
					Vector2 vec = Stairs[i].transform.position;
					vec.x += -0.5f;
					vec.y += -0.4f;
					Stairs[i].transform.position = vec;

				}

			}



			

		}
		else if(Input.GetKeyDown(KeyCode.X))
		{

			//계단 방향전환 성공시 생명시간 회복
			LifeTime.value += 0.05f;


			//계단 오른 카운트를 증가
			climbcount++;

			if (reverse)
			{
				reverse = false;
				player.GetComponent<SpriteRenderer>().flipX=false;

				for (int i = 0; i < 20; i++)
				{
					Vector2 vec = Stairs[i].transform.position;
					vec.x += -0.5f;
					vec.y += -0.4f;
					Stairs[i].transform.position = vec;

				}

			}
			else
			{
				reverse = true;
				player.GetComponent<SpriteRenderer>().flipX = true;


				for (int i = 0; i < 20; i++)
				{
					Vector2 vec = Stairs[i].transform.position;
					vec.x += 0.5f;
					vec.y += -0.4f;
					Stairs[i].transform.position = vec;

				}

			}


			

		}


		


	}
}
