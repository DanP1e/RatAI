using NeuralNetworkLibrary;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class BallTest : GenerationMember
{
    public float Speed;
    public Rigidbody Rigidbody;
    private GameObject Target;
    private void Awake()
    {
        FindTarget();
        Rigidbody = GetComponent<Rigidbody>();
    }
    private void Update()
    {
        Step();
        Score = -Vector3.Distance(transform.position, Target.transform.position);
    }
    
    private void FindTarget()
    {
        Target = GameObject.FindGameObjectWithTag("Target");
    }
    public void Step() 
    {
        Vector2 targetPos = new Vector2(Target.transform.position.x, Target.transform.position.z);
        Vector2 myPos = new Vector2(transform.position.x, transform.position.z);
        Vector2 dir = new Vector2(targetPos.x - myPos.x, targetPos.y - myPos.y).normalized;
        NeuralNet.SetNetworkInputValues(dir.x, dir.y);
        NeuralNet.CalculateNetwork();
        float[] vals = NeuralNet.GetOutputValues();
        Vector3 moveOffset = new Vector3(vals[0], 0, vals[1]) - new Vector3(0.5f, 0, 0.5f);
        Rigidbody.AddForce(moveOffset * Speed);
        
    }
}
