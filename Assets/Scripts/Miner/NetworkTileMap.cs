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

    public void RemoveTile(Vector3 hitPos) //Ÿ�� ���ֱ�
    {
        if (!IsServer)
            return;
        

        Vector3Int cellPos = tilemap.WorldToCell(hitPos); //����󿡼� �� ��ġ�� ��ġ���c. Ÿ�ϵ��� int������ �ϳ��ϳ� ����.

        //������ ��� ���. ��� Ȯ�� 30
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

    void OnTileDestroyed(NetworkListEvent<Vector3Int> changedEvent) //���⼭ changeEvent�� Ư�� ��ġ. Pos ��� ���� ��
    {
        if(changedEvent.Type == NetworkListEvent<Vector3Int>.EventType.Add)
        {
            tilemap.SetTile(changedEvent.Value, null);
        }
    }
}
