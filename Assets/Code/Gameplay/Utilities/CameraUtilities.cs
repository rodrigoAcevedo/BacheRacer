using UnityEngine;

public static class CameraUtilities
{
    // Returns the min value and max value in Y axis which is the one we are interested.
    public static (float, float) CalculateCameraBoundaries()
    {
        Camera mainCamera = Camera.main;
        if (mainCamera != null)
        {
            float cameraHeight = 2f * mainCamera.orthographicSize;
            float cameraWidth = cameraHeight * mainCamera.aspect;
        
            var position = mainCamera.transform.position;
            Bounds cameraBounds = new Bounds(position, new Vector3(cameraWidth, cameraHeight, 0));

            return (cameraBounds.min.y, cameraBounds.max.y);
        }
        Debug.LogError("BacheRunner ERROR! Camera not found!");
        return (0,0);
    }
}