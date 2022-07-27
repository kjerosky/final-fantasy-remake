using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public interface Unit {
    public int NumberOfHits { get; }
    public int HitMultiplier { get; }
    public int Accuracy { get; }
    public int Evasion { get; }
    public int CriticalRate { get; }
    public int Attack { get; }
    public int Defense { get; }
}
