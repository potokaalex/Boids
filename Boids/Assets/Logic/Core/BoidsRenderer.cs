using System.Runtime.CompilerServices;
using UnityEngine.Rendering;
using Unity.Collections;
using Unity.Mathematics;
using System.Linq;
using UnityEngine;
using System;

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
        private float2[] positions;
        private float[] rotations;

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
        public BoidsRenderer(Sprite sprite, Material material)
        {
            _propertyId = Shader.PropertyToID(ShaderPropertyName);
            _propertyBlock = new();
            _material = material;
            _mesh = GetMesh(sprite);

            positionsAndRotationsBuffer = new Vector4[BatchSize];
            positions = Array.Empty<float2>();
            rotations = Array.Empty<float>();
        }

        public void Render(NativeArray<float2> nativePositions, NativeArray<float> nativeRotations, int count)
        {
            if (positions.Length != nativePositions.Length)
                positions = new float2[count];

            if (rotations.Length != nativeRotations.Length)
                rotations = new float[count];

            nativePositions.CopyTo(positions);
            nativeRotations.CopyTo(rotations);

            Render(positions, rotations, count);
        }

        public void Render(float2[] positions, float[] rotations, int count)
        {
            for (var done = 0; done < count; done += BatchSize)
            {
                var run = math.min(count - done, BatchSize);

                for (var i = 0; i < run; i++)
                {
                    var position = positions[i + done];
                    var rotation = rotations[i + done];

                    positionsAndRotationsBuffer[i] = new(position.x, position.y, rotation, rotation);
                }

                _propertyBlock.SetVectorArray(_propertyId, positionsAndRotationsBuffer);

                Graphics.DrawMeshInstancedProcedural
                    (_mesh, 0, _material, new Bounds(Vector3.zero, Vector3.one * float.MaxValue), run, _propertyBlock, ShadowCastingMode.Off, false);
            }
        }

        [MethodImpl(MethodImplOptions.AggressiveInlining)]
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
