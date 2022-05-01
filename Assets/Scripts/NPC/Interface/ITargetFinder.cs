using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations;

public interface ITargetFinder
{
    //public Collider SearchCollider; // would OnCollider() be more efficient than doing update checks? probably
    //public void OnColliderEnter();

    // Serialize these references
    string TargetTag { get; }
    float Radius { get; }

    AimConstraint AimComponent { get; }
    ConstraintSource AimSource { get; }
    bool IsSeekingTarget { get; }

    void InitializeITargetFinder();
    void SeekTarget();
}
