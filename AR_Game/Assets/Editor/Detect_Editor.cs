using UnityEditor;
using UnityEngine;
using System.Collections;

[CustomEditor(typeof(Detect))]
public class Detect_Editor : Editor {

    public override void OnInspectorGUI()
    {
        Detect dt = (Detect)target;
        DrawDefaultInspector();

        if(dt.oyunTuru == Detect.Tur.BalonPatlatma)
        {
            dt.patlamaParticle = (GameObject)EditorGUILayout.ObjectField("Patlama particle: ",dt.patlamaParticle, typeof(GameObject),true );
            dt.balonPrefab = (GameObject)EditorGUILayout.ObjectField("Balon prefab'ı: ", dt.balonPrefab, typeof(GameObject), true);
            dt.spawnMin = EditorGUILayout.Vector3Field("Min spawn noktası: ", dt.spawnMin);
            dt.spawnMax = EditorGUILayout.Vector3Field("Max spawn noktası: ", dt.spawnMax);
            dt.baslangicSayisi = EditorGUILayout.IntField("Başlangıç sayısı: ", dt.baslangicSayisi);
            dt.balonSayisi = EditorGUILayout.IntField("Balon sayısı: ", dt.balonSayisi);
        }
    }

}
