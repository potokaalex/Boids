using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    public Mesh mesh;
    public Material mat;
    public Texture texture;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        var areaSize = new Vector3(Screen.width, Screen.height, 10);
        var bounds = new Bounds(Vector3.zero, areaSize / 2);

        var b = new Bounds(Vector3.zero, new Vector3(100.0f, 100.0f, 100.0f));

        Graphics.DrawMeshInstancedProcedural(mesh, 0, mat, b, 10);
    }
}
