using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour {

	void Start () {
        var requestStream = UniRx.Observable.Return<string>("https://api.github.com/users");

        requestStream.Subscribe(t =>
            {
                ObservableWWW.Get(t).Subscribe(
                    x => Debug.Log("onSuccess: " + x), // onSuccess
                    ex => Debug.LogException(ex)); // onError
            });
	}
	
}
