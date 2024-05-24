using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Companion))]
//public class StatusEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        기본 인스펙터 UI를 먼저 표시합니다.
//       DrawDefaultInspector();

//        타겟 스크립트를 가져옵니다.
//       Companion script = (Companion)target;

//        배열의 내용을 표시합니다.
//       EditorGUILayout.LabelField("Calculated Values", EditorStyles.boldLabel);
//        EditorGUI.indentLevel++; // 들여쓰기를 증가시켜 계층 구조를 만듭니다.

//        for (int i = 0; i < script.damages.Length; i++)
//        {
//            EditorGUILayout.LabelField("Grade " + i, "Damage: " + script.damages[i] +
//                ", Speed: " + script.protSpeeds[i] +
//                ", Rate: " + script.attackRates[i] +
//                ", Range: " + script.attackRanges[i]);
//        }

//        EditorGUI.indentLevel--; // 들여쓰기를 감소시켜 계층 구조를 복원합니다.
//    }
//}
