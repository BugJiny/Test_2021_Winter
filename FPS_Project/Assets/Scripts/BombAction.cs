using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    

	//���� ����Ʈ ������ ����
    public GameObject bombEffect;

	//����ź ������
	public int attackPower = 10;

	public float explosionRadius = 5f;

    private void OnCollisionEnter(Collision collision)
    {

		// ���� ȿ�� �ݰ� ������ ���̾ "Enemy"�� ��� ���� ������Ʈ���� Collider ������Ʈ�� �迭�� �����Ѵ�.
		Collider[] cols = Physics.OverlapSphere(transform.position, explosionRadius, 1 << 10);

		for(int i=0; i<cols.Length; i++)
		{
			cols[i].GetComponent<EnemyFSM>().HitEnemy(attackPower);
		}

        //����Ʈ�� ������ �ͼ� �߻���Ŵ.
        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;
       
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
}
