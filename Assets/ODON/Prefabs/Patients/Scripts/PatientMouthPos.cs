using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PatientMouthPos", menuName = "ScriptableObjects/PatientMouthPos")]
public class PatientMouthPos : ScriptableObject
{
    public Vector3 JawPos;
    public Vector3 diguePos;
}
