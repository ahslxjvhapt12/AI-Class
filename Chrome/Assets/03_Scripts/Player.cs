using TMPro;
using Unity.Burst.CompilerServices;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;

public class Player : Agent
{
    [SerializeField] float jumpPower;
    [SerializeField] private TextMeshPro scoreText;

    [SerializeField] ObstacleGenerator generator;

    Rigidbody2D _rigid;
    private int score;

    public override void Initialize()
    {
        _rigid = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        GameStart();
    }

    public override void CollectObservations(VectorSensor sensor)
    {

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var discreteActions = actions.DiscreteActions;

        if (discreteActions[0] == 1)
        {
            if (Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Ground")))
            {
                Jump();
            }
        }
        AddReward(0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        Debug.Log(2);
        var discreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.Space))
        {
            if (Physics2D.OverlapBox(transform.position, new Vector2(1, 1), 0, LayerMask.GetMask("Ground")))
            {
                discreteActionsOut[0] = 1;
            }
        }
    }
    private void IncreaseScore()
    {
        score++;
        scoreText.text = score.ToString();
    }

    private void GameStart()
    {
        Debug.Log("Restart");
        score = 0;
        scoreText.SetText($"Score : {score}");

        if (generator.obstacles.Count > 0)
        {

            foreach (GameObject item in generator.obstacles)
            {
                Destroy(item);
            }

            generator.obstacles.Clear();
            
        }
    }

    [ContextMenu("T")]
    private void Jump()
    {
        _rigid.AddForce(Vector3.up * jumpPower, ForceMode2D.Impulse);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Obstacle"))
        {
            AddReward(-1f);
            EndEpisode();

        }
        else if (other.gameObject.CompareTag("Scoring"))
        {
            AddReward(1f);
            IncreaseScore();
        }
    }
}
