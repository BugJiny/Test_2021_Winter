using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    

	//폭발 이펙트 프리팹 변수
    public GameObject bombEffect;

	//수류탄 데미지
	public int attackPower = 10;

	public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {

		// 폭발 효과 반경 내에서 레이어가 "Enemy"인 모든 게임 오브젝트들의 Collider 컴포넌트를 배열에 저장한다.
		Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);

		for(int i=0; i<cols.Length; i++)
		{
			cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
		}

        //이펙트를 가지고 와서 발생시킴.
        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;
       
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
}
