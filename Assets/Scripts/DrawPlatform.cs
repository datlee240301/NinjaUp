using UnityEngine;

public class DrawPlatform : MonoBehaviour {
    public GameObject platformPrefab; // Prefab bệ đỡ
    private Vector2 startPos; // Vị trí bắt đầu vẽ

    void Update() {
        if (Input.GetMouseButtonDown(0)) {
            startPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButtonUp(0)) {
            Vector2 endPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            CreatePlatform(startPos, endPos);
        }
    }

    // Tạo bệ đỡ giữa vị trí bắt đầu và vị trí kết thúc
    void CreatePlatform(Vector2 start, Vector2 end) {
        GameObject platform = Instantiate(platformPrefab);
        Vector2 middlePoint = (start + end) / 2;
        platform.transform.position = middlePoint;

        float length = Vector2.Distance(start, end);
        platform.transform.localScale = new Vector2(length, platform.transform.localScale.y);

        Vector2 direction = end - start;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        platform.transform.rotation = Quaternion.Euler(0, 0, angle);
    }
}
