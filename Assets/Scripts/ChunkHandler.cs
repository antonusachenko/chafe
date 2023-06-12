using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;


public class ChunkHandler: MonoBehaviour
{
    public Transform enter;
    public Transform exit;

    public Transform[] enemy_spawners;
    public Transform[] obstacle_spawners;

    public Transform player_start_point;
    public Transform player_target_point;

#if UNITY_EDITOR
    void OnDrawGizmosSelected()
    {
        float thickness = 5f;
        float length = 5f;
        Handles.color = Color.red;
        Handles.DrawLine(enter.transform.position, enter.transform.position + enter.forward * length, thickness);

        Handles.color = Color.blue;
        Handles.DrawLine(exit.transform.position, exit.transform.position + exit.forward * length, thickness);
    }
#endif

}
