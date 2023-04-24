using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkHandler: MonoBehaviour
{
    public Transform enter;
    public Transform exit;

    public Transform[] enemy_spawners;
    public Transform[] obstacle_spawners;

    public Transform player_start_point;
    public Transform player_target_point;
}
