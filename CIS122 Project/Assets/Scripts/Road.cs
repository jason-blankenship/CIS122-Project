#if UNITY_EDITOR
using UnityEditor;
using UnityEditor.Splines;
#endif

using System;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.Splines;
using Interpolators = UnityEngine.Splines.Interpolators;

namespace Unity.Splines.Examples
{
    [ExecuteInEditMode]
    [DisallowMultipleComponent]
    [RequireComponent(typeof(SplineContainer))]
    public class LoftRoadBehaviour : MonoBehaviour
    {
        [SerializeField]
        List<SplineData<float>> m_Widths = new List<SplineData<float>>();


        public List<SplineData<float>> Widths
        {
            get
            {
                foreach (var width in m_Widths)
                {
                    if (width.DefaultValue == 0)
                        width.DefaultValue = 1f;
                }

                return m_Widths;
            }
        }

        [SerializeField]
        SplineContainer m_Spline;

        public SplineContainer Container
        {
            get
            {
                if (m_Spline == null)
                    m_Spline = GetComponent<SplineContainer>();

                return m_Spline;
            }
            set => m_Spline = value;
        }

        [SerializeField]
        int m_SegmentsPerMeter = 1;

        [SerializeField]
        GameObject m_RoadSegmentPrefab; // Prefab to use for road segments

        [SerializeField]
        float m_TextureScale = 1f;

        public int SegmentsPerMeter => Mathf.Min(10, Mathf.Max(1, m_SegmentsPerMeter));

        public IReadOnlyList<Spline> splines => LoftSplines;

        public IReadOnlyList<Spline> LoftSplines
        {
            get
            {
                if (m_Spline == null)
                    m_Spline = GetComponent<SplineContainer>();

                if (m_Spline == null)
                {
                    Debug.LogError("Cannot loft road mesh because Spline reference is null");
                    return null;
                }

                return m_Spline.Splines;
            }
        }

        List<Vector3> m_Positions = new List<Vector3>();
        bool m_LoftRoadsRequested = false;

        void Update()
        {
            if (m_LoftRoadsRequested)
            {
                PlaceRoadSegments();
                m_LoftRoadsRequested = false;
            }
        }

        public void OnEnable()
        {
            if (m_Spline == null)
                m_Spline = GetComponent<SplineContainer>();

            PlaceRoadSegments();

#if UNITY_EDITOR
            EditorSplineUtility.AfterSplineWasModified += OnAfterSplineWasModified;
            EditorSplineUtility.RegisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
            Undo.undoRedoPerformed += PlaceRoadSegments;
#endif

            SplineContainer.SplineAdded += OnSplineContainerAdded;
            SplineContainer.SplineRemoved += OnSplineContainerRemoved;
            SplineContainer.SplineReordered += OnSplineContainerReordered;
            Spline.Changed += OnSplineChanged;
        }

        public void OnDisable()
        {
#if UNITY_EDITOR
            EditorSplineUtility.AfterSplineWasModified -= OnAfterSplineWasModified;
            EditorSplineUtility.UnregisterSplineDataChanged<float>(OnAfterSplineDataWasModified);
            Undo.undoRedoPerformed -= PlaceRoadSegments;
#endif

            SplineContainer.SplineAdded -= OnSplineContainerAdded;
            SplineContainer.SplineRemoved -= OnSplineContainerRemoved;
            SplineContainer.SplineReordered -= OnSplineContainerReordered;
            Spline.Changed -= OnSplineChanged;
        }

        void OnSplineContainerAdded(SplineContainer container, int index)
        {
            if (container != m_Spline)
                return;

            if (m_Widths.Count < LoftSplines.Count)
            {
                var delta = LoftSplines.Count - m_Widths.Count;
                for (var i = 0; i < delta; i++)
                {
#if  UNITY_EDITOR
                    Undo.RecordObject(this, "Modifying Widths SplineData");
#endif
                    m_Widths.Add(new SplineData<float>() { DefaultValue = 1f });
                }
            }

            PlaceRoadSegments();
        }

