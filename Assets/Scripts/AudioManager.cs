using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
	public AudioSource mouseClick;
	public AudioSource investigationOst;
	public AudioSource menuOst;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
	
	public void doClickSound(){
		mouseClick.PlayOneShot(mouseClick.clip);
	}
	
	public void startOstInvestigation(){
		while(investigationOst.volume < 0.3f){
			investigationOst.volume += 0.001f;
		}
		investigationOst.Play();
	}
	
	public void stopOstInvestigation(){
		while(investigationOst.volume > 0f){
			investigationOst.volume -= 0.001f;
		}
		investigationOst.Stop();
	}
	
	public void startOstMenu(){
		while(menuOst.volume < 0.3f){
			menuOst.volume += 0.001f;
		}
		menuOst.Play();
	}
	
	public void stopOstMenu(){
		while(menuOst.volume > 0f){
			menuOst.volume -= 0.001f;
		}
		menuOst.Stop();
	}

}
