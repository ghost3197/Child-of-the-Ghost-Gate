using UnityEngine;

public class ZoneManager : MonoBehaviour
{
    [Header("플레이어")]
    public Transform player;

    [Header("배경 오브젝트들 - 순서대로 드래그")]
    public GameObject[] backgrounds;
    // 0 = 건물입구 배경
    // 1 = 길거리 배경
    // 2 = 산속 배경
    // 3 = 동굴 배경

    [Header("구역 경계 X좌표")]
    public float[] zoneThresholds;
    // 0 = 길거리로 넘어가는 X좌표 (예: 20)
    // 1 = 산속으로 넘어가는 X좌표 (예: 40)
    // 2 = 동굴로 넘어가는 X좌표 (예: 60)

    private int currentZone = 0;   // 현재 구역 번호

    void Start()
    {
        if (player == null)
        {
            Debug.LogError("[ZoneManager] player가 비어 있습니다.", this);
            enabled = false;
            return;
        }

        if (backgrounds == null || backgrounds.Length == 0)
        {
            Debug.LogError("[ZoneManager] backgrounds가 비어 있습니다.", this);
            enabled = false;
            return;
        }

        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i] == null)
            {
                Debug.LogError("[ZoneManager] backgrounds 배열의 " + i + "번 칸이 비어 있습니다.", this);
                enabled = false;
                return;
            }
        }

        ShowZone(0);
    }


    void Update()
    {
        CheckZone();
    }

    void CheckZone()
    {
        // 플레이어 X좌표 확인
        float playerX = player.position.x;

        // 뒤에서부터 확인 (가장 멀리 있는 구역부터)
        for (int i = zoneThresholds.Length - 1; i >= 0; i--)
        {
            if (playerX >= zoneThresholds[i])
            {
                // 이 구역이 현재 구역과 다르면 배경 교체
                if (currentZone != i + 1)
                {
                    currentZone = i + 1;
                    ShowZone(currentZone);
                    Debug.Log("구역 변경: " + currentZone);
                }
                return;
            }
        }

        // 아무 경계도 안 넘었으면 0번 구역
        if (currentZone != 0)
        {
            currentZone = 0;
            ShowZone(0);
        }
    }

    void ShowZone(int zoneIndex)
    {
        for (int i = 0; i < backgrounds.Length; i++)
        {
            if (backgrounds[i] != null)
            {
                backgrounds[i].SetActive(false);
            }
        }

        if (zoneIndex >= 0 && zoneIndex < backgrounds.Length && backgrounds[zoneIndex] != null)
        {
            backgrounds[zoneIndex].SetActive(true);
        }
    }

}