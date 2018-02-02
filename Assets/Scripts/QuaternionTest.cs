using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class QuaternionTest : MonoBehaviour
{
	public Button GimbalLockTestButton;

	[Header("Target")]
	public Transform TargetTransform;
	public Slider TargetXSlider;
	public Slider TargetYSlider;
	public Slider TargetZSlider;
	public Slider TargetThetaSlider;
	public Text TargetXText;
	public Text TargetYText;
	public Text TargetZText;
	public Text TargetThetaText;
	public LineRenderer Axis;

	private void Update ()
	{
		GimbalLockTestButton.onClick.AddListener(SwitchToGimbalLockTestScene);
		UpdateUIValue();
	}

	private void UpdateUIValue()
	{
		Vector3 axis = new Vector3(TargetXSlider.value, TargetYSlider.value, TargetZSlider.value).normalized;
		float angle = TargetThetaSlider.value * Mathf.Deg2Rad;
		TargetTransform.transform.rotation = new Quaternion(Mathf.Sin(angle / 2f) * axis.x, Mathf.Sin(angle / 2f) * axis.y, Mathf.Sin(angle / 2f) * axis.z, Mathf.Cos(angle / 2f));
		Axis.SetPosition(0, axis * -100f);
		Axis.SetPosition(1, axis * 100f);

		TargetXText.text = TargetXSlider.value.ToString("f2");
		TargetYText.text = TargetYSlider.value.ToString("f2");
		TargetZText.text = TargetZSlider.value.ToString("f2");
		TargetThetaText.text = TargetThetaSlider.value.ToString("f2");
	}

	private void SwitchToGimbalLockTestScene()
	{
		Application.LoadLevel("GimbalLockTest");
	}
}