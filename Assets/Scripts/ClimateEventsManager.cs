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
    public float durationPerClimate = 10f;
    private float timer;

    // Efectos visuales de los climas para sensación de ejecución
    public ParticleSystem rainParticles;
    public ParticleSystem stormParticles;
    public UnityEngine.UI.Image lightningImage;

    //Se ejecuta al iniciar el objeto
    void Awake()
    {
        // Linea para evitar duplicados)
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
