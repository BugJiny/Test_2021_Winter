using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{

    public float moveSpeed = 7f;
    public float jumpPower = 10f;
    public bool isJumping = false;

    public int hp=100;

    CharacterController cc;

    float gravity = -20f;

    float yVelocity = 0;


   

    public void DamageAction(int damage)
    { 
       hp -= damage;

        Debug.Log("������ ���ݹ��� HP = " + hp);
        //print("������ ���ݹ��� ���� HP: hp");
    }

    // Start is called before the first frame update
    void Start()
    {

        cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //Ű���� �Է�(GetAxis)���� �޴´�.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);


        //�������ϰ�� 2�������� ���� + ĳ���Ͱ� ������Ʈ�� �ö󰬴ٰ� �����Ë� �߷°������� ������ �������°��� �����ڵ�
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }


        //InputSystem���� Jump�� �ش��ϴ� ���� ��������� + �������� �ƴҰ��
        if(Input.GetButtonDown("Jump") && !isJumping)
        {
            yVelocity = jumpPower;
            isJumping = true;
        }


        yVelocity += gravity * Time.deltaTime;
        dir.y = yVelocity;

        cc.Move(dir * moveSpeed * Time.deltaTime);
    }
}
