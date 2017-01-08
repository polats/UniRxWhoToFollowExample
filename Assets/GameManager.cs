using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour {

	void Start () {
        var requestStream = UniRx.Observable.Return<string>("https://api.github.com/users");

        requestStream.Subscribe( t =>
            {
                var responseStream = ObservableWWW.Get(t);

                responseStream.Subscribe(
                    response => // onSuccess
                    {
                        Debug.Log("response: " + response); 
                    },
                    e => // onError
                    {
                        Debug.LogException(e);
                    });
            });
	}
	
}
