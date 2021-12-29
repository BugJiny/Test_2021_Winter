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


	//옵션화면 UI 오브젝트 변수
	public GameObject gameOption;



	//옵션화면 켜기
	public void OpenOptionWindow()
	{

		//옵션 창을 활성화 한다.
		gameOption.SetActive(true);
		//게임속도를 0배속으로 전환한다.
		Time.timeScale = 0f;
		//게임상태를 일시정지 상태로 변경한다.
		gState = GameState.Pause;
	}

	//계속하기 옵션
	public void CloseOptionWindow()
	{

		//옵션 창을 비활성화 한다.
		gameOption.SetActive(false);
		//게임속도를 1배속으로 전환한다.
		Time.timeScale = 1f;
		//게임상태를 게임 중으로 변경한다.
		gState = GameState.Run;

	}

	//다시하기 옵션
	public void RestartGame()
	{
		//게임 속도를 1배속으로 전환한다.
		Time.timeScale = 1f;

		//현재 씬 번호를 다시 로드한다.
		//SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

		//로딩 화면 씬을 로드한다.
		SceneManager.LoadScene(1);



	}

	//게임 종료 옵션
	public void QuitGame()
	{
		//애플리케이션을 종료한다.
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

			//플레이어의 애니메이션을 멈춘다
			player.GetComponentInChildren<Animator>().SetFloat("MoveMotion", 0f);

			//상태 텍스트를 활성화 한다.
			gameLabel.SetActive(true);
			//상태 텍스트의 내용 변경
			gameText.text = "Game Over";
			//상태 텍스트의 색상 변경
			gameText.color = new Color32(255, 0, 0, 255);

			//상태 텍스트의 자식 오브젝트의 트랜스 폼 컴포넌트를 가져온다.
			Transform buttons = gameText.transform.GetChild(0);

			//버튼 오브젝트를 활성화 한다.
			buttons.gameObject.SetActive(true);

			
			//게임 상태를 '게임 오버'로 변겅
			gState = GameState.GameOver;

			

			
		}
		

		
    }
}
