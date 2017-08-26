using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManger : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{

		Debug.Log("battery Level:  " + SystemInfo.batteryLevel);
		Debug.Log("battery Status:  " + SystemInfo.batteryStatus);

		Debug.Log("Device Model:  " + SystemInfo.deviceModel);
		Debug.Log("Device Type:  " + SystemInfo.deviceType);


		Debug.Log("Graphics Device Type:  " + SystemInfo.graphicsDeviceType);
		Debug.Log("Graphics Device Vendor:  " + SystemInfo.graphicsDeviceVendor);
		Debug.Log("Graphics Device Version:  " + SystemInfo.graphicsDeviceVersion);
		Debug.Log("Graphics Device Memory size:  " + SystemInfo.graphicsMemorySize);
		Debug.Log("Graphics Device MultiThread:  " + SystemInfo.graphicsMultiThreaded);
		Debug.Log("Graphics Device DeviceName:  " + SystemInfo.graphicsDeviceName);
		Debug.Log("Graphics Device Shader Level:  " + SystemInfo.graphicsShaderLevel);
		Debug.Log("Graphics are Multi Threaded :  " + SystemInfo.graphicsMultiThreaded);

		Debug.Log("supported Render Target Count:  " + SystemInfo.supportedRenderTargetCount);
		Debug.Log("supported Compute Shaders:  " + SystemInfo.supportsComputeShaders);
		Debug.Log("supported Image Effects:  " + SystemInfo.supportsImageEffects);
		Debug.Log("supported instancing:  " + SystemInfo.supportsInstancing);
		Debug.Log("supported location services:  " + SystemInfo.supportsLocationService);

		Debug.Log("Max Cube Size Map:  " + SystemInfo.maxCubemapSize);
		Debug.Log("Max texture Size :  " + SystemInfo.maxTextureSize);

		Debug.Log("Unsupported Identifier :  " + SystemInfo.unsupportedIdentifier);



	}
	

}
