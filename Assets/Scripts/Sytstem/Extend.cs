using SoundManager;
using UnityEngine;

public static class Ex
{
    public static void PlayOneShot(this AudioSource target, AudioGet get)
    {
        target.PlayOneShot(SoundSystem.Get.GetAudioClip(get));
    }

    public static Vector2 Vec2GetDirForAngle(float angle)
    {

        Vector2 xy = new Vector2()
        {
            x = Mathf.Cos(angle * Mathf.Deg2Rad),
            y = Mathf.Sin(angle * Mathf.Deg2Rad)
        };
        return xy;

    }

    public static Vector3 Vec3GetDirForAngle(float angle)
    {

        Vector2 xy = new Vector2()
        {
            x = Mathf.Cos(angle * Mathf.Deg2Rad),
            y = Mathf.Sin(angle * Mathf.Deg2Rad)
        };
        return xy;

    }

    public static void UPLookAt(this Transform transfrom, Vector3 dir)
    {
        float angle = Vector3.Angle(Vector3.right, dir);
        angle *= RotationDir2D(Vector3.right, dir);
        transfrom.up = Vec3GetDirForAngle(angle);
    }
    public static void UPLookAt(this Transform transfrom,Transform target)
    {
        Vector3 dir = target.position - transfrom.position;
        float angle = Vector3.Angle(Vector3.right, dir);

        angle *= RotationDir2D(Vector3.right, dir);
        transfrom.up = Vec3GetDirForAngle(angle);
    }
    /// <summary>
    /// return 1f -1f;
    /// </summary>
    /// <returns></returns>
    public static float RotationDir2D(Vector3 To ,Vector3 From)
    {
        Vector3 cross = Vector3.Cross(To, From).normalized;

        if (cross.z == 0f)
        {
            if (Vector3.Dot(To,From)>0f)
            {
                cross.z = 1f;
            }
            else
            {
                cross.z = -1f;
            }
        }

        return cross.z / Mathf.Abs(cross.z);
    }
}