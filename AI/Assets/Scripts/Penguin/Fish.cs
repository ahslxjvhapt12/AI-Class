using UnityEngine;

public class Fish : MonoBehaviour
{
    private float fishSpeed;
    private float nextActionTime = -1f;
    private Vector3 targetposition;

    private void FixedUpdate()
    {
        Swim();
    }

    private void Swim()
    {
        if (Time.fixedTime >= nextActionTime)
        {
            // ���ο� ��ġ�� Ÿ�����ϰ� nextActionTime�� �ٽ� �����Ѵ�.
            fishSpeed = Random.Range(0.1f, 0.8f);
            targetposition = PenguinArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);
            transform.rotation = Quaternion.LookRotation(targetposition - transform.position);

            float timeToTarget = Vector2.Distance(transform.position, targetposition) / fishSpeed; ;
            nextActionTime = Time.fixedTime + timeToTarget;
        }
        else
        {
            // Ÿ���õ� ��ġ�� ���� �̵��Ѵ�
            transform.position += transform.forward * Time.fixedDeltaTime * fishSpeed;
        }
    }
}
