using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    

    [Header("Camera Shake Values")]
    [SerializeField]
    float _magnitude = 1f;
    [SerializeField]
    float _duration = 0.3f;

    [SerializeField]
    GameManager _gameManager;

    void Update()
    {
        if (_gameManager.isGameOVer)
        {
            StopAllCoroutines();
        }
    }

    public void CamaraShake()
    {
        StartCoroutine(ShakeSequence());
    }

    IEnumerator ShakeSequence()
    {
        Vector3 _cameraPos = transform.position;

        float elapsedTime = 0;

        while (elapsedTime <= _duration)
        {
            float xValue = Random.Range(-0.5f, 0.5f) * _magnitude;
            float yValue = Random.Range(-0.5f, 0.5f) * _magnitude;

            transform.position = new Vector3(xValue, yValue, _cameraPos.z);

            elapsedTime += Time.deltaTime;

            yield return 0;
        }

        transform.position = _cameraPos;

    }


}
