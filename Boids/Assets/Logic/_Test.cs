using BoidSimulation.Data;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Mathematics;
using UnityEngine;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;

public class _Test : MonoBehaviour
{
    [SerializeField] private Material material;

    private void Start()
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        material = new Material(shader);
    }

    private void OnDrawGizmos()
    {
        Matrix4x4 mat = new Matrix4x4();
        GL.PushMatrix();
        GL.MultMatrix(mat);
        GL.Begin(GL.LINES);
        RenderLineList(material.color);
        GL.End();
        GL.PopMatrix();
    }

    private void RenderLineList(Color color)
    {
        Shader shader = Shader.Find("Hidden/Internal-Colored");
        material = new Material(shader);

        material.SetPass(0);
        GL.Color(color);

        GL.Vertex3(0, 0, 0);
        GL.Vertex3(100, 100, 0);
    }
}


public class ExampleClass : MonoBehaviour
{
    // When added to an object, draws colored rays from the
    // transform position.
    public int lineCount = 100;
    public float radius = 3.0f;

    static Material lineMaterial;
    static void CreateLineMaterial()
    {
        if (!lineMaterial)
        {
            // Unity has a built-in shader that is useful for drawing
            // simple colored things.
            Shader shader = Shader.Find("Hidden/Internal-Colored");
            lineMaterial = new Material(shader);
            lineMaterial.hideFlags = HideFlags.HideAndDontSave;
            // Turn on alpha blending
            lineMaterial.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
            lineMaterial.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
            // Turn backface culling off
            lineMaterial.SetInt("_Cull", (int)UnityEngine.Rendering.CullMode.Off);
            // Turn off depth writes
            lineMaterial.SetInt("_ZWrite", 0);
        }
    }

    // Will be called after all regular rendering is done
    public void OnRenderObject()
    {
        CreateLineMaterial();
        // Apply the line material
        lineMaterial.SetPass(0);

        GL.PushMatrix();
        // Set transformation matrix for drawing to
        // match our transform
        GL.MultMatrix(transform.localToWorldMatrix);

        // Draw lines
        GL.Begin(GL.LINES);
        for (int i = 0; i < lineCount; ++i)
        {
            float a = i / (float)lineCount;
            float angle = a * Mathf.PI * 2;
            // Vertex colors change from red to green
            GL.Color(new Color(a, 1 - a, 0, 0.8F));
            // One vertex at transform position
            GL.Vertex3(0, 0, 0);
            // Another vertex at edge of circle
            GL.Vertex3(Mathf.Cos(angle) * radius, Mathf.Sin(angle) * radius, 0);
        }
        GL.End();
        GL.PopMatrix();
    }
}