using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public class DifficultyProfile
{
    public float playerDamageMult;
    public float enemyDamageMult;
    public float playerHealthMult;
    public float enemyHealthMult;
    public float enemyReactionMult;
    public float playerSpeedMult;
}
