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
        else if(dt.oyunTuru == Detect.Tur.ElmaIslemleri)
        {
            dt.elmaPrefab = (GameObject)EditorGUILayout.ObjectField("Elma prefab'ı: ", dt.elmaPrefab, typeof(GameObject), true);
            dt.islemObjesi = (GameObject)EditorGUILayout.ObjectField("İşlem(1) koyun: ", dt.islemObjesi, typeof(GameObject), true);
            dt.sepetObjesi = (GameObject)EditorGUILayout.ObjectField("Sepet(1) koyun: ", dt.sepetObjesi, typeof(GameObject), true);
            EditorGUILayout.HelpBox("Elmalar sağa ve alta doğru spawnlanacak", MessageType.Warning);
            dt.spawnMin = EditorGUILayout.Vector3Field("1.Spawn noktası: ", dt.spawnMin);
            dt.spawnMax = EditorGUILayout.Vector3Field("2.Spawn noktası: ", dt.spawnMax);
            dt.elmalarArasi = EditorGUILayout.FloatField("Elmalar arası mesafe: ", dt.elmalarArasi);
            dt.maxElmaSayisi = EditorGUILayout.IntField("Max elma sayısı: ", dt.maxElmaSayisi);
        }
    }

}
