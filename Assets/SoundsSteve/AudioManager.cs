using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioManager : MonoBehaviour
{
    static GameObject instance;
    [SerializeField]
    private AudioClip buttonPress = null;
    
    [SerializeField]  private AudioClip explosion = null;
   // [SerializeField]
   // private AudioClip playerShoot = null;
    [SerializeField] private AudioClip menuSong = null;
    [SerializeField] private AudioClip gameSong = null;
    [SerializeField]
    private static float volume = 1f;
    // Maximum number of AudioSources allowed for this game object.
    // For use with GetAvailableSource().
    [SerializeField]
    private static int maxAudioSourceCount = 20;

    private static List<AudioSource> sources = new List<AudioSource>();
    protected AudioSource music = null;
    enum song { game, menu };

    [SerializeField]
    song songToplay;

    static AudioSource SongSource;
    void Awake() 
        {
            if (instance == null)
            {
                GameObject.DontDestroyOnLoad(  gameObject);
            }
            else
            {
                Destroy(  gameObject);
            }
             if (PlayerPrefs.HasKey("volume")){
                    volume = PlayerPrefs.GetFloat("volume");
             }
        
            if (songToplay == song.game)
            {
                PlayGameMusic();
            }
            else {
                PlayMenuMusic();
            }
        }

    public static void resetVolume()
        {
        volume =   PlayerPrefs.GetFloat("volume");
            if (sources == null)
            {
                sources = new List<AudioSource>();
            }

         

            // Search for an available AudioSource, i.e. the AudioSource not playing an AudioClip.
            // If one is found, stop searching by simply returning it. 
            for (int i = 0; i < sources.Count; i++)
            {
                AudioSource source = sources[i];
                if (source.isPlaying == true)
                {
                    source.volume = volume;
                }
            }
            SongSource.volume = PlayerPrefs.GetFloat("volume");
        }
    public void PlayMenuMusic ()
    {
        PlaySound(  menuSong);
        if (music == null){
            music =   gameObject.AddComponent<AudioSource>();
		
        }
        music.clip = menuSong;
        music.loop = true;
        music.volume = volume;
        music.Play ();
        SongSource = music;
    }
    public void PlayGameMusic ()
    {
        PlaySound(  gameSong);
        if (music == null)
        {
            music =   gameObject.AddComponent<AudioSource>();

        }
        music.clip = gameSong;
        music.volume = volume;
        music.loop = true;
        music.Play ();
        SongSource = music;
    }
    public void PlaybuttonPress()
    {
        //	PlaySound(  buttonPress);
    }
    
    //put it on explosion prefab
    public void Playexplosion()
    {
        PlaySound(  explosion);
    }
    private void PlaySound(AudioClip clip)
    {
        // Grab an AudioSource to play this clip.
        AudioSource source = GetAudioSource();

        // Set the clip to play.
        source.clip = clip;
        source.volume = volume;
        // Play the clip.
        source.Play();
    }

    public static void PlaySoundClip(AudioClip clip)
    {
        // Grab an AudioSource to play this clip.
        AudioSource source = GetAvailableSource();

        // Set the clip to play.
        source.clip = clip;
        source.volume = volume;
        // Play the clip.
        source.Play();
    }
    private static AudioSource GetAudioSource()
    {
        // Try getting the AudioSource component attached to this game object.
        AudioSource source = Camera.main.gameObject.GetComponent<AudioSource>();

        // If an AudioSource component has not yet been added to this game object, add one to this game object
        // and to our list of AudioSources.
        if (source == null)
        {
            source = Camera.main.gameObject.AddComponent<AudioSource>();
            sources.Add(source);
        }

        return source;
    }
    public static void OnChange()
    {
        sources.Clear();


    }

    /// <summary>
    /// Looks for an available AudioSource. An available AudioSource is one that is not playing an AudioClip.
    /// If no available AudioSources exist, it permanently creates a new AudioSource.
    /// </summary>
    /// <returns>The free source.</returns>
    private static AudioSource GetAvailableSource()
    {
        // If the list of AudioSources has not been created yet, create it.
        if (  sources == null)
        {
           sources = new List<AudioSource>();
        }

        // If there are no AudioSources created, add the first AudioSource.
        if (  sources.Count == 0)
        {
            AudioSource firstSource = Camera.main.gameObject.AddComponent<AudioSource>();
              sources.Add(firstSource);
        }

        // Search for an available AudioSource, i.e. the AudioSource not playing an AudioClip.
        // If one is found, stop searching by simply returning it. 
        for (int i = 0; i <   sources.Count; i++)
        {
           
            AudioSource source =   sources[i];
            if(source== null)
            {


            }
            if (source.isPlaying == false)
            {
                return source;
            }
        }




        // Add a new AudioSource component to this manager, save it in the list of sources, and return it.
        if (  sources.Count < maxAudioSourceCount)
        {
            AudioSource newSource = Camera.main.gameObject.AddComponent<AudioSource>();
              sources.Add(newSource);
            return newSource;
        }

        // If we got to this point, that means we have reached our maximum allowed AudioSources.
        // We have no available AudioSources to return.
        return null;
    }


}
