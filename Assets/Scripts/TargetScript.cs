using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField] int points = 0;
    [SerializeField] int hitsNeeded = 1;
    [SerializeField] ParticleSystem explotingSystem;
	[SerializeField] AudioSource explosionSound;
	
    private int currentDamage;
	private TargetController controller;

    void Start()
    {
        currentDamage = hitsNeeded;
		controller = FindObjectOfType<TargetController>();
    }

	public void SetController(TargetController reference){
		controller = reference;
	}

    public void HitTaken(int damage)
    {
		Debug.Log("DisparoRecibido");
        currentDamage -= damage;

        if (currentDamage <= 0) {
			Instantiate(explotingSystem, this.transform.position, Quaternion.identity);
			explosionSound.Play();
			controller.AddPoints(points);
            Destroy(gameObject);
        }
    }
}
