using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuitGame : MonoBehaviour {

	public void quit()
    {
        bool isSignedIn = GameJolt.API.Manager.Instance.CurrentUser != null;
        if (isSignedIn)
        {
            GameJolt.API.Manager.Instance.CurrentUser.SignOut();
        }
        OptionsValues.optionValues.cancel.Play();
        Application.Quit();
    }
}
