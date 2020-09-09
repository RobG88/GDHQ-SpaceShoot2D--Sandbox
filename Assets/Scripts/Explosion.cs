using UnityEngine;

public class Explosion : MonoBehaviour
{
    Animator _anim;
    AudioSource _audioSource;
    [SerializeField] AudioClip _explosion;

    void Start()
    {
        _anim = GetComponent<Animator>();
        _audioSource = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.P))
        {
            Explode();
        }
    }

    private void Explode()
    {
        _audioSource.PlayOneShot(_explosion);
        _anim.SetTrigger("Explode");
        Destroy(this.gameObject, 2.5f);
    }
}
