using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(BezierCollider2D))]
public class BezierCollider2DEditor : Editor
{
	BezierCollider2D bezierCollider;
	EdgeCollider2D edgeCollider;
	LineRenderer lineRenderer;

	int lastPointsQuantity = 0;
	Vector2 lastFirstPoint         = Vector2.zero;
	Vector2 lastHandlerFirstPoint  = Vector2.zero;
	Vector2 lastSecondPoint        = Vector2.zero;
	Vector2 lastHandlerSecondPoint = Vector2.zero;

	public override void OnInspectorGUI()
	{
		bezierCollider = (BezierCollider2D)target;

		edgeCollider = bezierCollider.GetComponent<EdgeCollider2D>();
		lineRenderer = bezierCollider.GetComponent<LineRenderer>();

			edgeCollider.hideFlags = HideFlags.None;

		if (edgeCollider != null)
		{
			bezierCollider.pointsQuantity     = EditorGUILayout.IntField("Curve Points", bezierCollider.pointsQuantity, GUILayout.MinWidth(100));
			bezierCollider.firstPoint         = EditorGUILayout.Vector2Field("First Point", bezierCollider.firstPoint, GUILayout.MinWidth(100));
			bezierCollider.handlerFirstPoint  = EditorGUILayout.Vector2Field("Handler First Point", bezierCollider.handlerFirstPoint, GUILayout.MinWidth(100));
			bezierCollider.secondPoint        = EditorGUILayout.Vector2Field("Second Point", bezierCollider.secondPoint, GUILayout.MinWidth(100));
			bezierCollider.handlerSecondPoint = EditorGUILayout.Vector2Field("Handler SecondPoint", bezierCollider.handlerSecondPoint, GUILayout.MinWidth(100));

			EditorUtility.SetDirty(bezierCollider);

			if (bezierCollider.pointsQuantity > 0 && !bezierCollider.firstPoint.Equals(bezierCollider.secondPoint) &&
				(
					lastPointsQuantity     != bezierCollider.pointsQuantity ||
					lastFirstPoint         != bezierCollider.firstPoint ||
					lastHandlerFirstPoint  != bezierCollider.handlerFirstPoint ||
					lastSecondPoint        != bezierCollider.secondPoint ||
					lastHandlerSecondPoint != bezierCollider.handlerSecondPoint
				))
			{
				lastPointsQuantity     = bezierCollider.pointsQuantity;
				lastFirstPoint         = bezierCollider.firstPoint;
				lastHandlerFirstPoint  = bezierCollider.handlerFirstPoint;
				lastSecondPoint        = bezierCollider.secondPoint;
				lastHandlerSecondPoint = bezierCollider.handlerSecondPoint;
				Vector2[] points = bezierCollider.calculate2DPoints();
				edgeCollider.points = points;
				lineRenderer.positionCount = points.Count();
				lineRenderer.SetPositions(points.ToList().ConvertAll(v2 => (Vector3)v2).ToArray());
			}

		}
	}

	void OnSceneGUI()
	{
		if (bezierCollider != null)
		{
			Handles.color = Color.grey;

			Handles.DrawLine(bezierCollider.transform.position + (Vector3)bezierCollider.handlerFirstPoint, bezierCollider.transform.position + (Vector3)bezierCollider.firstPoint);
			Handles.DrawLine(bezierCollider.transform.position + (Vector3)bezierCollider.handlerSecondPoint, bezierCollider.transform.position + (Vector3)bezierCollider.secondPoint);

			bezierCollider.firstPoint         = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.firstPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.firstPoint)), Vector3.zero, Handles.DotCap) - bezierCollider.transform.position;
			bezierCollider.secondPoint        = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.secondPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.secondPoint)), Vector3.zero, Handles.DotCap) - bezierCollider.transform.position;
			bezierCollider.handlerFirstPoint  = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerFirstPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerFirstPoint)), Vector3.zero, Handles.DotCap) - bezierCollider.transform.position;
			bezierCollider.handlerSecondPoint = Handles.FreeMoveHandle(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerSecondPoint), Quaternion.identity, 0.04f * HandleUtility.GetHandleSize(bezierCollider.transform.position + ((Vector3)bezierCollider.handlerSecondPoint)), Vector3.zero, Handles.DotCap) - bezierCollider.transform.position;

			if (GUI.changed)
			{
				EditorUtility.SetDirty(target);
			}
		}
	}

}