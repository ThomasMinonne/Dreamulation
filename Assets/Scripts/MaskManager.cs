using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Data;
using System;
using Random = UnityEngine.Random;

public class MaskManager : MonoBehaviour
{
    public GameObject[] masks;
	public int initMasks;
	private int toInitMasks = 0;
	private string[] elaborateMask = new string[2];
	private int elaborateAnswer;
	public GameObject friendsDialogue;
	public GameObject foesDialogue;
	public GameObject idkDialogue;
	public GameObject gameMenu;
	public GameObject gameMenuMaskActive;
	private GameObject Culprit;
	private GameObject CulpritAssistant;
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
	
	private void shuffleList(List<GameObject> alpha){
		for (int i = 0; i < alpha.Count; i++) {
			GameObject temp = alpha[i];
			int randomIndex = Random.Range(i, alpha.Count);
			alpha[i] = alpha[randomIndex];
			alpha[randomIndex] = temp;
		}
	}
	
	public void setRelationships(){
		GameObject Cuthbert = null;
		GameObject CuthbertAssistant = null;
		for(int i = 0; i < masks.Length; i++){
			//Setting Friends to all the masks
			for(int j = 0; j < friendsNumber; j++){
				int temp = i+1+j;
				if(temp >= masks.Length) { temp -= masks.Length; }
				masks[i].GetComponent<MaskInfo>().friends.Add(masks[temp]);
				if(masks[i].GetComponent<MaskInfo>().personName == "Cuthbert Humble" && !guiltyAssistantSetted){
					Cuthbert = masks[i];
					CuthbertAssistant = masks[temp];
					string[] parsingNameCuthbert = Chop(masks[i].GetComponent<MaskInfo>().name, masks[i].GetComponent<MaskInfo>().name.Length - 2);
					string[] parsingNamefriend = Chop(masks[temp].GetComponent<MaskInfo>().name, masks[temp].GetComponent<MaskInfo>().name.Length - 2);
					//Condividono la maschera
					if(parsingNameCuthbert[0] == parsingNamefriend[0]){
						masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
						guiltyAssistantSetted = true;
						Debug.Log("MASCHERA");
					}
					else {//condividono il simbolo
						if(parsingNameCuthbert[1] == parsingNamefriend[1]){
							masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
							guiltyAssistantSetted = true;
							Debug.Log("SIMBOLO");
						}
					}//non condividono nulla
					if(!guiltyAssistantSetted){
						masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
						guiltyAssistantSetted = true;
						Debug.Log("NULLA");
					}
				}
			}
			//Setting Foes to all the masks
			for(int j = 0; j < foesNumber; j++){
				int temp = i+1+j+friendsNumber;
				if(temp >= masks.Length) { temp -= masks.Length; }
				masks[i].GetComponent<MaskInfo>().foes.Add(masks[temp]);
			}
			shuffleList(masks[i].GetComponent<MaskInfo>().friends);
			shuffleList(masks[i].GetComponent<MaskInfo>().foes);
		}
		setCuthbertRelationship(Cuthbert, CuthbertAssistant);
		int[] maskStored = new int[initMasks];
		while(toInitMasks < initMasks){ 
			int r = Random.Range(0, masks.Length);
			if(!masks[r].GetComponent<MaskInfo>().guilty && !masks[r].GetComponent<MaskInfo>().guiltyAssistant){
				if(toInitMasks == 0){
					maskStored[toInitMasks] = r;
					revealMasks(masks[r]);
					toInitMasks++;
				} else if(!Array.Exists(maskStored, element => element == r)){
					maskStored[toInitMasks] = r;
					revealMasks(masks[r]);
					toInitMasks++;
				}
			}	
		}
	}
	
	private void setCuthbertRelationship(GameObject Cuthbert, GameObject CuthbertAssistant){	
		Culprit = Cuthbert;
		CulpritAssistant = CuthbertAssistant;
		GameObject toLink2 = CuthbertAssistant.GetComponent<MaskInfo>().friends[0];
		if(!CuthbertAssistant.GetComponent<MaskInfo>().friends.Contains(Cuthbert)){
			CuthbertAssistant.GetComponent<MaskInfo>().friends.Add(Cuthbert);
			CuthbertAssistant.GetComponent<MaskInfo>().friends.RemoveAt(0);
			if(CuthbertAssistant.GetComponent<MaskInfo>().foes.Contains(Cuthbert)){
				for(int j = 0; j < foesNumber; j++){
					if(CuthbertAssistant.GetComponent<MaskInfo>().foes[j] == Cuthbert){
							CuthbertAssistant.GetComponent<MaskInfo>().foes.Add(toLink2);
					}
				}
			}
		}
		shuffleList(CuthbertAssistant.GetComponent<MaskInfo>().friends);
		shuffleList(CuthbertAssistant.GetComponent<MaskInfo>().foes);
	}
	
