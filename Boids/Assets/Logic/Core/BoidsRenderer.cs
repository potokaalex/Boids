using UnityEngine.Rendering;
using Unity.Mathematics;
using System.Linq;
using UnityEngine;
using Extensions;

namespace BoidSimulation
{
    public class BoidsRenderer
    {
        private const string ShaderPropertyName = "PositionsAndRotationsBuffer";
        private const int BatchSize = 1023;
        private readonly int _propertyId;

        private MaterialPropertyBlock _propertyBlock;
        private Material _material;
        private Mesh _mesh;

        private Vector4[] positionsAndRotationsBuffer;
        private float2 _halfAreaSize;

        public BoidsRenderer(Sprite sprite, Material material, float2 areaSize)
        {
            _propertyId = Shader.PropertyToID(ShaderPropertyName);
            _propertyBlock = new();
            _material = material;
            _mesh = GetMesh(sprite);
            _halfAreaSize = areaSize / 2;

            positionsAndRotationsBuffer = new Vector4[BatchSize];
        }

        public void Render(UnsafeArray<float2> positions, UnsafeArray<float> rotations,int count)
        {
            for (var done = 0; done < count; done += BatchSize)
            {
                var run = math.min(count - done, BatchSize);

                for (var i = 0; i < run; i++)
                {
                    var position = positions[i + done] - _halfAreaSize;
                    var rotation = rotations[i + done];

                    positionsAndRotationsBuffer[i] = new(position.x, position.y, rotation, rotation);
                }

                _propertyBlock.SetVectorArray(_propertyId, positionsAndRotationsBuffer);

                Graphics.DrawMeshInstancedProcedural
                    (_mesh, 0, _material, new Bounds(Vector3.zero, Vector3.one * float.MaxValue), run, _propertyBlock, ShadowCastingMode.Off, false);
            }
        }

        private Mesh GetMesh(Sprite sprite)
        {
            return new()
            {
                vertices = sprite.vertices.Select(v => (Vector3)v).ToArray(),
                triangles = sprite.triangles.Select(t => (int)t).ToArray(),
                uv = sprite.uv
            };
        }
    }
}
