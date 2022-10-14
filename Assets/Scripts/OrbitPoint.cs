using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class OrbitPoint : MonoBehaviour
{
    Boss _boss;

    float _orbitingSpeed;

    // Start is called before the first frame update
    void Start()
    {
        _boss = transform.parent.GetComponent<Boss>();

        _orbitingSpeed = _boss.OrbitingSpeed;
    }

    // Update is called once per frame
    void Update()
    {
        transform.RotateAround(_boss.transform.position, new Vector3(0, 0, 1), _orbitingSpeed * Time.deltaTime);
    }
}
