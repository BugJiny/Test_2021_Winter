using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoginManager : MonoBehaviour
{
	//������� ���̵� ����
	public InputField id;

	//������� �н����� ����
	public InputField pw;

	//�˻� �ؽ�Ʈ ����
	public Text notify;


	//���̵�� �н����� ���� �Լ�
	public void SaveuserData()
	{

		if(!CheckInput(id.text,pw.text))
		{
			return;
		}

		//���� �ý��ۿ� ����� �ִ� ���̵� �������� �ʴ´ٸ�...
		if(!PlayerPrefs.HasKey(id.text))
		{
			//������� ���̵�� Ű(key)�� �н����带 ��(value)���� ������ �����Ѵ�.
			PlayerPrefs.SetString(id.text, pw.text);
			notify.text = "���̵� ������ �Ϸ�ƽ��ϴ�.";
			notify.color = Color.green;
		}
		else
		{
			notify.text = "�̹� �����ϴ� ���̵� �Դϴ�.";
			notify.color = Color.red;
		}

		
	}

	//�α��� �Լ�
	public void CheckUserData()
	{
		if (!CheckInput(id.text, pw.text))
		{
			return;
		}

		//����ڰ� �Է��� ���̵� Ű�� ����� �ý��ۿ� ����� ���� �ҷ��´�.
		string pass = PlayerPrefs.GetString(id.text);

		//���� ����ڰ� �Է��� �н������ �ý��ۿ����ҷ��� ���� ���ؼ� ���ٸ�
		if(pw.text ==pass)
		{
			//�α����� �����ؼ� ���������� �̵��Ѵ�.
			SceneManager.LoadScene(1);
		}
		else
		{
			notify.text = "���̵� ��й�ȣ�� ��ġ���� �ʽ��ϴ�.";
			notify.color = Color.red;
		}
	}

	//�Է� �Ϸ� Ȯ�� �Լ�
	bool CheckInput(string id, string pw)
	{
		//���� ���̵�� �н������� �Է¶��� �ϳ��� �����̶�� 
		if(id=="" || pw=="")
		{
			notify.text = "���̵� �н����尡 �����Դϴ�.";
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
		//�˻� �ؽ�Ʈ â�� ����.
		notify.text = "";

	}

    // Update is called once per frame
    void Update()
    {
        
    }
}
