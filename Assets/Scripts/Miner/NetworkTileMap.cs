using Unity.Netcode;
using UnityEngine;
using UnityEngine.Tilemaps;

public class NetworkTileMap : NetworkBehaviour
{
    [SerializeField] GameObject[] minerals;

    Tilemap tilemap;

    NetworkList<Vector3Int> destroyedTiles = new NetworkList<Vector3Int>();
    // 

    private void Awake()
    {
        tilemap = GetComponent<Tilemap>();
    }

    public override void OnNetworkSpawn()
    {
        base.OnNetworkSpawn();

        destroyedTiles.OnListChanged += OnTileDestroyed;

        foreach(var tilePos in destroyedTiles)
        {
            tilemap.SetTile(tilePos, null);
        }
    }

    public void RemoveTile(Vector3 hitPos) //타일 없애기
    {
        if (!IsServer)
            return;
        

        Vector3Int cellPos = tilemap.WorldToCell(hitPos); //월드상에서 셀 위치로 위치변홤. 타일들이 int값으로 하나하나 있음.

        //아이템 드롭 기능. 드롭 확률 30
        int ranItemDrop = Random.Range(0, 101);
        if (ranItemDrop >= 70)
        {
            int ranIndex = Random.Range(0, minerals.Length);
            GameObject mineral = Instantiate(minerals[ranIndex], cellPos, Quaternion.identity);
            mineral.GetComponent<NetworkObject>().Spawn();
        }

        if(tilemap.GetTile(cellPos) != null)
        {
            destroyedTiles.Add(cellPos);
        }
    }

    void OnTileDestroyed(NetworkListEvent<Vector3Int> changedEvent) //여기서 changeEvent는 특정 위치. Pos 라고 보면 됨
    {
        if(changedEvent.Type == NetworkListEvent<Vector3Int>.EventType.Add)
        {
            tilemap.SetTile(changedEvent.Value, null);
        }
    }
}
