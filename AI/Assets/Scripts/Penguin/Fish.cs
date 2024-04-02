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
            // 새로운 위치를 타겟팅하고 nextActionTime을 다시 세팅한다.
            fishSpeed = Random.Range(0.1f, 0.8f);
            targetposition = PenguinArea.ChooseRandomPosition(transform.parent.position, 100f, 260f, 2f, 13f);
            transform.rotation = Quaternion.LookRotation(targetposition - transform.position);

            float timeToTarget = Vector2.Distance(transform.position, targetposition) / fishSpeed; ;
            nextActionTime = Time.fixedTime + timeToTarget;
        }
        else
        {
            // 타겟팅된 위치를 향해 이동한다
            transform.position += transform.forward * Time.fixedDeltaTime * fishSpeed;
        }
    }
}
