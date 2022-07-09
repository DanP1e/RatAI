using UnityEngine;
using System.Collections;
using NeuralNetworkLibrary;
public class GenerationSpawner : MonoBehaviour
{
    public int GenerationNumber { get; private set; }

    [Range(0.01f, 100f)]
    public float TimeScale;
    public GameObject Subject;
    public int SubjectsCount;
    public float GenerationTime;
    public float MutationPower;

    public BoxCollider SpawnArea;

    [SerializeField] private float _bestScore = 0;
    private GameObject[] _subjects;
    private GenerationMember[] _generationMembers;
    private GameObject _generationDonor = null;

    private void Start() 
    {
       
        SpawnGeneration();
    }
    private void Update()
    {
        Time.timeScale = TimeScale;
    }
    private void CreateGeneratonDonor(GameObject subject) 
    {
        GameObject donor = Instantiate(subject, transform.position, Quaternion.identity);
        if (!donor.GetComponent(typeof(GenerationMember)))
        {
            throw new System.ArgumentException($"Donor must have the GenerationMember component");
        }
        GenerationMember donorMember = (GenerationMember)donor.GetComponent(typeof(GenerationMember));
        donorMember.SetNeuralNetwork(new NeuralNetwork(2, 2));
        _generationDonor = donor;
    }
    private void SpawnGeneration()
    {
        Vector3 newPos = new Vector3();
        newPos.x = Random.Range(SpawnArea.center.x - SpawnArea.size.x / 2, SpawnArea.center.x + SpawnArea.size.x / 2);
        newPos.y = Random.Range(SpawnArea.center.y - SpawnArea.size.y / 2, SpawnArea.center.y + SpawnArea.size.y / 2);
        newPos.z = Random.Range(SpawnArea.center.z - SpawnArea.size.z / 2, SpawnArea.center.z + SpawnArea.size.z / 2);
        transform.position = newPos;

        if (_generationDonor == null) 
            CreateGeneratonDonor(Subject);
        _subjects = new GameObject[SubjectsCount];
        _generationMembers = new GenerationMember[SubjectsCount];
        for (int i = 0; i < SubjectsCount; i++)
        {
            _subjects[i] = Instantiate(_generationDonor, transform.position, Quaternion.identity);
            _generationMembers[i] = (GenerationMember)_subjects[i].GetComponent(typeof(GenerationMember));          
        }
        InstantiateWeights();
        Destroy(_generationDonor.gameObject);
        GenerationNumber++;
        StartCoroutine(DestroyGenerationByTime(GenerationTime));
    }
    private void InstantiateWeights()
    {
        GenerationMember donor = (GenerationMember)_generationDonor.GetComponent(typeof(GenerationMember));
        _generationMembers[0].SetNeuralNetwork((NeuralNetwork)donor.NeuralNet.Clone());

        for (int i = 1; i < SubjectsCount; i++) 
        {
            _generationMembers[i].SetNeuralNetwork((NeuralNetwork)donor.NeuralNet.Clone());
            _generationMembers[i].NeuralNet.MutateWeights(MutationPower);       
        }
    }
    private void DestroyGeneration()
    {
        _generationDonor = GetBetterSubject();
        for (int i = 0; i < _subjects.Length; i++)
        {
            if(_subjects[i] != _generationDonor)
            Destroy(_subjects[i]);
        }
        SpawnGeneration();
    }
    private GameObject GetBetterSubject()
    {
        int bestScoreID = 0;
        float bestScore = ((GenerationMember)_subjects[0].GetComponent(typeof(GenerationMember))).Score;
        for (int i = 1; i < _subjects.Length; i++)
        {
            float currentScore = ((GenerationMember)_subjects[i].GetComponent(typeof(GenerationMember))).Score;
            if (currentScore > bestScore)
            {
                bestScore = currentScore;
                bestScoreID = i;
            }
        }
        _bestScore = bestScore;
        return _subjects[bestScoreID];
    }
    private void Info()
    {
        foreach (var i in _subjects) 
        {
            Debug.Log(i.GetComponent<BallTest>().NeuralNet.NeuronWeights[0][0]);
        }
    }
    private IEnumerator DestroyGenerationByTime(float time) 
    {   
        yield return new WaitForSeconds(time);
        DestroyGeneration();
    }
    
}
