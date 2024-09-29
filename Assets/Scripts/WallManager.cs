using UnityEngine;

public class WallManager : MonoBehaviour {
    public GameObject leftWall; // GameObject đại diện cho tường bên trái
    public GameObject rightWall; // GameObject đại diện cho tường bên phải
    public float wallThickness = 0.5f; // Độ dày của tường

    private Camera mainCamera;

    void Start() {
        mainCamera = Camera.main;
        PositionWalls();
    }

    void Update() {
        // Cập nhật vị trí của tường dựa theo vị trí của camera
        PositionWalls();
    }

    void PositionWalls() {
        // Lấy chiều cao và chiều rộng của màn hình dựa trên vị trí của camera
        float screenHeight = 2f * mainCamera.orthographicSize; // Chiều cao màn hình theo camera
        float screenWidth = screenHeight * mainCamera.aspect;   // Chiều rộng màn hình theo camera

        // Đặt kích thước của tường theo chiều cao của màn hình và độ dày cố định
        leftWall.transform.localScale = new Vector3(wallThickness, screenHeight, 1f);
        rightWall.transform.localScale = new Vector3(wallThickness, screenHeight, 1f);

        // Đặt vị trí của tường
        Vector3 cameraPosition = mainCamera.transform.position;

        // Tường bên trái luôn ở cạnh trái màn hình
        leftWall.transform.position = new Vector3(cameraPosition.x - screenWidth / 2 - wallThickness / 2, cameraPosition.y, 0);

        // Tường bên phải luôn ở cạnh phải màn hình
        rightWall.transform.position = new Vector3(cameraPosition.x + screenWidth / 2 + wallThickness / 2, cameraPosition.y, 0);
    }
}
