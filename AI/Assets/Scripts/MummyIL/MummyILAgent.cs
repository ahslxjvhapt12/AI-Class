using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using System.Xml.Schema;

public class MummyILAgent : Agent
{
    public float moveSpeed = 10f, turnSpeed = 50f;

    private ColorTargeter colorTargeter;
    private new Rigidbody rigidbody;
    private Vector3 originPosition;

    public Material goodmaterial, badMaterial;
    private Material originMaterial;
    private Renderer floorRenderer;

    public override void Initialize()
    {
        colorTargeter = transform.parent.GetComponent<ColorTargeter>();
        rigidbody = GetComponent<Rigidbody>();
        originPosition = transform.localPosition;
        floorRenderer = transform.parent.Find("Floor").GetComponent<Renderer>();
        originMaterial = floorRenderer.material;
    }

    public override void OnEpisodeBegin()
    {
        colorTargeter.TargetingColor();

        rigidbody.velocity = rigidbody.angularVelocity = Vector3.zero;
        transform.localPosition = originPosition;
        transform.localRotation = Quaternion.identity;
    }

    public override void CollectObservations(VectorSensor sensor)
    {
        base.CollectObservations(sensor);
    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        var DiscreteActions = actions.DiscreteActions;

        Vector3 direction = Vector3.zero;
        Vector3 rotationAxis = Vector3.zero;

        // DiscreteAction[0] = 정지(0) 전진(1) 후진(2)
        switch (DiscreteActions[0])
        {
            case 1: direction = transform.forward; break;
            case 2: direction = -transform.forward; break;
        }

        // DiscreteAction[0] = 정지(0) 좌회전(1) 우회전(2)
        switch (DiscreteActions[1])
        {
            case 1: rotationAxis = Vector3.zero; break;
            case 2: rotationAxis = Vector3.up; break;
        }

        rigidbody.MovePosition(transform.position + direction * moveSpeed * Time.fixedDeltaTime);

        transform.Rotate(rotationAxis, turnSpeed * Time.fixedDeltaTime);

        AddReward(-0.01f);
    }
    public override void Heuristic(in ActionBuffers actionsOut)
    {

        var discreteActionsOut = actionsOut.DiscreteActions;

        if (Input.GetKey(KeyCode.W))
        {
            discreteActionsOut[0] = 1;
        }
        if (Input.GetKey(KeyCode.S))
        {
            discreteActionsOut[0] = 2;
        }
        if (Input.GetKey(KeyCode.A))
        {
            discreteActionsOut[1] = 1;
        }
        if (Input.GetKey(KeyCode.D))
        {
            discreteActionsOut[1] = 2;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag(colorTargeter.targetColor.ToString()))
        {
            StartCoroutine(ChangeFloorColor(goodmaterial));
            AddReward(1f);
            EndEpisode();
        }
        else if (collision.collider.CompareTag("Hint"))
        {

        }
        else
        {
            StartCoroutine(ChangeFloorColor(badMaterial));
            AddReward(-1f);
            EndEpisode();
        }
    }

    private IEnumerator ChangeFloorColor(Material changeMaterial)
    {
        floorRenderer.material = changeMaterial;
        yield return new WaitForSeconds(0.2f);
        floorRenderer.material = originMaterial;
    }
}
