using UnityEditor;
using UnityEngine;

/**
*	Editor script which displays the properties of a Transform.
*	Extended to show World Position.
*/

[CustomEditor(typeof(Transform))]
class TransformEditor : Editor
{
	// current target transform
	private Transform trans;
	// rotation euler angles for display and editing
	private Vector3 eulerRotation;

	public void OnEnable()
	{
		// get current target transform
		this.trans = (Transform)target;
	}

	public override void OnInspectorGUI()
	{
		// very important
		this.serializedObject.Update();

		// adds undo support for changes in editor
		Undo.RecordObject(this.trans, "Update Transform");

		#region Position
		// shows editable world position, assigns value to the transform
		this.trans.position = EditorGUILayout.Vector3Field("World Position", this.trans.position);
		// shows editable local position, assigns value to the transform
		this.trans.localPosition = EditorGUILayout.Vector3Field("Local Position", this.trans.localPosition);
		#endregion

		#region Rotation
		// shows editable euler angles, assigns value to variable
		this.eulerRotation = EditorGUILayout.Vector3Field("Local Rotation", trans.rotation.eulerAngles);

		// applies new euler angles as a Quaternion to the transform
		Quaternion newRotation = new Quaternion();
		newRotation.eulerAngles = this.eulerRotation;

		trans.rotation = newRotation;
		#endregion

		#region Scale
		// shows editable local scale, assigns value to the transform
		trans.localScale = EditorGUILayout.Vector3Field("Local Scale", trans.localScale);
		#endregion

		// very important
		this.serializedObject.ApplyModifiedProperties();
	}
}