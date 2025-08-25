using UnityEngine;
using UnityEngine.Tilemaps;

public class DigEvent : MonoBehaviour
{
    NetworkTileMap networkTilemap;
   
    Tilemap tilemap;
    [SerializeField] LayerMask tileLayer;
    [SerializeField] Transform[] hitPoints; //히트포인트가 여러개가 됐기 때문에 배열로 담음

    private void Awake()
    {       
        networkTilemap = FindFirstObjectByType<NetworkTileMap>();
    }

    public void OnDig() //타일 없애는 애 호출함 
    {
        for(int i = 0; i <hitPoints.Length; i++)
        {
            Collider2D coll = Physics2D.OverlapCircle(hitPoints[i].position, 0.1f, tileLayer); //자기 자신의 거리에서 특정 반지름 만큼 영역을 잡고 그 영역 중 타일 레이어를 잡으면 콜에다가 넣겠따. 

             if (coll != null)
            {
                networkTilemap.RemoveTile(hitPoints[i].position); //타일 위치 잡아서 null처리 해줘야 빈 것 됨

                break; //먼저 부딪힌 애꺼만 부시고 브레이크
            }
        }
    }
}
