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
					Debug.Log(parsingNameCuthbert[0]); 
					Debug.Log(parsingNamefriend[0]);
					if(parsingNameCuthbert[0] == parsingNamefriend[0]){
						masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
						guiltyAssistantSetted = true;
						Debug.Log("MASCHERA");
					}//Condividono la maschera
					else {
						if(parsingNameCuthbert[1] == parsingNamefriend[1]){
							masks[temp].GetComponent<MaskInfo>().setGuiltyAssistant();
							guiltyAssistantSetted = true;
							Debug.Log("SIMBOLO");
						}//condividono il simbolo
					}
					if(!guiltyAssistantSetted){
						//non condividono nulla
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
	}
	
	private void setCuthbertRelationship(GameObject Cuthbert, GameObject CuthbertAssistant){	
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
	}
	
	public void callGuest(){
		if(index_currentGuest == masks.Length){
			reshuffleGO(masks);
			index_currentGuest = 0;
		}
		masks[index_currentGuest].SetActive(true);
		currentGuest = masks[index_currentGuest];
	}
	
	public void callChoosenGuest(){
		/*if(index_currentGuest == masks.Length){
			reshuffleGO(masks);
			index_currentGuest = 0;
		}*/
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
