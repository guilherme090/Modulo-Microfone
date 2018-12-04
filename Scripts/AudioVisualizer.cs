using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class AudioVisualizer : MonoBehaviour {

    public Transform[] paralelepipedos;
    float multiplicadorMaximo = 5000;
    [Range(1, 5000)] public float multiplicador = 2000f;
    public int amostras = 1024;
    public FFTWindow fftWindow;
    public float tempoDeLerp = 0.5f;
    public Text texto;
    private float tempoFrame = 0.0f;
    private const float tempoPorFrame = 0.05f;
    int taxaDeAmostragem = 48000;
    public Slider sensibilidade;
    public float freqInicial = 100f;
    public LineRenderer aLinha;
    Vector3[] posicoes; 
    const float GRAPH_WIDTH = 800f;
    public float amplit;
    const float AMP_INICIAL = 4000f;
    const float RAZAO_DE_BANDA = 5.0f; //dividir a largura do espectro por esse valor e exibir só a primeira parte do espectro

    private void Start()
    {
        sensibilidade.value = multiplicador/multiplicadorMaximo;
        sensibilidade.onValueChanged.AddListener(delegate { MudouSensibilidade(sensibilidade); });
        amplit = AMP_INICIAL;
        //aLinha = GetComponent<LineRenderer>();
    }

    // Update is called once per frame
    void Update () {
        //atualiza tempo decorrido
        tempoFrame = tempoFrame + Time.deltaTime;
        if (tempoFrame >= tempoPorFrame)
        {
            tempoFrame = 0.0f;
            //inicializa o vetor
            float[] espectro = new float[amostras];

            //obtem os dados de espectro sonoro
            GetComponent<AudioSource>().GetSpectrumData(espectro, 0, fftWindow);

            //o loop e agora!
            for (int i = 0; i < paralelepipedos.Length; i++)
            {
                //aplica escala
                float alturaFinal = espectro[20*i] * multiplicador;

                float lerpY = Mathf.Lerp(paralelepipedos[i].localScale.y, alturaFinal, tempoDeLerp);
                Vector3 newScale = new Vector3(paralelepipedos[i].localScale.x, lerpY, paralelepipedos[i].localScale.z);

                //aplicar a escala ao objeto
                paralelepipedos[i].localScale = newScale;

                //            Debug.Log(paralelepipedos[i].localScale.x + lerpY + paralelepipedos[i].localScale.z);
                texto.text = " f0 = " + Ffund(espectro).ToString() + " Hz";
            }

            //aplicar um loop semelhante para o Line Renderer
            
            int quebraFuncao = (int)(espectro.Length / RAZAO_DE_BANDA); //quebrar o for neste ponto
            float passo = (GRAPH_WIDTH / (float)espectro.Length) * RAZAO_DE_BANDA; 
            posicoes = new Vector3[quebraFuncao + 1];
            for (int j = 0; j < espectro.Length; j++)
            {
                float alturaFinal = espectro[j] * amplit;
                posicoes[j] = new Vector3(passo * j , alturaFinal, 0);
                if (j >= quebraFuncao) break;
            }
            aLinha.positionCount = posicoes.Length;
            aLinha.SetPositions(posicoes);

        }
    }

    float Ffund(float[] espectro)
    {
        int indiceMaximo = 0;
        float alturaMaxima = 0.0f;
        int i = FreqToIndex (freqInicial);

        for ( ; i<espectro.Length; i++)
        {
            if (espectro[i] > alturaMaxima)
            {
                indiceMaximo = i;
                alturaMaxima = espectro[i];
            }
        }
        //calcular frequencia do indice maximo
        float frequenciaMaxima = IndexToFreq(indiceMaximo);
               
        return frequenciaMaxima;
    }

    float IndexToFreq (int indice)
    {
        return ((indice * (taxaDeAmostragem / 2)) / amostras);
    }

    int FreqToIndex(float freq)
    {
        return (int) ((freq * amostras * 2) / taxaDeAmostragem);
    }

    void MudouSensibilidade (Slider nivel)
    {
        multiplicador = (nivel.value) * multiplicadorMaximo;
        amplit = AMP_INICIAL * (nivel.value + 1);
    }
}
