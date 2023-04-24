using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using CurveLib.Curves;

#if UNITY_EDITOR
[CustomEditor(typeof(SplineController))]
public class SplineControllerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        SplineController myScript = (SplineController)target;
        if (GUILayout.Button("Generate Spline"))
        {
            myScript.GenerateSpline();
        }

        SplineController myScript2 = (SplineController)target;
        if (GUILayout.Button("Destroy Blue Line"))
        {
            myScript.DestroyVisibleSpline();
        }
    }
}
#endif

[ExecuteInEditMode]
public class SplineController : MonoBehaviour
{
    [SerializeField] private float _tension = 0.5f;
    [SerializeField] private float _wide = 0.2f;
    [SerializeField] private Material _material;
    [SerializeField] private GameObject go_spline;
    [SerializeField] private LineRenderer _generatedSplineChordal;
    public SplineCurve SplineCurve;
    public float SplineCurveLength;
    public Vector3[] SplineCurvePoints;
    private string _goSplineName = "ChordalSpline";

    private void OnEnable()
    {
        //RemakeSpline();
        var count = transform.childCount;
        if (count < 2)
            Debug.LogWarning("Create few points in 'Spline' object to start and press 'Generate Spline' ");
    }

    private void Start()
    {
        //if (go_spline != null)
        //    go_spline.GetComponent<Renderer>().enabled = false;  
        // 
    }

    public void GenerateSpline()
    {
        if (_generatedSplineChordal == null)
        {
            go_spline = new GameObject(_goSplineName);
            go_spline.transform.SetParent(transform.parent);
            _generatedSplineChordal = go_spline.AddComponent<LineRenderer>();
            _generatedSplineChordal.GetComponent<LineRenderer>().material = _material;
            _generatedSplineChordal.startWidth = _wide;
            _generatedSplineChordal.endWidth = _wide;
        }

        var count = transform.childCount;
        var points = new Vector3[count];
        for (int i = 0; i < count; i++)
        {
            points[i] = transform.GetChild(i).position;
        }

        {
            SplineCurve = new SplineCurve(points, false, SplineType.Chordal, tension: _tension);

            SplineCurveLength = SplineCurve.GetLength();
            SplineCurvePoints = SplineCurve.GetPoints((int)(SplineCurveLength * 4));
            _generatedSplineChordal.positionCount = SplineCurvePoints.Length;
            _generatedSplineChordal.SetPositions(SplineCurvePoints);
        }

        Debug.Log("Don't forget destroy blue spline for game after you create a chunk");
    }

    public void DestroyVisibleSpline()
    {
        if(go_spline != null)
            DestroyImmediate(go_spline);
        else
        {
            var go = GameObject.Find(_goSplineName);
            DestroyImmediate(go); 
        }

    }
}
