using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class TargetController : MonoBehaviour
{
	[SerializeField] private GameObject[] targetPrefab;
	private List<GameObject> targetList;
	
	private float waitingTime = 8.0f;
	private float extraPercent = 0.75f;
	private float movPercent = 0.25f;
	
	private float intervalTime = 0.75f;
	private float currentTime = 0.0f;
	
	private bool timerReached = false;
	private bool targetShowing = false;
	
	private int difficulty = 0;
	
	private int numTargets = 1;
	
	[Header("Fibonacci")]
	[SerializeField] private List<Transform> spots;
	
	[Header("Move")]
	[SerializeField] private List<Transform> endings;
	
	[Header("Screens")]
	[SerializeField] TMP_Text difficultText;
	[SerializeField] TMP_Text startText;
	[SerializeField] TMP_Text pointsText;
	
	private int maxPoint = 0;
	private int roundPoint = 0;
	private int currentPoint = 0;
	
	private int prevSize = 0;
	
	private bool inGame = false;
	private int roundCounter = 0;
    // Start is called before the first frame update
    void Start()
    {
        targetList = new List<GameObject>();
    }

    // Update is called once per frame
    void Update()
    {
		//if (!timerReached)
		if(inGame){
			currentTime += Time.deltaTime;

			if(targetShowing){
				if (currentTime > waitingTime)
				{
					foreach(GameObject t in targetList){
						Destroy(t);
					}
					targetList.Clear();
					targetShowing = false;
					currentTime = 0.0f;
					roundCounter++;
					roundPoint = 0;
					if(roundCounter > 9){
						inGame = false;
						
						ShowScore();
					}
				}
			}else{
				if (currentTime > intervalTime)
				{
					
					List<Transform> tempSpots = new List<Transform>(spots);
					
					for(int i = 0; i < Mathf.Clamp(numTargets + prevSize, 1, 8); i++){
						int idx = Random.Range(0, tempSpots.Count);
						
						int targetType = (Random.Range(0.0f, 1.0f) < extraPercent ? 1 : 0);
						
						targetList.Add(Instantiate(targetPrefab[targetType], tempSpots[idx].position, Quaternion.identity));					
						
						if(Random.Range(0.0f, 1.0f) < movPercent){
							targetList[i].transform.DOMove(endings[idx].position, waitingTime, false);
						}					
						
						roundPoint += 5 + 2 * targetType;
						
						tempSpots.RemoveAt(idx);
					}
					maxPoint += roundPoint;
					
					numTargets = Mathf.Clamp(prevSize, 1, 8);
					prevSize = targetList.Count;
					
					
					targetShowing = true;

					currentTime = 0.0f;
				}
			}
		}
		
		
    }
	
	public void ChangeDifficulty(int dif){
		difficulty += dif;
		if(difficulty < 0){
			difficulty = 2;
		}else if(difficulty > 2){
			difficulty = 0;
		}
		
		switch(difficulty){
			case 0:
				waitingTime = 8.0f;
				extraPercent = 0.75f;
				movPercent = 0.25f;
				difficultText.text = "EASY";
				break;
			case 1:
				waitingTime = 6.5f;
				extraPercent = 0.5f;
				movPercent = 0.5f;
				difficultText.text = "NORMAL";
				break;
			case 2:
				waitingTime = 5.0f;
				extraPercent = 0.25f;
				movPercent = 0.75f;
				difficultText.text = "HARD";
				break;
		}
	}
	
	public void StartGame(){
		if(!inGame){
			numTargets = 1;
			prevSize = 0;
			inGame = true;
			startText.text = "IN GAME";
		}
	}
	
	public void AddPoints(int points){
		currentPoint += points;
		Debug.Log(currentPoint);
	}
	
	public void ShowScore(){
		float rate = currentPoint / maxPoint;
		string pT = currentPoint.ToString();
		if(rate < 0.45f){
			pT += " - D";
		}else if(rate > 0.44f && rate < 0.65f){
			pT += " - C";
		}else if(rate > 0.64f && rate < 0.75f){
			pT += " - B";
		}else if(rate > 0.74f && rate < 0.85f){
			pT += " - A";
		}else if(rate > 0.75f){
			pT += " - S";
		}
		
		roundCounter = 0;
		pointsText.text = pT;
		
		startText.text = "PRESS THE SCREEN \n TO START";
		
		maxPoint = 0;
		currentPoint = 0;
		roundPoint = 0;
	}
}
