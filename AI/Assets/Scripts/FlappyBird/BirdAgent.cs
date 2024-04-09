using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using TMPro;
using UnityEngine.SceneManagement;

public class BirdAgent : Agent
{
    public Sprite[] sprites;
    public float strength = 5f;
    public float gravity = -9.81f;
    public float tilt = 5f;

    private SpriteRenderer spriteRenderer;
    private Vector3 direction;
    private int spriteIndex;

    private Spawner spawner;
    private TextMeshPro scoreText;
    private int score;
    
    public override void Initialize()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        spawner = transform.parent.Find("Spawner").GetComponent<Spawner>();
        scoreText = transform.parent.Find("ScoreText").GetComponent<TextMeshPro>();

        InvokeRepeating(nameof(AnimateSprite), 0.15f, 0.15f);
        Vector3 position = transform.position;
        position.y = 0f;
        transform.position = position;
        direction = Vector3.zero;
    }

    public override void OnEpisodeBegin()
    {

    }

    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(transform.position.y);
        sensor.AddObservation(direction);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        // 플레이어에 중력 적용
        direction.y += gravity * Time.deltaTime;
        transform.position += direction * Time.deltaTime;

        // 방향에 따라 스프라이트 기울이기
        Vector3 rotation = transform.eulerAngles;
        rotation.z = direction.y * tilt;
        transform.eulerAngles = rotation;
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        if (Input.GetKey(KeyCode.Space))
        {
            if (transform.localPosition.y <= 3.5f)
            {
                Jump();
            }
        }
    }

    private void Jump()
    {
        direction = Vector3.up * strength;
    }

    private void AnimateSprite()
    {
        spriteIndex++;

        if (spriteIndex >= sprites.Length)
        {
            spriteIndex = 0;
        }

        if (spriteIndex < sprites.Length && spriteIndex >= 0)
        {
            spriteRenderer.sprite = sprites[spriteIndex];
        }
    }
    private void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1f);
        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            IncreaseScore();
        }
    }

}
