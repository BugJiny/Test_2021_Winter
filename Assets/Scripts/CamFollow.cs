using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CamFollow : MonoBehaviour
{
    

    public Transform Target;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        //ī�޶� �÷��̾��� ���� ��ġ�� ����ٴϰԲ�
        transform.position = Target.position;
    }
}
