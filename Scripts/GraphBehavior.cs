using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GraphBehavior : MonoBehaviour {

    public LineRenderer aLinha;
    Vector3 [] posicoes;
    const int NUM_POSICOES = 500;
    const float AMPLITUDE = 80.0f;
    const float NUM_CICLOS = 3.0f;
    int graphWidth = 800;

	// Use this for initialization
	void Start () {
        aLinha = GetComponent <LineRenderer>();
        posicoes = new Vector3 [NUM_POSICOES];

        float passo = ((float)graphWidth / (float)NUM_POSICOES);

        for (int i=0 ; i<NUM_POSICOES ; i++)
        {
            posicoes[i] = new Vector3 (passo * (float) i , FuncSin(i) , 0) ;
        }
        aLinha.positionCount = posicoes.Length;
        aLinha.SetPositions(posicoes);
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    float FuncSin (int index)
    {
        return (AMPLITUDE * Mathf.Sin(NUM_CICLOS * 2 * Mathf.PI * ((float)index / (float)NUM_POSICOES)));
    }
}
