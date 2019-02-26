using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Client : MonoBehaviour
{

    private static SubscriberSocket client;
    private Vector2 data;
    public Vector2 Data { get => data; }


    /*
     * Server has to work when game is running!
     * The server must work when the game is running,
     * otherwise game will freeze!
     */
    private static void StartClient(string server_address = "127.0.0.1", int port = 5556)
    {
        try
        {
            client = new SubscriberSocket();
            string connect_to = "tcp://"+ server_address +":" + port.ToString();
            client.Connect(connect_to);
            client.Subscribe("");
        }
        catch (Exception e)
        {
            //Console.WriteLine(e.ToString());
            Debug.Log(e.ToString());
        }
        Debug.Log("Server connected!");
    }

    private static void ReadData()
    {
        byte[] bytes;
        if (client.TryReceiveFrameBytes(out bytes))
        {
            //TODO: konwersja z bytes do Vectora liczb
            Debug.Log("");
            //Console.WriteLine("> " + reply);
        }
    }

    void Start()
    {
        StartClient();
    }

    


    // Update is called once per frame
    void LateUpdate()
    {
        ReadData();
    }

    private void OnDestroy()
    {
        client.Dispose();
        NetMQConfig.Cleanup();
    }
}
