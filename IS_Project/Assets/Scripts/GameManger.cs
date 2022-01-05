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

	


	//���Ѱ�� ����� + ��ܰ� ĳ���� �Ӽ��߰�(Right,left)�ؼ� ���� �Ӽ��϶� �������߸� ����ð��� ��� ī��Ʈ�� �ö�..�ٸ��Ӽ����� �����̸� Die

	//���� ��ǥ �̻� ������ ���(�÷��̾��� ���� �Ⱥ��̰� �� ���)�� ���� ���ʿ� ��ġ��Ŵ(���� ���� ��ġ�� ��� ���� �˰� �־����)





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
		//�ð��� ���� ������
		LifeTime.value -= Time.deltaTime * hp_minus;


		Count.text = climbcount.ToString();

		//����ð��� 0�� �Ǿ����� ó��
		if (LifeTime.value <0)
		{
			//GameOver
			Application.Quit();
		}


		if(Input.GetKeyDown(KeyCode.Z))
		{
			//�ִϸ��̼� ����
			anim.SetTrigger("MoveAction");


			//��� ������ ������ ����ð� ȸ��
			LifeTime.value += 0.05f;


			//��� ���� ī��Ʈ�� ����
			climbcount++;

			//�б⿡ ���� �̵�
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

			//��� ������ȯ ������ ����ð� ȸ��
			LifeTime.value += 0.05f;


			//��� ���� ī��Ʈ�� ����
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
