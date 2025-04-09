using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(JointSettings))]
public class JointSettingsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        JointSettings settings = (JointSettings)target;

        GUILayout.Label("Motion Settings", EditorStyles.boldLabel);
        settings.xMotion = (ConfigurableJointMotion)EditorGUILayout.EnumPopup("X Motion", settings.xMotion);
        settings.yMotion = (ConfigurableJointMotion)EditorGUILayout.EnumPopup("Y Motion", settings.yMotion);
        settings.zMotion = (ConfigurableJointMotion)EditorGUILayout.EnumPopup("Z Motion", settings.zMotion);

        settings.angularXMotion = (ConfigurableJointMotion)EditorGUILayout.EnumPopup("Angular X Motion", settings.angularXMotion);
        settings.angularYMotion = (ConfigurableJointMotion)EditorGUILayout.EnumPopup("Angular Y Motion", settings.angularYMotion);
        settings.angularZMotion = (ConfigurableJointMotion)EditorGUILayout.EnumPopup("Angular Z Motion", settings.angularZMotion);

        EditorGUILayout.Space(10);
        GUILayout.Label("Linear Limit Settings", EditorStyles.boldLabel);
        settings.linearLimit.limit = EditorGUILayout.FloatField("Linear Limit", settings.linearLimit.limit);
        settings.linearLimitSpring.spring = EditorGUILayout.Slider("Spring", settings.linearLimitSpring.spring, 0f, 1000f);
        settings.linearLimitSpring.damper = EditorGUILayout.Slider("Damper", settings.linearLimitSpring.damper, 0f, 100f);

        EditorGUILayout.Space(10);
        GUILayout.Label("Joint Drives", EditorStyles.boldLabel);
        DrawDrive("X Drive", ref settings.xDrive);
        DrawDrive("Y Drive", ref settings.yDrive);
        DrawDrive("Z Drive", ref settings.zDrive);

        EditorGUILayout.Space(10);
        GUILayout.Label("Slerp Drive (Rotation)", EditorStyles.boldLabel);
        DrawDrive("Slerp Drive", ref settings.slerpDrive);

        EditorGUILayout.Space(10);
        GUILayout.Label("Target Settings", EditorStyles.boldLabel);
        settings.targetPosition = EditorGUILayout.Vector3Field("Target Position", settings.targetPosition);
        settings.targetVelocity = EditorGUILayout.Vector3Field("Target Velocity", settings.targetVelocity);
        settings.targetRotation = Quaternion.Euler(EditorGUILayout.Vector3Field("Target Rotation (Euler)", settings.targetRotation.eulerAngles));

        EditorGUILayout.Space(10);
        GUILayout.Label("Projection Settings", EditorStyles.boldLabel);
        settings.projectionMode = (JointProjectionMode)EditorGUILayout.EnumPopup("Projection Mode", settings.projectionMode);
        settings.projectionDistance = EditorGUILayout.Slider("Projection Distance", settings.projectionDistance, 0f, 1f);
        settings.projectionAngle = EditorGUILayout.Slider("Projection Angle", settings.projectionAngle, 0f, 20f);

        EditorGUILayout.Space(10);
        GUILayout.Label("Anchors", EditorStyles.boldLabel);
        settings.autoConfigureConnectedAnchor = EditorGUILayout.Toggle("Auto Configure Anchor", settings.autoConfigureConnectedAnchor);
        settings.anchor = EditorGUILayout.Vector3Field("Anchor", settings.anchor);
        settings.connectedAnchor = EditorGUILayout.Vector3Field("Connected Anchor", settings.connectedAnchor);

        // Save changes
        if (GUI.changed)
        {
            EditorUtility.SetDirty(settings);
        }
    }

    void DrawDrive(string label, ref JointDrive drive)
    {
        GUILayout.Label(label, EditorStyles.miniBoldLabel);
        drive.positionSpring = EditorGUILayout.Slider("Spring", drive.positionSpring, 0f, 1000f);
        drive.positionDamper = EditorGUILayout.Slider("Damper", drive.positionDamper, 0f, 100f);
        drive.maximumForce = EditorGUILayout.Slider("Max Force", drive.maximumForce, 0f, 10000f);
    }
}
