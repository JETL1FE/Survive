using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

//[CustomEditor(typeof(Companion))]
//public class StatusEditor : Editor
//{
//    public override void OnInspectorGUI()
//    {
//        �⺻ �ν����� UI�� ���� ǥ���մϴ�.
//       DrawDefaultInspector();

//        Ÿ�� ��ũ��Ʈ�� �����ɴϴ�.
//       Companion script = (Companion)target;

//        �迭�� ������ ǥ���մϴ�.
//       EditorGUILayout.LabelField("Calculated Values", EditorStyles.boldLabel);
//        EditorGUI.indentLevel++; // �鿩���⸦ �������� ���� ������ ����ϴ�.

//        for (int i = 0; i < script.damages.Length; i++)
//        {
//            EditorGUILayout.LabelField("Grade " + i, "Damage: " + script.damages[i] +
//                ", Speed: " + script.protSpeeds[i] +
//                ", Rate: " + script.attackRates[i] +
//                ", Range: " + script.attackRanges[i]);
//        }

//        EditorGUI.indentLevel--; // �鿩���⸦ ���ҽ��� ���� ������ �����մϴ�.
//    }
//}
