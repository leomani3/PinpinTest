using UnityEditor;
using UnityEngine;

public class RaycastPlacer : EditorWindow
{
    private GameObject objectToPlace;
    private Transform parentObject;
    private bool isPlacing = false;
    private bool randomRotation = false;
    private float minRotationY = 0f;
    private float maxRotationY = 360f;
    private bool randomScale = false;
    private float minScale = 1f;
    private float maxScale = 1f;

    [MenuItem("Tools/Raycast Placer")]
    public static void ShowWindow()
    {
        GetWindow<RaycastPlacer>("Raycast Placer");
    }

    private void OnGUI()
    {
        GUILayout.Label("Raycast Placer Tool", EditorStyles.boldLabel);

        EditorGUILayout.Space();
        GUILayout.BeginHorizontal();
        GUILayout.Label("Placing Status:", GUILayout.Width(100));
        var statusColor = isPlacing ? Color.green : Color.red;
        var statusText = isPlacing ? "Active" : "Inactive";
        GUIStyle statusStyle = new GUIStyle(EditorStyles.boldLabel) { normal = { textColor = statusColor } };
        GUILayout.Label(statusText, statusStyle);
        GUILayout.EndHorizontal();

        EditorGUILayout.Space();
        objectToPlace = (GameObject)EditorGUILayout.ObjectField("Object to Place", objectToPlace, typeof(GameObject), false);

        EditorGUILayout.Space();
        parentObject = (Transform)EditorGUILayout.ObjectField("Parent Object", parentObject, typeof(Transform), true);

        EditorGUILayout.Space();
        randomRotation = EditorGUILayout.Toggle("Random Y Rotation", randomRotation);
        if (randomRotation)
        {
            minRotationY = EditorGUILayout.FloatField("Min Y Rotation", minRotationY);
            maxRotationY = EditorGUILayout.FloatField("Max Y Rotation", maxRotationY);
        }

        EditorGUILayout.Space();
        randomScale = EditorGUILayout.Toggle("Random Scale", randomScale);
        if (randomScale)
        {
            minScale = EditorGUILayout.FloatField("Min Scale", minScale);
            maxScale = EditorGUILayout.FloatField("Max Scale", maxScale);
        }

        EditorGUILayout.Space();
        if (GUILayout.Button("Start Placing"))
        {
            if (objectToPlace != null)
            {
                isPlacing = true;
                SceneView.duringSceneGui += OnSceneGUI;
            }
            else
            {
                EditorUtility.DisplayDialog("Error", "Please assign an object to place.", "OK");
            }
        }

        if (GUILayout.Button("Stop Placing"))
        {
            isPlacing = false;
            SceneView.duringSceneGui -= OnSceneGUI;
        }
    }

    private void OnDisable()
    {
        isPlacing = false;
        SceneView.duringSceneGui -= OnSceneGUI;
    }

    private void OnSceneGUI(SceneView sceneView)
    {
        if (isPlacing && Event.current.type == EventType.MouseDown && Event.current.button == 0)
        {
            Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
            if (Physics.Raycast(ray, out RaycastHit hit))
            {
                PlaceObjectAtPosition(hit.point);
                Event.current.Use(); // Consume the event so it's not processed further
            }
        }
    }

    private void PlaceObjectAtPosition(Vector3 position)
    {
        GameObject newObject = (GameObject)PrefabUtility.InstantiatePrefab(objectToPlace);

        newObject.transform.position = position;

        if (randomRotation)
        {
            float randomY = Random.Range(minRotationY, maxRotationY);
            newObject.transform.rotation = Quaternion.Euler(0, randomY, 0);
        }

        if (randomScale)
        {
            float randomScaleValue = Random.Range(minScale, maxScale);
            newObject.transform.localScale = Vector3.one * randomScaleValue;
        }

        if (parentObject != null)
        {
            newObject.transform.SetParent(parentObject);
        }

        Undo.RegisterCreatedObjectUndo(newObject, "Placed Object");
    }
}
