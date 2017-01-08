﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UniRx;

public class GameManager : MonoBehaviour {

	void Start () {
        var requestStream = UniRx.Observable.Return<string>("https://api.github.com/users");

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
