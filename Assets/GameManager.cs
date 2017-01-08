using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class GameManager : MonoBehaviour {

    // set from editor
    public GameObject refreshButton;

	void Start () {
        
        var refreshClickStream = refreshButton.GetComponent<Button>().onClick.AsObservable();

        var requestOnRefreshStream = refreshClickStream.Select(
            t =>
            {
                var randomOffset = Random.Range(1,500);
                return "https://api.github.com/users?since=" + randomOffset;
            });

        var startupRequestStream = UniRx.Observable.Return<string>("https://api.github.com/users");

        var requestStream = UniRx.Observable.Merge(
            requestOnRefreshStream, startupRequestStream
            );
        
        var responseStream = requestStream.SelectMany(
            requestUrl =>
            {
                return ObservableWWW.Get(requestUrl);
            });

        responseStream.Subscribe(
            response => // onSuccess
            {
                Debug.Log("response: " + response); 
            },
            e => // onError
            {
                Debug.LogException(e);
            });
                
	}
	
}
