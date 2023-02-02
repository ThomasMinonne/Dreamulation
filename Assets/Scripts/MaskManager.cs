using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;

public class MaskManager : MonoBehaviour
{
    public GameObject[] masks;
	public GameObject friendsDialogue;
	public GameObject foesDialogue;
	public GameObject idkDialogue;
	public GameObject gameMenu;
	public float waitAnswerTimer;
	public string[] names;
	public int friendsNumber;
	public int foesNumber;
	private string tempFriendsText;
	private string tempFoesText;
	private bool guiltyAssistantSetted = false;
	private GameObject currentGuest;
	private int index_currentGuest = 0;
	private bool isGameRunning = false;
	
	void Awake()
	{
		reshuffleGO(masks);
	}
	
	// Start is called before the first frame update
    void Start()
    {
		reshuffleStrings(names);
		setMaskInfo();
		callGuest();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
			if(currentGuest != null){
				removeGuest();
			}
			callGuest();
        }
    }
	
	private void setMaskInfo(){
		for(int i = 0; i < masks.Length; i++){
			masks[i].GetComponent<MaskInfo>().setName(names[i]);
			if(names[i] == "Cuthbert Humble"){
				masks[i].GetComponent<MaskInfo>().setGuilty();
			}
		}
		setRelationships();
	}
	
	private GameObject getMaskInfo(string objectNameToGet){
		for(int i = 0; i < masks.Length; i++){
			if(masks[i].name == objectNameToGet){
				return masks[i];
			}
			if(masks[i].GetComponent<MaskInfo>().personName == objectNameToGet){
				return masks[i];
			}
		}
		return null;
	}
	
	private void reshuffleStrings(string[] texts){
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int t = 0; t < texts.Length; t++ )
        {
            string tmp = texts[t];
            int r = Random.Range(t, texts.Length);
            texts[t] = texts[r];
            texts[r] = tmp;
        }
    }
	
	private void reshuffleGO(GameObject[] gameObjects){
		Debug.Log("Sto shufflando");
        // Knuth shuffle algorithm :: courtesy of Wikipedia :)
        for (int g = 0; g < gameObjects.Length; g++ )
        {
            GameObject tmp = gameObjects[g];
            int r = Random.Range(g, gameObjects.Length);
            gameObjects[g] = gameObjects[r];
            gameObjects[r] = tmp;
        }
    }
	
	public void setRelationships(){
		for(int i = 0; i < masks.Length; i++){
			//Setting Friends to all the masks
			for(int j = 0; j < friendsNumber; j++){
				int temp = i+1+j;
				if(temp >= masks.Length) { temp -= masks.Length; }
				masks[i].GetComponent<MaskInfo>().friends.Add(masks[temp]);
				if(masks[i].GetComponent<MaskInfo>().personName == "Cuthbert Humble" && !guiltyAssistantSetted){
					string parsingNameCuthbert = masks[i].GetComponent<MaskInfo>().personName;
					int parsingNameCuthbertIndex = masks[i].GetComponent<MaskInfo>().personName.Length;
					parsingNameCuthbert.Substring(0, parsingNameCuthbertIndex - 2);
					string parsingNamefriend = masks[temp].GetComponent<MaskInfo>().personName;
					int parsingNamefriendIndex = masks[temp].GetComponent<MaskInfo>().personName.Length;
					parsingNamefriend.Substring(0, parsingNamefriendIndex - 2);
					if(parsingNameCuthbert == parsingNamefriend){
						masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
						guiltyAssistantSetted = true;
					}//Condividono la maschera
					else {
						parsingNameCuthbert.Substring(parsingNameCuthbertIndex - 2, parsingNameCuthbertIndex);
						parsingNamefriend.Substring(parsingNamefriendIndex - 2, parsingNamefriendIndex);
						if(parsingNameCuthbert == parsingNamefriend){
							masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
							guiltyAssistantSetted = true;
						}//condividono il simbolo
					}
					if(!guiltyAssistantSetted){
						//non condividono nulla
						masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
						guiltyAssistantSetted = true;
					}
				}
			}
			//Setting Foes to all the masks
			for(int j = 0; j < foesNumber; j++){
				int temp = i+1+j+friendsNumber;
				if(temp >= masks.Length) { temp -= masks.Length; }
				masks[i].GetComponent<MaskInfo>().foes.Add(masks[temp]);
			}
		}
	}
	
	private void setCuthbertRelationship(){
		GameObject tempObj = getMaskInfo("Cuthbert Humble");
		int maskAssociation = Random.Range(0, 1);
		List<GameObject> numbfriends = new List<GameObject>();
		if(maskAssociation == 0){	
			string parsingName = tempObj.name;
			parsingName.Substring(0, parsingName.Length - 2);
			GameObject tempFriend1 = getMaskInfo(parsingName + "_1");
			GameObject tempFriend2 = getMaskInfo(parsingName + "_2");
			GameObject tempFriend3 = getMaskInfo(parsingName + "_3");
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend1)){numbfriends.Add(tempFriend1);}
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend2)){numbfriends.Add(tempFriend2);}
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend3)){numbfriends.Add(tempFriend3);}
			if(numbfriends.Count > 1){
				int friend = Random.Range(0, numbfriends.Count-1);
				if(friend == 1)tempFriend1.GetComponent<MaskInfo>().setGuiltyAssistant();
				if(friend == 2)tempFriend2.GetComponent<MaskInfo>().setGuiltyAssistant();
				if(friend == 3)tempFriend3.GetComponent<MaskInfo>().setGuiltyAssistant();
			}
			else{
				//prendo il primo amico di Cutberth e lo tolgo
				//prendo il primo amico di tempFriend e lo tolgo
				//linko Cutberth e tempFriend
				//linko i due amici tolti
				GameObject toLink;
				GameObject toLink2;
				int friend = Random.Range(1, 3);
				if(friend == 1){
					toLink = tempFriend1.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend1;
					tempFriend1.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
				if(friend == 2){
					toLink = tempFriend2.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend2;
					tempFriend2.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
				if(friend == 3){
					toLink = tempFriend3.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend3;
					tempFriend3.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
			}
		}
		else {
			string parsingName = tempObj.name;
			parsingName.Substring(parsingName.Length - 2, parsingName.Length);
			Debug.Log(parsingName);
			GameObject tempFriend1 = getMaskInfo("Blue" + parsingName);
			GameObject tempFriend2 = getMaskInfo("Purple" + parsingName);
			GameObject tempFriend3 = getMaskInfo("Gold" + parsingName);
			GameObject tempFriend4 = getMaskInfo("LightBlue" + parsingName);
			GameObject tempFriend5 = getMaskInfo("Pink" + parsingName);
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend1)){numbfriends.Add(tempFriend1);}
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend2)){numbfriends.Add(tempFriend2);}
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend3)){numbfriends.Add(tempFriend3);}
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend4)){numbfriends.Add(tempFriend4);}
			if(tempObj.GetComponent<MaskInfo>().friends.Contains(tempFriend5)){numbfriends.Add(tempFriend5);}
			if(numbfriends.Count > 0){
				int friend = Random.Range(0, numbfriends.Count-1);
				if(friend == 1)tempFriend1.GetComponent<MaskInfo>().setGuiltyAssistant();
				if(friend == 2)tempFriend2.GetComponent<MaskInfo>().setGuiltyAssistant();
				if(friend == 3)tempFriend3.GetComponent<MaskInfo>().setGuiltyAssistant();
				if(friend == 4)tempFriend4.GetComponent<MaskInfo>().setGuiltyAssistant();
				if(friend == 5)tempFriend5.GetComponent<MaskInfo>().setGuiltyAssistant();
			}
			else{
				int friend = Random.Range(1, 5);
				GameObject toLink;
				GameObject toLink2;
				if(friend == 1){
					toLink = tempFriend1.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend1;
					tempFriend1.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
				if(friend == 2){
					toLink = tempFriend2.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend2;
					tempFriend2.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
				if(friend == 3){
					toLink = tempFriend3.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend3;
					tempFriend3.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
				if(friend == 4){
					toLink = tempFriend4.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend4;
					tempFriend4.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
				if(friend == 5){
					toLink = tempFriend5.GetComponent<MaskInfo>().friends[0];					
					toLink2 = tempObj.GetComponent<MaskInfo>().friends[0];
					tempObj.GetComponent<MaskInfo>().friends[0] = tempFriend2;
					tempFriend5.GetComponent<MaskInfo>().friends[0] = tempObj;
				}
			}
		}
	}
	public void setUIPerson(string personName){
		int currentPerson = System.Array.IndexOf(names, personName);
		string tempFriends = "";
		string tempFoes = "";
		for(int i = 0; i < masks[currentPerson].GetComponent<MaskInfo>().friends.Count; i++){
			if( i  == masks[currentPerson].GetComponent<MaskInfo>().friends.Count - 1 ){
				tempFriends += masks[currentPerson].GetComponent<MaskInfo>().friends[i].GetComponent<MaskInfo>().personName;
			}
			else tempFriends += masks[currentPerson].GetComponent<MaskInfo>().friends[i].GetComponent<MaskInfo>().personName + "\n";
		}
		for(int i = 0; i < masks[currentPerson].GetComponent<MaskInfo>().foes.Count; i++){
			if( i  == masks[currentPerson].GetComponent<MaskInfo>().foes.Count - 1 ){
				tempFoes += masks[currentPerson].GetComponent<MaskInfo>().foes[i].GetComponent<MaskInfo>().personName;
			}
			else tempFoes += masks[currentPerson].GetComponent<MaskInfo>().foes[i].GetComponent<MaskInfo>().personName + "\n";
		}
		masks[currentPerson].GetComponent<MaskInfo>().setFriends(tempFriends);
		masks[currentPerson].GetComponent<MaskInfo>().setFoes(tempFoes);
	}

	public void startgame(){
		isGameRunning = true;
	}
	
	public void finishgame(){
		isGameRunning = false;
	}
	
	public void callGuest(){
		if(index_currentGuest == masks.Length){
			reshuffleGO(masks);
			index_currentGuest = 0;
		}
		masks[index_currentGuest].SetActive(true);
		currentGuest = masks[index_currentGuest];
	}
	
	public void removeGuest(){
		currentGuest.SetActive(false);
		index_currentGuest++;
	}
	
	public void answer(string objectName){
		//checka la maschera, se il nome della maschera premuta è tra gli amici della maschera corrente, allora sblocca il dialogo dell'amicizia, altrimenti il contrario
		GameObject temp = getMaskInfo(objectName);
		for(int i = 0; i < currentGuest.GetComponent<MaskInfo>().friends.Count; i++){
			if(currentGuest.GetComponent<MaskInfo>().friends[i].GetComponent<MaskInfo>().personName == temp.GetComponent<MaskInfo>().personName){
				friendsDialogue.SetActive(true);
				Invoke("deActivatorFriendsDialogue", waitAnswerTimer);
				return;
			}
		}
		for(int i = 0; i < currentGuest.GetComponent<MaskInfo>().foes.Count; i++){
			if(currentGuest.GetComponent<MaskInfo>().foes[i].GetComponent<MaskInfo>().personName == temp.GetComponent<MaskInfo>().personName){
				foesDialogue.SetActive(true);
				Invoke("deActivatorFoesDialogue", waitAnswerTimer);
				return;
			}
		}
		idkDialogue.SetActive(true);
		Invoke("deActivatorIdkDialogue", waitAnswerTimer);
	}
	
	private void deActivatorFriendsDialogue(){
		friendsDialogue.SetActive(false);
		removeGuest();
		callGuest();
		gameMenu.SetActive(true);
	}
	
	private void deActivatorFoesDialogue(){
		foesDialogue.SetActive(false);
		removeGuest();
		callGuest();
		gameMenu.SetActive(true);
	}
	
	private void deActivatorIdkDialogue(){
		idkDialogue.SetActive(false);
		removeGuest();
		callGuest();
		gameMenu.SetActive(true);
	}
}
