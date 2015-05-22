using UnityEngine;
using System.Collections;

public class ToScene : MonoBehaviour {
	
	public enum Scene 
	{
		Init, // Scene 場景名稱 (一定與實際場景名稱相符)
		ImageControl,  // 以下為每個場景的名稱，之後記得到 Unity 介面去選擇要進入的場景
		Show
	};
	
	public Scene SceneName;
	
	public bool useClick = true;
	public bool useBack;
	
	void OnClick () {
		if(useClick){
			Application.LoadLevel(SceneName.ToString());
		}
	}
	
	void Update () {
		if(useBack){
			if(Input.GetKeyDown(KeyCode.Escape)){
				Application.LoadLevel(SceneName.ToString());
			}
		}
	}
}
