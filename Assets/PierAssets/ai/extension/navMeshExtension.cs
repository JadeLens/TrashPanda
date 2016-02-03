
using UnityEngine;
using System.Collections;
namespace extensions
{
    public static class RandomPointOnNavMesh
    {
       
        public static bool RandomPoint(this NavMeshAgent nav, Vector3 center, float range, out Vector3 result)
        {
             
            for (int i = 0; i < 30; i++)
            {
                Vector3 randomPoint = center + Random.insideUnitSphere * range;
                NavMeshHit hit;
                if (NavMesh.SamplePosition(randomPoint, out hit, 1.0f, NavMesh.AllAreas))
                {
                    result = hit.position;
                    return true;
                }
            }
            result = Vector3.zero;
            return false;
        }
      
    }
 
}