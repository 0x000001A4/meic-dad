﻿using System;
using Grpc.Net.Client;
using BankServer.domain;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace BankServer
{
    public class Program
    {

        

        public static void Main(string[] args) 
        {

            Console.WriteLine("INITIALIZE: On");
            Console.WriteLine(args[1]);
            ServerConfiguration config = ServerConfiguration.ReadConfigFromFile(args[0]);
            BankManager bankManager = new BankManager();
            BankSlotManager bankSlotManager = new BankSlotManager(config);
            SlotTimer slotTimer = new SlotTimer(bankSlotManager,(uint)config.GetSlotDuration(),config.GetSlotFisrtTime());
            slotTimer.Execute();
            

            string serverHostname = "localhost";
            uint serverPort = 2;
            GrpcChannel channel = GrpcChannel.ForAddress("http://" + serverHostname + ":" + serverPort.ToString());

            CompareAndSwapService.CompareAndSwapServiceClient client = new CompareAndSwapService.CompareAndSwapServiceClient(channel);
            while (true) {
                Console.ReadKey();
                try {
                    client.CompareAndSwap(new CompareAndSwapReq { Leader = 1, Slot = 0 });
                } catch (Exception e) {
                    Console.WriteLine(e);
                }
            }
        }
    }
}
