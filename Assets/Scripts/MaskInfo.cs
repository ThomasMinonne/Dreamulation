using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MaskInfo : MonoBehaviour
{
	public string personName;
	public int stressCapacity;
	public bool isStressed;
	public bool guilty = false;
	public bool guiltyAssistant = false;
	public int tension_level = 0;
	public List<GameObject> friends = new List<GameObject>();
	public List<GameObject> foes = new List<GameObject>();
	public GameObject slotUI;
	public TMP_Text friendsProText;
	public TMP_Text foesProText;
	
	// Start is called before the first frame update
    void Start()
    {
    }
	
    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void setName(string nametoset){
		personName = nametoset;
	}
	
	public void setGuilty(){
		guilty = true;
	}
	
	public void setGuiltyAssistant(){
		guiltyAssistant = true;
	}
	
	public void setFriends(string toSet){
		//friendsProText = GameObject.Find("Friends").GetComponent<TMP_Text>();
		friendsProText.text = toSet;
	}
	
	public void setFoes(string toSet){
		//foesProText = GameObject.Find("Grudges").GetComponent<TMP_Text>();
		foesProText.text = toSet;
	}
	
	public void setStress(){
		tension_level += stressCapacity;
		if(tension_level >= 100){
			isStressed = true;
		}
	}
}
