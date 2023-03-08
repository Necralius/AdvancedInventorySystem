using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

//Code made by Victor Paulo Melo da Silva - Junior Unity Programmer - https://www.linkedin.com/in/victor-nekra-dev/
//ParticlesDatabase - Code Update Version 0.2 - (Refactored code).
//Feel free to take all the code logic and apply in yours projects.
//This project represents a work to improve my personal portifolio, and has no intention of obtaining any financial return.

#region - Particle Database Scriptable Declaration -
[CreateAssetMenu(fileName = "New Particles Database", menuName = "FpsProject/Particles System/New Particles Database")]
public class ParticlesDatabase : ScriptableObject
{
    //This statements represent an Particle Database
    public List<ParticleObject> PaticleValue;
    
    //The above method select the particle base using his tag on a List.First method, later the method take this base and return the particle using the ParticleType argument to indentify whitch particle to return
    public GameObject GetParticleByBaseTagAndType(string BaseTag, string ParticleType) => PaticleValue.First(particleObject => particleObject.ParticleTag == BaseTag).GetParticleByType(ParticleType);
}
#endregion

#region - Particle Object Base Model -
//The above class represents the ParticleBase model that store the particles data and the particle tag
[Serializable]
public class ParticleObject
{
    public string ParticleTag;
    public GameObject ParticleDataImpact;
    public GameObject ParticleBulletHole;
    public GameObject GetParticleByType(string ParticleType)//This method uses and Try Catch block to select the requested particles
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
#endregion