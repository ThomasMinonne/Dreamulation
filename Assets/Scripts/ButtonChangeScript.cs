using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonChangeScript : MonoBehaviour
{
	public Texture smile;
	public Texture bad;
	public Texture idk;
	
    // Start is called before the first frame update
    void Start()
    {
        for(int i = 0; i < transform.childCount; i++){
			if(transform.GetChild(i).GetComponent<Button>() != null){
				GameObject temp = transform.GetChild(i).gameObject;
				transform.GetChild(i).GetComponent<Button>().onClick.AddListener(() => setGridImage(temp));
			}
		}
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void setGridImage(GameObject button){
		if(button.transform.GetChild(0).GetComponent<RawImage>().texture == null){
			button.transform.GetChild(0).GetComponent<RawImage>().texture = smile;
			button.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(255, 255, 255, 255);
			return;
		}
		if(button.transform.GetChild(0).GetComponent<RawImage>().texture == smile){
			button.transform.GetChild(0).GetComponent<RawImage>().texture = bad;
			return;
		}
		if(button.transform.GetChild(0).GetComponent<RawImage>().texture == bad){
			button.transform.GetChild(0).GetComponent<RawImage>().texture = idk;
			return;
		}
		if(button.transform.GetChild(0).GetComponent<RawImage>().texture == idk){
			button.transform.GetChild(0).GetComponent<RawImage>().texture = null;
			button.transform.GetChild(0).GetComponent<RawImage>().color = new Color32(0, 0, 0, 255);
			return;
		}
	}
}
