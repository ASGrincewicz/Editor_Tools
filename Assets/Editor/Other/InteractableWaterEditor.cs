using System;
using Unity.VisualScripting;
using UnityEngine;
using UnityEditor;
using UnityEditor.UIElements;
using UnityEngine.UIElements;
using Water;

namespace Editor
{
    [CustomEditor(typeof(InteractableWater))]
    public class InteractableWaterEditor : UnityEditor.Editor
    {

        private InteractableWater _water;

        private void OnEnable()
        {
            _water = (InteractableWater)target;
        }
        
        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
           serializedObject.Update();
           if (GUILayout.Button("Generate Mesh"))
           {
               _water.GenerateMesh();
           }

           if (GUILayout.Button("Reset Edge Collider"))
           {
               _water.ResetEdgeCollider();
           }
        }

        private void ChangeDimensions(ref float width, ref float height, float calculatedWidthMax, float calculatedHeightMax)
        {
            width = Mathf.Max(0.1f, calculatedWidthMax);
            height = Mathf.Max(0.1f, calculatedHeightMax);
        }

        private void OnSceneGUI()
        {
            //Draw wireframe box
            Handles.color = _water.gizmoColor;
            Vector3 center = _water.transform.position;
            Vector3 size = new Vector3(_water.width, _water.height, 0.1f);
            Handles.DrawWireCube(center,size);
            
            //Handles for width and height
            float handleSize = HandleUtility.GetHandleSize(center) * 0.1f;
            Vector3 snap = Vector3.one * 0.1f;
            
            //Corner Handles
            Vector3[] corners = new Vector3[4];
            corners[0] = center + new Vector3(-_water.width / 2, -_water.height / 2, 0);//Bottom Left
            corners[1] = center + new Vector3(_water.width / 2, _water.height / 2, 0f);//Bottom Right
            corners[2] = center + new Vector3(-_water.width / 2, -_water.height / 2, 0);//Top Left
            corners[3] = center + new Vector3(_water.width / 2, _water.height / 2, 0f);//Top Right
            
            //Handle for each corner
            EditorGUI.BeginChangeCheck();
            Vector3 newBottomLeft = Handles.FreeMoveHandle(corners[0], handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                ChangeDimensions(ref _water.width, ref _water.height, corners[1].x - newBottomLeft.x, corners[3].y - newBottomLeft.y);
                _water.transform.position += new Vector3((newBottomLeft.x - corners[0].x) / 2,
                    (newBottomLeft.y - corners[0].y) / 2, 0);
            }
            
            EditorGUI.BeginChangeCheck();
            Vector3 newBottomRight = Handles.FreeMoveHandle(corners[1], handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                ChangeDimensions(ref _water.width, ref _water.height, newBottomRight.x - corners[0].x, corners[3].y - newBottomRight.y);
                _water.transform.position += new Vector3((newBottomRight.x - corners[1].x) / 2,
                    (newBottomRight.y - corners[1].y) / 2, 0);
            }
            
            EditorGUI.BeginChangeCheck();
            Vector3 newTopLeft = Handles.FreeMoveHandle(corners[2], handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                ChangeDimensions(ref _water.width, ref _water.height, corners[3].x - newTopLeft.x , newTopLeft.y - corners[0].y);
                _water.transform.position += new Vector3((newTopLeft.x - corners[2].x) / 2,
                    (newTopLeft.y - corners[2].y) / 2, 0);
            }
            
            EditorGUI.BeginChangeCheck();
            Vector3 newTopRight = Handles.FreeMoveHandle(corners[3], handleSize, snap, Handles.CubeHandleCap);
            if (EditorGUI.EndChangeCheck())
            {
                ChangeDimensions(ref _water.width, ref _water.height, newTopRight.x -  corners[2].x, newTopRight.y - corners[1].y);
                _water.transform.position += new Vector3((newTopRight.x - corners[3].x) / 2,
                    (newTopRight.y - corners[3].y) / 2, 0);
            }
            
            if(GUI.changed)
            {
                _water.GenerateMesh();
            }
        }
       
    }
}