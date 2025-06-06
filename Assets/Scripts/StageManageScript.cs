using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StageManageScript : MonoBehaviour
{
    [SerializeField]
    List<GameObject> stages = new List<GameObject>();

    [SerializeField]
    private GameObject pos;

    void GenerateStage()
    {
        GameObject stage = Helper.GetRandom(stages);
        Instantiate(stage, pos.transform.position, transform.rotation);
    }

    public class Helper : MonoBehaviour
    {
        internal static T GetRandom<T>(List<T> Params)
        {
            return Params[Random.Range(0, Params.Count)];
        }
    }
}
