using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour {

	void Start () {
        var requestStream = UniRx.Observable.Return<string>("https://api.github.com/users");

        requestStream.Subscribe(
            requestUrl =>
            {
                var responseStream = ObservableWWW.Get(requestUrl);

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

        var responseMetastream = requestStream.Select(
              requestUrl =>
            {
                return ObservableWWW.Get(requestUrl);
            });
	}
	
}
