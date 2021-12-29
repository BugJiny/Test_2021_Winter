using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerFire : MonoBehaviour
{


	//���� ��� ����

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


	//�ִϸ����� ����
	Animator anim;

	//ī�޶� Ȯ�� Ȯ�ο� ����
	bool ZoomMode = false;

	//���� ��� �ؽ�Ʈ
	public Text wModeText;

	//�� �߻� ȿ�� ������Ʈ �迭

	public GameObject[] eff_Flash;

	//���� ������ ��������Ʈ ����
	public GameObject weapon01;
	public GameObject weapon02;

	//ũ�ν���� ��������Ʈ ����
	public GameObject crosshair01;
	public GameObject crosshair02;

	//���콺 ������ ��ư Ŭ�� ������ ��������Ʈ ����
	public GameObject weapon01_R;
	public GameObject weapon02_R;

	//���콺 ��Ŭ�� �ܸ�� ��������Ʈ ����
	public GameObject crosshair02_zoom;



    // Start is called before the first frame update
    void Start()
    {

        ps = bulletEffect.GetComponent<ParticleSystem>();

		//�ִϸ���Ʈ ������Ʈ ��������
		anim = GetComponentInChildren<Animator>();

		//���� �⺻ ��带 ��� ���� �����Ѵ�.
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

	
		//���� Ű������ ���� 1���� �Է¹�����, �����带 �Ϲ� ���� �����Ѵ�.

		if(Input.GetKeyDown(KeyCode.Alpha1))
		{
			wMode = WeaponMode.Normal;

			//ī�޶��� ȭ���� �ٽ� ������� �����ش�.
			Camera.main.fieldOfView = 60f;

			//�Ϲ� ��� �ؽ�Ʈ ���
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

			//�������� ��� �ؽ�Ʈ ���
			wModeText.text = "Sniper Mode";

			weapon01.SetActive(false);
			crosshair01.SetActive(false);
			weapon01_R.SetActive(false);

			weapon02.SetActive(true);
			crosshair02.SetActive(true);
			weapon02_R.SetActive(true);
		}

		//���콺 ���ʹ�ư�� Ŭ���� ���̹߻�
		if (Input.GetMouseButtonDown(0))
        {


			//�� ����Ʈ�� �ǽ��Ѵ�.
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

		//��ָ�� : ���콺 �����ʹ�ư�� ������ �ü��������� ����ź�� ������ �ʹ�.
		//�������� ���: ���콺 ������ ��ư�� ������ ȭ���� Ȯ���ϰ� �ʹ�.
		//���콺 �����ʹ�ư�� Ŭ���� ��ź ��ô.
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
					//����, �� ��� ���°� �ƴ϶�� ī�޶� Ȯ���ϰ� �� ��� ���·� ��ȯ�Ѵ�.
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


		//���ڸ� �����ϰ� �̴´�.
		int num = Random.Range(0, eff_Flash.Length - 1);

		//����Ʈ ������Ʈ �迭���� ���� ���ڿ� �ش��ϴ� ����Ʈ ������Ʈ�� Ȱ��ȭ �Ѵ�.
		eff_Flash[num].SetActive(true);

		//������ �ð���ŭ ��ٸ���.
		yield return new WaitForSeconds(duration);

		//����Ʈ ������Ʈ�� �ٽ� ��Ȱ��ȭ �Ѵ�.
		eff_Flash[num].SetActive(false);
	}


}
