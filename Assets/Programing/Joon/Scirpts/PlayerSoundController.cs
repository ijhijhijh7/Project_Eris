using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerSoundController : MonoBehaviour
{
    [SerializeField] AudioClip landingClip;
    [SerializeField] AudioClip runClip;
    [SerializeField] AudioClip attack1Clip;
    [SerializeField] AudioClip attack2Clip;
    [SerializeField] AudioClip attack3Clip;
    [SerializeField] AudioClip dashClip;
    [SerializeField] AudioClip dieClip;
    [SerializeField] AudioClip defeatClip;

    [SerializeField] AudioSource runAudioSource;


    // AudioSource 컴포넌트
    private AudioSource audioSource;

    void Start()
    {
        // AudioSource 초기화
        audioSource = GetComponent<AudioSource>();
        if (audioSource == null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
        }
    }

    // 점프 사운드 재생
    public void PlayLandingSound()
    {
        PlaySound(landingClip);
    }

    // 걷기 사운드 재생
    public void StartRunSound()
    {
        runAudioSource.enabled = true; // 오디오 소스 활성화
    }

    // 걷기 사운드 정지
    public void StopRunSound()
    {
        runAudioSource.enabled = false; // 오디오 소스 비활성화
    }

    // 공격 사운드 재생
    public void PlayAttack1Sound()
    {
        PlaySound(attack1Clip);
    }

    public void PlayAttack2Sound()
    {
        PlaySound(attack2Clip);
    }

    public void PlayAttack3Sound()
    {
        PlaySound(attack3Clip);
    }

    public void PlayDashSound()
    {
        PlaySound(dashClip);
    }

    public void PlayDieSound()
    {
        PlaySound(dieClip);
    }

    public void PlayDefeatSound()
    {
        PlaySound(defeatClip);
    }



    // 사운드 재생을 위한 공통 메서드
    private void PlaySound(AudioClip clip)
    {
        if (clip != null)
        {
            audioSource.PlayOneShot(clip);
        }
    }
}
