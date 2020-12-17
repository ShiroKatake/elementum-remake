using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSound : MonoBehaviour
{
	public AudioClip playerLandA;
	public AudioClip playerLandB;
	public AudioClip playerLandC;

	public AudioClip playerLadderA;
	public AudioClip playerLadderB;
	public AudioClip playerLadderC;

	public AudioClip playerDeath;

	private int type;

	// Start is called before the first frame update
	void Start()
    {
		type = 0;
    }

    // Update is called once per frame
    void Update()
    {
    }

	public float LadderSound(float timer)
	{
		if (timer < -0.3f)
		{
				switch (type)
				{
					case 0:
						SoundManager.PlaySound(playerLadderA, 0.5f);
					type = 1;
						break;
					case 1:
						SoundManager.PlaySound(playerLadderB, 0.5f);
					type = 0;
						break;
					case 2:
						SoundManager.PlaySound(playerLadderC);
						break;

				}
				return 0;
		}
		return timer;
	}

	public void LandSound()
	{
		switch (Random.Range(0, 2))
		{
			case 0:
				SoundManager.PlaySound(playerLandA);
				break;
			case 1:
				SoundManager.PlaySound(playerLandB);
				break;
			case 2:
				SoundManager.PlaySound(playerLandC);
				break;

		}
	}

	public void DeathSound()
	{
		SoundManager.PlaySound(playerDeath, 0.35f);
	}
}
