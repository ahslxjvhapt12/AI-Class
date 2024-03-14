using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;

public class FloorAgent : Agent
{
    public Transform ballTrm;
    private Rigidbody ballRigid;

    // 처음 한번 실행되는 초기화 함수
    public override void Initialize()
    {

        ballRigid = ballTrm.GetComponent<Rigidbody>();

    }

    // 각 에피소드가 시작될 때 
    public override void OnEpisodeBegin()
    {
        // Floor X Y Z 무작위하게 회전시킨다

        transform.rotation = new Quaternion(0, 0, 0, 0);
        transform.Rotate(new Vector3(1, 0, 0), Random.Range(-10f, 10f));
        transform.Rotate(new Vector3(0, 0, 1), Random.Range(-10f, 10f));

        ballRigid.velocity = Vector3.zero;
        ballTrm.localPosition = new Vector3(Random.Range(-1.5f, 1.5f), 1.5f, Random.Range(-1.5f, 1.5f));

    }

    // 에이전트의 관측을 생성하는 함수 => 벡터 관측
    public override void CollectObservations(VectorSensor sensor)
    {
        // 관측하는 값의 갯수 : 8(1, 1, 3, 3)
        sensor.AddObservation(transform.rotation.x);
        sensor.AddObservation(transform.rotation.z);
        sensor.AddObservation(ballRigid.velocity);
        sensor.AddObservation(ballTrm.position - transform.position);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {

        var continuousActions = actions.ContinuousActions;
        float z_rotation = Mathf.Clamp(continuousActions[0], -1f, 1f);
        float x_rotation = Mathf.Clamp(continuousActions[1], -1f, 1f);

        transform.Rotate(new Vector3(0, 0, 1), z_rotation);
        transform.Rotate(new Vector3(1, 0, 0), x_rotation);

        if (ballTrm.position.y - transform.position.y < -2f)
        {

            SetReward(-1f);
            EndEpisode();

        }
        else if (Mathf.Abs(ballTrm.position.x - transform.position.x) > 2.5f)
        {

            SetReward(-1f);
            EndEpisode();

        }
        else if (Mathf.Abs(ballTrm.position.z - transform.position.z) > 2.5f)
        {

            SetReward(-1f);
            EndEpisode();

        }
        else
        {

            SetReward(0.1f);

        }

    }

    // 사람이 Agent를 수동으로 제어하는 방법을 설정하는 함수
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        // 방향키 입력을 통해 행동값을 설정한다.
        var ContinuousActionsOut = actionsOut.ContinuousActions;
        ContinuousActionsOut[0] = -Input.GetAxis("Horizontal");
        ContinuousActionsOut[1] = Input.GetAxis("Vertical");

    }
}
