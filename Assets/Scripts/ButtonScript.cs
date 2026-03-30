using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonScript : MonoBehaviour
{
	[SerializeField] private int diff = 0;
	[SerializeField] private TargetController tC;
	[SerializeField] private LayerMask layer;
	
	[SerializeField] private bool mode = false;
	
    void OnTriggerEnter(Collider other) {
		Debug.Log("We entered trigger qith " + other.gameObject);
		if(!mode){
			tC.ChangeDifficulty(diff);
		}else{
			tC.StartGame();
		}
		

    }
}
