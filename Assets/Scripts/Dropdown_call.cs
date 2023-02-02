using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Dropdown_call : MonoBehaviour
{
	public MaskManager manager;
	private bool UInotset = true;
    // Start is called before the first frame update
    void Start()
    {
        if(UInotset){ manager.setUIPerson("Addyson Mathew"); UInotset = false;}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void callSetUiPerson(){
		manager.setUIPerson(GetComponent<TMP_Dropdown>().options[GetComponent<TMP_Dropdown>().value].text);
	}
}
