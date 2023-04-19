using System.Linq;
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

public readonly struct RenderInfo
{
    private static readonly int MainTexPropertyId = Shader.PropertyToID("_MainTex");
    public readonly Mesh mesh;
    public readonly Material mat;

    public RenderInfo(Mesh m, Material material)
    {
        mesh = m;
        mat = material;
    }

    public static RenderInfo FromSprite(Material baseMaterial, Sprite s)
    {
        var renderMaterial = UnityEngine.Object.Instantiate(baseMaterial);
        renderMaterial.enableInstancing = true;
        renderMaterial.SetTexture(MainTexPropertyId, s.texture);
        Mesh m = new Mesh
        {
            vertices = s.vertices.Select(v => (Vector3)v).ToArray(),
            triangles = s.triangles.Select(t => (int)t).ToArray(),
            uv = s.uv
        };
        return new RenderInfo(m, renderMaterial);
    }
}

public class FObject
{
    private static readonly Unity.Mathematics.Random r = new Unity.Mathematics.Random(10);
    public Vector2 position;
    public readonly float scale;
    private readonly Vector2 velocity;
    public float rotation;
    private readonly float rotationRate;
    public float time;

    public FObject()
    {
        position = new Vector2((float)r.NextDouble() * 10f - 5f, (float)r.NextDouble() * 8f - 4f);
        velocity = new Vector2((float)r.NextDouble() * 0.4f - 0.2f, (float)r.NextDouble() * 0.4f - 0.2f);
        rotation = (float)r.NextDouble();
        rotationRate = (float)r.NextDouble() * 0.6f - 0.2f;
        scale = 0.6f + (float)r.NextDouble() * 0.8f;
        time = (float)r.NextDouble() * 6f;
    }
    public void DoUpdate(float dT)
    {
        position += velocity * dT;
        rotation += rotationRate * dT;
        time += dT;
    }
}
