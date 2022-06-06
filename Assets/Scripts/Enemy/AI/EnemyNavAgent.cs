using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyNavAgent : MonoBehaviour {
  public List<Vector3> WayPoints;
  public float GetWPThresholdDist = 1f;
  public float RaycastDist = 10.0f;
  public bool DebugMode = false;
  public Rigidbody Shell;
  public float FireRate = 3f;
  public int RayCastRateFrame = 60;
  public Transform FireTransform;

  private NavMeshAgent _agent;
  private int _currentWayPointIdx = 0;
  private Vector3 _target;
  private bool _fired;
  private float _fireTimer = 0.0f;

  private void Awake() {
    _agent = GetComponent<NavMeshAgent>();
    WayPoints =
      new List<GameObject>(GameObject.FindGameObjectsWithTag("EnemyWayPoints")).ConvertAll(x => x.transform.position);
  }

  private void Start() {
    _agent.destination = WayPoints[_currentWayPointIdx];  
  }

  private void OnEnable() {
    _agent.destination = WayPoints[_currentWayPointIdx];
  }

  private void Update() {

    int curFrame = Time.frameCount;
    if (curFrame % RayCastRateFrame == 0) {
      for (var i = -60; i <= 60; i += 15) {
        Physics.Raycast(transform.position, Quaternion.Euler(0, i, 0) * transform.forward, out var hit, RaycastDist);
        if (DebugMode)
          Debug.DrawRay(transform.position, Quaternion.Euler(0, i, 0) * transform.forward * RaycastDist, Color.red, 30.0f);

        if (hit.collider != null) {
          if (hit.collider.tag == "Player") {
            _target = hit.point;
            if (!_fired)
              Fire();
          }
        }
      }
    }

    if (!GetWayPoint(WayPoints[_currentWayPointIdx])) return;

    _currentWayPointIdx = (_currentWayPointIdx + 1) % WayPoints.Count;
    _agent.destination = WayPoints[_currentWayPointIdx];
  }

  private void Fire() {
    _fired = true;
    Vector3 target = GameObject.FindGameObjectWithTag("Player").transform.position;
    Rigidbody shellInstance =
      Instantiate(Shell,
                  FireTransform.position,
                  Quaternion.Euler((_target - FireTransform.position) * Random.Range(-0.15f, 0.15f)))
                  as Rigidbody;
    shellInstance.velocity = (_target - transform.position) * Random.Range(1.5f, 3.0f);

    StartCoroutine("Reload");
  }

  private IEnumerator Reload() {
    yield return new WaitForSeconds(FireRate);
    _fired = false;
  }

  private bool GetWayPoint(Vector3 destination) {
    return Vector3.Distance(transform.position, destination) < GetWPThresholdDist;
  }
}
