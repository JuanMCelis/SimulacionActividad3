using System;
using UnityEngine;

public class ClimateEventsManager : MonoBehaviour
{

    //Instancia global para poder llamar esta clase desde otros scripts
    public static ClimateEventsManager Instance { get; private set; }

    // Tipos de clima (despejado, lluvia y tormenta)
    public enum ClimateState { Clear, Rain, Storm } //Enumera los climas del 0 al 2 siendo 0 - Despejado, 1 - Lluvia y 2 - tormenta
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
        timer = UnityEngine.Random.Range(minTime, maxTime);
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

            // Se define un tiempo aleatorio entre 10 segundos a 20 segundos
            timer = UnityEngine.Random.Range(minTime, maxTime);
        }
    }


    // Función donde se ejecutara la logica del cambio climatico
    void NextClimate()
    {

        int randomClimate = UnityEngine.Random.Range(0, 3); // Nos bota un numero del 0 al 2
        SetClimate((ClimateState)randomClimate); //Segun el numero que sale se escoge el clima al que va a pasar. Llama a la enum del inicio y si sale 0 despejado 1 lluvia y 2 tormenta.
    }

    void SetClimate(ClimateState newClimate)
    {
        CurrentClimate = newClimate; //Aqui se va a guardar el clima que se va a ejecutar. 

        // Los efectos del clima anterior se borrarar de la pantalla
        if (rainParticles != null) //Si hay lluvia la desactiva
            rainParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (stormParticles != null) //Si hay tormenta la desactiva
            stormParticles.Stop(true, ParticleSystemStopBehavior.StopEmittingAndClear);

        if (lightningImage != null) //El rayo lo hace invisible 
            lightningImage.color = new Color(1, 1, 1, 0);

        // Un switch para activar un nuevo clima
        switch (newClimate)
        {
            case ClimateState.Clear: //Si se recibe de climateState el numero 0, el clima se despeja
                Debug.Log("Clima despejado");
                break;

            case ClimateState.Rain: //Si se recibe de climateState el numero 1, el clima es lluvia
                Debug.Log("Lluvia");
                if (rainParticles != null)
                    rainParticles.Play(); // Salen las particulas de lluvia
                break;

            case ClimateState.Storm:
                Debug.Log("Tormenta");  //Si se recibe de climateState el numero 0, el clima es tormenta
                if (stormParticles != null)
                    stormParticles.Play(); //Hace que salgan las cosas para simular la tormenta

                if (lightningImage != null)  
                    StartCoroutine(LightningFlash());  //Activa flashes para simular rayos
                break;
        }

        // Avisar a otros scripts
        OnClimateChanged?.Invoke(newClimate);
    }


    // Funcion para el efecto de rayo como un parpadeo rapido en pantalla
    System.Collections.IEnumerator LightningFlash() //Crea una funcion que se ejecuta con pausas mientras sigue funcionando la simulación.
    {
        while (CurrentClimate == ClimateState.Storm) //Un while que funciona si el clima es tormenta ----> Seria algo tipo; mientras el clima sea tormenta ejecuta lo de down
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(3f, 6f)); // Está linea lo que nos permite que haya un tiempo tipo como de recarga para que vuelva a salir el efecto, esta configuarada
            //entre 3 a 6 segundos

            if (lightningImage != null) //Verifica que la imagen exista
            {
                lightningImage.color = new Color(1, 1, 1, 0.8f); //Activa el falsh  del rayo haciendolo visible en 0.8 de opacidad
                yield return new WaitForSeconds(0.1f); //Espera 0.1 segundos para que sea muy rapido el efecto tipo un destello
                lightningImage.color = new Color(1, 1, 1, 0); // Desactiva el efecto haciendolo invisible con 0 de opacidad
            }
        }
    }


    // Funcion para reducir la vision del conejo y el depredador segund el clima
    public float GetVisionMultiplier()
    {
        switch (CurrentClimate) //Segun el clima 
        {
            case ClimateState.Rain:
                return 0.8f;

            case ClimateState.Storm:
                return 0.5f;

            default:
                return 1f;
        }
    }

}
