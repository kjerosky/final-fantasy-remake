using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface Interactable {

    public bool isCurrentlyInteractable();
    public void interact(Transform initiator);
}
