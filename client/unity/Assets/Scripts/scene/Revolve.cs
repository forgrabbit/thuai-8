using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Revolve : MonoBehaviour
{
    [Tooltip("��ת����")]
    public Transform BlackHole;

    [Tooltip("��ת�ٶȣ���/�룩")]
    public float orbitSpeed = 30f;

    private void Start()
    {
        BlackHole = GameObject.Find("BlackHole").GetComponent<Transform>();
    }

    void Update()
    {
        if (BlackHole != null)
        {
            // ��̫����ת��Y��Ϊ��ת�ᣬ����ƽ���˶���
            transform.RotateAround(
                BlackHole.position,
                Vector3.up,
                orbitSpeed * Time.deltaTime
            );
        }
    }
}
