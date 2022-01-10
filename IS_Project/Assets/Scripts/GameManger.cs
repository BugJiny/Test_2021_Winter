using System.Collections;
using System.Collections.Generic;
using System.IO;  //���� �����ϱ� ���� �ʿ���
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;




[System.Serializable]
public class Data
{
	public int Best_Score;
	public int Coin;
	public int Character;

	//enum Character
	//{
	//	BUSINESSMAN
	//}



	public void printData()
	{
		Debug.Log("Coin:" + Coin);
		Debug.Log("Best_Score:" + Best_Score);
		Debug.Log("Character:" + Character);
	}
}


public class GameManger : MonoBehaviour
{

	public GameObject[] Stairs;
	public Slider LifeTime;
	public Text Count;
	public GameObject score;
	public GameObject Coin;



	private GameObject player;
	private Animator anim;

	private bool reverse = false;
	private int reverse_num = 0;

	private float hp_minus=0.1f;
	private int climbcount=0;
	private bool die=false;

	private Vector2 stairs_maxpos;

	//1��°�� �����Ǵ� ���Ѻ����� ��ġ�� �����ʾ� �ӽ÷� ���
	private bool pos_set=true;


	//������ ���尪��


	private Sprite cheerleader;
	private Sprite salaryman;


	private Data data;
	private int best;
	private int coin_check;
	private int get_coin;
	private bool get=true;  //업데이트에서 코인을 올려주기떄문에 한번만 올라가도록 체크하는 변수.
	string str;

	enum Stairs_Dir
	{
		RIGHT,
		LEFT
			
	}


	private Stairs_Dir[] stairs_dir= new Stairs_Dir[20];

	//���Ѱ�� ����� + ��ܰ� ĳ���� �Ӽ��߰�(Right,left)�ؼ� ���� �Ӽ��϶� �������߸� �����ð��� ��� ī��Ʈ�� �ö�..�ٸ��Ӽ����� �����̸� Die

	//���� ��ǥ �̻� ������ ���(�÷��̾��� ���� �Ⱥ��̰� �� ���)�� ���� ���ʿ� ��ġ��Ŵ(���� ���� ��ġ�� ��� ���� �˰� �־����)








	// Start is called before the first frame update
	void Start()
    {

		//salaryman = (Sprite)Resources.Load("Salaryman",typeof(Sprite));
		//cheerleader = (Sprite)Resources.Load("Cheerleader",typeof(Sprite));
		salaryman = Resources.Load<Sprite>("Salaryman");
		cheerleader = Resources.Load<Sprite>("Cheerleader");




		player = GameObject.Find("Player");
		anim = player.GetComponent<Animator>();

		Count.text = climbcount.ToString();

		stairs_maxpos = Stairs[19].transform.position;

		Coin.transform.position = new Vector3(stairs_maxpos.x, stairs_maxpos.y + 0.35f);

		for (int i=0;i<20;i++)
		{
			if(i >=0 && i<3)
				stairs_dir[i] = Stairs_Dir.RIGHT;
			else if(i>=3 && i<11)
				stairs_dir[i] = Stairs_Dir.LEFT;
			else if(i>=11 && i<17)
				stairs_dir[i] = Stairs_Dir.RIGHT;
			else
				stairs_dir[i] = Stairs_Dir.LEFT;

		}



		str = File.ReadAllText(Application.dataPath + "/TestJson.json");
		data = JsonUtility.FromJson<Data>(str);

		

		data.Character = TitleManager.character;
		coin_check = data.Coin;
		best = data.Best_Score;

		get_coin = 0;



		switch (data.Character)
		{
			case 1:
				player.GetComponent<SpriteRenderer>().sprite = salaryman;
				break;
			case 2:
				player.GetComponent<SpriteRenderer>().sprite = cheerleader;
				break;
		}

		Debug.Log("캐릭터번호:" +data.Character);


		/*
		//data insert
		Data data = new Data();
		data.Best_Score = 0;
		data.Coin = 0;
		data.Character = 0;

		//Object데이터를 JSon형식으로 스트링에 넣음.
		string str = JsonUtility.ToJson(data);
		Debug.Log("ToJson: " + str);



		//JSon형식의 데이터를 다시 오브젝트 형으로 변환.
		Data data2 = JsonUtility.FromJson<Data>(str);
		data2.printData();


		//file save
		Data data3 = new Data();
		data3.Best_Score = 200;
		data3.Coin = 100;
		File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data3));


		//file load
		string str2 = File.ReadAllText(Application.dataPath + "/TestJson.json");
		Data data4 = JsonUtility.FromJson<Data>(str2);
		data4.printData();

		*/



	}



	private void create_stairs(int i)
	{
		if (player.transform.position.y - Stairs[i].transform.position.y >= 2.0f)
		{

			int rand = Random.Range(0, 2);

			//Vector2 v = Stairs[i].transform.position;
			Vector2 v = stairs_maxpos;


			if (pos_set)
			{
				v.y += 0.8f;
				pos_set = false;
			}
			else
				v.y += 0.4f;




			switch ((Stairs_Dir)rand)
			{
				case Stairs_Dir.RIGHT:
					v.x +=0.5f;
					stairs_dir[i] = Stairs_Dir.RIGHT;
					break;

				case Stairs_Dir.LEFT:
					v.x -=0.5f;
					stairs_dir[i] = Stairs_Dir.LEFT;
					break;

			}

			stairs_maxpos = v;
			Stairs[i].transform.position = v;


			//마지막배열(19)의 계단이 지워질때(회수로는 20번째마다) 코인의 위치 변경 및 코인획득 가능하게 
			if (i == 19)
			{
				Coin.SetActive(true);
				Coin.transform.position = new Vector3(stairs_maxpos.x, stairs_maxpos.y + 0.35f);
				Debug.Log("x:" + stairs_maxpos.x);
				Debug.Log("y:" + stairs_maxpos.y);
				get = true;
			}


			
		}
	}

