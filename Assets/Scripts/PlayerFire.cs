using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFire : MonoBehaviour
{


    public GameObject bulletEffect;
    ParticleSystem ps;

    public GameObject firePosition;
    public GameObject bombFactory;

    public float throwPower = 15f;


    public int weaponPower = 5;


    // Start is called before the first frame update
    void Start()
    {

        ps = bulletEffect.GetComponent<ParticleSystem>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //마우스 왼쪽버튼을 클릭시 레이발사
        if(Input.GetMouseButtonDown(0))
        {
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
         
        //마우스 오른쪽버튼을 클릭시 폭탄 투척.
        if(Input.GetMouseButtonDown(1))
        {
            GameObject bomb = Instantiate(bombFactory);
            bomb.transform.position = firePosition.transform.position;

            Rigidbody rb = bomb.GetComponent<Rigidbody>();

            rb.AddForce(Camera.main.transform.forward * throwPower, ForceMode.Impulse);
        }
        
    }
}
