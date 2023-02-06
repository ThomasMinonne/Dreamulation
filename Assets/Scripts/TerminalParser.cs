using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TerminalParser : MonoBehaviour
{
	//private string input;
    // Start is called before the first frame update
    void Start()
    {
		this.GetComponent<InputField>().onEndEdit.AddListener(SubmitName);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void SubmitName(string arg0)
    {
        //Debug.Log(this.GetComponent<InputField>().text);
		//input = s;
		Debug.Log(arg0);
    }
}
