using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GameStatics {

    public static Dictionary<string, int> layers;

    public static void Initialize()
    {
        layers = new Dictionary<string, int>();
        layers.Add("Ground", LayerMask.GetMask("Ground"));
        
        //Add all layers to a List
        //for (int i = 8; i <= 31; i++) //user defined layers start with layer 8 and unity supports 31 layers
        //{
        //    string layerName = LayerMask.LayerToName(i); //get the name of the layer
        //    if (layerN.Length > 0) //only add the layer if it has been named (comment this line out if you want every layer)
        //        layerNames.Add(layer)
        //}
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
        return collDist.pointA - collDist.pointB;
    }
}
