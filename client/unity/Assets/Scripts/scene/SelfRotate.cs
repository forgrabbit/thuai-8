using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    [Tooltip("��ת�ٶȣ���/�룩")]
    public float rotationSpeed = 10f;

    void Update()
    {
        // ��Y����ת���ɸ�����Ҫ������ת�ᣩ
        transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime);
    }
}
