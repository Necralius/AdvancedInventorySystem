using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

[CreateAssetMenu(fileName = "New Particles Database", menuName = "FpsProject/Particles System/New Particles Database")]
public class ParticlesDatabase : ScriptableObject
{
    public List<ParticleObject> PaticleValue;
    
    public GameObject GetParticleByBaseTagAndType(string BaseTag, string ParticleType) => PaticleValue.First(particleObject => particleObject.ParticleTag == BaseTag).GetRandomParticle(ParticleType);
}

[Serializable]
public class ParticleObject
{
    public string ParticleTag;
    public GameObject ParticleDataImpact;
    public GameObject ParticleBulletHole;
    public GameObject GetRandomParticle(string ParticleType)
    {
        try
        {
            if (ParticleType == "Impact") return ParticleDataImpact;
            else if (ParticleType == "Decal") return ParticleBulletHole;
        }
        catch(Exception ex)
        {
            Debug.LogWarning("An error ocurred: " + ex);
        }
        return null;
    }
}