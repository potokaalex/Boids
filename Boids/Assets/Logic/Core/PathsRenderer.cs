using Unity.Mathematics;
using UnityEngine;
using BoidSimulation.Data;
using System;
using UnityEngine.Rendering;
using System.Collections.Generic;
//using System.Drawing;

namespace BoidSimulation
{
    public class PathsRenderer : IDisposable
    {
        private LineRenderer _lineRenderer;
        private float2[] _points;
        Material material;
        BoidsHistory _history;
        //bool isRendering;

        public PathsRenderer(Material linesMaterial)
        {
            _lineRenderer = CreateLineRenderer(linesMaterial);
            _points = Array.Empty<float2>();

            Shader shader = Shader.Find("Hidden/Internal-Colored");
            material = new Material(shader);

            //material = linesMaterial;

            Camera.onPostRender += OnPostRender;
        }

        public void Render(BoidsHistory history)
        {
            _history = history;

            //for (var i = 0; i < _points.Length; i++)
            //    _lineRenderer.SetPosition(i, new(_points[i].x, _points[i].y, 1));
            //isRendering = true;
        }

        private LineRenderer CreateLineRenderer(Material material)
        {
            var gameObject = new GameObject(nameof(PathsRenderer));
            var lineRenderer = gameObject.AddComponent<LineRenderer>();

            gameObject.hideFlags = HideFlags.NotEditable;

            lineRenderer.motionVectorGenerationMode = MotionVectorGenerationMode.ForceNoMotion;
            lineRenderer.shadowCastingMode = ShadowCastingMode.Off;
            //lineRenderer.bounds = new(new(), Vector3.one * float.MaxValue);
            lineRenderer.receiveShadows = false;
            lineRenderer.material = material;

            return lineRenderer;
        }

        void OnPostRender(Camera camera)
        {
            //if (isRendering)
            RenderLines();
        }

        private void RenderLines()
        {
            Matrix4x4 mat = new Matrix4x4();
            //mat.SetTRS(RootObj.transform.position, RootObj.transform.rotation, RootObj.transform.localScale);
            GL.PushMatrix();
            GL.MultMatrix(mat);
            GL.Begin(GL.LINES);
            RenderLineList(_history, material.color);
            GL.End();
            GL.PopMatrix();
        }

        private void RenderLineList(BoidsHistory history, Color color)
        {
            material.SetPass(0);
            GL.Color(color);

            GL.Vertex3(0, 0, 0);
            GL.Vertex3(100, 100, 0);

            return;

            for (var boidIndex = 0; boidIndex < history.GetHistoryLength(); boidIndex++)
            {
                ref var queue = ref history.GetPositionsQueue(boidIndex);

                if (_points.Length < queue.GetCurrentLength())
                {
                    _points = new float2[queue.GetCurrentLength()];
                    _lineRenderer.positionCount = queue.GetCurrentLength();
                }

                queue.GetItems(ref _points);

                foreach (var point in _points)
                    GL.Vertex3(point.x, point.y, 0);
            }
        }

        public void Dispose()
        {
            Camera.onPostRender -= OnPostRender;
        }
    }
}
