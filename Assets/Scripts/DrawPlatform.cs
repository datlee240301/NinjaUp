using UnityEngine;

public class DrawPlatform : MonoBehaviour {
    public GameObject platformPrefab; // Prefab của platform
    private GameObject currentPlatform; // Platform hiện tại đang được vẽ
    private Vector2 startPoint; // Điểm bắt đầu vẽ platform
    private bool isDrawing; // Trạng thái vẽ

    private NinjaController ninjaController; // Tham chiếu đến NinjaController để kiểm tra trạng thái nhảy

    void Start() {
        // Tìm đối tượng Ninja và lấy component NinjaController
        ninjaController = FindObjectOfType<NinjaController>();
        if (ninjaController == null) {
            Debug.LogError("Không tìm thấy NinjaController!");
        }
    }

    void Update() {
        // Chỉ cho phép vẽ platform khi ninja đang rơi
        if (ninjaController != null && ninjaController.rb.velocity.y < 0) {
            // Khi nhấn chuột (hoặc chạm màn hình) để bắt đầu vẽ platform
            if (Input.GetMouseButtonDown(0)) {
                startPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                CreatePlatformAtStartPoint(startPoint);
                isDrawing = true;
            }

            // Khi đang giữ chuột (hoặc giữ tay trên màn hình) để điều chỉnh độ dài của platform
            if (Input.GetMouseButton(0) && isDrawing) {
                Vector2 currentPoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
                UpdatePlatformLength(startPoint, currentPoint);
            }

            // Khi thả chuột (hoặc ngừng chạm màn hình) thì dừng vẽ
            if (Input.GetMouseButtonUp(0) && isDrawing) {
                isDrawing = false;
                currentPlatform = null; // Kết thúc platform hiện tại
            }
        }
    }

    // Tạo platform ngay khi bắt đầu vẽ
    private void CreatePlatformAtStartPoint(Vector2 startPoint) {
        currentPlatform = Instantiate(platformPrefab, startPoint, Quaternion.identity);
    }

    // Cập nhật chiều dài và vị trí của platform khi vẽ
    private void UpdatePlatformLength(Vector2 startPoint, Vector2 currentPoint) {
        if (currentPlatform != null) {
            // Tính toán khoảng cách giữa điểm bắt đầu và điểm hiện tại
            Vector2 direction = currentPoint - startPoint;
            float distance = direction.magnitude;

            // Tính toán vị trí trung tâm và cập nhật vị trí của platform
            Vector2 platformCenter = (startPoint + currentPoint) / 2f;
            currentPlatform.transform.position = platformCenter;

            // Xoay platform theo hướng kéo
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            currentPlatform.transform.rotation = Quaternion.Euler(0, 0, angle);

            // Cập nhật kích thước platform theo độ dài kéo
            Vector3 newScale = currentPlatform.transform.localScale;
            newScale.x = distance; // Cập nhật chiều dài theo trục x
            currentPlatform.transform.localScale = newScale;
        }
    }
}
