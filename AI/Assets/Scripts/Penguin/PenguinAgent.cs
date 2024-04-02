using System;
using System.Data.SqlTypes;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngine.UI;

public class PenguinAgent : Agent
{
    public float moveSpeed = 5f;
    public float turnSpeed = 180f;
    public GameObject heartPrefab;
    public GameObject refurgitatedFishPrefab;

    private PenguinArea penguinArea;
    private new Rigidbody rigidbody;
    private GameObject babyPenguin;
    private bool isFull;

    public override void Initialize()
    {
        penguinArea = transform.parent.Find("PenguinArea").GetComponent<PenguinArea>();
        babyPenguin = penguinArea.penguinBaby;
        rigidbody = GetComponent<Rigidbody>();
    }

    public override void OnEpisodeBegin()
    {
        isFull = false;
        penguinArea.ResetArea();
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 관측 값 8개
        sensor.AddObservation(isFull);
        sensor.AddObservation(Vector3.Distance(transform.position, babyPenguin.transform.position));
        sensor.AddObservation((babyPenguin.transform.position - transform.position).normalized);
        sensor.AddObservation(transform.forward);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var DiscreteActions = actions.DiscreteActions;

        // 앞으로 가지 않을지(0), 갈지(1)
        float forwardAmount = DiscreteActions[0];

        float turnAmount = 0f;
        if (DiscreteActions[1] == 1)
        {
            turnAmount = -1f;
        }
        else if (DiscreteActions[1] == 2)
        {
            turnAmount = 1f;
        }

        rigidbody.MovePosition(transform.position + transform.forward * forwardAmount * moveSpeed * Time.fixedDeltaTime);
        transform.Rotate(Vector3.up * turnAmount * turnSpeed * Time.fixedDeltaTime);

        AddReward(-1f / MaxStep);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var DiscreteActionsOut = actionsOut.DiscreteActions;
        if (Input.GetKey(KeyCode.W))
        {
            DiscreteActionsOut[0] = 1;
        }
        else if (Input.GetKey(KeyCode.A))
        {
            DiscreteActionsOut[1] = 1;
        }
        else if (Input.GetKey(KeyCode.D))
        {
            DiscreteActionsOut[1] = 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.transform.CompareTag("Fish"))
        {
            EatFish(collision.gameObject);
        }
        else if (collision.transform.CompareTag("BabyPenguin"))
        {
            RegurgitateFish();
        }
    }

    private void EatFish(GameObject fishObject)
    {
        if (isFull) return;
        isFull = true;

        penguinArea.RemoveFishInList(fishObject);
        AddReward(1f);
    }

    private void RegurgitateFish()
    {
        if (!isFull) return;
        isFull = false;

        GameObject regurgitatedFish = Instantiate(refurgitatedFishPrefab);
        regurgitatedFish.transform.parent = transform.parent;
        regurgitatedFish.transform.localPosition = babyPenguin.transform.localPosition + Vector3.up * 0.01f;
        Destroy(regurgitatedFish, 4f);

        GameObject heart = Instantiate(heartPrefab);
        heart.transform.parent = transform.parent;
        heart.transform.localPosition = babyPenguin.transform.localPosition + Vector3.up;

        AddReward(1f);

        if(penguinArea.remainingFish <= 0)
        {
            EndEpisode();
        }
    }

}
