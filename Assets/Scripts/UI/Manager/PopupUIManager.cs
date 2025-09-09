using System;
using UnityEngine;
using UnityEngine.UI;
using TMPro;



public enum PopupType {

	YES_NO,
	CLOSE,
	ONEBUTTON,
}



public class PopupUIManager : MonoBehaviour {

    static PopupUIManager Instance  = null;
	[SerializeField]
	TextMeshProUGUI title			= null;
	[SerializeField]
	TextMeshProUGUI content			= null;
	[SerializeField]
	Button yesButton				= null;
	[SerializeField]
	Button noButton					= null;
	[SerializeField]
	Button closeButton				= null;
	[SerializeField]
	Button oneButton				= null;
	[SerializeField]
	TextMeshProUGUI yesBtnMes		= null;
	[SerializeField]
	TextMeshProUGUI noBtnMes		= null;
	[SerializeField]
	TextMeshProUGUI oneBtnMes		= null;
	Animator anim					= null;
	Action yesAction				= null;
	Action no_close_Action			= null;
	bool isOpen						= false;



	void Awake() {

		Instance	= this;
		anim		= GetComponent<Animator>();
	}



	public static void In( PopupType _popupType, string _title, string _content, string _yesBtnMes, string _noBtnMes, Action _yesAction, Action _no_close_Action ) {

		Instance._In( _popupType, _title, _content, _yesBtnMes, _noBtnMes, _yesAction, _no_close_Action );
	}



	void _In( PopupType _popupType, string _title, string _content, string _yesBtnMes, string _noBtnMes, Action _yesAction, Action _no_close_Action ) {

		switch ( _popupType ) {

			case PopupType.YES_NO:
			{
				yesButton.gameObject.SetActive( true );
				noButton.gameObject.SetActive( true );
				closeButton.gameObject.SetActive( false );
				oneButton.gameObject.SetActive( false );

				yesButton.onClick.RemoveAllListeners();
				yesButton.onClick.AddListener( ClickYes );
				noButton.onClick.RemoveAllListeners();
				noButton.onClick.AddListener( ClickNo_Close );

				break;
			}
			case PopupType.CLOSE:
			{
				yesButton.gameObject.SetActive( false );
				noButton.gameObject.SetActive( false );
				closeButton.gameObject.SetActive( true );
				oneButton.gameObject.SetActive( false );

				closeButton.onClick.RemoveAllListeners();
				closeButton.onClick.AddListener( ClickNo_Close );

				break;
			}
			case PopupType.ONEBUTTON:
			{
				yesButton.gameObject.SetActive( false );
				noButton.gameObject.SetActive( false );
				closeButton.gameObject.SetActive( false );
				oneButton.gameObject.SetActive( true );

				oneButton.onClick.RemoveAllListeners();
				oneButton.onClick.AddListener( ClickYes );

				break;
			}
		}

		title.text      = _title;
		content.text    = _content;

		content.ForceMeshUpdate( true, true );

		yesBtnMes.text  = _yesBtnMes;
		noBtnMes.text   = _noBtnMes;
		oneBtnMes.text  = _yesBtnMes;

		yesAction		= _yesAction;
		no_close_Action	= _no_close_Action;

		isOpen			= true;

		anim.Play( "In" );
	}



	void ClickYes() {

		SoundManager.PlaySE( SE.UI_BTNCLICK );
		VibrationManager.VibrateTap();

		isOpen          = false;

		anim.Play( "Out" );

		yesAction?.Invoke();
	}



	void ClickNo_Close() {

		SoundManager.PlaySE( SE.UI_CLOSEBTNCLICK );
		VibrationManager.VibrateTap();

		anim.Play( "Out" );

		no_close_Action?.Invoke();
	}



	public void CompleteInAnimation() {

		Vector2 size	= content.GetComponent<RectTransform>().sizeDelta;
		size.y			= content.bounds.size.y;
		content.GetComponent<RectTransform>().sizeDelta	= size;
	}



	public void CompleteOutAnimation() {

		isOpen          = false;
	}



	public static bool IsOpen() {

		return Instance.isOpen;
	}
}
