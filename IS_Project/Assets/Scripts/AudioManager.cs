using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	private AudioSource manager;
    // Start is called before the first frame update
    void Start()
    {

		manager = GetComponent<AudioSource>();
		manager.clip = Resources.Load<AudioClip>("Title_Music");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
