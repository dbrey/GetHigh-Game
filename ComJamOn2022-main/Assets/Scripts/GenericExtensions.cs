using Random = UnityEngine.Random;
using System.Threading;
using UnityEngine;

public static class GenericExtensions
{
    public static T GetRandom<T>(this T[] array) => array[Random.Range(0, array.Length)];

    public static void CancelAndGenerateNew(ref CancellationTokenSource source)
    {
        source.Cancel();
        source = new CancellationTokenSource();
    }

    public static Vector3 RandomPointInBounds(this Bounds bounds)
    {
        return new Vector3(
            Random.Range(bounds.min.x, bounds.max.x),
            Random.Range(bounds.min.y, bounds.max.y),
            0
        );
    }
}
