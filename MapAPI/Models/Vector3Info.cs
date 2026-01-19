/**
 * @file Vector3Info.cs
 * @description DTO for 3D position coordinates
 * @created 2025-01-19
 */

namespace MapAPI.Models
{
    /// <summary>
    /// Represents a 3D position in world coordinates
    /// </summary>
    public class Vector3Info
    {
        public float X { get; set; }
        public float Y { get; set; }
        public float Z { get; set; }

        public Vector3Info() { }

        public Vector3Info(float x, float y, float z)
        {
            X = x;
            Y = y;
            Z = z;
        }

        public Vector3Info(UnityEngine.Vector3 vector)
        {
            X = vector.x;
            Y = vector.y;
            Z = vector.z;
        }
    }
}
