using UnityEngine;

public class PlateformeInvisibleTrigger : MonoBehaviour
{
    public GameObject Passage;
    public Animator plateformeAnimator; // Selon la bonne plateforme montante
    public AudioClip sfxDeMontee;
    AudioSource audioSource;
    public float delai = 1.3f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            plateformeAnimator.SetTrigger("Activate");
            Invoke("SoundEffectMontee", delai);
        }
    }

    void SoundEffectMontee()
    {
        audioSource.PlayOneShot(sfxDeMontee);
    }
}
