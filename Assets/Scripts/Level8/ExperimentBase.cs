using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExperimentBase : MonoBehaviour
{
    [SerializeField] string[] Instructions;
    int currentInstIndex;
    

    public virtual void StartExperiment()
    {
        UpdateInstruction();
    }

    public void UpdateInstruction()
    {
        ExperimentManager.instance.UpdateInstruction(Instructions[currentInstIndex]);

    }
    public virtual void StartExperiment2()
    {
        UpdateInstruction2();
    }
    
    public void UpdateInstruction2()
    {
        ExperimentManager.instance.UpdateInstruction2(Instructions[currentInstIndex]);


    }

    public virtual void StartExperiment3()
    {
        UpdateInstruction3();
    }

    public void UpdateInstruction3()
    {
        ExperimentManager.instance.UpdateInstruction3(Instructions[currentInstIndex]);


    }
}