	public static string[] Chop(string value, int length){
		int strLength = value.Length;
		int strCount = (strLength + length - 1) / length;
		string[] result = new string[strCount];
		for (int i = 0; i < strCount; ++i)
		{
			result[i] = value.Substring(i * length, Mathf.Min(length, strLength));
			strLength -= length;
		}
		return result;
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
		if(Culprit.GetComponent<MaskInfo>().slotUI.transform.childCount > 0){
			if(Culprit.GetComponent<MaskInfo>().slotUI.transform.GetChild(0).name == "Cuthbert Humble"){
				if(CulpritAssistant.GetComponent<MaskInfo>().slotUI.transform.childCount > 0){
					if(CulpritAssistant.GetComponent<MaskInfo>().slotUI.transform.GetChild(0).name == CulpritAssistant.GetComponent<MaskInfo>().personName){
						setFinish(true);
						return;
					}
				}
			}
		}
		setFinish(false);
	}
	
	private void setFinish(bool victory){
		if(victory){Debug.Log("VINTOOOOOOOOO");}
		else {Debug.Log("PERSOOOOOOO");}
	}
	
	public void callGuest(){
		if(index_currentGuest == masks.Length){
			reshuffleGO(masks);
			index_currentGuest = 0;
		}
		masks[index_currentGuest].SetActive(true);
		currentGuest = masks[index_currentGuest];
	}
	
	public void callChoosenGuest(GameObject maskToActiveFromUI){
		if(currentGuest != null){
			removeGuest();
		}
		maskToActiveFromUI.SetActive(true);
		currentGuest = maskToActiveFromUI;
	}
	
	public void removeGuest(){
		
		currentGuest.SetActive(false);
		index_currentGuest++;
	}
	
	public void answer(string objectName){
		//checka la maschera, se il nome della maschera premuta è tra gli amici della maschera corrente, allora sblocca il dialogo dell'amicizia, altrimenti il contrario
		Debug.Log(elaborateAnswer);
		if(elaborateAnswer == 0){
			if(objectName.Length == 1){
				elaborateMask[1] = objectName;
				Debug.Log("Numebr setting");
			} else {
				elaborateMask[0] = objectName;
				Debug.Log("Mask setting");
			}
			elaborateAnswer++;
			return;
		}
		if(elaborateAnswer == 1){
			Debug.Log("sono entrato");
			if(elaborateMask[0] != objectName && elaborateMask[1] != objectName){
				Debug.Log("Array setting");
				if(objectName.Length == 1){
					elaborateMask[1] = objectName;
					Debug.Log("Numebr setting");
				} else {
					elaborateMask[0] = objectName;
					Debug.Log("Mask setting");
				}
				elaborateAnswer++;
			}
			else{
				if(objectName.Length == 1){
					elaborateMask[1] = "a";
				} else {
					elaborateMask[0] = "a";
				}
				elaborateAnswer--;
				return;
			}
		} 
		if(elaborateAnswer == 2){
			GameObject temp = getMaskInfo(elaborateMask[0]+elaborateMask[1]);
			for(int i = 0; i < currentGuest.GetComponent<MaskInfo>().friends.Count; i++){
				if(currentGuest.GetComponent<MaskInfo>().friends[i].GetComponent<MaskInfo>().personName == temp.GetComponent<MaskInfo>().personName){
					friendsDialogue.SetActive(true);
					Invoke("deActivatorFriendsDialogue", waitAnswerTimer);
					elaborateAnswer = 0;
					return;
				}
			}
			for(int i = 0; i < currentGuest.GetComponent<MaskInfo>().foes.Count; i++){
				if(currentGuest.GetComponent<MaskInfo>().foes[i].GetComponent<MaskInfo>().personName == temp.GetComponent<MaskInfo>().personName){
					foesDialogue.SetActive(true);
					Invoke("deActivatorFoesDialogue", waitAnswerTimer);
					elaborateAnswer = 0;
					return;
				}
			}
			idkDialogue.SetActive(true);
			Invoke("deActivatorIdkDialogue", waitAnswerTimer);
			elaborateAnswer = 0;
			elaborateMask[0] = "a";
			elaborateMask[1] = "a";
		}
	}
	
	private void deActivatorFriendsDialogue(){
		friendsDialogue.SetActive(false);
		//removeGuest();
		//callGuest();
		gameMenuMaskActive.SetActive(true);
	}
	
	private void deActivatorFoesDialogue(){
		foesDialogue.SetActive(false);
		//removeGuest();
		//callGuest();
		gameMenuMaskActive.SetActive(true);
	}
	
	private void deActivatorIdkDialogue(){
		idkDialogue.SetActive(false);
		//removeGuest();
		//callGuest();
		gameMenuMaskActive.SetActive(true);
	}

	private void revealMasks(GameObject maskToReveal){
		GameObject slotMask = maskToReveal.GetComponent<MaskInfo>().slotUI;
		Debug.Log(maskToReveal.GetComponent<MaskInfo>().personName);
		GameObject toSetInslotMask = GameObject.Find(maskToReveal.GetComponent<MaskInfo>().personName);
		toSetInslotMask.transform.parent = slotMask.transform;
		toSetInslotMask.transform.position = slotMask.transform.position;
		toSetInslotMask.GetComponent<Button>().interactable = false;
		toSetInslotMask.GetComponent<Draggable>().enabled = false;
	}
}
