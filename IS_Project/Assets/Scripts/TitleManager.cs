using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO;

public class TitleManager : MonoBehaviour
{

	//public static int character;

	DataManager data1 = new DataManager();

	private Data data = new Data();

	public GameObject MusicOff_img;
	public GameObject SoundOff_img;
	public GameObject Character_Change;

	private AudioSource touch;
	private AudioClip touch_snd;
	private Sprite[] title = new Sprite[4];  //타이틀 이미지
	private float title_time; //타이틀 시간측정 변수
	private int title_ctrl = 0;  //타이틀 제어변수


	private bool sound_off=false;
	private bool music_off=false;






	public void Sound_Btn()
	{
		if(sound_off)
		{
			sound_off = false;
			SoundOff_img.SetActive(false);

		}
		else
		{
			touch.Play();
			sound_off = true;
			SoundOff_img.SetActive(true);
			
		}


	}


	public void Music_Btn()
	{
		if (music_off)
		{
			GameObject.Find("AudioManager").GetComponent<AudioSource>().volume = 1f;
			music_off = false;
			MusicOff_img.SetActive(false);
		}
		else
		{
			GameObject.Find("AudioManager").GetComponent<AudioSource>().volume = 0f;
			music_off = true;
			MusicOff_img.SetActive(true);
		}



		if (!sound_off)
			touch.Play();

	}

	public void Start_Btn()
	{
		if (!sound_off)
			touch.Play();

		SceneManager.LoadScene("GameScene");
	}

	public void Change_Btn()
	{
		if (!sound_off)
			touch.Play();


		Character_Change.SetActive(true);
		this.gameObject.SetActive(false);
	}

	public void Select_Cheerleader()
	{
		Debug.Log("치어리더 선택");

		//data.Character = 2;
		//File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data));


		//싱글톤 이용
		data1.Character = 2;
		File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data1));

	
		this.gameObject.SetActive(true);
		Character_Change.SetActive(false);
	}

	public void Select_Salaryman()
	{
		Debug.Log("회사원 선택");

		//data.Character = 1;
		//File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data));

		//싱글톤 이용
		data1.Character = 1;
		File.WriteAllText(Application.dataPath + "/TestJson.json", JsonUtility.ToJson(data1));

		
		this.gameObject.SetActive(true);
		Character_Change.SetActive(false);

	}

	// Start is called before the first frame update
	void Start()
    {

		//타이틀 이미지 할당
		title[0] = Resources.Load<Sprite>("Title1");
		title[1] = Resources.Load<Sprite>("Title2");
		title[2] = Resources.Load<Sprite>("Title3");
		title[3] = Resources.Load<Sprite>("Title4");

		//터치 사운드 할당 및 터치 사운드 제어변수 초기화
		touch_snd = Resources.Load<AudioClip>("Touch_Sound");
		touch = GetComponent<AudioSource>();
		touch.clip = touch_snd;



		string str = File.ReadAllText(Application.dataPath + "/TestJson.json");
		data = JsonUtility.FromJson<Data>(str);


		


	}

    // Update is called once per frame
    void Update()
    {
		title_time += Time.deltaTime;

		if(title_time > 0.15)
		{
			title_time = 0;
			title_ctrl++;
			title_ctrl %= 4;

			GetComponent<Image>().sprite = title[title_ctrl];
		}


	}
}
