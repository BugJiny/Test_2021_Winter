using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
	//사용자의 아이디 변수
	public InputField id;

	//사용자의 패스워드 변수
	public InputField pw;

	//검사 텍스트 변수
	public Text notify;


	//아이디와 패스워드 저장 함수
	public void SaveuserData()
	{

		if(!CheckInput(id.text,pw.text))
		{
			return;
		}

		//만약 시스템에 저장돼 있는 아이디가 존재하지 않는다면...
		if(!PlayerPrefs.HasKey(id.text))
		{
			//사용자의 아이디는 키(key)로 패스워드를 값(value)으로 설정해 저장한다.
			PlayerPrefs.SetString(id.text, pw.text);
			notify.text = "아이디 생성이 완료됐습니다.";
			notify.color = Color.green;
		}
		else
		{
			notify.text = "이미 존재하는 아이디 입니다.";
			notify.color = Color.red;
		}

		
	}

	//로그인 함수
	public void CheckUserData()
	{
		if (!CheckInput(id.text, pw.text))
		{
			return;
		}

		//사용자가 입력한 아이디를 키로 사용해 시스템에 저장된 값을 불러온다.
		string pass = PlayerPrefs.GetString(id.text);

		//만약 사용자가 입력한 패스워드와 시스템에서불러온 값을 비교해서 같다면
		if(pw.text ==pass)
		{
			//로그인이 성공해서 다음씬으로 이동한다.
			SceneManager.LoadScene(1);
		}
		else
		{
			notify.text = "아이디나 비밀번호가 일치하지 않습니다.";
			notify.color = Color.red;
		}
	}

	//입력 완료 확인 함수
	bool CheckInput(string id, string pw)
	{
		//만약 아이디와 패스워드의 입력란이 하나라도 공백이라면 
		if(id=="" || pw=="")
		{
			notify.text = "아이디나 패스워드가 공백입니다.";
			notify.color = Color.red;
			return false;
		}
		else
		{
			return true;
		}

		

	}

    // Start is called before the first frame update
    void Start()
    {
		//검사 텍스트 창을 비운다.
		notify.text = "";

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
