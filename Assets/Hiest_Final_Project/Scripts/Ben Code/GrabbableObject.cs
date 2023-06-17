using UnityEngine;
using UnityEngine.Timeline;

public class GrabbableObject : MonoBehaviour
{
    public Material highlightMaterial;
    private Material defaultMaterial;

    public AudioClip grabAudio;
    private AudioSource audioSource;

    private void Start()
    {
       // assign audioSource to a new AudioSource
        audioSource = gameObject.AddComponent<AudioSource>();
        // if grabAudio is not null, Audiosource.clip = grabAudio
        if (grabAudio != null)
        {
            audioSource.clip = grabAudio;
        }
        // Set audio to be 3D
        audioSource.spatialBlend = 1.0f;
    }

    private void Update()
    {
        // SetHighlight(toggleGrabbed);
    }

    private void Awake()
    {
        defaultMaterial = GetComponent<Renderer>().material;
    }

    public void SetHighlight(bool value)
    {
        GetComponent<Renderer>().material
            = value ? highlightMaterial : defaultMaterial;
    }

    public void PlayAudio()
    {
        // if audiosource.clip is not null, play the sound
        if (audioSource.clip != null)
        {
            // Play the sound
            audioSource.Play();
        }
    }
}
