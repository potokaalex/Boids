using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using Unity.VisualScripting.Dependencies.NCalc;
using UnityEngine;
using UnityEngine.Jobs;
using UnityEngine.UIElements;

public class Startup : MonoBehaviour
{
    public GameObject EntityPrefab;
    public int EntitiesCount;

    private StateMachine _stateMachine;

    private void Start()
    {
        //_stateMachine = new();
    }

    void FixedUpdate()
    {

    }

    private void OnDestroy()
    {

    }
}

public class StateMachine
{
    private Dictionary<Type, IState> _states;
    private IState _currentState;

    public StateMachine(IEnumerable<IState> states)
    {
        foreach (var state in states)
            _states.Add(state.GetType(), state);
    }

    public void SwitchTo<TState>() where TState : IState
    {
        _currentState?.Exit();
        _currentState = _states[typeof(TState)];
        _currentState.Enter();
    }
}

public interface IState
{
    public void Enter() { }

    public void Exit() { }
}

public class BoidsSimulationState : IState
{
    public void Enter() { }

    public void Exit() { }
}

public class PauseState : IState
{
    public void Enter() { }

    public void Exit() { }
}

/*
[Serializable]
public ref struct BoidsSimulationData
{
    [SerializeField] private GameObject _boidPrefab;
    [SerializeField] private int _numberOfBoids;
    public int width = 150;
    public int height = 150;

    public float AvoidanceDistance = 20;
    public float SightDistance = 75;

    public float CohesionFactor = 1;
    public float SeparationFactor = 5;
    public float AlignmentFactor = 1;

    //public const int HistorySize = 100;
    //public Queue<Vector2> History;

    private NativeArray<Vector2> _positions;
    private NativeArray<Vector2> _velocities;
    private NativeArray<Vector2> _accelerations;
    private TransformAccessArray _transforms;
}
*/