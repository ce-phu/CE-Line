using UnityEngine;
using UnityEngine.UI;
using TMPro;



public class LoadingUIManager : MonoBehaviour {

    static LoadingUIManager Instance    = null;
	[SerializeField]
	Slider loadingGauge					= null;
	[SerializeField]
	TextMeshProUGUI versionText			= null;
	[SerializeField]
	GameObject system					= null;
	[SerializeField]
	Image image							= null;
	[SerializeField]
	RectTransform imageRect				= null;



	void Awake() {
		
        Instance    = this;

		if ( SystemManager.GetIsAlive() == false ) {

			Instantiate( system );
		}
	}


	public static void SetLoadingGauge( float _value ) {

		Instance._SetLoadingGauge( _value );
	}



	void _SetLoadingGauge( float _value ) {

		if ( loadingGauge != null ) {

			loadingGauge.value		= _value;
		}
	}


	public static void SetVersion( string _version ) {

		Instance._SetVersion( _version );
	}



	void _SetVersion( string _version ) {

		versionText.text	= _version;
	}
}
