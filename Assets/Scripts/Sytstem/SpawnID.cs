using UnityEngine;


public class SpawnID:MonoBehaviour
{
    public int identification = 0;

    public int spawn_max = 0;

    private ISpawnID type;

    public ISpawnID Type
    {
        get
        {
            return type;
        }

        set
        {
            if (type ==null)
            {
                type = value;
            }
         
        }
    }
}
public interface ISpawnID
{
    SpawnID Spawn_ID { set; get; }

    void ResetValue();

    void Clear();

}
