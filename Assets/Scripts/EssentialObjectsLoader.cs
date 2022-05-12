using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EssentialObjectsLoader : MonoBehaviour {

    [SerializeField] GameObject essentialObjectsPrefab;
    [SerializeField] Vector3 defaultPlayerPosition;

    public Vector3 DefaultPlayerPosition => defaultPlayerPosition;

    void Awake() {
        if (FindObjectOfType<EssentialObjects>() == null) {
            Instantiate(essentialObjectsPrefab, Vector3.zero, Quaternion.identity);
        }
    }
}
