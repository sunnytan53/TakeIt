using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Data", menuName = "ScriptableObjects/Face", order = 1)]
public class Face :ScriptableObject
{
    public Texture IdleFace, WalkFace, JumpFace, AttackFace, DamageFace;
}
