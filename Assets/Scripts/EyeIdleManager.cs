using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EyeIdleManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(blinkeye());
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	IEnumerator blinkeye()
    {
        while(true){
			int wait_time = Random.Range (1, 4);
			yield return new WaitForSeconds (wait_time);
			Debug.Log("Blink");
			GetComponent<Animator>().Play("BlinkEye");
		}
	}
}
