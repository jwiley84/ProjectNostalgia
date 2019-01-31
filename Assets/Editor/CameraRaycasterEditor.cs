using UnityEditor;//since it alters only the unity program, no other 'using' is needed

[CustomEditor(typeof(CameraRaycaster))] //this declares what the editor is replacing
/// remember, this will fully replace the GUI of the camera raycaster

public class CameraRaycasterEditor : Editor //inherits from Editor, not monodevelopment
{
    bool isLayerPrioritiesUnfolded = true; // store the UI state

    public override void OnInspectorGUI() //when the GUI loads
    {
        serializedObject.Update(); // Serialize cameraRaycaster instance (means makes it saveable to memory (thus 'readable')

        isLayerPrioritiesUnfolded = EditorGUILayout.Foldout(isLayerPrioritiesUnfolded, "Layer Priorities"); //is what we're changing unfolded.
        if (isLayerPrioritiesUnfolded) //if it is
        {
            EditorGUI.indentLevel++; //indent over one
            {
                //whenever we get around to it, do the PrintString() first, as a 'here's what it does' thing.
                PrintString(); //this is just to show what a custom editor does. 
                BindArraySize(); //make a unfolded GUI as long as the LayerPriority array
                BindArrayElements(); //fill the GUI with the content of the laryer priority array
            }
            EditorGUI.indentLevel--; //then unindent
        }

        serializedObject.ApplyModifiedProperties(); // De-serialize back to cameraRaycaster (and create undo point)
    }

    void PrintString()
    {
        var currentText = serializedObject.FindProperty("stringToPrint");
        currentText.stringValue = EditorGUILayout.TextField("Me fucking around: ", currentText.stringValue);
    }

    void BindArraySize()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        int requiredArraySize = EditorGUILayout.IntField("Size", currentArraySize);
        if (requiredArraySize != currentArraySize)
        {
            serializedObject.FindProperty("layerPriorities.Array.size").intValue = requiredArraySize;
        }
    }

    void BindArrayElements()
    {
        int currentArraySize = serializedObject.FindProperty("layerPriorities.Array.size").intValue;
        for (int i = 0; i < currentArraySize; i++)
        {
            var prop = serializedObject.FindProperty(string.Format("layerPriorities.Array.data[{0}]", i));
            prop.intValue = EditorGUILayout.LayerField(string.Format("Layer {0}:", i), prop.intValue);
        }
    }
}
