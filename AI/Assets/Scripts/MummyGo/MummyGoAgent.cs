using System.Collections;
using Unity.MLAgents;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Sensors;
using UnityEngine;
using UnityEngineInternal;


public class MummyGoAgent : Agent
{
    public Material good;
    public Material bad;
    public Material origin;
    private Renderer floor;

    public Transform target;
    private new Rigidbody rigid;

    public override void Initialize()
    {

        rigid = GetComponent<Rigidbody>();
        floor = transform.parent.Find("Floor").GetComponent<Renderer>();

        origin = floor.material;

    }

    public override void OnEpisodeBegin()
    {
        rigid.velocity = Vector3.zero;

        transform.localPosition = new Vector3(Random.Range(-4f, 4f), 0.05f, Random.Range(-4f, 4f));
        target.localPosition = new Vector3(Random.Range(-4f, 4f), 0.5f, Random.Range(-4f, 4f));
        StartCoroutine(RecoverCo());
    }

    private IEnumerator RecoverCo()
    {
        yield return new WaitForSeconds(0.2f);
        floor.material = origin;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        // 8
        sensor.AddObservation(target.localPosition);
        sensor.AddObservation(transform.localPosition);
        sensor.AddObservation(rigid.velocity.x);
        sensor.AddObservation(rigid.velocity.z);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var continuousActions = actions.ContinuousActions;

        Vector3 direction = (Vector3.forward * continuousActions[0]) + (Vector3.right * continuousActions[1]);
        rigid.AddForce(direction.normalized * 50f);

        SetReward(-0.01f);
    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var ContinuousActionsOut = actionsOut.ContinuousActions;
        ContinuousActionsOut[0] = Input.GetAxis("Vertical");
        ContinuousActionsOut[1] = Input.GetAxis("Horizontal");
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.tag == "Wall")
        {
            floor.material = bad;
            SetReward(-1f);
            EndEpisode();
        }
        if(collision.collider.tag == "Target")
        {
            floor.material = good;
            SetReward(1f);
            EndEpisode();
        }

    }

}
