using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(Ship))]
public class ShipInspector : Editor
{

    Ship currentShip;
    public string[] shipTabs = new string[] { "Ship", "Crew" };
    public int shipChoice = 0;
    public int currCrewMember = 0;
    public int shipStatPool = 70;
    string s;

    const int ARM_MIN = 10;
    const int ATK_MIN = 10;
    const int AGI_MIN = 10;

    //public string gunChoice;

    void OnEnable()
    {
        currentShip = (Ship)target;
    }

    public override void OnInspectorGUI()
    {
        DrawShipInfo();
        //base.OnInspectorGUI();
    }

    void DrawShipInfo()
    {
        EditorGUILayout.BeginVertical("box");

        shipChoice = GUILayout.SelectionGrid(shipChoice, shipTabs, 2);

        
        if (shipChoice == 0)
        {
            s = "Ship stats";

            EditorGUILayout.LabelField(s, EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Stat Points Remaining: ");
            EditorGUILayout.LabelField(shipStatPool.ToString());
            EditorGUILayout.EndHorizontal();

            currentShip.armor = EditorGUILayout.IntSlider("Armor", currentShip.armor, ARM_MIN, shipStatPool);

            currentShip.attack = EditorGUILayout.IntSlider("Attack", currentShip.attack, ATK_MIN, shipStatPool);
            
            currentShip.agility = EditorGUILayout.IntSlider("Agility", currentShip.agility, AGI_MIN, shipStatPool);

            if (shipStatPool < 0)
                shipStatPool = 0;

            currentShip.hitPoints = EditorGUILayout.IntSlider("Hit Points", currentShip.hitPoints, 1, 100);
            ProgressBar(currentShip.hitPoints / 100f, "Hit Points");

            serializedObject.Update();

            SerializedProperty gunChoice = serializedObject.FindProperty("shipGuns");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(gunChoice, new GUIContent("Gun Choice", "Currently equipped ship gun."), true);
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();

            SerializedProperty crewMemberChoice = serializedObject.FindProperty("crewMembers");
            EditorGUILayout.BeginHorizontal();
            GUILayout.Space(10);
            EditorGUILayout.PropertyField(crewMemberChoice, new GUIContent("Crew Member", "Currently selected crew member."), true);
            EditorGUILayout.EndHorizontal();
            serializedObject.ApplyModifiedProperties();
        }

        else
        {
            s = "Crew stats";
            EditorGUILayout.BeginVertical();

            EditorGUILayout.LabelField(s, EditorStyles.boldLabel);
            currCrewMember = EditorGUILayout.IntField(new GUIContent("Current Crew Member: ", "Current selected crew member for editing."), currCrewMember);

            if (currCrewMember < 0)
                currCrewMember = 0;
            else if (currCrewMember > currentShip.crewMembers.Length - 1)
                currCrewMember = currentShip.crewMembers.Length - 1;

            currentShip.crewMembers[currCrewMember].crewName = EditorGUILayout.TextField("Name: ", currentShip.crewMembers[currCrewMember].crewName);
            currentShip.crewMembers[currCrewMember].def = EditorGUILayout.IntSlider("Defense: ", currentShip.crewMembers[currCrewMember].def, 10, 100);
            currentShip.crewMembers[currCrewMember].str = EditorGUILayout.IntSlider("Strength: ", currentShip.crewMembers[currCrewMember].str, 10, 100);
            currentShip.crewMembers[currCrewMember].agi = EditorGUILayout.IntSlider("Agility: ", currentShip.crewMembers[currCrewMember].agi, 10, 100);
            currentShip.crewMembers[currCrewMember].health = EditorGUILayout.IntSlider("Health: ", currentShip.crewMembers[currCrewMember].health, 1, 100);
            ProgressBar(currentShip.crewMembers[currCrewMember].health / 100f, "Health");

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndVertical();
    }

    void ProgressBar(float value, string label)
    {
        Rect rect = GUILayoutUtility.GetRect(18, 18, "TextField");
        EditorGUI.ProgressBar(rect, value, label);
    }

}
