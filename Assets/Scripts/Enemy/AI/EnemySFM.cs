using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemySFM : SFM {
  public enum FSMState {
    Patrol,
    Attack,
    Dead
  }

  public List<Vector3> WayPoints;
  public float GetWPThresholdDist = 1.0f;
  public FSMState CurrentState = FSMState.Patrol;

  private NavMeshAgent _agent;
  private int _currentWayPointIdx = 0;
  private bool _isDead = false;

  protected override void Initialize() {
    _isDead = false;

    _agent = GetComponent<NavMeshAgent>();
    WayPoints =
      new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyWayPoints")).ConvertAll(x => x.transform.position);

    _agent.destination = WayPoints[_currentWayPointIdx];
  }

}
