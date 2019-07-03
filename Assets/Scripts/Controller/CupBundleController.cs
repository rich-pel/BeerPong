using BeardedManStudios.Forge.Networking.Unity;
using BeardedManStudios.Forge.Networking.Generated;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CupBundleController : MonoBehaviour
{
    private CupController[] cupsInGroup = new CupController[10];

    [SerializeField] private int NetworkPrefabIndex;
    [SerializeField] private float radius = 0.7f;

    private int _numRows = 4;
    private float _gab = 0.011f;

    private float _distanceX;
    private float _distanceY;


    public bool InitClient(bool bRedCups)
    {
        byte sanityCount = 0;
        GameObject[] objs = SceneManager.GetActiveScene().GetRootGameObjects();
        foreach (GameObject obj in objs)
        {
            CupController cup = obj.GetComponent<CupController>();
            if (cup != null && cup.networkObject.cupIsRed == bRedCups)
            {
                cupsInGroup[cup.networkObject.cupIndex] = cup;
                sanityCount++;
            }
        }

        if (sanityCount != 10)
        {
            Debug.LogError("We're missing cups! Found " + sanityCount + "/10");
            return false;
        }

        return true;
    }

    public void Init(bool bRedCups)
    {
        // get radius from somewhere
        _distanceY = radius + _gab;
        _distanceX = radius / 2 + _gab;

        if (NetworkPrefabIndex < 0 || NetworkPrefabIndex >= NetworkManager.Instance.SyncedCupNetworkObject.Length)
        {
            Debug.LogError("NetworkPrefabIndex is out of Range!");
            return;
        }

        CreateCups(bRedCups);
    }

    /// <summary>
    /// Instantiates Cups in diamond formation
    /// </summary>
    void CreateCups(bool bRedCups)
    {
        byte _count = 0;

        for (int y = 0; y < _numRows; y++)
        {
            for (int x = -y; x <= y; x += 2)
            {
                GameObject prefab = NetworkManager.Instance.SyncedCupNetworkObject[NetworkPrefabIndex];

                // local position
                // Vector3 pos = new Vector3(x * _distance /2, 0f, y * _distance);
                Vector3 pos = new Vector3(x * _distanceX, 0.0f, y * _distanceY);

                Quaternion rot = prefab.transform.rotation;
                pos = transform.TransformPoint(pos); // dis is fucking neat man!

                // Instance settings
                SyncedCupBehavior syncedCup = NetworkManager.Instance.InstantiateSyncedCup(NetworkPrefabIndex, pos, rot, true);
                CupController cup = syncedCup as CupController;

                // this should actually never fail
                if (cup == null)
                {
                    Debug.LogError("SyncedCupBehavior is not a CupController!");
                    return;
                }

                cup.networkObject.cupIndex = _count;
                cup.networkObject.cupIsRed = bRedCups;
                cup.networkObject.homePosition = pos;
                cup.networkObject.homeRotation = rot;
                cup.Init(); // needs to be initialized this frame!

                GameObject myGO = syncedCup.gameObject;
                myGO.name = "Cup" + _count;
                cup.father = this;
                cupsInGroup[_count++] = cup;
                
                // TODO: Lass das den copController machen: myGO.GetComponent<CupController>().father = this.transform; 
            }
        }

    }

    public int GetNumberOfActiveCups()
    {
        int cupsActive = 0;

        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
                cupsActive++;
        }
        
        return cupsActive;
    }

    public void SetCupsOwnership(bool IOwnThem)
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
            {
                cup.SetOwnership(IOwnThem);
            }
        }
    }

    public bool AmIOwnerOfCups()
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy && !cup.networkObject.IsOwner) return false;
        }
        return true;
    }

    public void ResetCups(bool activeOnly)
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (activeOnly && !cup.gameObject.activeInHierarchy) continue;

            cup.SetActive(true); // do not forget to activate on client aswell
            cup.Reset();
        }
    }

    public void SyncCups(bool sync)
    {
        foreach (CupController cup in cupsInGroup)
        {
            if (cup.gameObject.activeInHierarchy)
            {
                cup.SetSync(sync);
            }
        }
    }
}