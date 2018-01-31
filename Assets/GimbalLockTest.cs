using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GimbalLockTest : MonoBehaviour
{
	
	public Button EulerButton;
	public Button QuaternionButton;

	[Header("Target")]
	public Transform TargetTransform;
	public Slider TargetXSlider;
	public Slider TargetYSlider;
	public Slider TargetZSlider;
	public Text TargetXText;
	public Text TargetYText;
	public Text TargetZText;

	[Header("Current")]
	public Transform CurTranform;
	public Slider CurXSlider;
	public Slider CurYSlider;
	public Slider CurZSlider;
	public Text CurXText;
	public Text CurYText;
	public Text CurZText;

	private float durationTime = 5f;
	private float timeCache = 0f;
	private Vector3 eulerAnglesCache = Vector3.zero;
	private Quaternion quaternionCache = Quaternion.identity;
	private bool isStart = false;

	private enum InterpolationMethod {EulerAngleRotation, QuaternionRotation};
	private InterpolationMethod curMethod;


	private void Start ()
	{
		UpdateUIValue();
		EulerButton.onClick.AddListener (OnEulerClick);
		QuaternionButton.onClick.AddListener (OnQuaternionClick);
	}
	
	private void Update ()
	{
		UpdateUIValue();
		SlerpCurrentTransform();
	}

	private void UpdateUIValue()
	{
		TargetTransform.transform.eulerAngles = new Vector3(TargetXSlider.value, TargetYSlider.value, TargetZSlider.value);
		CurTranform.transform.eulerAngles = new Vector3(CurXSlider.value, CurYSlider.value, CurZSlider.value);

		TargetXText.text = TargetXSlider.value.ToString("f2");
		TargetYText.text = TargetYSlider.value.ToString("f2");
		TargetZText.text = TargetZSlider.value.ToString("f2");

		CurXText.text = CurXSlider.value.ToString("f2");
		CurYText.text = CurYSlider.value.ToString("f2");
		CurZText.text = CurZSlider.value.ToString("f2");
	}

	private void SlerpCurrentTransform()
	{
		if (isStart) {
			float timeFactor = (Time.time - timeCache) / durationTime;
			if(curMethod == InterpolationMethod.EulerAngleRotation)
			{
				float x = Mathf.Lerp (eulerAnglesCache.x, TargetXSlider.value, timeFactor);
				float y = Mathf.Lerp (eulerAnglesCache.y, TargetYSlider.value, timeFactor);
				float z = Mathf.Lerp (eulerAnglesCache.z, TargetZSlider.value, timeFactor);
				
				CurXSlider.value = x;
				CurYSlider.value = y;
				CurZSlider.value = z;
			}
			else if(curMethod == InterpolationMethod.QuaternionRotation)
			{
				Vector3 targetEuler = new Vector3(TargetXSlider.value, TargetYSlider.value, TargetZSlider.value);
				Quaternion targetQuaternion = Quaternion.Euler(targetEuler);
				Quaternion rotation = Quaternion.Slerp(quaternionCache, targetQuaternion, timeFactor);

				CurXSlider.value = rotation.eulerAngles.x;
				CurYSlider.value = rotation.eulerAngles.y;
				CurZSlider.value = rotation.eulerAngles.z;
			}
			isStart = timeFactor < 1;
		}
	}

	private void OnEulerClick ()
	{
		eulerAnglesCache = new Vector3(CurXSlider.value, CurYSlider.value, CurZSlider.value);
		curMethod = InterpolationMethod.EulerAngleRotation;
		Play();
	}

	private void OnQuaternionClick ()
	{
		Vector3 euler = new Vector3(CurXSlider.value, CurYSlider.value, CurZSlider.value);
		quaternionCache = Quaternion.Euler(euler);
		curMethod = InterpolationMethod.QuaternionRotation;
		Play();
	}

	private void Play()
	{
		timeCache = Time.time;
		isStart = true;
	}
}