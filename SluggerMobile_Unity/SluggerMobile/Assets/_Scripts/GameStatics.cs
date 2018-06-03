using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics {

    public static Dictionary<string, int> layers;

    public static void Initialize()
    {
        layers = new Dictionary<string, int>();
        layers.Add("Ground", LayerMask.GetMask("Ground"));
    }

    public static bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    public static Vector3 ClosestPointBetweenColliders(Collider2D col1, Collider2D col2)
    {
        ColliderDistance2D collDist = col1.Distance(col2);
        return collDist.pointB;
    }

    public static Vector3 ClosestVectorBetweenColliders(Collider2D col1, Collider2D col2)
    {
        ColliderDistance2D collDist = col1.Distance(col2);
        return collDist.pointB - collDist.pointA;
    }
}
