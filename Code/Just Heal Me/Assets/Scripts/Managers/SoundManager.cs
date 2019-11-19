using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
	private AudioSource _audioSource;

	public AudioClip HitSound;
	public AudioClip HealSound;
	public AudioClip CastingHealSound;
	public AudioClip StunSound;

	// Start is called before the first frame update
	void Start()
    {
		_audioSource = GetComponent<AudioSource>();
	}

    // Update is called once per frame
    void Update()
    {
        
    }

	public void PlayHitSound()
	{
		_audioSource.PlayOneShot(HitSound);
	}

	public void PlayHealSound()
	{
		_audioSource.PlayOneShot(HealSound);
	}

	public void PlayCastingHealSound()
	{
		_audioSource.PlayOneShot(CastingHealSound);
	}

	public void PlayStunSound()
	{
		_audioSource.PlayOneShot(StunSound);
	}
}
