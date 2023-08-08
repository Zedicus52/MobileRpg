using MobileRpg.Enums;
using MobileRpg.ScriptableObjects;
using UnityEditor;
using UnityEngine;

namespace MobileRpg.Inspector
{
    [CustomEditor(typeof(SpellConfig))]
    public class SpellConfigEditor : Editor
    {
        public override void OnInspectorGUI()
        {
            SpellConfig config = (SpellConfig)target;
            
            SerializedProperty id = serializedObject.FindProperty("_id");
            EditorGUILayout.PropertyField(id, new GUIContent("Spell Id"));
            
            SerializedProperty spellType = serializedObject.FindProperty("_spellType");
            EditorGUILayout.PropertyField(spellType, new GUIContent("Spell Type"));
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("UI settings"), EditorStyles.boldLabel);
            EditorGUILayout.Space();
            
            SerializedProperty spellName = serializedObject.FindProperty("_name");
            EditorGUILayout.PropertyField(spellName, new GUIContent("Spell Name"));
            
            SerializedProperty spellDescription = serializedObject.FindProperty("_description");
            EditorGUILayout.PropertyField(spellDescription, new GUIContent("Spell Description"));
            
            SerializedProperty icon = serializedObject.FindProperty("_icon");
            EditorGUILayout.PropertyField(icon, new GUIContent("Spell Icon"));
            
            EditorGUILayout.Space();
            EditorGUILayout.LabelField(new GUIContent("Spell settings"), EditorStyles.boldLabel);
            EditorGUILayout.Space();

            SerializedProperty manaCost = serializedObject.FindProperty("_manaCost");
            EditorGUILayout.PropertyField(manaCost, new GUIContent("Mana cost"));

            if (config.SpellType == SpellType.ReusableActive)
            {
                SerializedProperty minDamageField = serializedObject.FindProperty("_applyStepsCount");
                EditorGUILayout.PropertyField(minDamageField, new GUIContent("Apply Steps Count"));
            }

            if (config.SpellType == SpellType.DisposableActive || config.SpellType == SpellType.ReusableActive)
            {
                SerializedProperty minDamageField = serializedObject.FindProperty("_minDamage");
                EditorGUILayout.PropertyField(minDamageField, new GUIContent("Min Damage"));
                
                SerializedProperty maxDamageField = serializedObject.FindProperty("_maxDamage");
                EditorGUILayout.PropertyField(maxDamageField, new GUIContent("Max Damage"));
            }
            
            
            serializedObject.ApplyModifiedProperties();
        }
    }
}