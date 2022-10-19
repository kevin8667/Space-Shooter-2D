using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetingCursor : MonoBehaviour
{
    [SerializeField]
    float _targetingDuration = 3f;

    GameObject _player;

    Boss _boss;

    [SerializeField]
    AudioClip _lockOnSFX;

    AudioSource _audioSource;

    ParticleSystem.EmissionModule _cursorEmission;

    // Start is called before the first frame update
    void Start()
    {
        _player = GameObject.Find("Player");

        _cursorEmission = transform.GetChild(0).GetComponent<ParticleSystem>().emission;

        _boss = FindObjectOfType<Boss>();

        StartCoroutine(SearchingTarget());

        _audioSource = GetComponent<AudioSource>();

        if (_audioSource != null)
        {
            _audioSource.clip = _lockOnSFX;
        }

    }

    IEnumerator SearchingTarget()
    {
        float elapsedTime = 0;

        while (elapsedTime < _targetingDuration && _player != null)
        {
            elapsedTime += Time.deltaTime;

            transform.position = Vector2.MoveTowards(transform.position, _player.transform.position, 4f * Time.deltaTime);

            _boss.TurretFaceTarget();

            yield return null;
        }

        yield return new WaitForSeconds(0.1f);

        _audioSource.Play();

        _cursorEmission.rateOverTime = 6f;

        yield return new WaitForSeconds(1f);

        if(_boss != null)
        {
            _boss.StartShootingLaser();
        }

        Destroy(gameObject, 2f);
    }
}
