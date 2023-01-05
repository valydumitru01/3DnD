using UnityEngine;
using System.Collections;
using Unity.Services.Authentication;

public class PlatformDefines : MonoBehaviour {
  void Start () {
    #if UNITY_EDITOR
    if (ParrelSync.ClonesManager.IsClone())
    {
        // When using a ParrelSync clone, switch to a different authentication profile to force the clone
        // to sign in as a different anonymous user account.
        string customArgument = ParrelSync.ClonesManager.GetArgument();
        AuthenticationService.Instance.SwitchProfile($"Clone_{customArgument}_Profile");
    }
    #endif
  }          
}