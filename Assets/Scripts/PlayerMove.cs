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

        Debug.Log("적에게 공격받음 HP = " + hp);
        //print("적에게 공격받음 남은 HP: hp");
    }

    // Start is called before the first frame update
    void Start()
    {

        cc = GetComponent<CharacterController>();
        
    }

    // Update is called once per frame
    void Update()
    {

        //키보드 입력(GetAxis)값을 받는다.
        float h = Input.GetAxis("Horizontal");
        float v = Input.GetAxis("Vertical");

        Vector3 dir = new Vector3(h, 0, v);
        dir = dir.normalized;

        dir = Camera.main.transform.TransformDirection(dir);


        //점프중일경우 2단점프를 막기 + 캐릭터가 오브젝트에 올라갔다가 내려올떄 중력값에의해 빠르게 떨어지는것을 막는코드
        if(isJumping && cc.collisionFlags == CollisionFlags.Below)
        {
            isJumping = false;
            yVelocity = 0;
        }


        //InputSystem에서 Jump에 해당하는 값을 눌렀을경우 + 점프중이 아닐경우
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
