﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class TCPServer : MonoBehaviour
{
    public GameObject EmotionPanelGO;
    public GameObject HeartBeatGO;
    public GameObject EmotionGO;
    public GameObject CertaintyGO;

    private Text HeartBeatText;
    private Text EmotionText;
    private Text CertaintyText;

    public static bool connectionEstablished = false;
    public static string data = null;
    volatile bool keepReading = true;
    volatile bool keepWaiting = true;
    System.Threading.Thread SocketThread;
    public static EmotionData Emotion = null;


    public static TCPServer instance;
    void Awake()
    {
        if (!instance)
        {
            instance = this as TCPServer;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            DestroyObject(gameObject);
        }
    }

    // Use this for initialization
    void Start()
    {
        DontDestroyOnLoad(transform.gameObject);
        HeartBeatText = HeartBeatGO.GetComponent<Text>();
        EmotionText = EmotionGO.GetComponent<Text>();
        CertaintyText = CertaintyGO.GetComponent<Text>();

        Application.runInBackground = true;
        startServer();
    }

    private void Update()
    {
        if (connectionEstablished && Emotion != null)
        {
            EmotionPanelGO.SetActive(true);
            HeartBeatText.text = "Tętno: " + Emotion.heartBeat;
            EmotionText.text = "Emocja: " + Emotion.emotionType;
            CertaintyText.text = "Pewność: " + Emotion.certainty + " %";
        }
    }

    void startServer()
    {
        SocketThread = new System.Threading.Thread(networkCode);
        SocketThread.IsBackground = true;
        SocketThread.Start();
    }

    void stopServer()
    {
        keepReading = false;
        keepWaiting = false;
        Debug.Log("stopServer called!");

        if (SocketThread != null)
        {
            SocketThread.Abort();
            SocketThread.Interrupt();
        }

        if (handler != null)
        {
            handler.Close();
            if (handler.Connected)
            {
                handler.Disconnect(false);
                Debug.Log("Disconnected!");
            }
        }
    }

    //Socket listener;
    TcpListener listener;
    Socket handler;

    void networkCode()
    {
        string data;

        // Data buffer for incoming data.
        byte[] bytes = new Byte[1024];
        listener = new TcpListener(IPAddress.Any, 11000);

        try
        {
            listener.Start();

            // Start listening for connections.
            while (keepWaiting)
            {
                // Program is suspended while waiting for an incoming connection.
                Debug.Log("Waiting for Connection");
                handler = handler != null ? handler : listener.AcceptSocket();

                Debug.Log("Connection established");
                connectionEstablished = true;
                data = null;

                // An incoming connection needs to be processed.
                while (keepReading)
                {
                    bytes = new byte[1024];
                    int bytesRec = handler.Receive(bytes);

                    if (bytesRec <= 0)
                    {
                        keepReading = false;
                        handler.Disconnect(true);
                        break;
                    }

                    // TODO: HACK - Why 1 byte is broken? I should start from 0!
                    data += Encoding.UTF8.GetString(bytes, 1, bytesRec);

                    if (data.IndexOf("heartBeat") > -1)
                    {
                        break;
                    }

                    System.Threading.Thread.Sleep(100);
                }

                //Debug.Log("Original Data: " + data);
                String formattedData = Regex.Replace(data, "[^0-9a-zA-Z{}\",:]+", "");
                if (formattedData.Substring(0, 1).Equals("\""))
                {
                    formattedData = formattedData.Remove(0, 1);
                }

                formattedData = ExtractJSON(formattedData);
                //Debug.Log("Formatted Data: " + formattedData);

                try
                {
                    Emotion = JsonUtility.FromJson<EmotionData>(formattedData);
                } catch (Exception _e)
                {
                    Debug.Log("Parsing error. Using last emotion.");
                }
                
                //Debug.Log("Data: " + Emotion);
                System.Threading.Thread.Sleep(100);
            }
        }
        catch (Exception e)
        {
            Debug.Log(e.ToString());
        }
    }

    void OnApplicationQuit()
    {
        Debug.Log("OnApplicationQuit");
        stopServer();
    }

    private void magnificentDebug()
    {
        //Debug.Log("BytesRec: " + bytesRec);
        //Debug.Log("Bytes:" + bytes);
        //Debug.Log("UTF32: " + Encoding.UTF32.GetString(bytes, 0, bytesRec));
        //Debug.Log("UTF8: " + Encoding.UTF8.GetString(bytes, 0, bytesRec));
        //Debug.Log("UTF7: " + Encoding.UTF7.GetString(bytes, 0, bytesRec));
        //Debug.Log("ASCII: " + Encoding.ASCII.GetString(bytes, 0, bytesRec));
        //Debug.Log("Unicode: " + Encoding.Unicode.GetString(bytes, 0, bytesRec));
        //Debug.Log("Default: " + System.Text.Encoding.Default.GetString(bytes, 0, bytesRec));

        //Debug.Log("UTF32: " + Encoding.UTF32.GetString(bytes, 1, bytesRec));
        //Debug.Log("UTF8: " + Encoding.UTF8.GetString(bytes, 1, bytesRec));
        //Debug.Log("UTF7: " + Encoding.UTF7.GetString(bytes, 1, bytesRec));
        //Debug.Log("ASCII: " + Encoding.ASCII.GetString(bytes, 1, bytesRec));
        //Debug.Log("Unicode: " + Encoding.Unicode.GetString(bytes, 1, bytesRec));
        //Debug.Log("Default: " + System.Text.Encoding.Default.GetString(bytes, 1, bytesRec));
    }

    private string ExtractJSON(string s)
    {
        // You should check for errors in real-world code, omitted for brevity
        int startIndex = s.IndexOf("{");
        int endIndex = s.IndexOf("}") + 1;
        return s.Substring(startIndex, endIndex - startIndex);
    }
}
