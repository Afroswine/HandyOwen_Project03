using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region Player
#endregion Player End (⌒ー⌒)ﾉ

#region Actions
public interface IPrimaryActionable
{
    //Stats
    float PDamage { get; }
    float PActionSpeed { get; }
    float PFalloffRange { get; }
    float PMaxRange { get; }
    float PAccuracyVariance { get; }

    //Ammo
    int PCurrentAmmo { get; }
    int PMaxAmmo { get; }

    //States
    bool PReady { get; }
    bool PIsReloading { get; }
    bool PAmmoFull { get; }

    //Visuals
    ParticleSystem PFiredParticle { get; }
    ParticleSystem PHitParticle { get; }

    void PrimaryAction();
    IEnumerator PrimaryActionTimer();
}

public interface ISecondaryActionable
{
    //Stats
    float SDamage { get; }
    float SActionSpeed { get; }
    float SFalloffRange { get; }
    float SMaxRange { get; }
    float SAccuracyVariance { get; }

    //Ammo
    int SAmmoCurrent { get; }
    int SAmmoMax { get; }

    //States
    bool SReady { get; }
    bool SIsReloading { get; }
    bool SAmmoFull { get; }

    //Visuals
    ParticleSystem SFiredParticle { get; }
    ParticleSystem SHitParticle { get; }

    void SecondaryAction();
    IEnumerator SecondaryActionTimer();
}

public interface IUnlimitedPrimaryActionable
{
    //Stats
    float PDamage { get; }
    float PActionSpeed { get; }
    float PFalloffRange { get; }
    float PMaxRange { get; }
    float PAccuracyVariance { get; }

    //States
    bool PReady { get; }
    bool PIsReloading { get; }
    bool PAmmoFull { get; }

    //Visuals
    ParticleSystem PFiredParticle { get; }
    ParticleSystem PHitParticle { get; }

    void PrimaryAction();
    IEnumerator PrimaryActionTimer();
}

public interface IUnlimitedSecondaryActionable
{
    //Stats
    float SDamage { get; }
    float SActionSpeed { get; }
    float SFalloffRange { get; }
    float SMaxRange { get; }
    float SAccuracyVariance { get; }

    //States
    bool SReady { get; }
    bool SIsReloading { get; }
    bool SAmmoFull { get; }

    //Visuals
    ParticleSystem SFiredParticle { get; }
    ParticleSystem SHitParticle { get; }

    void SecondaryAction();
    IEnumerator SecondaryActionTimer();
}
#endregion Actions End  (⌒ー⌒)ﾉ

#region Weapons
public interface IWeaponTypePistol: IPrimaryActionable
{
    //Transforms
    Camera CameraController { get; }
    Transform PivotPoint { get; }
    Transform RayOrigin { get; }

    //Collision Filter
    LayerMask HitLayers { get; }

    //Collision Object
    RaycastHit ObjectHit { get; }
}
#endregion Weapons End  (⌒ー⌒)ﾉ

#region Enemies
#endregion Enemies End  (⌒ー⌒)ﾉ
