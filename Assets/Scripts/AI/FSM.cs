using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFM : MonoBehaviour {
  protected virtual void Initialize() { } // Override this method in derived classes
  protected virtual void FSMUpdate() { } // Override this method in derived classes
  protected virtual void FSMFixedUpdate() { } // Override this method in derived classes

  private void Start() { Initialize(); }
  private void Update() { FSMUpdate(); }
  private void FixedUpdate() { FSMFixedUpdate(); }
}
