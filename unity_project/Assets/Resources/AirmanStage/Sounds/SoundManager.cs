using UnityEngine;
using System.Collections;

public class SoundManager : MonoBehaviour 
{
	// Private Instance Variables
	private string path = "AirmanStage/Sounds/";
	private AudioSource m_stageMusic;
	private AudioSource m_stageEndMusic;
	private AudioSource m_megamanLeavesStageSound;
	private AudioSource m_deathSound;
	private AudioSource m_hurtingSound;
	private AudioSource m_landingSound;
	private AudioSource m_shootingSound;
	private AudioSource m_bossMusic;
	private AudioSource m_bossDoorSound;
	private AudioSource m_bossHurtingSound;
	private AudioSource m_healthBarFillSound;
	
	/* */
	public void PlayBossDoorSound() { m_bossDoorSound.Play(); }
	
	/* */
	public void StopBossDoorSound() { m_bossDoorSound.Stop(); }
	
	/* */
	public void PlayHealthBarFillSound() {	m_healthBarFillSound.Play(); }
	
	/**/
	public void StopHealthBarFillSound() {	m_healthBarFillSound.Stop(); }
	
	/* */
	public void PlayStageMusic() {	m_stageMusic.Play(); }
	
	/* */
	public void StopStageMusic() { m_stageMusic.Stop(); }
	
	/* */
	public void PlayBossTheme() { m_bossMusic.Play(); }
	
	/* */
	public void StopBossTheme() { m_bossMusic.Stop(); }
	
	/* */
	public void PlayBossHurtingSound() { m_bossHurtingSound.Play(); }
	
	/* */
	public void StopBossHurtingSound() { m_bossHurtingSound.Stop(); }
	
	/* */
	public void PlayStageFinishedSong() { m_stageEndMusic.Play(); }
	
	/* */
	public void StopStageFinishedSong() { m_stageEndMusic.Stop(); }
	
	/* */
	public void PlayDeathSound() { m_deathSound.Play(); }
	
	/* */
	public void StopDeathSound() { m_deathSound.Stop(); }
	
	/* */
	public void PlayHurtingSound() { m_hurtingSound.Play(); }
	
	/* */
	public void StopHurtingSound() { m_hurtingSound.Stop(); }
	
	/* */
	public void PlayLandingSound() { m_landingSound.Play(); }
	
	/* */
	public void StopLandingSound() { m_landingSound.Stop(); }
	
	/* */
	public void PlayShootingSound() { m_shootingSound.Play(); }
	
	/* */
	public void StopShootingSound() { m_shootingSound.Stop(); }
	
	/* */
	public void PlayMegamanLeavesStageSound() { m_megamanLeavesStageSound.Play(); }
	
	/* */
	public void StopMegamanLeavesStageSound() { m_megamanLeavesStageSound.Stop(); }
	
	/* */
	private AudioSource AddAudio(AudioClip clip, bool loop, bool playAwake, float vol) 
	{
		AudioSource newAudio = gameObject.AddComponent<AudioSource>();
		newAudio.clip = clip;
		newAudio.loop = loop;
		newAudio.playOnAwake = playAwake;
		newAudio.volume = vol;
		return newAudio;
	}
	
	/* Use this for initialization */
	private void Awake() 
	{		
		AudioClip stageMusic = (AudioClip) Resources.Load( path + "StageMusic" );
		m_stageMusic = AddAudio(stageMusic, true, true, 0.50f);
		
		AudioClip stageEndMusic = (AudioClip) Resources.Load( path + "StageEndMusic" );
		m_stageEndMusic = AddAudio(stageEndMusic, false, false, 1.0f);
		
		AudioClip deathSound = (AudioClip) Resources.Load( path + "DeathSound" );
		m_deathSound = AddAudio(deathSound, false, false, 0.25f);
		
		AudioClip healthBarFillSound = (AudioClip) Resources.Load( path + "HealthBarFillSound" );
		m_healthBarFillSound = AddAudio(healthBarFillSound, true, false, 1.0f);
		
		AudioClip hurtingSound = (AudioClip) Resources.Load( path + "HurtingSound" );
		m_hurtingSound = AddAudio(hurtingSound, false, false, 1.0f);
		
		AudioClip landingSound = (AudioClip) Resources.Load( path + "LandingSound" );
		m_landingSound = AddAudio(landingSound, false, false, 1.0f);
		
		AudioClip shootingSound = (AudioClip) Resources.Load( path + "ShootingSound" );
		m_shootingSound = AddAudio(shootingSound, false, false, 1.0f);		
		
		AudioClip bossMusic = (AudioClip) Resources.Load( path + "BossMusic" );
		m_bossMusic = AddAudio(bossMusic, true, true, 0.90f);
		
		AudioClip bossDoorSound = (AudioClip) Resources.Load( path + "BossDoorSound" );
		m_bossDoorSound = AddAudio(bossDoorSound, false, false, 0.75f);
		
		AudioClip bossHurtingSound = (AudioClip) Resources.Load( path + "BossHurtingSound" );
		m_bossHurtingSound = AddAudio(bossHurtingSound, false, false, 0.95f);
		
		AudioClip megamanLeaves = (AudioClip) Resources.Load( path + "MegamanLeaveStageSound" );
		m_megamanLeavesStageSound = AddAudio(megamanLeaves, false, false, 0.99f);
		
		PlayStageMusic();
	}
}
