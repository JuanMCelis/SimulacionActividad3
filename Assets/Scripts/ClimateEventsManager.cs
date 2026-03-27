using System;
using UnityEngine;

public class ClimateEventsManager : MonoBehaviour
{

    //Instancia global para poder llamar esta clase desde otros scripts
    public static ClimateEventsManager Instance { get; private set; }

    // Tipos de clima (despejado, lluvia y tormenta)
    public enum ClimateState { Clear, Rain, Storm }
    public ClimateState CurrentClimate { get; private set; } = ClimateState.Clear;

    //Evento (opcional, por si otros scripts quieren reaccionar)
    public event Action<ClimateState> OnClimateChanged;

    //Tiempo que va a durar cada clima
    public float minTime = 10f;
    public float maxTime = 20f;
    private float timer;

    // Efectos visuales de los climas para sensación de ejecución
    public ParticleSystem rainParticles;
    public ParticleSystem stormParticles;
    public UnityEngine.UI.Image lightningImage;

 
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        timer = durationPerClimate;
        SetClimate(ClimateState.Clear); //Esta linea lo que hace es que siempre el clima empezara definido como despejado
    }

    // Update is called once per frame
    void Update()
    {
        timer -= Time.deltaTime;

        // Cuando el temporizador  para cambiar al clima llega a 0 el clima va a cambiar
        if (timer <= 0f)
        {
            NextClimate(); //Llama a la funcion de cambio de clima

            // Se define un tiempo aleatorio entre 15 segundos o 25 segundos
            timer = UnityEngine.Random.Range(15f, 25f);
        }
    }
}
