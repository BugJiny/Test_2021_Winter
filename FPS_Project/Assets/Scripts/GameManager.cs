using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{

	public static GameManager gm;
	// Start is called before the first frame update



	public GameObject gameLabel;

	Text gameText;


	PlayerMove player;

	


	public enum GameState
	{
		Ready,
		Run,
		Pause,
		GameOver

	}

	public GameState gState;


	//�ɼ�ȭ�� UI ������Ʈ ����
	public GameObject gameOption;



	//�ɼ�ȭ�� �ѱ�
	public void OpenOptionWindow()
	{

		//�ɼ� â�� Ȱ��ȭ �Ѵ�.
		gameOption.SetActive(true);
		//���Ӽӵ��� 0������� ��ȯ�Ѵ�.
		Time.timeScale = 0f;
		//���ӻ��¸� �Ͻ����� ���·� �����Ѵ�.
		gState = GameState.Pause;
	}

	//����ϱ� �ɼ�
	public void CloseOptionWindow()
	{

		//�ɼ� â�� ��Ȱ��ȭ �Ѵ�.
		gameOption.SetActive(false);
		//���Ӽӵ��� 1������� ��ȯ�Ѵ�.
		Time.timeScale = 1f;
		//���ӻ��¸� ���� ������ �����Ѵ�.
		gState = GameState.Run;

	}

	//�ٽ��ϱ� �ɼ�
	public void RestartGame()
	{
		//���� �ӵ��� 1������� ��ȯ�Ѵ�.
		Time.timeScale = 1f;

		//���� �� ��ȣ�� �ٽ� �ε��Ѵ�.
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		//�ε� ȭ�� ���� �ε��Ѵ�.
		SceneManager.LoadScene(1);



	}

	//���� ���� �ɼ�
	public void QuitGame()
	{
		//���ø����̼��� �����Ѵ�.
		Application.Quit();

	}

	private void Awake()
	{

		if (gm == null)
		{
			gm = this;
		}
	}

	void Start()
    {

		gState = GameState.Ready;

		gameText = gameLabel.GetComponent<Text>();

		gameText.text = "Ready...";

		gameText.color = new Color32(255, 185, 0, 255);

		StartCoroutine(ReadyToStart());

		player = GameObject.Find("Player").GetComponent<PlayerMove>();

	}

	IEnumerator ReadyToStart()
	{
		yield return new WaitForSeconds(2f);

		gameText.text = "Go!";

		yield return new WaitForSeconds(0.5f);

		gameLabel.SetActive(false);

		gState = GameState.Run;

	}
    // Update is called once per frame
    void Update()
    {

		if (Input.GetKeyDown(KeyCode.Escape))
			Application.Quit();

        if(player.hp <=0)
		{

			//�÷��̾��� �ִϸ��̼��� �����
			player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

			//���� �ؽ�Ʈ�� Ȱ��ȭ �Ѵ�.
			gameLabel.SetActive(true);
			//���� �ؽ�Ʈ�� ���� ����
			gameText.text = "Game Over";
			//���� �ؽ�Ʈ�� ���� ����
			gameText.color = new Color32(255, 0, 0, 255);

			//���� �ؽ�Ʈ�� �ڽ� ������Ʈ�� Ʈ���� �� ������Ʈ�� �����´�.
			Transform buttons = gameText.transform.GetChild(0);

			//��ư ������Ʈ�� Ȱ��ȭ �Ѵ�.
			buttons.gameObject.SetActive(true);

			
			//���� ���¸� '���� ����'�� ����
			gState = GameState.GameOver;

			

			
		}
		

		
    }
}
