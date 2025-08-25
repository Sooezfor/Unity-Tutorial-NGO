using UnityEngine;
using UnityEngine.Tilemaps;

public class DigEvent : MonoBehaviour
{
    NetworkTileMap networkTilemap;
   
    Tilemap tilemap;
    [SerializeField] LayerMask tileLayer;
    [SerializeField] Transform[] hitPoints; //��Ʈ����Ʈ�� �������� �Ʊ� ������ �迭�� ����

    private void Awake()
    {       
        networkTilemap = FindFirstObjectByType<NetworkTileMap>();
    }

    public void OnDig() //Ÿ�� ���ִ� �� ȣ���� 
    {
        for(int i = 0; i <hitPoints.Length; i++)
        {
            Collider2D coll = Physics2D.OverlapCircle(hitPoints[i].position, 0.1f, tileLayer); //�ڱ� �ڽ��� �Ÿ����� Ư�� ������ ��ŭ ������ ��� �� ���� �� Ÿ�� ���̾ ������ �ݿ��ٰ� �ְڵ�. 

             if (coll != null)
            {
                networkTilemap.RemoveTile(hitPoints[i].position); //Ÿ�� ��ġ ��Ƽ� nulló�� ����� �� �� ��

                break; //���� �ε��� �ֲ��� �νð� �극��ũ
            }
        }
    }
}
