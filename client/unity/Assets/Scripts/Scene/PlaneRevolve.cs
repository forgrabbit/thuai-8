using BattleCity;
using QFramework;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlaneRevolve : MonoBehaviour
{
    [Tooltip("��ת����")]
    public Transform BlackHole;

    [Tooltip("������ת�ٶȣ���/�룩")]
    public float orbitSpeed = 30f;

    [Tooltip("����ģʽ����ʱ�䣨�룩")]
    public float normalDuration = 60f;

    [Tooltip("����ģʽ����ʱ�䣨�룩")]
    public float acceleratedDuration = 4f;

    [Tooltip("���ü���")]
    public bool useAcceleration = false;

    private float totalDuration;
    private float speedMultiplier;
    private float initialRadius;
    private float initialAngle;
    private float elapsedTime;

    private Vector3 initialPosition;
    private Quaternion initialRotation;
    public Vector3 velocity = Vector3.zero;



    void Start()
    {
        if (BlackHole == null)
            BlackHole = GameObject.Find("BlackHole").transform;

        Vector3 dir = transform.position - BlackHole.position;
        initialRadius = dir.magnitude;
        initialAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        totalDuration = useAcceleration ? acceleratedDuration : normalDuration;
        speedMultiplier = normalDuration / totalDuration;
        elapsedTime = 0f;

        initialPosition = transform.position;
        initialRotation = transform.rotation;

        TypeEventSystem.Global.Register<BattleEndEvent>(e =>
        {
            ResetPosition();
        });
    }

    void Update()
    {
        if (SceneData.GameStage == "Battle")
        {
            Approach();
        }
        else
        {
            JustRevolve();
        }
    }

    void Approach()
    {
        if (elapsedTime >= totalDuration) return;

        elapsedTime += Time.deltaTime;
        float progress = Mathf.Clamp01(elapsedTime / totalDuration);

        // ��¼�ƶ�ǰ��λ�����ڼ��㷽��
        Vector3 previousPosition = transform.position;

        // ���㵱ǰ�������
        float currentRadius = initialRadius * (1 - progress);
        float currentAngle = initialAngle + orbitSpeed * speedMultiplier * elapsedTime;

        // ����λ��
        float angleRad = currentAngle * Mathf.Deg2Rad;
        Vector3 newPosition = BlackHole.position + new Vector3(
            Mathf.Cos(angleRad),
            0,
            Mathf.Sin(angleRad)
        ) * currentRadius;

        transform.position = newPosition;

        Vector3 moveDirection = newPosition - previousPosition;
        if (moveDirection != Vector3.zero)
        {
            // ʹ�� LookRotation ������Ĭ�����Ϸ���
            // transform.rotation = Quaternion.LookRotation(moveDirection.normalized, Vector3.up);

            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);
            transform.rotation = Quaternion.Slerp(
                transform.rotation,
                targetRotation,
                Time.deltaTime * 10f
            );

            // �����Ҫ����ԭ����б�Ƕȣ�������Ӷ�����ת��
            // transform.rotation *= Quaternion.Euler(0, 0, ԭ����б�Ƕ�);
        }

        

        // �������ĺ�ֹͣ����
        if (progress >= 1f)
            enabled = false;
    }

    void ResetPosition()
    {
        // ��������λ�ú���ת
        transform.position = initialPosition;
        transform.rotation = initialRotation;

        // ���¼����ʼ�������
        Vector3 dir = transform.position - BlackHole.position;
        initialRadius = dir.magnitude;
        initialAngle = Mathf.Atan2(dir.z, dir.x) * Mathf.Rad2Deg;

        // ���ü�ʱ��������״̬
        elapsedTime = 0f;
        enabled = true;
    }

    void JustRevolve()
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

    // �ṩ���ٿ��صĹ�������
    public void ToggleAcceleration(bool accelerated)
    {
        useAcceleration = accelerated;
        // ���³�ʼ������
        Start();
    }
}