        void OnSplineContainerRemoved(SplineContainer container, int index)
        {
            if (container != m_Spline)
                return;

            if (index < m_Widths.Count)
            {
#if UNITY_EDITOR
                Undo.RecordObject(this, "Modifying Widths SplineData");
#endif
                m_Widths.RemoveAt(index);
            }

            PlaceRoadSegments();
        }

        void OnSplineContainerReordered(SplineContainer container, int previousIndex, int newIndex)
        {
            if (container != m_Spline)
                return;

            PlaceRoadSegments();
        }

        void OnAfterSplineWasModified(Spline s)
        {
            if (LoftSplines == null)
                return;

            foreach (var spline in LoftSplines)
            {
                if (s == spline)
                {
                    m_LoftRoadsRequested = true;
                    break;
                }
            }
        }

        void OnSplineChanged(Spline spline, int knotIndex, SplineModification modification)
        {
            OnAfterSplineWasModified(spline);
        }

        void OnAfterSplineDataWasModified(SplineData<float> splineData)
        {
            foreach (var width in m_Widths)
            {
                if (splineData == width)
                {
                    m_LoftRoadsRequested = true;
                    break;
                }
            }
        }

        public void PlaceRoadSegments()
        {
            foreach (Transform child in transform)
            {
                DestroyImmediate(child.gameObject); // Remove previous road segments
            }

            for (var i = 0; i < LoftSplines.Count; i++)
            {
                PlaceRoadAlongSpline(LoftSplines[i], i);
            }
        }

        public void PlaceRoadAlongSpline(Spline spline, int widthDataIndex)
        {
            if (spline == null || spline.Count < 2)
                return;

            float length = spline.GetLength();
            if (length <= 0.001f)
                return;

            var segmentsPerLength = SegmentsPerMeter * length;
            var segments = Mathf.CeilToInt(segmentsPerLength);
            var segmentStepT = (1f / SegmentsPerMeter) / length;
            var steps = segments + 1;

            float t = 0f;
            for (int i = 0; i < steps; i++)
            {
                SplineUtility.Evaluate(spline, t, out var pos, out var dir, out var up);

                // If dir evaluates to zero (linear or broken zero length tangents?)
                if (math.length(dir) == 0)
                {
                    var nextPos = spline.GetPointAtLinearDistance(t, 0.01f, out _);
                    dir = math.normalizesafe(nextPos - pos);

                    if (math.length(dir) == 0)
                    {
                        nextPos = spline.GetPointAtLinearDistance(t, -0.01f, out _);
                        dir = -math.normalizesafe(nextPos - pos);
                    }

                    if (math.length(dir) == 0)
                        dir = new float3(0, 0, 1);
                }

                var scale = transform.lossyScale;
                var tangent = math.normalizesafe(math.cross(up, dir)) * new float3(1f / scale.x, 1f / scale.y, 1f / scale.z);

                var w = 1f;
                if (widthDataIndex < m_Widths.Count)
                {
                    w = m_Widths[widthDataIndex].DefaultValue;
                    if (m_Widths[widthDataIndex] != null && m_Widths[widthDataIndex].Count > 0)
                    {
                        w = m_Widths[widthDataIndex].Evaluate(spline, t, PathIndexUnit.Normalized, new Interpolators.LerpFloat());
                        w = math.clamp(w, .001f, 10000f);
                    }
                }

                // Ensure m_RoadSegmentPrefab is assigned before instantiating
                if (m_RoadSegmentPrefab == null)
                {
                    Debug.LogError("Road Segment Prefab is not assigned in the Inspector!");
                    return;
                }

                var roadSegment = Instantiate(m_RoadSegmentPrefab, pos + tangent * w, Quaternion.LookRotation(dir, up), transform);
                roadSegment.transform.localScale = new Vector3(w, 1f, 1f); // Adjust the scale of each segment based on the width

                t = math.min(1f, t + segmentStepT);
            }
        }

    }
}
