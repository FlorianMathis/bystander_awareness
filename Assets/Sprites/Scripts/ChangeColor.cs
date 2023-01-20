using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;


public class ChangeColor : MonoBehaviour
{
    // Settings
    private Shader _shader;
    private Material _material;
    private Camera _camera;
    private CommandBuffer _commandBuffer;
 
    public CameraEvent _cameraEvent = CameraEvent.BeforeImageEffects;
    public GameObject _objectToHighlight;
 
    public void OnEnable()
    {
        if (_camera == null)
            _camera = Camera.main;
 
        _shader = Shader.Find("Hidden/OverlayShader");
        _material = new Material(_shader);
        _commandBuffer = new CommandBuffer();
        _camera = Camera.main;
        _camera.AddCommandBuffer(_cameraEvent, _commandBuffer);
    }
 
    public void OnDisable()
    {
        if (_commandBuffer != null && _camera != null)
        {
            _camera.RemoveCommandBuffer(_cameraEvent, _commandBuffer);
            _commandBuffer.Release();
            _commandBuffer = null;
        }
    }
 
    void LateUpdate()
    {
        if(_commandBuffer != null)
        {
            // Command buffers are not cleared automatically
            _commandBuffer.Clear();
            _commandBuffer.DrawAllMeshes(_objectToHighlight, _material, 0);
            // Overlay effect code goes here
        }
    }
}