	private void CheckDie()
	{
		int num = climbcount % 20;


		//올라갈려는 계단과 방향이 맞는다면
		if(reverse_num == (int)stairs_dir[num])
		{
			climbcount++;
		}
		else
		{
			die = true;
			anim.SetTrigger("DieAction");
			StartCoroutine("Die");
			Debug.Log("Die");
		}

	}

	IEnumerator Die()
	{
		yield return new WaitForSeconds(1.5f);

		while (true)
		{
			Vector2 vec = player.transform.position;
			vec.y -= 0.1f;

			player.transform.position = vec;

			yield return new WaitForSeconds(0.03f);

			if (player.transform.position.y <= -6)
				break;
		}

		score.SetActive(true);
		score.transform.Find("Score").GetComponent<Text>().text += climbcount;

		if (climbcount >= best)
		{
			best = climbcount;

			data.Best_Score = best;
		}

		score.transform.Find("BestScore").GetComponent<Text>().text += best;
		score.transform.Find("Coin").GetComponent<Text>().text += get_coin;

		coin_check += get_coin;
		data.Coin = coin_check;

		File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data));

		yield return null;


	}


	public void ReStart()
	{
		SceneManager.LoadScene(SceneManager.GetActiveScene().name);
	}
	public void LoadTitle()
	{

		SceneManager.LoadScene("TitleScene");

	}

    // Update is called once per frame
    void Update()
    {

		//코인 획득 조건
		if (climbcount != 0 && climbcount % 20 == 0 && get)
		{
			get = false;
			//코인획득
			get_coin++;
			//코인을 일시적으로 지워줬다가.
			Coin.SetActive(false);
			
		}



		//�ð��� ���� ��������
		LifeTime.value -= Time.deltaTime * hp_minus;


		Count.text = climbcount.ToString();

		//�����ð��� 0�� �Ǿ����� ó��
		if (LifeTime.value <=0 &&  !die )
		{
			die = true;

			anim.SetTrigger("DieAction");

			StartCoroutine("Die");

			Debug.Log("Die");
		}

		


		if(Input.GetKeyDown(KeyCode.Z) && !die)
		{
			//�ִϸ��̼� ����
			anim.SetTrigger("MoveAction");


			//��� ������ ������ �����ð� ȸ��
			LifeTime.value += 0.05f;

			
			//�ְ� ������ ��ܰ� ����(����� �̵���Ű�� ����)
			stairs_maxpos.y -= 0.4f;


			CheckDie();

			//�б⿡ ���� �̵�
			if (reverse)
			{
				for (int i = 0; i < 20; i++)
				{
					create_stairs(i);
					Vector2 vec = Stairs[i].transform.position;
					vec.y += -0.4f;
					vec.x += 0.5f;
					Stairs[i].transform.position = vec;
					

				}
				stairs_maxpos.x += 0.5f;

				Vector2 vec1 = Coin.transform.position;
				vec1.y += -0.4f;
				vec1.x += 0.5f;
				Coin.transform.position = vec1;

			}
			else
			{

				for (int i = 0; i < 20; i++)
				{
					create_stairs(i);
					Vector2 vec = Stairs[i].transform.position;
					vec.y += -0.4f;
					vec.x += -0.5f;
					Stairs[i].transform.position = vec;
					
				}
				stairs_maxpos.x += -0.5f;


				Vector2 vec1 = Coin.transform.position;
				vec1.y += -0.4f;
				vec1.x += -0.5f;
				Coin.transform.position = vec1;


			}



		}
		else if(Input.GetKeyDown(KeyCode.X) && !die)
		{

			//��� ������ȯ ������ �����ð� ȸ��
			LifeTime.value += 0.05f;

			//�ְ� ������ ��ܰ� ����(����� �̵���Ű�� ����)
			stairs_maxpos.y -= 0.4f;

			
			if (reverse)
			{
				reverse = false;
				reverse_num = 0;
				player.GetComponent<SpriteRenderer>().flipX=false;
				for (int i = 0; i < 20; i++)
				{
					create_stairs(i);
					Vector2 vec = Stairs[i].transform.position;
					vec.y += -0.4f;
					vec.x += -0.5f;
					Stairs[i].transform.position = vec;
					
				}
				stairs_maxpos.x += -0.5f;


				Vector2 vec1 = Coin.transform.position;
				vec1.y += -0.4f;
				vec1.x += -0.5f;
				Coin.transform.position = vec1;

			}
			else
			{
				reverse = true;
				reverse_num = 1;
				player.GetComponent<SpriteRenderer>().flipX = true;
				

				for (int i = 0; i < 20; i++)
				{
					create_stairs(i);
					Vector2 vec = Stairs[i].transform.position;
					vec.y += -0.4f;
					vec.x += 0.5f;
					Stairs[i].transform.position = vec;
					
				}
				stairs_maxpos.x += 0.5f;


				Vector2 vec1 = Coin.transform.position;
				vec1.y += -0.4f;
				vec1.x += 0.5f;
				Coin.transform.position = vec1;

			}

			CheckDie();


		}
	}
}
