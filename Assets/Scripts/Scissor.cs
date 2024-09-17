using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Scissor : MonoBehaviour {
    public Animator scissorAnim;

    private void Awake() {
        scissorAnim = GetComponent<Animator>();
    }
}
