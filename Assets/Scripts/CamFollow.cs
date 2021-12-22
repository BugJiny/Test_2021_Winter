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
        //카메라가 플레이어의 눈의 위치에 따라다니게끔
        transform.position = Target.position;
    }
}
