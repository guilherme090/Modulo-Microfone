using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MicScript : MonoBehaviour {
	public AudioSource AS;
    public string microfone = "";
    public bool tocou = false;
    public Dropdown microfoneLista;

    private List<string> opcoes = new List<string>();
	void Start () {
        AS = GetComponent<AudioSource> ();
        AS.Stop();
        	    
        foreach (string device in Microphone.devices)
        {
            //vai criando a lista de microfones
            Debug.Log(device);
            if (microfone == "")
            microfone = device;
            opcoes.Add(microfone);
        }

        //coloca a lista de microfones no drop-down
        microfoneLista.AddOptions(opcoes);
        microfoneLista.onValueChanged.AddListener(delegate{microfoneListaMudouValor(microfoneLista);});

        Debug.Log(Microphone.IsRecording(microfone));
        Debug.Log("Gravando");
        AS.clip = Microphone.Start(microfone, true, 5, 48000);
        //AS.loop = true;
        Debug.Log(microfone);
        /* int minFreq;
           int maxFreq;
           Microphone.GetDeviceCaps(null, out minFreq, out maxFreq);
           Debug.Log("Min = " + minFreq + "/ Max = " + maxFreq); */
        AS.Play();
    }
	
	void Update () {
        tocarMic();  
    }

    void tocarMic()
    {
        if (AS.isPlaying == false)
        {
            Debug.Log("Tocando de novo");
            AS.Play();
        }
    }

    public void microfoneListaMudouValor(Dropdown listaMic)
    {
        Debug.Log("Trocando de microfone...");
        AS.Stop();
        microfone = opcoes[listaMic.value];
    }
}


