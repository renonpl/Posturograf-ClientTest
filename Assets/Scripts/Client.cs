using NetMQ;
using NetMQ.Sockets;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class Client : MonoBehaviour
{

    private static SubscriberSocket client;
    private static Vector2 data;
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
        string reply = client.ReceiveFrameString();
        //if (client.TryReceiveFrameString(out reply))
        {
            string[] s = reply.Split(' ');
            s = s.Where(val => System.Text.RegularExpressions.Regex.IsMatch(val, @"\d")).ToArray();
            data.x = Convert.ToInt32(s[0]);
            data.y = Convert.ToInt32(s[1]);
        }
    }

    private static void ReadData2()
    {
        byte[] reply;
        if (client.TryReceiveFrameBytes(out reply))
        {
            for(int i =0; i < reply.Length;i++)
                if (reply[i] > 0x39 || reply[i] < 0x30) reply[i] = 0x30;
            int x = 0;
            for (int i = 0; i <= 2; i++)
                x = 10 * x + reply[i] - 48;
            data.x = x;
            x = 0;
            for (int i = 4; i <= 6; i++)
                x = 10 * x + reply[i] - 48;
            data.y = x;
            //Debug.Log(Encoding.UTF8.GetString(x1, 0, x1.Length) + "<->" + Encoding.UTF8.GetString(x2, 0, x2.Length));
        }
    }


    void Start()
    {
        StartClient();
    }

    


    // Update is called once per frame
    void Update()
    {
        ReadData2();
        Debug.Log(data.x.ToString() + " " + data.y.ToString());
    }

    private void OnDestroy()
    {
        client.Dispose();
        NetMQConfig.Cleanup();
    }
}
