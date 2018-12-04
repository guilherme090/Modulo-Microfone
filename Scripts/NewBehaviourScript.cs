using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class NewBehaviourScript : MonoBehaviour {

    private float tempoMudancaDirecao = 5f;
    private float tempoQuePassou;

    // Use this for initialization
    void Start () {

        
    }
	
	// Update is called once per frame
	void Update () {
        tempoQuePassou += Time.deltaTime;
        if (tempoQuePassou >= tempoMudancaDirecao)
        {
            tempoQuePassou = 0f;
            transform.rotation = Quaternion.Euler(-90, 0, 0);
            NavMeshAgent agent = GetComponent<NavMeshAgent>();
            Vector3 randomDirection = Random.insideUnitSphere * 5;
            randomDirection += transform.position;
            agent.destination = randomDirection;
        }
	}
}
