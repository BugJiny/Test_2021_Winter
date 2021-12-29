using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{


	//무기 모드 변수

	enum WeaponMode
	{
		Normal,
		Sniper
	}
	WeaponMode wMode;

    public GameObject bulletEffect;
    ParticleSystem ps;

    public GameObject firePosition;
    public GameObject bombFactory;

    public float throwPower = 15f;


    public int weaponPower = 5;


	//애니메이터 변수
	Animator anim;

	//카메라 확대 확인용 변수
	bool ZoomMode = false;

	//무기 모드 텍스트
	public Text wModeText;

	//총 발사 효과 오브젝트 배열

	public GameObject[] eff_Flash;

	//무기 아이콘 스프라이트 변수
	public GameObject weapon01;
	public GameObject weapon02;

	//크로스헤어 스프라이트 변수
	public GameObject crosshair01;
	public GameObject crosshair02;

	//마우스 오른쪽 버튼 클릭 아이콘 스프라이트 변수
	public GameObject weapon01_R;
	public GameObject weapon02_R;

	//마우스 우클릭 줌모드 스프라이트 변수
	public GameObject crosshair02_zoom;



    // Start is called before the first frame update
    void Start()
    {

        ps = bulletEffect.GetComponent<ParticleSystem>();

		//애니메이트 컴포넌트 가져오기
		anim = GetComponentInChildren<Animator>();

		//무기 기본 모드를 노멀 모드로 설정한다.
		wMode = WeaponMode.Normal;

		wModeText.text = "Normal Mode";

	}

    // Update is called once per frame
    void Update()
    {

		if (GameManager.gm.gState != GameManager.GameState.Run)
		{
			return;
		}

	
		//만약 키보드의 숫자 1번을 입력받으면, 무기모드를 일반 모드로 변경한다.

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			wMode = WeaponMode.Normal;

			//카메라의 화면을 다시 원래대로 돌려준다.
			Camera.main.fieldOfView = 60f;

			//일반 모드 텍스트 출력
			wModeText.text = "Normal Mode";

			weapon01.SetActive(true);
			crosshair01.SetActive(true);
			weapon01_R.SetActive(true);

			weapon02.SetActive(false);
			crosshair02.SetActive(false);
			weapon02_R.SetActive(false);
			crosshair02_zoom.SetActive(false);

			Camera.main.fieldOfView = 60f;
			ZoomMode = false;



		}
		else if(Input.GetKeyDown(KeyCode.Alpha2))
		{
			wMode = WeaponMode.Sniper;

			//스나이퍼 모드 텍스트 출력
			wModeText.text = "Sniper Mode";

			weapon01.SetActive(false);
			crosshair01.SetActive(false);
			weapon01_R.SetActive(false);

			weapon02.SetActive(true);
			crosshair02.SetActive(true);
			weapon02_R.SetActive(true);
		}

		//마우스 왼쪽버튼을 클릭시 레이발사
		if (Input.GetMouseButtonDown(0))
        {


			//총 이펙트를 실시한다.
			StartCoroutine(ShootEffectOn(0.05f));

			if(anim.GetFloat("MoveMotion") == 0)
			{
				anim.SetTrigger("Attack");
			}

            Ray ray = new Ray(Camera.main.transform.position, Camera.main.transform.forward);
            RaycastHit hitInfo = new RaycastHit();

            if(Physics.Raycast(ray,out hitInfo))
            {

                if (hitInfo.transform.gameObject.layer == LayerMask.NameToLayer("Enemy"))
                {
                    EnemyFSM eFSM = hitInfo.transform.GetComponent<EnemyFSM>();
                    eFSM.HitEnemy(weaponPower);
                }
                else
                {
                    bulletEffect.transform.position = hitInfo.point;

                    bulletEffect.transform.forward = hitInfo.normal;
                    ps.Play();
                }

                
            }
        }

		//노멀모드 : 마우스 오른쪽버튼을 누르면 시선방향으로 수류탄을 던지고 싶다.
		//스나이퍼 모드: 마우스 오른쪽 버튼을 누르면 화면을 확대하고 싶다.
		//마우스 오른쪽버튼을 클릭시 폭탄 투척.
		if (Input.GetMouseButtonDown(1))
        {

			switch(wMode)
			{
				case WeaponMode.Normal:
					GameObject bomb = Instantiate(bombFactory);
					bomb.transform.position = firePosition.transform.position;

					Rigidbody rb = bomb.GetComponent<Rigidbody>();

					rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);

					break;

				case WeaponMode.Sniper:
					//만약, 줌 모드 상태가 아니라면 카메라를 확대하고 줌 모드 상태로 전환한다.
					if(!ZoomMode)
					{
						Camera.main.fieldOfView = 15f;
						ZoomMode = true;
						crosshair02_zoom.SetActive(true);
						crosshair02.SetActive(false);
					}
					else
					{
						Camera.main.fieldOfView = 60f;
						ZoomMode = false;
						crosshair02_zoom.SetActive(false);
						crosshair02.SetActive(true);
					}

					break;
			}
          
        }

		

	


	}


	IEnumerator ShootEffectOn(float duration)
	{


		//숫자를 랜덤하게 뽑는다.
		int num = Random.Range(0, eff_Flash.Length - 1);

		//이펙트 오브젝트 배열에서 뽑힌 숫자에 해당하는 이펙트 오브젝트를 활성화 한다.
		eff_Flash[num].SetActive(true);

		//지정한 시간만큼 기다린다.
		yield return new WaitForSeconds(duration);

		//이펙트 오브젝트를 다시 비활성화 한다.
		eff_Flash[num].SetActive(false);
	}


}
