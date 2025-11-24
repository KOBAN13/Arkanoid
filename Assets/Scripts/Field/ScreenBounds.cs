using UnityEngine;

namespace Field
{
    public class ScreenBounds : MonoBehaviour
    {
        [SerializeField] private Transform _leftBound;
        [SerializeField] private Transform _rightBound;
        [SerializeField] private Transform _topBound;
        [SerializeField] private Transform _bottomBound;

        [SerializeField] private float thickness = 1f;

        private void Start()
        {
            UpdateBounds();
        }
    

        private void UpdateBounds()
        {
            var mainCamera = Camera.main;

            var zDist = Mathf.Abs(mainCamera.transform.position.z - _leftBound.position.z);

            var bottomLeft = mainCamera.ScreenToWorldPoint(new Vector3(0, 0, zDist));
            var topRight   = mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, zDist));

            var width  = topRight.x - bottomLeft.x;
            var height = topRight.y - bottomLeft.y;

            var centerX = (bottomLeft.x + topRight.x) * 0.5f;
            var centerY = (bottomLeft.y + topRight.y) * 0.5f;
        
            _leftBound.position = new Vector3(bottomLeft.x - thickness / 2f, centerY, _leftBound.position.z);
            _leftBound.localScale = new Vector3(thickness, height + thickness * 2f, 1f);

            _rightBound.position = new Vector3(topRight.x + thickness / 2f, centerY, _rightBound.position.z);
            _rightBound.localScale = new Vector3(thickness, height + thickness * 2f, 1f);
        
            _topBound.position = new Vector3(centerX, topRight.y + thickness / 2f, _topBound.position.z);
            _topBound.localScale = new Vector3(width + thickness * 2f, thickness, 1f);
        
            _bottomBound.position = new Vector3(centerX, bottomLeft.y - thickness / 2f, _bottomBound.position.z);
            _bottomBound.localScale = new Vector3(width + thickness * 2f, thickness, 1f);
        }
    }
}
