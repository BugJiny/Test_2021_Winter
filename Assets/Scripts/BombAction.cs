using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombAction : MonoBehaviour
{
    


    public GameObject bombEffect;


    private void OnCollisionEnter(Collision collision)
    {

        //이펙트를 가지고 와서 발생시킴.
        GameObject eff = Instantiate(bombEffect);

        eff.transform.position = transform.position;
       
        Destroy(this.gameObject);
    }

    // Start is called before the first frame update
}
