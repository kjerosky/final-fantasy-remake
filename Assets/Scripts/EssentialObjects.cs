using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjects : MonoBehaviour {

    void Awake() {
        DontDestroyOnLoad(gameObject);
    }
}
