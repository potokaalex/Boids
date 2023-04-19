using UnityEngine.Rendering;
using UnityEngine;

public class Manager : MonoBehaviour
{
    private static readonly int posDirPropertyId = Shader.PropertyToID("posDirBuffer");
    //private static readonly int timePropertyId = Shader.PropertyToID("timeBuffer");

    private MaterialPropertyBlock pb;
    private readonly Vector4[] posDirArr = new Vector4[batchSize];
    private readonly float[] timeArr = new float[batchSize];
    private const int batchSize = 7;
    public int instanceCount;

    public Sprite sprite;
    public Material baseMaterial;
    private RenderInfo ri;
    public string layerRenderName;
    private int layerRender;
    private FObject[] objects;
    //...

    private void Start()
    {
        pb = new MaterialPropertyBlock();
        layerRender = 10;//LayerMask.NameToLayer(layerRenderName);
        ri = RenderInfo.FromSprite(baseMaterial, sprite);
        Camera.onPreCull += RenderMe;
        objects = new FObject[instanceCount];
        for (int ii = 0; ii < instanceCount; ++ii)
        {
            objects[ii] = new FObject();
        }
    }

    private void Update()
    {
        float dT = Time.deltaTime;
        for (int ii = 0; ii < instanceCount; ++ii)
        {
            objects[ii].DoUpdate(dT);
        }
    }

    private void RenderMe(Camera c)
    {
        if (!Application.isPlaying)
            return;
        
        for (int done = 0; done < instanceCount; done += batchSize)
        {
            int run = Mathf.Min(instanceCount - done, batchSize);

            for (int batchInd = 0; batchInd < run; ++batchInd)
            {
                var obj = objects[done + batchInd];
                posDirArr[batchInd] = new Vector4(obj.position.x, obj.position.y,
                    Mathf.Cos(obj.rotation) * obj.scale, Mathf.Sin(obj.rotation) * obj.scale);
                timeArr[batchInd] = obj.time;
            }

            pb.SetVectorArray(posDirPropertyId, posDirArr);
            //pb.Set
            //pb.SetFloatArray(timePropertyId, timeArr);
            CallRender(c, run);
        }
    }

    private void CallRender(Camera c, int count)
    {
        Graphics.DrawMeshInstancedProcedural(ri.mesh, 0, ri.mat,
            bounds: new Bounds(Vector3.zero, Vector3.one * 1000f),
            count: count,
            properties: pb,
            castShadows: ShadowCastingMode.Off,
            receiveShadows: false,
            layer: layerRender,
            camera: c);
    }
}