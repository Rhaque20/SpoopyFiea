using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Concerns : MonoBehaviour
{
    private List<Concern> listOfConcerns;
    [SerializeField]
    private float minimumDistanceToConsiderSameConcern = 2f;
    private BehaviourTree behaviourTree;
    public Concern GetHighestConcern()
    {
        Concern highestConcern = null;
        foreach (var concern in listOfConcerns)
        {
            if (highestConcern == null)
            {
                highestConcern = concern;
            }
            else
            {
                if (concern.suspicion > highestConcern.suspicion)
                {
                    highestConcern = concern;
                }  
            }
        }
        return highestConcern;
    }

    public void RemoveConcern(Concern concern)
    {
        if (listOfConcerns.Contains(concern))
        {
            listOfConcerns.Remove(concern);
            Destroy(concern);
        }
    }

    //SuspicionStateSystem calls this to place the new stimulus
    public void UpdateConcerns(AbstractStimulus stim)
    {
        if (behaviourTree.currentBehaviour == BehaviourTree.BehaviourTreeState.Attack)
        {
            return;
        }
        Debug.Log("Number of concerns : " + listOfConcerns.Count);
        foreach (var concern in listOfConcerns)
        {
            if (Vector3.Distance(concern.position, stim.position) < minimumDistanceToConsiderSameConcern)
            {
                concern.position = stim.position;
                concern.suspicion += stim.value * Time.deltaTime;
                if (stim is IsHeardStimulus)
                {
                    concern.suspicion = Mathf.Clamp(concern.suspicion, 0, behaviourTree.attackSuspicionValue);
                }
                return;
            }    
        }
 
        Concern newConcern = gameObject.AddComponent<Concern>();
        newConcern.position = stim.position;
        newConcern.suspicion = stim.value;
        listOfConcerns.Add(newConcern); //Adding this new concern to the list
    }

    public List<Concern> GetAllConcerns()
    { 
        return listOfConcerns;
    }

    // Start is called before the first frame update
    void Start()
    {
        listOfConcerns = new List<Concern>();
        behaviourTree = GetComponent<BehaviourTree>();
        DetectionController.ProcessStimulus += UpdateConcerns;
    }
    // Update is called once per frame
    void Update()
    {
        Debug.Log($"{listOfConcerns.Count} number of concerns on AI");
    }
}
