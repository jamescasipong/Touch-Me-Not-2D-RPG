using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;


public interface Interactable
{
    IEnumerator Interact(Transform initiator);

}
